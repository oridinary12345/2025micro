using System.Collections;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
	protected virtual void Awake()
	{
		UnityCanvasContainer componentInChildren = base.transform.GetComponentInChildren<UnityCanvasContainer>();
		if (componentInChildren != null)
		{
			componentInChildren.Init();
		}
	}

	public void Show()
	{
		Push(this);
	}

	public void Hide()
	{
		if (MonoSingleton<UIMenuStack>.Instance.Peek() != this)
		{
			UnityEngine.Debug.LogWarning(this + " should not be allowed to Pop() the Peek() menu, which is: " + MonoSingleton<UIMenuStack>.Instance.Peek());
		}
		else
		{
			Pop();
		}
	}

	protected void Push(UIMenu menu)
	{
		MonoSingleton<UIMenuStack>.Instance.Push(menu);
	}

	protected void Pop()
	{
		MonoSingleton<UIMenuStack>.Instance.Pop();
	}

	public virtual IEnumerator PushAnimationCR()
	{
		yield return null;
	}

	public virtual IEnumerator PopAnimationCR()
	{
		yield return null;
	}

	public virtual void OnFocusGaining()
	{
	}

	public virtual void OnFocusGained()
	{
	}

	public virtual void OnFocusLost()
	{
	}

	public virtual void OnPush()
	{
	}

	public virtual void OnPop()
	{
	}
}