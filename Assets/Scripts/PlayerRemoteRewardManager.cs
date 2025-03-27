using UnityEngine;

public class PlayerRemoteRewardManager
{
	private static string _sheetUrl = "https://docs.google.com/spreadsheets/d/1yTFiqzTv25onXyei3-bsbBkTP5kL1gsXc48XlNL_nwQ/export?format=tsv&gid=580132204";

	private readonly RemoteRewardProfile _profile;

	private readonly PlayerRewardManager _rewardManager;

	private string _playerUid;

	public RemoteRewardsEvents Events
	{
		get;
		private set;
	}

	public PlayerRemoteRewardManager(string uid, RemoteRewardProfile profile, PlayerRewardManager rewardManager)
	{
		_playerUid = uid;
		_profile = profile;
		_rewardManager = rewardManager;
		Events = new RemoteRewardsEvents();
	}

	private bool IsCodeRedeemed(string code)
	{
		return _profile.RedeemedCode.Contains(code);
	}

	public bool RedeemCode(string code)
	{
		if (IsCodeRedeemed(code))
		{
			return false;
		}
		_profile.RedeemedCode.Add(code);
		return true;
	}

	public void ValidateCode(string code)
	{
		if (IsCodeRedeemed(code))
		{
			UnityEngine.Debug.Log("ValidateCode failed because code is already redeemed.");
		}
		else
		{
			MonoSingleton<FileDownloader>.Instance.DownloadCSV(_sheetUrl, delegate(CSVFile csv)
			{
				OnCSVFileDownloaded(code, csv);
			}, delegate(string error)
			{
				UnityEngine.Debug.Log("Error: " + error);
			});
		}
	}

	private void OnCodeValidationDone(string originalCode, RemoteRewardConfig config)
	{
		if (!config.IsValid())
		{
			UnityEngine.Debug.Log("OnCodeValidationDone() Config is not valid");
		}
		else if (config.PromoCode != originalCode)
		{
			UnityEngine.Debug.Log("OnCodeValidationDone() Promo code doesn't match...");
		}
		else if (RedeemCode(config.PromoCode))
		{
			LootProfile lootProfile = new LootProfile();
			lootProfile.Amount = config.Amount;
			lootProfile.LootId = config.LootId;
			RewardLoot reward = new RewardLoot(lootProfile, 1f, false);
			_rewardManager.Redeem(reward, CurrencyReason.remoteReward);
			Events.OnRemoteRewardReceived(reward);
		}
	}

	public void OnCSVFileDownloaded(string originalCode, CSVFile csv)
	{
		RemoteRewardConfig remoteRewardConfig = null;
		for (int i = 0; i < csv.EntriesCount; i++)
		{
			string @string = csv.GetString(i, "PlayerId");
			if (@string == _playerUid)
			{
				RemoteRewardConfig remoteRewardConfig2 = new RemoteRewardConfig();
				remoteRewardConfig2.PlayerId = @string;
				remoteRewardConfig2.PromoCode = csv.GetString(i, "PromoCode");
				remoteRewardConfig2.LootId = csv.GetString(i, "LootId");
				remoteRewardConfig2.Amount = csv.GetInt(i, "Amount");
				remoteRewardConfig = remoteRewardConfig2;
				break;
			}
		}
		if (remoteRewardConfig != null)
		{
			OnCodeValidationDone(originalCode, remoteRewardConfig);
		}
	}
}