using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
	[SerializeField]
	private Transform _toFollow;

	private Transform _transform;

	private void Awake()
	{
		_transform = base.transform;
	}

	private void Update()
	{
		Transform transform = _transform;
		Vector3 localPosition = _toFollow.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = _toFollow.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = _toFollow.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition3.z + 0.2f);
		_transform.localScale = _toFollow.localScale;
	}
}