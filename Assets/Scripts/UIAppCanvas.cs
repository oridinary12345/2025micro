using DG.Tweening;
using UnityEngine;

public class UIAppCanvas : MonoBehaviour
{
	[SerializeField]
	private UIHomePanel _home;

	[SerializeField]
	private UITextOverlayPanel _textOverlayPanel;

	[SerializeField]
	private UIMenuHudNotification _notificationHud;

	[SerializeField]
	private UIBetaAnnouncementPanel _betaAnnouncementPanel;

	[SerializeField]
	private UIQuitAppPopup _quitAppPanel;

	[SerializeField]
	private GameController _gameController;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		DOTween.Init();
		DOTween.useSafeMode = true;
		Universe.Create();
		_gameController.Create();
		base.transform.gameObject.AddComponent<MenuFXManager>().Setup(App.Instance.FXManager, _gameController.CharacterEvents, App.Instance.MenuEvents);
		_textOverlayPanel.Init(App.Instance.MenuEvents);
		_notificationHud.Init(App.Instance.Player.MonsterMissions);
		_home.Show();
		if (App.Instance.Player.IsBetaWarningSeen())
		{
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.C))
		{
			App.Instance.Player.LootManager.Add("lootCoin", 9999, CurrencyReason.unknown);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && MonoSingleton<UIMenuStack>.Instance.Peek() == _home)
		{
			_quitAppPanel.Show();
		}
	}
}