using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	public class FadeTransition : TransitionKitDelegate
	{
		public Color fadeToColor = Color.black;

		public float duration = 0.375f;

		public float fadedDelay;

		public string nextScene = string.Empty;

		public Shader shaderForTransition()
		{
			return Shader.Find("prime31/Transitions/Fader");
		}

		public Mesh meshForDisplay()
		{
			return null;
		}

		public Texture2D textureForDisplay()
		{
			return null;
		}

		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.color = fadeToColor;
			if (!string.IsNullOrEmpty(nextScene))
			{
				SceneManager.LoadScene(nextScene);
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(duration));
			transitionKit.makeTextureTransparent();
			if (fadedDelay > 0f)
			{
				yield return new WaitForSeconds(fadedDelay);
			}
			if (!string.IsNullOrEmpty(nextScene))
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(duration, true));
		}
	}
}