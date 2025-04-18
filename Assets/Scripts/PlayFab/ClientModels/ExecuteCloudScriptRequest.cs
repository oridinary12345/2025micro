using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ExecuteCloudScriptRequest : PlayFabRequestCommon
	{
		public string FunctionName;

		public object FunctionParameter;

		public bool? GeneratePlayStreamEvent;

		public CloudScriptRevisionOption? RevisionSelection;

		public int? SpecificRevision;
	}
}