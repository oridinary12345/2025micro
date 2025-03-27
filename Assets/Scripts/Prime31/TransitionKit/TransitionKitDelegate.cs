using System.Collections;
using UnityEngine;

namespace Prime31.TransitionKit
{
	public interface TransitionKitDelegate
	{
		Shader shaderForTransition();

		Mesh meshForDisplay();

		Texture2D textureForDisplay();

		IEnumerator onScreenObscured(TransitionKit transitionKit);
	}
}