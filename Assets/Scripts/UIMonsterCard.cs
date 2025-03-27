using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterCard : MonoBehaviour
{
	public UIGameButton Button;

	[SerializeField]
	private Image _overlayImage;

	[SerializeField]
	private Image _monsterImage;

	[SerializeField]
	private TextMeshProUGUI _levelText;

	[SerializeField]
	private TextMeshProUGUI _coinsPaymentText;

	[SerializeField]
	private TextMeshProUGUI _awakenTimerText;

	[SerializeField]
	private TextMeshProUGUI _monsterCountText;

	[SerializeField]
	private Slider _monsterCountSlider;

	[SerializeField]
	private Slider _paymentTimerSlider;

	[SerializeField]
	private GameObject _lockedText;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private GameObject _monsterFrame;

	[SerializeField]
	private Image _backgroundImage;

	private MuseumData _museumData;

	private readonly YieldInstruction _wait = new WaitForEndOfFrame();

	private Coroutine _timerCR;

	public static UIMonsterCard Create(MuseumData museumData)
	{
		UIMonsterCard uIMonsterCard = UnityEngine.Object.Instantiate(Resources.Load<UIMonsterCard>("UI/UIMonsterCard"));
		return uIMonsterCard.Init(museumData);
	}

	public UIMonsterCard Init(MuseumData museumData)
	{
		bool flag = museumData.IsUnlocked();
		Button = base.gameObject.GetComponent<UIGameButton>();
		Button.OnClick(OnMonsterCardClicked);
		Button.interactable = flag;
		Button.enabled = flag;
		_museumData = museumData;
		_museumData.Events.MonsterAwakenEvent += OnMonsterAwaken;
		_museumData.Events.MonsterFallAsleepEvent += OnMonsterFallAsleep;
		UpdateContent();
		_lockedText.SetActive(!flag);
		_paymentTimerSlider.gameObject.SetActive(flag);
		_monsterCountSlider.gameObject.SetActive(flag);
		_awakenTimerText.gameObject.SetActive(flag);
		_monsterFrame.gameObject.SetActive(flag);
		_overlayImage.gameObject.SetActive(flag);
		_levelText.gameObject.SetActive(flag);
		_canvasGroup.alpha = ((!flag) ? 0.5f : 1f);
		_backgroundImage.color = new Color(1f, 1f, 1f, (!flag) ? 0.5f : 1f);
		return this;
	}

	private void OnDestroy()
	{
		if (_museumData != null && _museumData.Events != null)
		{
			_museumData.Events.MonsterAwakenEvent -= OnMonsterAwaken;
			_museumData.Events.MonsterFallAsleepEvent -= OnMonsterFallAsleep;
		}
	}

	private void OnMonsterAwaken(MuseumData data)
	{
		if (data.Config.Id == _museumData.Config.Id)
		{
			UpdateContent();
		}
	}

	private void OnMonsterFallAsleep(MuseumData data)
	{
		if (data.Config.Id == _museumData.Config.Id)
		{
			UpdateContent();
		}
	}

	private void OnMonsterCardClicked()
	{
		if (_museumData.IsReadyToWakeUp())
		{
			_museumData.WakeUp();
		}
	}

	private void UpdateContent()
	{
		_overlayImage.enabled = _museumData.IsSleeping();
		_monsterCountText.text = _museumData.CurrentMonsterKilledCount.ToString();
		_monsterCountSlider.value = _museumData.MonsterKilledObjective01;
		_coinsPaymentText.text = "+" + _museumData.GetPaymentAmout();
		_paymentTimerSlider.value = _museumData.GetPaymentProgress01();
		_levelText.text = ((!_museumData.HasReachLevelMax()) ? ("LVL " + (_museumData.Level + 1)) : "MAX");
		_monsterImage.sprite = Resources.Load<Sprite>("Monsters/" + _museumData.Config.Id);
		UpdateTimer();
	}

	private void UpdateTimer()
	{
		if (_timerCR != null)
		{
			StopCoroutine(_timerCR);
			_timerCR = null;
		}
		if (_museumData.IsSleeping())
		{
			_awakenTimerText.text = string.Empty;
		}
		else
		{
			_timerCR = StartCoroutine(UpdateTimerCR());
		}
	}

	private IEnumerator UpdateTimerCR()
	{
		while (!_museumData.IsSleeping())
		{
			_awakenTimerText.text = _museumData.GetTimeBeforeSleep();
			_paymentTimerSlider.value = _museumData.GetPaymentProgress01();
			yield return _wait;
		}
		_awakenTimerText.SetText(string.Empty);
		_timerCR = null;
		yield return null;
		UpdateContent();
	}
}