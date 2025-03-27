using System.Collections.Generic;
using UnityEngine;

public class UIMuseumPanel : MonoBehaviour
{
	private PlayerMuseumManager _museumManager;

	public void Init(PlayerMuseumManager museumManager, string worldId)
	{
		_museumManager = museumManager;
		List<MuseumConfig> configByWorldId = MonoSingleton<MuseumConfigs>.Instance.GetConfigByWorldId(worldId);
		foreach (MuseumConfig item in configByWorldId)
		{
			MuseumData museumData = _museumManager.GetMuseumData(item.Id);
			if (museumData != null)
			{
				UIMonsterCard uIMonsterCard = UIMonsterCard.Create(museumData);
				uIMonsterCard.GetComponent<RectTransform>().SetParent(base.transform, false);
			}
		}
	}

	private void OnEnable()
	{
		RegisterEvents();
	}

	private void OnDisable()
	{
		UnRegisterEvents();
	}

	private void RegisterEvents()
	{
		UnRegisterEvents();
	}

	private void UnRegisterEvents()
	{
		if (!MonoSingleton<GameAdsController>.IsCreated())
		{
		}
	}
}