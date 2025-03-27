using System;

public class LootEvents
{
	public event Action<LootProfile, int, CurrencyReason> LootUpdatedEvent;

	public void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (this.LootUpdatedEvent != null)
		{
			this.LootUpdatedEvent(loot, delta, reason);
		}
	}
}