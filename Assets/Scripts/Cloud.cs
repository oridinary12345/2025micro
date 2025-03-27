using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoSingleton<Cloud>
{
	public CloudUser CurrentUser
	{
		get;
		private set;
	}

	public event Action CloudSyncFinishedEvent;

	protected override void Init()
	{
		LoadCurrentUser();
	}

	public static void Reset()
	{
		PlayFabClientAPI.ForgetAllCredentials();
	}

	public void StartSyncFlow()
	{
		//if (MonoSingleton<AppConfigs>.Instance.GetValue("UsePlayfab", false))
		{
			LoginGuestAndroid();
		}
	}

	private void LoginGuestIOS()
	{
		string deviceId = GetDeviceId();
		string empty = string.Empty;
		if (string.IsNullOrEmpty(deviceId))
		{
		}
		GetPlayerCombinedInfoRequestParams getPlayerCombinedInfoRequestParams = new GetPlayerCombinedInfoRequestParams();
		getPlayerCombinedInfoRequestParams.GetUserAccountInfo = false;
		getPlayerCombinedInfoRequestParams.GetUserData = true;
		GetPlayerCombinedInfoRequestParams infoRequestParameters = getPlayerCombinedInfoRequestParams;
		LoginWithIOSDeviceIDRequest loginWithIOSDeviceIDRequest = new LoginWithIOSDeviceIDRequest();
		loginWithIOSDeviceIDRequest.CreateAccount = true;
		loginWithIOSDeviceIDRequest.DeviceId = deviceId;
		loginWithIOSDeviceIDRequest.DeviceModel = SystemInfo.deviceModel;
		loginWithIOSDeviceIDRequest.EncryptedRequest = string.Empty;
		loginWithIOSDeviceIDRequest.InfoRequestParameters = infoRequestParameters;
		loginWithIOSDeviceIDRequest.LoginTitlePlayerAccountEntity = true;
		loginWithIOSDeviceIDRequest.OS = empty;
		loginWithIOSDeviceIDRequest.PlayerSecret = string.Empty;
		loginWithIOSDeviceIDRequest.TitleId = "C986";
		LoginWithIOSDeviceIDRequest request = loginWithIOSDeviceIDRequest;
		PlayFabClientAPI.LoginWithIOSDeviceID(request, delegate(LoginResult result)
		{
			OnPlayfabLoginComplete(result, deviceId);
		}, OnPlayfabLoginFailed);
	}

	private void LoginGuestAndroid()
	{
		GetPlayerCombinedInfoRequestParams getPlayerCombinedInfoRequestParams = new GetPlayerCombinedInfoRequestParams();
		getPlayerCombinedInfoRequestParams.GetUserAccountInfo = false;
		getPlayerCombinedInfoRequestParams.GetUserData = true;
		GetPlayerCombinedInfoRequestParams infoRequestParameters = getPlayerCombinedInfoRequestParams;
		LoginWithAndroidDeviceIDRequest loginWithAndroidDeviceIDRequest = new LoginWithAndroidDeviceIDRequest();
		loginWithAndroidDeviceIDRequest.CreateAccount = true;
		loginWithAndroidDeviceIDRequest.AndroidDevice = SystemInfo.deviceModel;
		loginWithAndroidDeviceIDRequest.EncryptedRequest = string.Empty;
		loginWithAndroidDeviceIDRequest.InfoRequestParameters = infoRequestParameters;
		loginWithAndroidDeviceIDRequest.LoginTitlePlayerAccountEntity = true;
		loginWithAndroidDeviceIDRequest.OS = SystemInfo.operatingSystem;
		loginWithAndroidDeviceIDRequest.PlayerSecret = string.Empty;
		loginWithAndroidDeviceIDRequest.TitleId = "C986";
		LoginWithAndroidDeviceIDRequest request = loginWithAndroidDeviceIDRequest;
	}

	private void OnPlayfabLoginComplete(LoginResult result, string deviceId)
	{
		if (result == null)
		{
			UnityEngine.Debug.LogWarning("OnPlayfabLoginComplete but with a null result...");
			OnPlayfabLoginFailed(new PlayFabError());
			return;
		}
		SetPlayfabId(result.PlayFabId);
		SetDeviceId(deviceId);
		SetCurrentUser(result.PlayFabId);
		if (result.InfoResultPayload != null && result.InfoResultPayload.UserData != null)
		{
			UserDataRecord value = null;
			if (!result.InfoResultPayload.UserData.TryGetValue("profile", out value))
			{
			}
		}
		UploadCurrentProgress();
		CloudSyncOver();
	}

	private void OnPlayfabLoginFailed(PlayFabError error)
	{
		if (error.Error != PlayFabErrorCode.ServiceUnavailable)
		{
			UnityEngine.Debug.LogWarning("Login to playfab failed. " + error.Error + ". " + error.ErrorMessage);
		}
		CloudSyncOver();
	}

	protected void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			UploadCurrentProgress();
		}
	}

	private void UploadCurrentProgress()
	{
		if (PlayFabClientAPI.IsClientLoggedIn())
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["profile"] = App.Instance.Player.ToJson();
			UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
			updateUserDataRequest.Data = dictionary;
			updateUserDataRequest.Permission = UserDataPermission.Private;
			UpdateUserDataRequest request = updateUserDataRequest;
			UnityEngine.Debug.Log("UpdateUserData. Size = " + dictionary["profile"].Length + ", profile: " + dictionary["profile"]);
			if (dictionary["profile"].Length > 10000)
			{
				UnityEngine.Debug.LogWarning("You CANT upload a player profile that is over 10000 bytes");
			}
			else
			{
				PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdateComplete, OnUserDataUpdateFailed);
			}
		}
	}

	private void OnUserDataUpdateComplete(UpdateUserDataResult result)
	{
	}

	private void OnUserDataUpdateFailed(PlayFabError error)
	{
		if (error.Error == PlayFabErrorCode.InvalidParams)
		{
			UnityEngine.Debug.LogWarning("Your JSON save file is probably too big");
		}
		if (error.Error != PlayFabErrorCode.ServiceUnavailable)
		{
			UnityEngine.Debug.LogWarning("UpdateUserData failed. " + error.Error + ". " + error.ErrorMessage);
		}
	}

	private string GetPlayfabId()
	{
		return PlayerPrefs.GetString("PLAYFABID", string.Empty);
	}

	private void SetPlayfabId(string playfabId)
	{
		PlayerPrefs.SetString("PLAYFABID", playfabId);
	}

	private void SetCurrentUser(string playfabId)
	{
		CurrentUser = new CloudUser
		{
			PlayfabId = playfabId
		};
		PlayerPrefs.SetString("CURRENTUSER", JsonConvert.SerializeObject(CurrentUser));
	}

	private void LoadCurrentUser()
	{
		string @string = PlayerPrefs.GetString("CURRENTUSER", string.Empty);
		CurrentUser = (JsonConvert.DeserializeObject(@string, typeof(CloudUser)) as CloudUser);
	}

	private string GetDeviceId()
	{
		return PlayerPrefs.GetString("VENDORID", string.Empty);
	}

	private void SetDeviceId(string vendorId)
	{
		PlayerPrefs.SetString("VENDORID", vendorId);
	}

	private void CloudSyncOver()
	{
		if (this.CloudSyncFinishedEvent != null)
		{
			this.CloudSyncFinishedEvent();
		}
	}
}