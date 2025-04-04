using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UnlockContainerInstanceRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterId;

		public string ContainerItemInstanceId;

		public string KeyItemInstanceId;
	}
}