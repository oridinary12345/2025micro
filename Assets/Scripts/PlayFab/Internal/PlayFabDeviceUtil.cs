using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.SharedModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayFab.Internal
{
	public static class PlayFabDeviceUtil
	{
		private static bool _needsAttribution;

		private static bool _gatherDeviceInfo;

		[CompilerGenerated]
		private static Action<AttributeInstallResult> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Action<EmptyResult> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Action<PlayFabError> _003C_003Ef__mg_0024cache2;

		private static void DoAttributeInstall()
		{
			if (_needsAttribution && !PlayFabSettings.DisableAdvertising)
			{
				AttributeInstallRequest attributeInstallRequest = new AttributeInstallRequest();
				switch (PlayFabSettings.AdvertisingIdType)
				{
				case "Adid":
					attributeInstallRequest.Adid = PlayFabSettings.AdvertisingIdValue;
					break;
				case "Idfa":
					attributeInstallRequest.Idfa = PlayFabSettings.AdvertisingIdValue;
					break;
				}
				PlayFabClientAPI.AttributeInstall(attributeInstallRequest, OnAttributeInstall, null);
			}
		}

		private static void OnAttributeInstall(AttributeInstallResult result)
		{
			PlayFabSettings.AdvertisingIdType += "_Successful";
		}

		private static void SendDeviceInfoToPlayFab()
		{
			if (!PlayFabSettings.DisableDeviceInfo && _gatherDeviceInfo)
			{
				DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
				deviceInfoRequest.Info = JsonWrapper.DeserializeObject<Dictionary<string, object>>(JsonWrapper.SerializeObject(new PlayFabDataGatherer()));
				DeviceInfoRequest request = deviceInfoRequest;
				PlayFabClientAPI.ReportDeviceInfo(request, OnGatherSuccess, OnGatherFail);
			}
		}

		private static void OnGatherSuccess(EmptyResult result)
		{
		}

		private static void OnGatherFail(PlayFabError error)
		{
			UnityEngine.Debug.Log("OnGatherFail: " + error.GenerateErrorReport());
		}

		public static void OnPlayFabLogin(PlayFabResultCommon result)
		{
			LoginResult loginResult = result as LoginResult;
			RegisterPlayFabUserResult registerPlayFabUserResult = result as RegisterPlayFabUserResult;
			if (loginResult != null || registerPlayFabUserResult != null)
			{
				UserSettings settingsForUser = null;
				string playFabId = null;
				EntityTokenResponse entityInfo = null;
				if (loginResult != null)
				{
					settingsForUser = loginResult.SettingsForUser;
					playFabId = loginResult.PlayFabId;
					entityInfo = loginResult.EntityToken;
				}
				else if (registerPlayFabUserResult != null)
				{
					settingsForUser = registerPlayFabUserResult.SettingsForUser;
					playFabId = registerPlayFabUserResult.PlayFabId;
					entityInfo = registerPlayFabUserResult.EntityToken;
				}
				_OnPlayFabLogin(settingsForUser, playFabId, entityInfo);
			}
		}

		private static void _OnPlayFabLogin(UserSettings settingsForUser, string playFabId, EntityTokenResponse entityInfo)
		{
			_needsAttribution = (_gatherDeviceInfo = false);
			if (settingsForUser != null)
			{
				_needsAttribution = settingsForUser.NeedsAttribution;
				_gatherDeviceInfo = settingsForUser.GatherDeviceInfo;
			}
			if (PlayFabSettings.AdvertisingIdType != null && PlayFabSettings.AdvertisingIdValue != null)
			{
				DoAttributeInstall();
			}
			else
			{
				GetAdvertIdFromUnity();
			}
			SendDeviceInfoToPlayFab();
		}

		private static void GetAdvertIdFromUnity()
		{
			Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingId, bool trackingEnabled, string error)
			{
				PlayFabSettings.DisableAdvertising = !trackingEnabled;
				if (trackingEnabled)
				{
					PlayFabSettings.AdvertisingIdType = "Adid";
					PlayFabSettings.AdvertisingIdValue = advertisingId;
					DoAttributeInstall();
				}
			});
		}
	}
}