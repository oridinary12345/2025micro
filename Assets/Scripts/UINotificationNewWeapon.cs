using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationNewWeapon : UINotificationPanel
{
	[SerializeField]
	private Image _weaponShadowImage;

	[SerializeField]
	private Image _missionSliderFillImage;

	public void Init(WeaponConfig weaponConfig)
	{
		_weaponShadowImage.sprite = Resources.Load<Sprite>("Weapons/" + weaponConfig.Id + "/UI_w_" + weaponConfig.Id);
	}

	public override void OnShown()
	{
		Color colorStart = _missionSliderFillImage.color;
		_missionSliderFillImage.DOColor(Color.white, 0.3f).SetEase(Ease.Flash, 6f).SetDelay(0.4f)
			.OnComplete(delegate
			{
				_missionSliderFillImage.color = colorStart;
			});
	}
}