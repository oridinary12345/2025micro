using UnityEngine;

public class Follow : MonoBehaviour
{
	private Transform _transform;

	private Transform _target;

	public void Init(Transform target)
	{
		_transform = base.transform;
		_target = target;
	}

	private void Update()
	{
		if (!(_transform == null) && !(_target == null))
		{
			_transform.position = _target.position;
		}
	}
}