using System;
using UnityEngine.Purchasing;

public class AppEvents
{
	public event Action AppPausedEvent;

	public event Action AppResumedEvent;

	public event Action<Reward, Product> InAppPurchaseDoneEvent;

	public void OnAppPaused()
	{
		if (this.AppPausedEvent != null)
		{
			this.AppPausedEvent();
		}
	}

	public void OnAppResumed()
	{
		if (this.AppResumedEvent != null)
		{
			this.AppResumedEvent();
		}
	}

	public void OnInAppPurchaseDone(Reward reward, Product product)
	{
		if (this.InAppPurchaseDoneEvent != null)
		{
			this.InAppPurchaseDoneEvent(reward, product);
		}
	}
}