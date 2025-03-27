using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuStack : MonoSingleton<UIMenuStack>
{
	private readonly Stack<UIMenu> _stack = new Stack<UIMenu>();

	private Coroutine _transitionCoroutine;

	private UIMenu _pushingMenu;

	private UIMenu _poppingMenu;

	public void Push(UIMenu menu)
	{
		UIMenu uIMenu = Peek();
		if (uIMenu != null)
		{
			uIMenu.OnFocusLost();
		}
		_stack.Push(menu);
		_transitionCoroutine = StartCoroutine(PushMenuCR(menu));
	}

	private IEnumerator PushMenuCR(UIMenu menu)
	{
		if (_poppingMenu != null)
		{
			while (_transitionCoroutine != null)
			{
				yield return null;
			}
		}
		_pushingMenu = menu;
		menu.gameObject.SetActive( true);
		menu.OnPush();
		yield return StartCoroutine(menu.PushAnimationCR());
		menu.OnFocusGained();
		_pushingMenu = null;
		_transitionCoroutine = null;
	}

	public UIMenu Pop()
	{
		if (_stack.Count == 0)
		{
			return null;
		}
		UIMenu uIMenu = _stack.Pop();
		_transitionCoroutine = StartCoroutine(PopMenuCR(uIMenu));
		return uIMenu;
	}

	private IEnumerator PopMenuCR(UIMenu menu)
	{
		if (_pushingMenu != null)
		{
			while (_transitionCoroutine != null)
			{
				yield return null;
			}
		}
		_poppingMenu = menu;
		menu.OnFocusLost();
		menu.OnPop();
		UIMenu peek = Peek();
		if (peek != null)
		{
			peek.OnFocusGaining();
		}
		yield return StartCoroutine(menu.PopAnimationCR());
		menu.gameObject.SetActive( false);
		if (peek != null)
		{
			peek.OnFocusGained();
		}
		_poppingMenu = null;
		_transitionCoroutine = null;
	}

	public UIMenu Peek()
	{
		if (_stack.Count > 0)
		{
			return _stack.Peek();
		}
		return null;
	}
}