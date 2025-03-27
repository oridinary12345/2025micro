using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	public class TransitionKit : MonoBehaviour
	{
		public static bool keepTransitionKitInstance;

		private const int _transitionKitLayer = 31;

		private TransitionKitDelegate _transitionKitDelegate;

		public Camera transitionKitCamera;

		public Material material;

		private static TransitionKit _instance;

		public static TransitionKit instance
		{
			get
			{
				if (!_instance)
				{
					_instance = (UnityEngine.Object.FindObjectOfType(typeof(TransitionKit)) as TransitionKit);
					if (!_instance)
					{
						GameObject gameObject = new GameObject("TransitionKit");
						gameObject.layer = 31;
						gameObject.transform.position = new Vector3(99999f, 99999f, 99999f);
						_instance = gameObject.AddComponent<TransitionKit>();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return _instance;
			}
		}

		public static event Action onScreenObscured;

		public static event Action onTransitionComplete;

		private T getOrAddComponent<T>() where T : Component
		{
			T val = base.gameObject.GetComponent<T>();
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				val = base.gameObject.AddComponent<T>();
			}
			return val;
		}

		private void initialize()
		{
			MeshFilter orAddComponent = getOrAddComponent<MeshFilter>();
			orAddComponent.mesh = (_transitionKitDelegate.meshForDisplay() ?? generateQuadMesh());
			material = getOrAddComponent<MeshRenderer>().material;
			material.shader = (_transitionKitDelegate.shaderForTransition() ?? Shader.Find("prime[31]/Transitions/Fader"));
			material.color = Color.white;
			_instance.StartCoroutine(_instance.setupCameraAndTexture());
		}

		private Mesh generateQuadMesh()
		{
			float num = 5f;
			float num2 = num * ((float)Screen.width / (float)Screen.height);
			Mesh mesh = new Mesh();
			mesh.vertices = new Vector3[4]
			{
				new Vector3(0f - num2, 0f - num, 0f),
				new Vector3(0f - num2, num, 0f),
				new Vector3(num2, 0f - num, 0f),
				new Vector3(num2, num, 0f)
			};
			mesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};
			mesh.triangles = new int[6]
			{
				0,
				1,
				2,
				3,
				2,
				1
			};
			return mesh;
		}

		private IEnumerator setupCameraAndTexture()
		{
			yield return new WaitForEndOfFrame();
			material.mainTexture = (_transitionKitDelegate.textureForDisplay() ?? getScreenshotTexture());
			transitionKitCamera = getOrAddComponent<Camera>();
			transitionKitCamera.orthographic = true;
			transitionKitCamera.nearClipPlane = -1f;
			transitionKitCamera.farClipPlane = 1f;
			transitionKitCamera.depth = float.MaxValue;
			transitionKitCamera.cullingMask = int.MinValue;
			transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKitCamera.enabled = true;
			if (TransitionKit.onScreenObscured != null)
			{
				TransitionKit.onScreenObscured();
			}
			yield return StartCoroutine(_transitionKitDelegate.onScreenObscured(this));
			cleanup();
		}

		private Texture2D getScreenshotTexture()
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			texture2D.Apply();
			return texture2D;
		}

		private void cleanup()
		{
			if (!(_instance == null))
			{
				if (TransitionKit.onTransitionComplete != null)
				{
					TransitionKit.onTransitionComplete();
				}
				_transitionKitDelegate = null;
				if (keepTransitionKitInstance)
				{
					GetComponent<MeshRenderer>().material.mainTexture = null;
					GetComponent<MeshFilter>().mesh = null;
					base.gameObject.SetActive( false);
					transitionKitCamera.enabled = false;
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
					_instance = null;
				}
			}
		}

		public void transitionWithDelegate(TransitionKitDelegate transitionKitDelegate)
		{
			base.gameObject.SetActive( true);
			_transitionKitDelegate = transitionKitDelegate;
			initialize();
		}

		public void makeTextureTransparent()
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, Color.clear);
			texture2D.Apply();
			material.mainTexture = texture2D;
		}

		public IEnumerator waitForLevelToLoad(string level)
		{
			while (SceneManager.GetActiveScene().name != level)
			{
				yield return null;
			}
		}

		public IEnumerator tickProgressPropertyInMaterial(float duration, bool reverseDirection = false)
		{
			float start = (!reverseDirection) ? 0f : 1f;
			float end = (!reverseDirection) ? 1f : 0f;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float step = Mathf.Lerp(start, end, Mathf.Pow(elapsed / duration, 2f));
				material.SetFloat("_Progress", step);
				yield return null;
			}
		}
	}
}