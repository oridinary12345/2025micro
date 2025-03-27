using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINextWorldPopup : UIMenuPopup
{
	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private TextMeshProUGUI _mission1Text;

	[SerializeField]
	private TextMeshProUGUI _mission1ProgressText;

	[SerializeField]
	private Slider _mission1Slider;

	[SerializeField]
	private TextMeshProUGUI _mission2Text;

	[SerializeField]
	private TextMeshProUGUI _mission2ProgressText;

	[SerializeField]
	private Slider _mission2Slider;

	[SerializeField]
	private TextMeshProUGUI _mission3Text;

	[SerializeField]
	private TextMeshProUGUI _mission3ProgressText;

	[SerializeField]
	private Slider _mission3Slider;

	[SerializeField]
	private TextMeshProUGUI _unlockPriceText;

	[SerializeField]
	private GameObject _missionStateBox;

	[SerializeField]
	private GameObject _notReadyBox;

	private WorldData _worldToUnlock;

	public void Init(WorldData world)
	{
		if (world.UnlockMissions.Count != 3)
		{
			UnityEngine.Debug.LogWarning("World unlock should have 2 missions, but it's not the case with " + world.Config.Id);
			base.gameObject.SetActive( false);
			return;
		}
		_worldToUnlock = world;
		_titleText.text = $"WORLD {_worldToUnlock.Config.Index + 1} - {_worldToUnlock.Config.Title}";
		UpdateMainBox();
		Mission mission = world.UnlockMissions[0];
		_mission1Slider.DOValue(mission.Progress01, 0f);
		_mission1Text.text = mission.GetDescription();
		_mission1ProgressText.text = mission.GetProgressionText();
		if (world.UnlockMissions.Count > 1)
		{
			Mission mission2 = world.UnlockMissions[1];
			_mission2Slider.DOValue(mission2.Progress01, 0f);
			_mission2Text.text = mission2.GetDescription();
			_mission2ProgressText.text = mission2.GetProgressionText();
			if (world.UnlockMissions.Count > 2)
			{
				Mission mission3 = world.UnlockMissions[2];
				_mission3Slider.DOValue(mission3.Progress01, 0f);
				_mission3Text.text = mission3.GetDescription();
				_mission3ProgressText.text = mission3.GetProgressionText();
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