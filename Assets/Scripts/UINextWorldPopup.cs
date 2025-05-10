using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UINextWorldPopup : UIMenuPopup
{
	[SerializeField]
	private Text _titleText;

	[SerializeField]
	private Text _mission1Text;

	[SerializeField]
	private Text _mission1ProgressText;

	[SerializeField]
	private Slider _mission1Slider;

	[SerializeField]
	private Text _mission2Text;

	[SerializeField]
	private Text _mission2ProgressText;

	[SerializeField]
	private Slider _mission2Slider;

	[SerializeField]
	private Text _mission3Text;

	[SerializeField]
	private Text _mission3ProgressText;

	[SerializeField]
	private Slider _mission3Slider;

	[SerializeField]
	private Text _unlockPriceText;

	[SerializeField]
	private GameObject _missionStateBox;

	[SerializeField]
	private GameObject _notReadyBox;

	private WorldData _worldToUnlock;

	public void Init(WorldData world)
	{
		if (world == null || world.UnlockMissions == null)
		{
			Debug.LogError("World or UnlockMissions is null in UINextWorldPopup.Init");
			return;
		}

		if (world.UnlockMissions.Count != 3)
		{
			Debug.LogWarning("World unlock should have 2 missions, but it's not the case with " + (world.Config?.Id ?? "unknown"));
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			return;
		}

		_worldToUnlock = world;

		if (_titleText != null && _worldToUnlock.Config != null)
		{
			_titleText.text = $"WORLD {_worldToUnlock.Config.Index + 1} - {_worldToUnlock.Config.Title}";
		}

		UpdateMainBox();

		Mission mission = world.UnlockMissions[0];
		if (mission != null)
		{
			if (_mission1Slider != null)
			{
				_mission1Slider.DOValue(mission.Progress01, 0f);
			}
			if (_mission1Text != null)
			{
				_mission1Text.text = mission.GetDescription();
			}
			if (_mission1ProgressText != null)
			{
				_mission1ProgressText.text = mission.GetProgressionText();
			}
		}

		if (world.UnlockMissions.Count > 1)
		{
			Mission mission2 = world.UnlockMissions[1];
			if (mission2 != null)
			{
				if (_mission2Slider != null)
				{
					_mission2Slider.DOValue(mission2.Progress01, 0f);
				}
				if (_mission2Text != null)
				{
					_mission2Text.text = mission2.GetDescription();
				}
				if (_mission2ProgressText != null)
				{
					_mission2ProgressText.text = mission2.GetProgressionText();
				}
			}

			if (world.UnlockMissions.Count > 2)
			{
				Mission mission3 = world.UnlockMissions[2];
				if (mission3 != null)
				{
					if (_mission3Slider != null)
					{
						_mission3Slider.DOValue(mission3.Progress01, 0f);
					}
					if (_mission3Text != null)
					{
						_mission3Text.text = mission3.GetDescription();
					}
					if (_mission3ProgressText != null)
					{
						_mission3ProgressText.text = mission3.GetProgressionText();
					}
				}
			}
			else
			{
				_mission3Slider.gameObject.SetActive( false);
				_mission3Text.gameObject.SetActive( false);
				_mission3ProgressText.gameObject.SetActive( false);
			}
		}
	}

	public void UpdateMainBox()
	{
		WorldData nextLockedWorldData = App.Instance.Player.LevelManager.GetNextLockedWorldData();
		bool flag = !nextLockedWorldData.Config.IsValid() || _worldToUnlock.Config.Id == nextLockedWorldData.Config.Id;
		_missionStateBox.SetActive(flag);
		_notReadyBox.SetActive(!flag);
	}
}