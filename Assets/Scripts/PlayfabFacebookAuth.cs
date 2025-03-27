using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabFacebookAuth : MonoBehaviour
{
	private string _message;

	public void Start()
	{
		SetMessage("Initializing Facebook...");
	}

	private void OnFacebookInitialized()
	{
		SetMessage("Logging into Facebook...");
	}

	private void OnPlayfabFacebookAuthComplete(LoginResult result)
	{
		SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
	}

	private void OnPlayfabFacebookAuthFailed(PlayFabError error)
	{
		SetMessage("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport(), true);
	}

	public void SetMessage(string message, bool error = false)
	{
		_message = message;
		if (error)
		{
			UnityEngine.Debug.LogError(_message);
		}
		else
		{
			UnityEngine.Debug.Log(_message);
		}
	}

	public void OnGUI()
	{
	}
}