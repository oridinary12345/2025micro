using System;

public class StateMachineEvent<T, TT>
{
	public event Action<T, TT> StateEnterEvent;

	public event Action<T, TT> StateExitEvent;

	public void OnStateEnter(T t, TT tt)
	{
		if (this.StateEnterEvent != null)
		{
			this.StateEnterEvent(t, tt);
		}
	}

	public void OnStateExit(T t, TT tt)
	{
		if (this.StateExitEvent != null)
		{
			this.StateExitEvent(t, tt);
		}
	}
}