using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CurrentGamesRequest : PlayFabRequestCommon
	{
		public string BuildVersion;

		public string GameMode;

		public Region? Region;

		public string StatisticName;

		public CollectionFilter TagFilter;
	}
}