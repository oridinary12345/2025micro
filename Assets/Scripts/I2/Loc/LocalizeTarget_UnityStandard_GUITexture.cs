using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_GUITexture : LocalizeTarget<GUITexture>
	{
		static LocalizeTarget_UnityStandard_GUITexture()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizeTargetDesc_Type<GUITexture, LocalizeTarget_UnityStandard_GUITexture> localizeTargetDesc_Type = new LocalizeTargetDesc_Type<GUITexture, LocalizeTarget_UnityStandard_GUITexture>();
			localizeTargetDesc_Type.Name = "GUITexture";
			localizeTargetDesc_Type.Priority = 100;
			LocalizationManager.RegisterTarget(localizeTargetDesc_Type);
		}

		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((!mTarget.texture) ? string.Empty : mTarget.texture.name);
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture texture = mTarget.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}