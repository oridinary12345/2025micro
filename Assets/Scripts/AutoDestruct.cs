using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
	public void Init(float time)
	{
		Invoke("Destruct", time);
	}

	private void Destruct()
	{
		if (base.gameObject != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}