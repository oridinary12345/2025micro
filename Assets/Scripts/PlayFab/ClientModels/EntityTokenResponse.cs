using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class EntityTokenResponse : PlayFabResultCommon
	{
		public EntityKey Entity;

		public string EntityToken;

		public DateTime? TokenExpiration;
	}
}