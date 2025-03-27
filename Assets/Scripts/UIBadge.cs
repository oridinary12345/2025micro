using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBadge : MonoBehaviour
{
	[SerializeField]
	private Image _badgeImage;

	private Tweener _moveAnim;

	public void SetVisible(bool isVisible, bool animate)
	{
		_badgeImage.enabled = isVisible;
		if (isVisible && animate)
		{
			if (_moveAnim == null)
			{
				_moveAnim = _badgeImage.transform.DOLocalMoveY(15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
			}
			else
			{
				_moveAnim.Play();
			}
		}
		else if (_moveAnim != null)
		{
			DOTween.Kill(_moveAnim);
			_moveAnim = null;
		}
	}

	private void OnDestroy()
	{
		if (_moveAnim != null)
		{
			_moveAnim.Kill();
			_moveAnim = null;
		}
	}
}