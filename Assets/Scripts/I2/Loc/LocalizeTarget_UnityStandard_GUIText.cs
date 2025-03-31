using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_GUIText : LocalizeTarget<Text>
	{
		private TextAlignment mAlignment_RTL = TextAlignment.Right;

		private TextAlignment mAlignment_LTR;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;

		static LocalizeTarget_UnityStandard_GUIText()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityStandard_GUIText> localizeTargetDesc_Type = new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityStandard_GUIText>();
			localizeTargetDesc_Type.Name = "Text";
			localizeTargetDesc_Type.Priority = 100;
			LocalizationManager.RegisterTarget(localizeTargetDesc_Type);
		}

		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return true;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = mTarget.text;
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font newFont = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (newFont != null && mTarget.font != newFont)
			{
				mTarget.font = newFont;
			}
			if (mTarget.text != mainTranslation)
			{
				mTarget.text = mainTranslation;
			}
		}
	}
}