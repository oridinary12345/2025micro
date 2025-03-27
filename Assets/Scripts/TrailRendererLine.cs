using UnityEngine;

public class TrailRendererLine : MonoBehaviour
{
	private LineRenderer _line;

	private Transform _toFollowTransform;

	private bool _follow;

	public void Init(Vector3 origin, Transform toFollow)
	{
		_toFollowTransform = toFollow;
		_line = base.gameObject.GetComponent<LineRenderer>();
		_line.useWorldSpace = false;
		_line.positionCount = 2;
		_line.SetPosition(0, origin);
		_line.SetPosition(1, origin);
		StopFollow();
	}

	private void Update()
	{
		if (_follow)
		{
			_line.SetPosition(1, _toFollowTransform.position);
		}
	}

	public void StartFollow()
	{
		_follow = true;
		base.gameObject.SetActive( true);
	}

	public void StopFollow()
	{
		_follow = false;
		base.gameObject.SetActive( false);
	}
}