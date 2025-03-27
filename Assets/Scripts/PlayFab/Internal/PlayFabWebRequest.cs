using PlayFab.Json;
using PlayFab.SharedModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;

namespace PlayFab.Internal
{
	public class PlayFabWebRequest : IPlayFabHttp
	{
		private static readonly Queue<Action> ResultQueueTransferThread = new Queue<Action>();

		private static readonly Queue<Action> ResultQueueMainThread = new Queue<Action>();

		private static readonly List<CallRequestContainer> ActiveRequests = new List<CallRequestContainer>();

		private static bool certValidationSet = false;

		private static Thread _requestQueueThread;

		private static readonly object _ThreadLock = new object();

		private static readonly TimeSpan ThreadKillTimeout = TimeSpan.FromSeconds(60.0);

		private static DateTime _threadKillTime = DateTime.UtcNow + ThreadKillTimeout;

		private static bool _isApplicationPlaying;

		private static int _activeCallCount;

		private static string _unityVersion;

		private static bool _sessionStarted;

		[CompilerGenerated]
		private static ThreadStart _003C_003Ef__mg_0024cache0;

		public static RemoteCertificateValidationCallback CustomCertValidationHook
		{
			set
			{
				ServicePointManager.ServerCertificateValidationCallback = value;
				certValidationSet = true;
			}
		}

		public bool SessionStarted
		{
			get
			{
				return _sessionStarted;
			}
			set
			{
				_sessionStarted = value;
			}
		}

		public string AuthKey
		{
			get;
			set;
		}

		public string EntityToken
		{
			get;
			set;
		}

		public static void SkipCertificateValidation()
		{
			RemoteCertificateValidationCallback remoteCertificateValidationCallback2 = ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;
			certValidationSet = true;
		}

		public void InitializeHttp()
		{
			SetupCertificates();
			_isApplicationPlaying = true;
			_unityVersion = Application.unityVersion;
		}

		public void OnDestroy()
		{
			_isApplicationPlaying = false;
			lock (ResultQueueTransferThread)
			{
				ResultQueueTransferThread.Clear();
			}
			lock (ActiveRequests)
			{
				ActiveRequests.Clear();
			}
			lock (_ThreadLock)
			{
				_requestQueueThread = null;
			}
		}

		private void SetupCertificates()
		{
			ServicePointManager.DefaultConnectionLimit = 10;
			ServicePointManager.Expect100Continue = false;
			if (!certValidationSet)
			{
				UnityEngine.Debug.LogWarning("PlayFab API calls will likely fail because you have not set up a HttpWebRequest certificate validation mechanism");
				UnityEngine.Debug.LogWarning("Please set a validation callback into PlayFab.Internal.PlayFabWebRequest.CustomCertValidationHook, or set PlayFab.Internal.PlayFabWebRequest.SkipCertificateValidation()");
			}
		}

		private static bool AcceptAllCertifications(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		public void SimpleGetCall(string fullUrl, Action<byte[]> successCallback, Action<string> errorCallback)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				SimpleHttpsWorker("GET", fullUrl, null, successCallback, errorCallback);
			});
			thread.Start();
		}

		public void SimplePutCall(string fullUrl, byte[] payload, Action successCallback, Action<string> errorCallback)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				SimpleHttpsWorker("PUT", fullUrl, payload, delegate
				{
					successCallback();
				}, errorCallback);
			});
			thread.Start();
		}

		private void SimpleHttpsWorker(string httpMethod, string fullUrl, byte[] payload, Action<byte[]> successCallback, Action<string> errorCallback)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
			httpWebRequest.UserAgent = "UnityEngine-Unity; Version: " + _unityVersion;
			httpWebRequest.Method = httpMethod;
			httpWebRequest.KeepAlive = PlayFabSettings.RequestKeepAlive;
			httpWebRequest.Timeout = PlayFabSettings.RequestTimeout;
			httpWebRequest.AllowWriteStreamBuffering = false;
			httpWebRequest.ReadWriteTimeout = PlayFabSettings.RequestTimeout;
			if (payload != null)
			{
				httpWebRequest.ContentLength = payload.LongLength;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(payload, 0, payload.Length);
				}
			}
			try
			{
				WebResponse response = httpWebRequest.GetResponse();
				byte[] array = null;
				using (Stream stream2 = response.GetResponseStream())
				{
					if (stream2 != null)
					{
						array = new byte[response.ContentLength];
						stream2.Read(array, 0, array.Length);
					}
				}
				successCallback(array);
			}
			catch (WebException ex)
			{
				try
				{
					using (Stream stream3 = ex.Response.GetResponseStream())
					{
						if (stream3 != null)
						{
							using (StreamReader streamReader = new StreamReader(stream3))
							{
								errorCallback(streamReader.ReadToEnd());
							}
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
			}
		}

		public void MakeApiCall(CallRequestContainer reqContainer)
		{
			reqContainer.HttpState = HttpRequestState.Idle;
			lock (ActiveRequests)
			{
				ActiveRequests.Insert(0, reqContainer);
			}
			ActivateThreadWorker();
		}

		private static void ActivateThreadWorker()
		{
			lock (_ThreadLock)
			{
				if (_requestQueueThread == null)
				{
					_requestQueueThread = new Thread(WorkerThreadMainLoop);
					_requestQueueThread.Start();
				}
			}
		}

		private static void WorkerThreadMainLoop()
		{
			try
			{
				lock (_ThreadLock)
				{
					_threadKillTime = DateTime.UtcNow + ThreadKillTimeout;
				}
				List<CallRequestContainer> list = new List<CallRequestContainer>();
				bool flag;
				do
				{
					lock (ActiveRequests)
					{
						list.AddRange(ActiveRequests);
						ActiveRequests.Clear();
						_activeCallCount = list.Count;
					}
					int count = list.Count;
					for (int num = count - 1; num >= 0; num--)
					{
						switch (list[num].HttpState)
						{
						case HttpRequestState.Error:
							list.RemoveAt(num);
							break;
						case HttpRequestState.Idle:
							Post(list[num]);
							break;
						case HttpRequestState.Sent:
							if (list[num].HttpRequest.HaveResponse)
							{
								ProcessHttpResponse(list[num]);
							}
							break;
						case HttpRequestState.Received:
							ProcessJsonResponse(list[num]);
							list.RemoveAt(num);
							break;
						}
					}
					lock (_ThreadLock)
					{
						DateTime utcNow = DateTime.UtcNow;
						if (count > 0 && _isApplicationPlaying)
						{
							_threadKillTime = utcNow + ThreadKillTimeout;
						}
						flag = (utcNow <= _threadKillTime);
						if (!flag)
						{
							_requestQueueThread = null;
						}
					}
					Thread.Sleep(1);
				}
				while (flag);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				_requestQueueThread = null;
			}
		}

		private static void Post(CallRequestContainer reqContainer)
		{
			try
			{
				reqContainer.HttpRequest = (HttpWebRequest)WebRequest.Create(reqContainer.FullUrl);
				reqContainer.HttpRequest.UserAgent = "UnityEngine-Unity; Version: " + _unityVersion;
				reqContainer.HttpRequest.SendChunked = false;
				reqContainer.HttpRequest.Proxy = null;
				foreach (KeyValuePair<string, string> requestHeader in reqContainer.RequestHeaders)
				{
					reqContainer.HttpRequest.Headers.Add(requestHeader.Key, requestHeader.Value);
				}
				reqContainer.HttpRequest.ContentType = "application/json";
				reqContainer.HttpRequest.Method = "POST";
				reqContainer.HttpRequest.KeepAlive = PlayFabSettings.RequestKeepAlive;
				reqContainer.HttpRequest.Timeout = PlayFabSettings.RequestTimeout;
				reqContainer.HttpRequest.AllowWriteStreamBuffering = false;
				reqContainer.HttpRequest.Proxy = null;
				reqContainer.HttpRequest.ContentLength = reqContainer.Payload.LongLength;
				reqContainer.HttpRequest.ReadWriteTimeout = PlayFabSettings.RequestTimeout;
				using (Stream stream = reqContainer.HttpRequest.GetRequestStream())
				{
					stream.Write(reqContainer.Payload, 0, reqContainer.Payload.Length);
				}
				reqContainer.HttpState = HttpRequestState.Sent;
			}
			catch (WebException ex)
			{
				reqContainer.JsonResponse = (ResponseToString(ex.Response) ?? (ex.Status + ": WebException making http request to: " + reqContainer.FullUrl));
				WebException exception = new WebException(reqContainer.JsonResponse, ex);
				UnityEngine.Debug.LogException(exception);
				QueueRequestError(reqContainer);
			}
			catch (Exception innerException)
			{
				reqContainer.JsonResponse = "Unhandled exception in Post : " + reqContainer.FullUrl;
				Exception exception2 = new Exception(reqContainer.JsonResponse, innerException);
				UnityEngine.Debug.LogException(exception2);
				QueueRequestError(reqContainer);
			}
		}

		private static void ProcessHttpResponse(CallRequestContainer reqContainer)
		{
			try
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)reqContainer.HttpRequest.GetResponse();
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					reqContainer.JsonResponse = ResponseToString(httpWebResponse);
				}
				if (httpWebResponse.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(reqContainer.JsonResponse))
				{
					reqContainer.JsonResponse = (reqContainer.JsonResponse ?? "No response from server");
					QueueRequestError(reqContainer);
				}
				else
				{
					reqContainer.HttpState = HttpRequestState.Received;
				}
			}
			catch (Exception innerException)
			{
				string text = "Unhandled exception in ProcessHttpResponse : " + reqContainer.FullUrl;
				reqContainer.JsonResponse = (reqContainer.JsonResponse ?? text);
				Exception exception = new Exception(text, innerException);
				UnityEngine.Debug.LogException(exception);
				QueueRequestError(reqContainer);
			}
		}

		private static void QueueRequestError(CallRequestContainer reqContainer)
		{
			reqContainer.Error = PlayFabHttp.GeneratePlayFabError(reqContainer.ApiEndpoint, reqContainer.JsonResponse, reqContainer.CustomData);
			reqContainer.HttpState = HttpRequestState.Error;
			lock (ResultQueueTransferThread)
			{
				ResultQueueTransferThread.Enqueue(delegate
				{
					PlayFabHttp.SendErrorEvent(reqContainer.ApiRequest, reqContainer.Error);
					if (reqContainer.ErrorCallback != null)
					{
						reqContainer.ErrorCallback(reqContainer.Error);
					}
				});
			}
		}

		private static void ProcessJsonResponse(CallRequestContainer reqContainer)
		{
			try
			{
				HttpResponseObject httpResponseObject = JsonWrapper.DeserializeObject<HttpResponseObject>(reqContainer.JsonResponse);
				if (httpResponseObject == null || httpResponseObject.code != 200)
				{
					QueueRequestError(reqContainer);
				}
				else
				{
					reqContainer.JsonResponse = JsonWrapper.SerializeObject(httpResponseObject.data);
					reqContainer.DeserializeResultJson();
					reqContainer.ApiResult.Request = reqContainer.ApiRequest;
					reqContainer.ApiResult.CustomData = reqContainer.CustomData;
					SingletonMonoBehaviour<PlayFabHttp>.instance.OnPlayFabApiResult(reqContainer.ApiResult);
					lock (ResultQueueTransferThread)
					{
						ResultQueueTransferThread.Enqueue(delegate
						{
							PlayFabDeviceUtil.OnPlayFabLogin(reqContainer.ApiResult);
						});
					}
					lock (ResultQueueTransferThread)
					{
						ResultQueueTransferThread.Enqueue(delegate
						{
							try
							{
								PlayFabHttp.SendEvent(reqContainer.ApiEndpoint, reqContainer.ApiRequest, reqContainer.ApiResult, ApiProcessingEventType.Post);
								reqContainer.InvokeSuccessCallback();
							}
							catch (Exception exception2)
							{
								UnityEngine.Debug.LogException(exception2);
							}
						});
					}
				}
			}
			catch (Exception innerException)
			{
				string text = "Unhandled exception in ProcessJsonResponse : " + reqContainer.FullUrl;
				reqContainer.JsonResponse = (reqContainer.JsonResponse ?? text);
				Exception exception = new Exception(text, innerException);
				UnityEngine.Debug.LogException(exception);
				QueueRequestError(reqContainer);
			}
		}

		public void Update()
		{
			lock (ResultQueueTransferThread)
			{
				while (ResultQueueTransferThread.Count > 0)
				{
					Action item = ResultQueueTransferThread.Dequeue();
					ResultQueueMainThread.Enqueue(item);
				}
			}
			while (ResultQueueMainThread.Count > 0)
			{
				Action action = ResultQueueMainThread.Dequeue();
				action();
			}
		}

		private static string ResponseToString(WebResponse webResponse)
		{
			if (webResponse == null)
			{
				return null;
			}
			try
			{
				using (Stream stream = webResponse.GetResponseStream())
				{
					if (stream != null)
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							return streamReader.ReadToEnd();
						}
					}
					return null;
				}
			}
			catch (WebException ex)
			{
				try
				{
					using (Stream stream2 = ex.Response.GetResponseStream())
					{
						if (stream2 != null)
						{
							using (StreamReader streamReader2 = new StreamReader(stream2))
							{
								return streamReader2.ReadToEnd();
							}
						}
						return null;
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
					return null;
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
				return null;
			}
		}

		public int GetPendingMessages()
		{
			int num = 0;
			lock (ActiveRequests)
			{
				num += ActiveRequests.Count + _activeCallCount;
			}
			lock (ResultQueueTransferThread)
			{
				return num + ResultQueueTransferThread.Count;
			}
		}
	}
}