using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
	private float angle = 360f;

	private float time = 0.3f;

	private Vector3 axis = Vector3.forward;

	private void Awake()
	{
	}

	private void Update()
	{
		base.transform.Rotate(axis, angle * Time.deltaTime / time);
	}
}