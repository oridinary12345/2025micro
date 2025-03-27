using UnityEngine;

public class MuseumConfig
{
	public string Id;

	public string WorldId;

	public int PaymentDelaySec;

	public int KillAmountNeededBase;

	private readonly int _paymentBaseAmount;

	private readonly float _paymentAmountLevelUpPow;

	private readonly int _awakeTimeMin;

	private readonly int _awakeTimeMinLevelUpBonus;

	public MuseumConfig(int paymentBaseAmount, float paymentAmountLevelUpPow, int awakeTimeMin, int awakeTimeMinLevelUpBonus)
	{
		_paymentBaseAmount = paymentBaseAmount;
		_paymentAmountLevelUpPow = paymentAmountLevelUpPow;
		_awakeTimeMin = awakeTimeMin;
		_awakeTimeMinLevelUpBonus = awakeTimeMinLevelUpBonus;
	}

	public int GetLevel(int monsterKilledTotal)
	{
		int num = 0;
		for (int i = GetKillAmountNeeded(num); monsterKilledTotal >= i; i += GetKillAmountNeeded(num))
		{
			num++;
		}
		return num;
	}

	public int GetCurrentMonsterKilled(int monsterKilledTotal)
	{
		int num = monsterKilledTotal;
		for (int i = 0; i < GetLevel(monsterKilledTotal); i++)
		{
			num -= GetKillAmountNeeded(i);
		}
		return Mathf.Max(0, num);
	}

	public int GetMaxAwakeTimeMin(int level)
	{
		return _awakeTimeMin + level * _awakeTimeMinLevelUpBonus;
	}

	public int GetPaymentAmount(int level)
	{
		return Mathf.FloorToInt((float)_paymentBaseAmount + Mathf.Pow(level, _paymentAmountLevelUpPow));
	}

	public int GetKillAmountNeeded(int level)
	{
		if (level == 0)
		{
			return KillAmountNeededBase;
		}
		return GetKillAmountNeeded(level - 1) + level * KillAmountNeededBase;
	}
}