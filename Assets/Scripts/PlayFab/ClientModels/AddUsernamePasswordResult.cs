using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddUsernamePasswordResult : PlayFabResultCommon
	{
		public string Username;
	}
}