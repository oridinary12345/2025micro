using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace I2.Loc
{
	[Serializable]
	public struct LocalizedString
	{
		public string mTerm;

		public bool mRTL_IgnoreArabicFix;

		public int mRTL_MaxLineLength;

		public bool mRTL_ConvertNumbers;

		public LocalizedString(string term)
		{
			mTerm = term;
			mRTL_IgnoreArabicFix = false;
			mRTL_MaxLineLength = 0;
			mRTL_ConvertNumbers = false;
		}

		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		public static implicit operator LocalizedString(string term)
		{
			LocalizedString result = default(LocalizedString);
			result.mTerm = term;
			return result;
		}

		public override string ToString()
		{
			string translation = LocalizationHelper.GetTranslation(mTerm);
			// 不再使用 LocalizationManager.ApplyLocalizationParams
			return translation;
		}
	}
}