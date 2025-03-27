using System;
using UnityEngine;

[Serializable]
public class RemoteRewardConfig
{
	[SerializeField]
	public string PlayerId;

	[SerializeField]
	public string PromoCode;

	[SerializeField]
	public string LootId;

	[SerializeField]
	public int Amount;

	public static RemoteRewardConfig Invalid = new RemoteRewardConfig
	{
		PlayerId = "invalid",
		PromoCode = string.Empty,
		LootId = string.Empty,
		Amount = 0
	};

	public bool IsValid()
	{
		return PlayerId != Invalid.PlayerId && Amount != 0 && !string.IsNullOrEmpty(PlayerId) && !string.IsNullOrEmpty(LootId) && !string.IsNullOrEmpty(PromoCode);
	}

	public override string ToString()
	{
		return $"[CSVDownloader] PlayerId = {PlayerId}, PromoCode = {PromoCode}, LootId = {LootId}, Amount = {Amount}";
	}
}