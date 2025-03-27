using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class UIActionSelector : MonoBehaviour
{
	[SerializeField]
	private HorizontalScrollSnap _selector;

	[SerializeField]
	private UIGameWeaponSelector _weaponSelector;

	[SerializeField]
	private UIGameWeaponSelector _itemSelector;

	[SerializeField]
	private UIGameButton _previousPage;

	[SerializeField]
	private UIGameButton _nextPage;

	private Dictionary<int, UIGameWeaponSelector> _selectorByIndexes = new Dictionary<int, UIGameWeaponSelector>();

	private void Awake()
	{
		_selectorByIndexes[0] = _weaponSelector;
		_selectorByIndexes[1] = _itemSelector;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void RegisterEvents()
	{
		_selector.OnSelectionPageChangedEvent.AddListener(OnSelectionPageChanged);
		_selector.OnSelectionChangeEndEvent.AddListener(OnSelectionChangeEnded);
	}

	private void UnRegisterEvents()
	{
		if (_selector != null)
		{
			_selector.OnSelectionPageChangedEvent.RemoveListener(OnSelectionPageChanged);
			_selector.OnSelectionChangeEndEvent.RemoveListener(OnSelectionChangeEnded);
		}
	}

	private void OnSelectionChangeEnded(int pageIndex)
	{
	}

	private void OnSelectionPageChanged(int pageIndex)
	{
	}
}