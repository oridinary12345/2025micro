using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class StartPurchaseResult : PlayFabResultCommon
	{
		public List<CartItem> Contents;

		public string OrderId;

		public List<PaymentOption> PaymentOptions;

		public Dictionary<string, int> VirtualCurrencyBalances;
	}
}