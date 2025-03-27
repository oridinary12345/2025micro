using System;
using UnityEngine;

public class RewardRigidbody : MonoBehaviour
{
	public Vector2 Velocity;

	private bool UseGravity;

	private Transform _target;

	private Action _onDestinationReached;

	private float _pullForce;

	private void Awake()
	{
	}

	public void SetTarget(Transform target, float pullForce, Action onDestinationReached)
	{
		_target = target;
		_pullForce = pullForce;
		_onDestinationReached = onDestinationReached;
		UseGravity = false;
	}

	private void Update()
	{
		if (_target != null)
		{
			Vector3 position = _target.position;
			float x = position.x;
			Vector3 position2 = _target.position;
			Vector3 a = new Vector2(x, position2.y);
			Vector3 position3 = base.transform.position;
			float x2 = position3.x;
			Vector3 position4 = base.transform.position;
			float num = Vector3.Distance(a, new Vector2(x2, position4.y));
			UpdateVelocity(num);
			float num2 = num - Velocity.magnitude * Time.unscaledDeltaTime;
			if (num2 <= 0.01f && _onDestinationReached != null)
			{
				_onDestinationReached();
			}
		}
		UpdatePosition();
	}

	private void UpdateVelocity(float distance)
	{
		Vector2 vector = _target.position - base.transform.position;
		if (distance < 4f || Velocity.magnitude > _pullForce)
		{
			Velocity = vector.normalized * _pullForce;
		}
		else
		{
			Velocity += vector.normalized * _pullForce * Time.unscaledDeltaTime;
		}
	}

	private void UpdatePosition()
	{
		Vector2 a = Velocity * Time.unscaledDeltaTime;
		if (UseGravity)
		{
			a += new Vector2(0f, -9.81f * Time.unscaledDeltaTime);
		}
		base.transform.position += new Vector3(a.x, a.y, 0f);
	}
}