using System;

public class ChestEvents
{
	public event Action<string> ChestRedeemedEvent;

	public void OnChestRedeemed(string chestId)
	{
		if (this.ChestRedeemedEvent != null)
		{
			this.ChestRedeemedEvent(chestId);
		}
	}
}