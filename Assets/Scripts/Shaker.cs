using System;
using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
	private int _numberOfShakes = 3;

	private float _startingShakeDistance = 0.8f;

	private float _decreasePercentage = 0.5f;

	private float _shakeSpeed = 55f;

	private Transform _transform;

	private Transform _additionalTransform;

	private void Awake()
	{
		_transform = base.transform;
	}

	public void Init(float shakeDistance)
	{
		_startingShakeDistance = shakeDistance;
	}

	public void SetAdditionalTransform(Transform t)
	{
		_additionalTransform = t;
	}

	public void Shake()
	{
		StartCoroutine(ShakeCR());
	}

	private IEnumerator ShakeCR()
	{
		float hitTime = Time.time;
		Vector3 originalPosition3 = _transform.localPosition;
		Vector3 originalPosition2 = _additionalTransform.localPosition;
		int shake = _numberOfShakes;
		float shakeDistance = _startingShakeDistance;
		while (shake > 0)
		{
			float timer = (Time.time - hitTime) * _shakeSpeed;
			float offset = Mathf.Sin(timer) * shakeDistance;
			float x3 = originalPosition3.x + offset;
			Transform transform = _transform;
			float x4 = x3;
			Vector3 localPosition = _transform.localPosition;
			float y = localPosition.y;
			Vector3 localPosition2 = _transform.localPosition;
			transform.localPosition = new Vector3(x4, y, localPosition2.z);
			float x2 = originalPosition2.x + offset;
			Transform additionalTransform = _additionalTransform;
			float x5 = x2;
			Vector3 localPosition3 = _additionalTransform.localPosition;
			float y2 = localPosition3.y;
			Vector3 localPosition4 = _additionalTransform.localPosition;
			additionalTransform.localPosition = new Vector3(x5, y2, localPosition4.z);
			if (timer > (float)Math.PI * 2f)
			{
				hitTime = Time.time;
				shakeDistance *= _decreasePercentage;
				shake--;
			}
			yield return null;
		}
		Transform transform2 = _transform;
		float x6 = originalPosition3.x;
		Vector3 localPosition5 = _transform.localPosition;
		float y3 = localPosition5.y;
		Vector3 localPosition6 = _transform.localPosition;
		transform2.localPosition = new Vector3(x6, y3, localPosition6.z);
		Transform additionalTransform2 = _additionalTransform;
		float x7 = originalPosition2.x;
		Vector3 localPosition7 = _additionalTransform.localPosition;
		float y4 = localPosition7.y;
		Vector3 localPosition8 = _additionalTransform.localPosition;
		additionalTransform2.localPosition = new Vector3(x7, y4, localPosition8.z);
	}
}