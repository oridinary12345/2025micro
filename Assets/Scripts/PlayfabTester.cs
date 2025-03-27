using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabTester : MonoBehaviour
{
	private void Start()
	{
		LoginGuestIOS();
	}

	private void LoginGuestIOS()
	{
	}

	private void OnPlayfabLoginIOSComplete(LoginResult result, string deviceId)
	{
		UnityEngine.Debug.Log("OnPlayfabLoginIOSComplete: " + JsonUtility.ToJson(result));
		SetPlayfabId(result.PlayFabId);
		SetDeviceId(deviceId);
		UpdateUserData();
	}

	private void OnPlayfabLoginIOSFailed(PlayFabError error)
	{
		UnityEngine.Debug.LogWarning("Login to playfab failed. " + error.Error + ". " + error.ErrorMessage);
	}

	private void UpdateUserData()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["profile"] = App.Instance.Player.ToJson();
		UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest();
		updateUserDataRequest.Data = dictionary;
		updateUserDataRequest.Permission = UserDataPermission.Private;
		UpdateUserDataRequest request = updateUserDataRequest;
		UnityEngine.Debug.Log("UpdateUserData. Size = " + dictionary["profile"].Length + " - profile: " + dictionary["profile"]);
		PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdateComplete, OnUserDataUpdateFailed);
	}

	private void OnUserDataUpdateComplete(UpdateUserDataResult result)
	{
		UnityEngine.Debug.Log("OnUserDataUpdateComplete: " + JsonUtility.ToJson(result));
		LoadUserData();
	}

	private void OnUserDataUpdateFailed(PlayFabError error)
	{
		UnityEngine.Debug.LogWarning("UpdateUserData failed. " + error.Error + ". " + error.ErrorMessage);
	}

	private void LoadUserData()
	{
		List<string> list = new List<string>();
		list.Add("profile");
		List<string> keys = list;
		GetUserDataRequest getUserDataRequest = new GetUserDataRequest();
		getUserDataRequest.Keys = keys;
		GetUserDataRequest getUserDataRequest2 = getUserDataRequest;
		UnityEngine.Debug.Log("LoadUserData: " + JsonUtility.ToJson(getUserDataRequest2));
		PlayFabClientAPI.GetUserData(getUserDataRequest2, OnLoadUserDataComplete, OnLoadUserDataFailed);
	}

	private void OnLoadUserDataComplete(GetUserDataResult result)
	{
		UnityEngine.Debug.Log("OnLoadUserDataComplete: " + JsonUtility.ToJson(result));
		UnityEngine.Debug.Log("OnLoadUserDataComplete: " + result.Data["profile"].Value);
	}

	private void OnLoadUserDataFailed(PlayFabError error)
	{
		UnityEngine.Debug.LogWarning("LoadUserData failed. " + error.Error + ". " + error.ErrorMessage);
	}

	private string GetPlayfabId()
	{
		return PlayerPrefs.GetString("PLAYFABID", string.Empty);
	}

	private void SetPlayfabId(string playfabId)
	{
		PlayerPrefs.SetString("PLAYFABID", playfabId);
	}

	private string GetDeviceId()
	{
		return PlayerPrefs.GetString("VENDORID", string.Empty);
	}

	private void SetDeviceId(string vendorId)
	{
		PlayerPrefs.SetString("VENDORID", vendorId);
	}
}