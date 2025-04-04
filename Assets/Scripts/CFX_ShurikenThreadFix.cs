using System.Collections;
using UnityEngine;

public class CFX_ShurikenThreadFix : MonoBehaviour
{
	private ParticleSystem[] systems;

	private void OnEnable()
	{
		systems = GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = systems;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Stop( true);
			particleSystem.Clear( true);
		}
		StartCoroutine("WaitFrame");
	}

	private IEnumerator WaitFrame()
	{
		yield return null;
		ParticleSystem[] array = systems;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Play( true);
		}
	}
}