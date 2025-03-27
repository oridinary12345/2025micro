using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		public bool mAlignmentWasRTL;

		public bool mInitializeAlignment = true;

		static LocalizeTarget_TextMeshPro_UGUI()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI> localizeTargetDesc_Type = new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>();
			localizeTargetDesc_Type.Name = "TextMeshPro UGUI";
			localizeTargetDesc_Type.Priority = 100;
			LocalizationManager.RegisterTarget(localizeTargetDesc_Type);
		}

		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.TextMeshPFont;
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
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((!mTarget) ? null : mTarget.text);
			secondaryTerm = ((!(mTarget.font != null)) ? string.Empty : mTarget.font.name);
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (mTarget.font != secondaryTranslatedObj)
				{
					mTarget.font = secondaryTranslatedObj;
				}
			}
			else
			{
				Material secondaryTranslatedObj2 = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj2 != null && mTarget.fontMaterial != secondaryTranslatedObj2)
				{
					if (!secondaryTranslatedObj2.name.StartsWith(mTarget.font.name, StringComparison.Ordinal))
					{
						secondaryTranslatedObj = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, (!secondaryTranslation.EndsWith(secondaryTranslatedObj2.name, StringComparison.Ordinal)) ? secondaryTranslatedObj2.name : secondaryTranslation);
						if (secondaryTranslatedObj != null)
						{
							mTarget.font = secondaryTranslatedObj;
						}
					}
					mTarget.fontSharedMaterial = secondaryTranslatedObj2;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(mAlignmentWasRTL, mTarget.alignment, out mAlignment_LTR, out mAlignment_RTL);
			}
			else
			{
 TextAlignmentOptions alignRTL; TextAlignmentOptions alignLTR;				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(mAlignmentWasRTL, mTarget.alignment, out alignLTR, out alignRTL);
				if ((mAlignmentWasRTL && mAlignment_RTL != alignRTL) || (!mAlignmentWasRTL && mAlignment_LTR != alignLTR))
				{
					mAlignment_LTR = alignLTR;
					mAlignment_RTL = alignRTL;
				}
				mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation == null || !(mTarget.text != mainTranslation))
			{
				return;
			}
			if (mainTranslation != null && cmp.CorrectAlignmentForRTL)
			{
				mTarget.alignment = ((!LocalizationManager.IsRight2Left) ? mAlignment_LTR : mAlignment_RTL);
				mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
				if (LocalizationManager.IsRight2Left)
				{
					mainTranslation = I2Utils.ReverseText(mainTranslation);
				}
			}
			mTarget.text = mainTranslation;
		}
	}
}