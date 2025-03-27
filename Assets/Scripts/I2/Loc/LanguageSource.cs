using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
    [AddComponentMenu("I2/Localization/Source")]
    [ExecuteInEditMode]
    public class LanguageSource : MonoBehaviour
    {
        public enum eGoogleUpdateFrequency
        {
            Always,
            Never,
            Daily,
            Weekly,
            Monthly,
            OnlyOnce
        }

        public enum eAllowUnloadLanguages
        {
            Never,
            OnlyInDevice,
            EditorAndDevice
        }

        public enum MissingTranslationAction
        {
            Empty,
            Fallback,
            ShowWarning
        }

        public bool NeverDestroy;

        public bool UserAgreesToHaveItOnTheScene;

        public bool UserAgreesToHaveItInsideThePluginsFolder;

        public bool GoogleLiveSyncIsUptoDate = true;

        [NonSerialized]
        public bool mIsGlobalSource;

        public List<UnityEngine.Object> Assets = new List<UnityEngine.Object>();

        [NonSerialized]
        public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);

        public string Google_WebServiceURL;

        public string Google_SpreadsheetKey;

        public string Google_SpreadsheetName;

        public string Google_LastUpdatedVersion;

        public eGoogleUpdateFrequency GoogleUpdateFrequency = eGoogleUpdateFrequency.Weekly;

        public eGoogleUpdateFrequency GoogleInEditorCheckFrequency = eGoogleUpdateFrequency.Daily;

        public float GoogleUpdateDelay = 5f;

        public List<LanguageData> mLanguages = new List<LanguageData>();

        public bool IgnoreDeviceLanguage;

        public eAllowUnloadLanguages _AllowUnloadingLanguages = eAllowUnloadLanguages.OnlyInDevice;

        public static string EmptyCategory = "Default";

        public static char[] CategorySeparators = "/\\".ToCharArray();

        public List<TermData> mTerms = new List<TermData>();

        public bool CaseInsensitiveTerms;

        [NonSerialized]
        public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

        public MissingTranslationAction OnMissingTranslation = MissingTranslationAction.Fallback;

        public string mTerm_AppName;

        public event Action<LanguageSource, bool, string> Event_OnSourceUpdateFromGoogle;

        private void Awake()
        {
            NeverDestroy = false;
            if (NeverDestroy)
            {
                if (ManagerHasASimilarSource())
                {
                    UnityEngine.Object.Destroy(this);
                    return;
                }
                if (Application.isPlaying)
                {
                    UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
                }
            }
            LocalizationManager.AddSource(this);
            UpdateDictionary();
            UpdateAssetDictionary();
            LocalizationManager.LocalizeAll(true);
        }

        private void OnDestroy()
        {
            NeverDestroy = false;
            if (!NeverDestroy)
            {
                LocalizationManager.RemoveSource(this);
            }
        }

        public string GetSourceName()
        {
            string text = base.gameObject.name;
            Transform parent = base.transform.parent;
            while ((bool)parent)
            {
                text = parent.name + "_" + text;
                parent = parent.parent;
            }
            return text;
        }

        public bool IsEqualTo(LanguageSource Source)
        {
            if (Source.mLanguages.Count != mLanguages.Count)
            {
                return false;
            }
            int i = 0;
            for (int count = mLanguages.Count; i < count; i++)
            {
                if (Source.GetLanguageIndex(mLanguages[i].Name) < 0)
                {
                    return false;
                }
            }
            if (Source.mTerms.Count != mTerms.Count)
            {
                return false;
            }
            for (int j = 0; j < mTerms.Count; j++)
            {
                if (Source.GetTermData(mTerms[j].Term) == null)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool ManagerHasASimilarSource()
        {
            int i = 0;
            for (int count = LocalizationManager.Sources.Count; i < count; i++)
            {
                LanguageSource languageSource = LocalizationManager.Sources[i];
                if (languageSource != null && languageSource.IsEqualTo(this) && languageSource != this)
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearAllData()
        {
            mTerms.Clear();
            mLanguages.Clear();
            mDictionary.Clear();
            mAssetDictionary.Clear();
        }

        public bool IsGlobalSource()
        {
            return mIsGlobalSource;
        }

        public void UpdateAssetDictionary()
        {
            Assets.RemoveAll((UnityEngine.Object x) => x == null);
            mAssetDictionary = Assets.Distinct().ToDictionary((UnityEngine.Object o) => o.name);
        }

        public UnityEngine.Object FindAsset(string Name)
        {
            if (Assets != null)
            {
                if (mAssetDictionary == null || mAssetDictionary.Count != Assets.Count)
                {
                    UpdateAssetDictionary();
                }
                UnityEngine.Object value; if (mAssetDictionary.TryGetValue(Name, out value))
                {
                    return value;
                }
            }
            return null;
        }

        public bool HasAsset(UnityEngine.Object Obj)
        {
            return Assets.Contains(Obj);
        }

        public void AddAsset(UnityEngine.Object Obj)
        {
            if (!Assets.Contains(Obj))
            {
                Assets.Add(Obj);
                UpdateAssetDictionary();
            }
        }

        public string Export_I2CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Key[*]Type[*]Desc");
            foreach (LanguageData mLanguage in mLanguages)
            {
                stringBuilder.Append("[*]");
                if (!mLanguage.IsEnabled())
                {
                    stringBuilder.Append('$');
                }
                stringBuilder.Append(GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code));
            }
            stringBuilder.Append("[ln]");
            int count = mLanguages.Count;
            bool flag = true;
            foreach (TermData mTerm in mTerms)
            {
                string term;
                if (string.IsNullOrEmpty(Category) || (Category == EmptyCategory && mTerm.Term.IndexOfAny(CategorySeparators) < 0))
                {
                    term = mTerm.Term;
                }
                else
                {
                    if (!mTerm.Term.StartsWith(Category + "/") || !(Category != mTerm.Term))
                    {
                        continue;
                    }
                    term = mTerm.Term.Substring(Category.Length + 1);
                }
                if (!flag)
                {
                    stringBuilder.Append("[ln]");
                }
                flag = false;
                if (!specializationsAsRows)
                {
                    AppendI2Term(stringBuilder, count, term, mTerm, Separator, null);
                }
                else
                {
                    List<string> allSpecializations = mTerm.GetAllSpecializations();
                    for (int i = 0; i < allSpecializations.Count; i++)
                    {
                        if (i != 0)
                        {
                            stringBuilder.Append("[ln]");
                        }
                        string forceSpecialization = allSpecializations[i];
                        AppendI2Term(stringBuilder, count, term, mTerm, Separator, forceSpecialization);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        private static void AppendI2Term(StringBuilder Builder, int nLanguages, string Term, TermData termData, char Separator, string forceSpecialization)
        {
            AppendI2Text(Builder, Term);
            if (!string.IsNullOrEmpty(forceSpecialization) && forceSpecialization != "Any")
            {
                Builder.Append("[");
                Builder.Append(forceSpecialization);
                Builder.Append("]");
            }
            Builder.Append("[*]");
            Builder.Append(termData.TermType.ToString());
            Builder.Append("[*]");
            Builder.Append(termData.Description);
            for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
            {
                Builder.Append("[*]");
                string text = termData.Languages[i];
                if (!string.IsNullOrEmpty(forceSpecialization))
                {
                    text = termData.GetTranslation(i, forceSpecialization);
                }
                AppendI2Text(Builder, text);
            }
        }

        private static void AppendI2Text(StringBuilder Builder, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith("'") || text.StartsWith("="))
                {
                    Builder.Append('\'');
                }
                Builder.Append(text);
            }
        }

        private string Export_Language_to_Cache(int langIndex, bool fillTermWithFallback)
        {
            if (!mLanguages[langIndex].IsLoaded())
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < mTerms.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append("[i2t]");
                }
                TermData termData = mTerms[i];
                stringBuilder.Append(termData.Term);
                stringBuilder.Append("=");
                string Translation = termData.Languages[langIndex];
                if (OnMissingTranslation == MissingTranslationAction.Fallback && string.IsNullOrEmpty(Translation) && TryGetFallbackTranslation(termData, out Translation, langIndex, null, true))
                {
                    stringBuilder.Append("[i2fb]");
                    if (fillTermWithFallback)
                    {
                        termData.Languages[langIndex] = Translation;
                    }
                }
                if (!string.IsNullOrEmpty(Translation))
                {
                    stringBuilder.Append(Translation);
                }
            }
            return stringBuilder.ToString();
        }

        public string Export_CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int count = mLanguages.Count;
            stringBuilder.AppendFormat("Key{0}Type{0}Desc", Separator);
            foreach (LanguageData mLanguage in mLanguages)
            {
                stringBuilder.Append(Separator);
                if (!mLanguage.IsEnabled())
                {
                    stringBuilder.Append('$');
                }
                AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code), Separator);
            }
            stringBuilder.Append("\n");
            mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
            foreach (TermData mTerm in mTerms)
            {
                string term;
                if (string.IsNullOrEmpty(Category) || (Category == EmptyCategory && mTerm.Term.IndexOfAny(CategorySeparators) < 0))
                {
                    term = mTerm.Term;
                }
                else
                {
                    if (!mTerm.Term.StartsWith(Category + "/") || !(Category != mTerm.Term))
                    {
                        continue;
                    }
                    term = mTerm.Term.Substring(Category.Length + 1);
                }
                if (specializationsAsRows)
                {
                    foreach (string allSpecialization in mTerm.GetAllSpecializations())
                    {
                        AppendTerm(stringBuilder, count, term, mTerm, allSpecialization, Separator);
                    }
                }
                else
                {
                    AppendTerm(stringBuilder, count, term, mTerm, null, Separator);
                }
            }
            return stringBuilder.ToString();
        }

        private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string specialization, char Separator)
        {
            AppendString(Builder, Term, Separator);
            if (!string.IsNullOrEmpty(specialization) && specialization != "Any")
            {
                Builder.AppendFormat("[{0}]", specialization);
            }
            Builder.Append(Separator);
            Builder.Append(termData.TermType.ToString());
            Builder.Append(Separator);
            AppendString(Builder, termData.Description, Separator);
            for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
            {
                Builder.Append(Separator);
                string text = termData.Languages[i];
                if (!string.IsNullOrEmpty(specialization))
                {
                    text = termData.GetTranslation(i, specialization);
                }
                AppendTranslation(Builder, text, Separator, null);
            }
            Builder.Append("\n");
        }

        private static void AppendString(StringBuilder Builder, string Text, char Separator)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                Text = Text.Replace("\\n", "\n");
                if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
                {
                    Text = Text.Replace("\"", "\"\"");
                    Builder.AppendFormat("\"{0}\"", Text);
                }
                else
                {
                    Builder.Append(Text);
                }
            }
        }

        private static void AppendTranslation(StringBuilder Builder, string Text, char Separator, string tags)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                Text = Text.Replace("\\n", "\n");
                if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
                {
                    Text = Text.Replace("\"", "\"\"");
                    Builder.AppendFormat("\"{0}{1}\"", tags, Text);
                }
                else
                {
                    Builder.Append(tags);
                    Builder.Append(Text);
                }
            }
        }

        public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
        {
            string value = Export_Google_CreateData();
            WWWForm wWWForm = new WWWForm();
            wWWForm.AddField("key", Google_SpreadsheetKey);
            wWWForm.AddField("action", "SetLanguageSource");
            wWWForm.AddField("data", value);
            wWWForm.AddField("updateMode", UpdateMode.ToString());
            return new WWW(LocalizationManager.GetWebServiceURL(this), wWWForm);
        }

        private string Export_Google_CreateData()
        {
            List<string> categories = GetCategories(true);
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = true;
            foreach (string item in categories)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    stringBuilder.Append("<I2Loc>");
                }
                bool flag2 = true;
                string category = item;
                bool specializationsAsRows = flag2;
                string value = Export_I2CSV(category, ',', specializationsAsRows);
                stringBuilder.Append(item);
                stringBuilder.Append("<I2Loc>");
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }

        public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
        {
            List<string[]> cSV = LocalizationReader.ReadCSV(CSVstring, Separator);
            return Import_CSV(Category, cSV, UpdateMode);
        }

        public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
        {
            List<string[]> cSV = LocalizationReader.ReadI2CSV(I2CSVstring);
            return Import_CSV(Category, cSV, UpdateMode);
        }

        public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
        {
            string[] array = CSV[0];
            int num = 1;
            int num2 = -1;
            int num3 = -1;
            string[] texts = new string[1]
            {
                "Key"
            };
            string[] texts2 = new string[1]
            {
                "Type"
            };
            string[] texts3 = new string[2]
            {
                "Desc",
                "Description"
            };
            if (array.Length > 1 && ArrayContains(array[0], texts))
            {
                if (UpdateMode == eSpreadsheetUpdateMode.Replace)
                {
                    ClearAllData();
                }
                if (array.Length > 2)
                {
                    if (ArrayContains(array[1], texts2))
                    {
                        num2 = 1;
                        num = 2;
                    }
                    if (ArrayContains(array[1], texts3))
                    {
                        num3 = 1;
                        num = 2;
                    }
                }
                if (array.Length > 3)
                {
                    if (ArrayContains(array[2], texts2))
                    {
                        num2 = 2;
                        num = 3;
                    }
                    if (ArrayContains(array[2], texts3))
                    {
                        num3 = 2;
                        num = 3;
                    }
                }
                int num4 = Mathf.Max(array.Length - num, 0);
                int[] array2 = new int[num4];
                for (int i = 0; i < num4; i++)
                {
                    if (string.IsNullOrEmpty(array[i + num]))
                    {
                        array2[i] = -1;
                        continue;
                    }
                    string text = array[i + num];
                    bool flag = true;
                    if (text.StartsWith("$"))
                    {
                        flag = false;
                        text = text.Substring(1);
                    }
                    string code; string Language; GoogleLanguages.UnPackCodeFromLanguageName(text, out Language, out code);
                    int num5 = -1;
                    num5 = (string.IsNullOrEmpty(code) ? GetLanguageIndex(Language, true, false) : GetLanguageIndexFromCode(code));
                    if (num5 < 0)
                    {
                        LanguageData languageData = new LanguageData();
                        languageData.Name = Language;
                        languageData.Code = code;
                        languageData.Flags = (byte)(0 | ((!flag) ? 1 : 0));
                        mLanguages.Add(languageData);
                        num5 = mLanguages.Count - 1;
                    }
                    array2[i] = num5;
                }
                num4 = mLanguages.Count;
                int j = 0;
                for (int count = mTerms.Count; j < count; j++)
                {
                    TermData termData = mTerms[j];
                    if (termData.Languages.Length < num4)
                    {
                        Array.Resize(ref termData.Languages, num4);
                        Array.Resize(ref termData.Flags, num4);
                    }
                }
                int k = 1;
                for (int count2 = CSV.Count; k < count2; k++)
                {
                    array = CSV[k];
                    string Term = (!string.IsNullOrEmpty(Category)) ? (Category + "/" + array[0]) : array[0];
                    string text2 = null;
                    if (Term.EndsWith("]"))
                    {
                        int num6 = Term.LastIndexOf('[');
                        if (num6 > 0)
                        {
                            text2 = Term.Substring(num6 + 1, Term.Length - num6 - 2);
                            if (text2 == "touch")
                            {
                                text2 = "Touch";
                            }
                            Term = Term.Remove(num6);
                        }
                    }
                    ValidateFullTerm(ref Term);
                    if (string.IsNullOrEmpty(Term))
                    {
                        continue;
                    }
                    TermData termData2 = GetTermData(Term);
                    if (termData2 == null)
                    {
                        termData2 = new TermData();
                        termData2.Term = Term;
                        termData2.Languages = new string[mLanguages.Count];
                        termData2.Flags = new byte[mLanguages.Count];
                        for (int l = 0; l < mLanguages.Count; l++)
                        {
                            termData2.Languages[l] = string.Empty;
                        }
                        mTerms.Add(termData2);
                        mDictionary.Add(Term, termData2);
                    }
                    else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
                    {
                        continue;
                    }
                    if (num2 > 0)
                    {
                        termData2.TermType = GetTermType(array[num2]);
                    }
                    if (num3 > 0)
                    {
                        termData2.Description = array[num3];
                    }
                    for (int m = 0; m < array2.Length && m < array.Length - num; m++)
                    {
                        if (string.IsNullOrEmpty(array[m + num]))
                        {
                            continue;
                        }
                        int num7 = array2[m];
                        if (num7 >= 0)
                        {
                            string text3 = array[m + num];
                            if (text3 == "-")
                            {
                                text3 = string.Empty;
                            }
                            else if (text3 == string.Empty)
                            {
                                text3 = null;
                            }
                            termData2.SetTranslation(num7, text3, text2);
                        }
                    }
                }
                if (Application.isPlaying)
                {
                    SaveLanguages(HasUnloadedLanguages());
                }
                return string.Empty;
            }
            return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
        }

        private bool ArrayContains(string MainText, params string[] texts)
        {
            int i = 0;
            for (int num = texts.Length; i < num; i++)
            {
                if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static eTermType GetTermType(string type)
        {
            int i = 0;
            for (int num = 9; i <= num; i++)
            {
                eTermType eTermType = (eTermType)i;
                if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
                {
                    return (eTermType)i;
                }
            }
            return eTermType.Text;
        }

        private void Import_Language_from_Cache(int langIndex, string langData, bool useFallback, bool onlyCurrentSpecialization)
        {
            int num2;
            for (int num = 0; num < langData.Length; num = num2 + 5)
            {
                num2 = langData.IndexOf("[i2t]", num);
                if (num2 < 0)
                {
                    num2 = langData.Length;
                }
                int num3 = langData.IndexOf("=", num);
                if (num3 >= num2)
                {
                    break;
                }
                string term = langData.Substring(num, num3 - num);
                num = num3 + 1;
                TermData termData = GetTermData(term);
                if (termData != null)
                {
                    string text = null;
                    if (num != num2)
                    {
                        text = langData.Substring(num, num2 - num);
                        if (text.StartsWith("[i2fb]"))
                        {
                            text = ((!useFallback) ? null : text.Substring(6));
                        }
                        if (onlyCurrentSpecialization && text != null)
                        {
                            text = SpecializationManager.GetSpecializedText(text);
                        }
                    }
                    termData.Languages[langIndex] = text;
                }
            }
        }

        public static void FreeUnusedLanguages()
        {
            LanguageSource languageSource = LocalizationManager.Sources[0];
            int languageIndex = languageSource.GetLanguageIndex(LocalizationManager.CurrentLanguage);
            for (int i = 0; i < languageSource.mTerms.Count; i++)
            {
                TermData termData = languageSource.mTerms[i];
                for (int j = 0; j < termData.Languages.Length; j++)
                {
                    if (j != languageIndex)
                    {
                        termData.Languages[j] = null;
                    }
                }
            }
        }

        public void Import_Google_FromCache()
        {
            if (GoogleUpdateFrequency == eGoogleUpdateFrequency.Never || !I2Utils.IsPlaying())
            {
                return;
            }
            string sourcePlayerPrefName = GetSourcePlayerPrefName();
            string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (text.StartsWith("[i2e]", StringComparison.Ordinal))
            {
                text = StringObfucator.Decode(text.Substring(5, text.Length - 5));
            }
            bool flag = false;
            string text2 = Google_LastUpdatedVersion;
            if (PersistentStorage.HasSetting("I2SourceVersion_" + sourcePlayerPrefName))
            {
                text2 = PersistentStorage.GetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, Google_LastUpdatedVersion);
                flag = IsNewerVersion(Google_LastUpdatedVersion, text2);
            }
            if (!flag)
            {
                PersistentStorage.DeleteFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
                PersistentStorage.DeleteSetting("I2SourceVersion_" + sourcePlayerPrefName);
                return;
            }
            if (text2.Length > 19)
            {
                text2 = string.Empty;
            }
            Google_LastUpdatedVersion = text2;
            Import_Google_Result(text, eSpreadsheetUpdateMode.Replace);
        }


        private bool IsNewerVersion(string currentVersion, string newVersion)
        {
            long num;
            long num2;
            return !string.IsNullOrEmpty(newVersion) && (string.IsNullOrEmpty(currentVersion) || (!long.TryParse(newVersion, out num) || !long.TryParse(currentVersion, out num2)) || num > num2);
        }
        public void Import_Google(bool ForceUpdate, bool justCheck)
        {
            if ((ForceUpdate || GoogleUpdateFrequency != eGoogleUpdateFrequency.Never) && I2Utils.IsPlaying())
            {
                eGoogleUpdateFrequency googleUpdateFrequency = GoogleUpdateFrequency;
                string sourcePlayerPrefName = GetSourcePlayerPrefName();
                if (!ForceUpdate && googleUpdateFrequency != 0)
                {
                    string setting_String = PersistentStorage.GetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, string.Empty);
                    try
                    {
                        DateTime result; if (DateTime.TryParse(setting_String, out result))
                        {
                            double totalDays = (DateTime.Now - result).TotalDays;
                            switch (googleUpdateFrequency)
                            {
                                case eGoogleUpdateFrequency.OnlyOnce:
                                    return;
                                case eGoogleUpdateFrequency.Daily:
                                    if (totalDays < 1.0)
                                    {
                                        return;
                                    }
                                    break;
                                case eGoogleUpdateFrequency.Weekly:
                                    if (totalDays < 8.0)
                                    {
                                        return;
                                    }
                                    break;
                                case eGoogleUpdateFrequency.Monthly:
                                    if (totalDays < 31.0)
                                    {
                                        return;
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                PersistentStorage.SetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
                CoroutineManager.Start(Import_Google_Coroutine(justCheck));
            }
        }

        private string GetSourcePlayerPrefName()
        {
            string text = base.name;
            if (!string.IsNullOrEmpty(Google_SpreadsheetKey))
            {
                text += Google_SpreadsheetKey;
            }
            if (Array.IndexOf(LocalizationManager.GlobalSources, base.name) >= 0)
            {
                return text;
            }
            return SceneManager.GetActiveScene().name + "_" + text;
        }

        private IEnumerator Import_Google_Coroutine(bool JustCheck)
        {
            WWW www = Import_Google_CreateWWWcall(false, JustCheck);
            if (www == null)
            {
                yield break;
            }
            while (!www.isDone)
            {
                yield return null;
            }
            if (string.IsNullOrEmpty(www.error))
            {
                byte[] bytes = www.bytes;
                string wwwText = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                bool flag = string.IsNullOrEmpty(wwwText) || wwwText == "\"\"";
                if (JustCheck)
                {
                    if (!flag)
                    {
                        UnityEngine.Debug.LogWarning("Spreadsheet is not up-to-date and Google Live Synchronization is enabled\nWhen playing in the device the Spreadsheet will be downloaded and translations may not behave as what you see in the editor.\nTo fix this, Import or Export replace to Google");
                        GoogleLiveSyncIsUptoDate = false;
                    }
                    yield break;
                }
                if (!flag)
                {
                    string value = Import_Google_Result(wwwText, eSpreadsheetUpdateMode.Replace, true);
                    if (string.IsNullOrEmpty(value))
                    {
                        if (this.Event_OnSourceUpdateFromGoogle != null)
                        {
                            this.Event_OnSourceUpdateFromGoogle(this, true, www.error);
                        }
                        LocalizationManager.LocalizeAll(true);
                        UnityEngine.Debug.Log("Done Google Sync");
                    }
                    else
                    {
                        if (this.Event_OnSourceUpdateFromGoogle != null)
                        {
                            this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
                        }
                        UnityEngine.Debug.Log("Done Google Sync: source was up-to-date");
                    }
                    yield break;
                }
            }
            if (this.Event_OnSourceUpdateFromGoogle != null)
            {
                this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
            }
            UnityEngine.Debug.Log("Language Source was up-to-date with Google Spreadsheet");
        }

        public WWW Import_Google_CreateWWWcall(bool ForceUpdate, bool justCheck)
        {
            if (!HasGoogleSpreadsheet())
            {
                return null;
            }
            string text = PersistentStorage.GetSetting_String("I2SourceVersion_" + GetSourcePlayerPrefName(), Google_LastUpdatedVersion);
            if (text.Length > 19)
            {
                text = string.Empty;
            }
            if (IsNewerVersion(text, Google_LastUpdatedVersion))
            {
                Google_LastUpdatedVersion = text;
            }
            string url = string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", LocalizationManager.GetWebServiceURL(this), Google_SpreadsheetKey, (!ForceUpdate) ? Google_LastUpdatedVersion : "0");
            return new WWW(url);
        }

        public bool HasGoogleSpreadsheet()
        {
            return !string.IsNullOrEmpty(Google_WebServiceURL) && !string.IsNullOrEmpty(Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
        }

        public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode, bool saveInPlayerPrefs = false)
        {
            try
            {
                string empty = string.Empty;
                if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
                {
                    return empty;
                }
                int num = JsonString.IndexOf("version=", StringComparison.Ordinal);
                int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
                if (num < 0 || num2 < 0)
                {
                    return "Invalid Response from Google, Most likely the WebService needs to be updated";
                }
                num += "version=".Length;
                num2 += "script_version=".Length;
                string text = JsonString.Substring(num, JsonString.IndexOf(",", num, StringComparison.Ordinal) - num);
                int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2, StringComparison.Ordinal) - num2));
                if (text.Length > 19)
                {
                    text = string.Empty;
                }
                if (num3 != LocalizationManager.GetRequiredWebServiceVersion())
                {
                    return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
                }
                if (saveInPlayerPrefs && !IsNewerVersion(Google_LastUpdatedVersion, text))
                {
                    return "LanguageSource is up-to-date";
                }
                if (saveInPlayerPrefs)
                {
                    string sourcePlayerPrefName = GetSourcePlayerPrefName();
                    PersistentStorage.SaveFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName, "[i2e]" + StringObfucator.Encode(JsonString) + ".loc");
                    PersistentStorage.SetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, text);
                    PersistentStorage.ForceSaveSettings();
                }
                Google_LastUpdatedVersion = text;
                if (UpdateMode == eSpreadsheetUpdateMode.Replace)
                {
                    ClearAllData();
                }
                int num4 = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
                while (num4 > 0)
                {
                    num4 += "[i2category]".Length;
                    int num5 = JsonString.IndexOf("[/i2category]", num4, StringComparison.Ordinal);
                    string category = JsonString.Substring(num4, num5 - num4);
                    num5 += "[/i2category]".Length;
                    int num6 = JsonString.IndexOf("[/i2csv]", num5, StringComparison.Ordinal);
                    string i2CSVstring = JsonString.Substring(num5, num6 - num5);
                    num4 = JsonString.IndexOf("[i2category]", num6, StringComparison.Ordinal);
                    Import_I2CSV(category, i2CSVstring, UpdateMode);
                    if (UpdateMode == eSpreadsheetUpdateMode.Replace)
                    {
                        UpdateMode = eSpreadsheetUpdateMode.Merge;
                    }
                }
                GoogleLiveSyncIsUptoDate = true;
                if (I2Utils.IsPlaying())
                {
                    SaveLanguages(true);
                }
                return empty;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex);
                return ex.ToString();
            }
        }

        public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
        {
            int i = 0;
            for (int count = mLanguages.Count; i < count; i++)
            {
                if ((!SkipDisabled || mLanguages[i].IsEnabled()) && string.Compare(mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return i;
                }
            }
            if (AllowDiscartingRegion)
            {
                int num = -1;
                int num2 = 0;
                int j = 0;
                for (int count2 = mLanguages.Count; j < count2; j++)
                {
                    if (!SkipDisabled || mLanguages[j].IsEnabled())
                    {
                        int commonWordInLanguageNames = GetCommonWordInLanguageNames(mLanguages[j].Name, language);
                        if (commonWordInLanguageNames > num2)
                        {
                            num2 = commonWordInLanguageNames;
                            num = j;
                        }
                    }
                }
                if (num >= 0)
                {
                    return num;
                }
            }
            return -1;
        }

        public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
        {
            int languageIndex = GetLanguageIndex(language, AllowDiscartingRegion, false);
            return (languageIndex >= 0) ? mLanguages[languageIndex] : null;
        }

        public bool IsCurrentLanguage(int languageIndex)
        {
            return LocalizationManager.CurrentLanguage == mLanguages[languageIndex].Name;
        }

        public int GetLanguageIndexFromCode(string Code, bool exactMatch = true)
        {
            int i = 0;
            for (int count = mLanguages.Count; i < count; i++)
            {
                if (string.Compare(mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return i;
                }
            }
            if (!exactMatch)
            {
                int j = 0;
                for (int count2 = mLanguages.Count; j < count2; j++)
                {
                    if (string.Compare(mLanguages[j].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return j;
                    }
                }
            }
            return -1;
        }

        public static int GetCommonWordInLanguageNames(string Language1, string Language2)
        {
            if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
            {
                return 0;
            }
            char[] separator = "( )-/\\".ToCharArray();
            string[] array = Language1.ToLower().Split(separator);
            string[] array2 = Language2.ToLower().Split(separator);
            int num = 0;
            string[] array3 = array;
            foreach (string value in array3)
            {
                if (!string.IsNullOrEmpty(value) && array2.Contains(value))
                {
                    num++;
                }
            }
            string[] array4 = array2;
            foreach (string value2 in array4)
            {
                if (!string.IsNullOrEmpty(value2) && array.Contains(value2))
                {
                    num++;
                }
            }
            return num;
        }

        public static bool AreTheSameLanguage(string Language1, string Language2)
        {
            Language1 = GetLanguageWithoutRegion(Language1);
            Language2 = GetLanguageWithoutRegion(Language2);
            return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string GetLanguageWithoutRegion(string Language)
        {
            int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
            if (num < 0)
            {
                return Language;
            }
            return Language.Substring(0, num).Trim();
        }

        public void AddLanguage(string LanguageName)
        {
            AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName));
        }

        public void AddLanguage(string LanguageName, string LanguageCode)
        {
            if (GetLanguageIndex(LanguageName, false) < 0)
            {
                LanguageData languageData = new LanguageData();
                languageData.Name = LanguageName;
                languageData.Code = LanguageCode;
                mLanguages.Add(languageData);
                int count = mLanguages.Count;
                int i = 0;
                for (int count2 = mTerms.Count; i < count2; i++)
                {
                    Array.Resize(ref mTerms[i].Languages, count);
                    Array.Resize(ref mTerms[i].Flags, count);
                }
            }
        }

        public void RemoveLanguage(string LanguageName)
        {
            int languageIndex = GetLanguageIndex(LanguageName, false, false);
            if (languageIndex < 0)
            {
                return;
            }
            int count = mLanguages.Count;
            int i = 0;
            for (int count2 = mTerms.Count; i < count2; i++)
            {
                for (int j = languageIndex + 1; j < count; j++)
                {
                    mTerms[i].Languages[j - 1] = mTerms[i].Languages[j];
                    mTerms[i].Flags[j - 1] = mTerms[i].Flags[j];
                }
                Array.Resize(ref mTerms[i].Languages, count - 1);
                Array.Resize(ref mTerms[i].Flags, count - 1);
            }
            mLanguages.RemoveAt(languageIndex);
        }

        public List<string> GetLanguages(bool skipDisabled = true)
        {
            List<string> list = new List<string>();
            int i = 0;
            for (int count = mLanguages.Count; i < count; i++)
            {
                if (!skipDisabled || mLanguages[i].IsEnabled())
                {
                    list.Add(mLanguages[i].Name);
                }
            }
            return list;
        }

        public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
        {
            List<string> list = new List<string>();
            int i = 0;
            for (int count = mLanguages.Count; i < count; i++)
            {
                if (!skipDisabled || mLanguages[i].IsEnabled())
                {
                    string text = mLanguages[i].Code;
                    if (!allowRegions && text != null && text.Length > 2)
                    {
                        text = text.Substring(0, 2);
                    }
                    if (!string.IsNullOrEmpty(text) && !list.Contains(text))
                    {
                        list.Add(text);
                    }
                }
            }
            return list;
        }

        public bool IsLanguageEnabled(string Language)
        {
            int languageIndex = GetLanguageIndex(Language, false);
            return languageIndex >= 0 && mLanguages[languageIndex].IsEnabled();
        }

        public void EnableLanguage(string Language, bool bEnabled)
        {
            int languageIndex = GetLanguageIndex(Language, false);
            if (languageIndex >= 0)
            {
                mLanguages[languageIndex].SetEnabled(bEnabled);
            }
        }

        public bool AllowUnloadingLanguages()
        {
            return _AllowUnloadingLanguages != eAllowUnloadLanguages.Never;
        }

        private string GetSavedLanguageFileName(int languageIndex)
        {
            if (languageIndex < 0)
            {
                return null;
            }
            return "LangSource_" + GetSourcePlayerPrefName() + "_" + mLanguages[languageIndex].Name + ".loc";
        }

        public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages, bool useFallback, bool onlyCurrentSpecialization, bool forceLoad)
        {
            if (!AllowUnloadingLanguages() || !PersistentStorage.CanAccessFiles())
            {
                return;
            }
            if (languageIndex >= 0 && (forceLoad || !mLanguages[languageIndex].IsLoaded()))
            {
                string savedLanguageFileName = GetSavedLanguageFileName(languageIndex);
                string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Temporal, savedLanguageFileName, false);
                if (!string.IsNullOrEmpty(text))
                {
                    Import_Language_from_Cache(languageIndex, text, useFallback, onlyCurrentSpecialization);
                    mLanguages[languageIndex].SetLoaded(true);
                }
            }
            if (!UnloadOtherLanguages || !I2Utils.IsPlaying())
            {
                return;
            }
            for (int i = 0; i < mLanguages.Count; i++)
            {
                if (i != languageIndex)
                {
                    UnloadLanguage(i);
                }
            }
        }

        public void LoadAllLanguages(bool forceLoad = false)
        {
            for (int i = 0; i < mLanguages.Count; i++)
            {
                LoadLanguage(i, false, false, false, forceLoad);
            }
        }

        public void UnloadLanguage(int languageIndex)
        {
            if (AllowUnloadingLanguages() && PersistentStorage.CanAccessFiles() && I2Utils.IsPlaying() && mLanguages[languageIndex].IsLoaded() && mLanguages[languageIndex].CanBeUnloaded() && !IsCurrentLanguage(languageIndex) && PersistentStorage.HasFile(PersistentStorage.eFileType.Temporal, GetSavedLanguageFileName(languageIndex)))
            {
                foreach (TermData mTerm in mTerms)
                {
                    mTerm.Languages[languageIndex] = null;
                }
                mLanguages[languageIndex].SetLoaded(false);
            }
        }

        public void SaveLanguages(bool unloadAll, PersistentStorage.eFileType fileLocation = PersistentStorage.eFileType.Temporal)
        {
            if (!AllowUnloadingLanguages() || !PersistentStorage.CanAccessFiles())
            {
                return;
            }
            for (int i = 0; i < mLanguages.Count; i++)
            {
                string text = Export_Language_to_Cache(i, IsCurrentLanguage(i));
                if (!string.IsNullOrEmpty(text))
                {
                    PersistentStorage.SaveFile(PersistentStorage.eFileType.Temporal, GetSavedLanguageFileName(i), text);
                }
            }
            if (!unloadAll)
            {
                return;
            }
            for (int j = 0; j < mLanguages.Count; j++)
            {
                if (unloadAll && !IsCurrentLanguage(j))
                {
                    UnloadLanguage(j);
                }
            }
        }

        public bool HasUnloadedLanguages()
        {
            for (int i = 0; i < mLanguages.Count; i++)
            {
                if (!mLanguages[i].IsLoaded())
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
        {
            if (Categories == null)
            {
                Categories = new List<string>();
            }
            foreach (TermData mTerm in mTerms)
            {
                string categoryFromFullTerm = GetCategoryFromFullTerm(mTerm.Term, OnlyMainCategory);
                if (!Categories.Contains(categoryFromFullTerm))
                {
                    Categories.Add(categoryFromFullTerm);
                }
            }
            Categories.Sort();
            return Categories;
        }

        public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
        {
            int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
            return (num >= 0) ? FullTerm.Substring(num + 1) : FullTerm;
        }

        public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
        {
            int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
            return (num >= 0) ? FullTerm.Substring(0, num) : EmptyCategory;
        }

        public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
        {
            int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
            if (num < 0)
            {
                Category = EmptyCategory;
                Key = FullTerm;
            }
            else
            {
                Category = FullTerm.Substring(0, num);
                Key = FullTerm.Substring(num + 1);
            }
        }

        public void UpdateDictionary(bool force = false)
        {
            if (force || mDictionary == null || mDictionary.Count != mTerms.Count)
            {
                StringComparer stringComparer = (!CaseInsensitiveTerms) ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
                if (mDictionary.Comparer != stringComparer)
                {
                    mDictionary = new Dictionary<string, TermData>(stringComparer);
                }
                else
                {
                    mDictionary.Clear();
                }
                int i = 0;
                for (int count = mTerms.Count; i < count; i++)
                {
                    TermData termData = mTerms[i];
                    ValidateFullTerm(ref termData.Term);
                    mDictionary[termData.Term] = mTerms[i];
                    mTerms[i].Validate();
                }
                if (I2Utils.IsPlaying())
                {
                    SaveLanguages(true);
                }
            }
        }

        public string GetTranslation(string term, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
        {
            string Translation; if (TryGetTranslation(term, out Translation, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch))
            {
                return Translation;
            }
            return string.Empty;
        }

        public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
        {
            int languageIndex = GetLanguageIndex((overrideLanguage != null) ? overrideLanguage : LocalizationManager.CurrentLanguage, true, false);
            if (languageIndex >= 0 && (!skipDisabled || mLanguages[languageIndex].IsEnabled()))
            {
                TermData termData = GetTermData(term, allowCategoryMistmatch);
                if (termData != null)
                {
                    Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
                    if (Translation == "---")
                    {
                        Translation = string.Empty;
                        return true;
                    }
                    if (!string.IsNullOrEmpty(Translation))
                    {
                        return true;
                    }
                    Translation = null;
                }
                if (OnMissingTranslation == MissingTranslationAction.ShowWarning)
                {
                    Translation = $"<!-Missing Translation [{term}]-!>";
                    return true;
                }
                if (OnMissingTranslation == MissingTranslationAction.Fallback && termData != null)
                {
                    return TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
                }
                if (OnMissingTranslation == MissingTranslationAction.Empty)
                {
                    Translation = string.Empty;
                    return true;
                }
            }
            Translation = null;
            return false;
        }

        private bool TryGetFallbackTranslation(TermData termData, out string Translation, int langIndex, string overrideSpecialization = null, bool skipDisabled = false)
        {
            string text = mLanguages[langIndex].Code;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains('-'))
                {
                    text = text.Substring(0, text.IndexOf('-'));
                }
                for (int i = 0; i < mLanguages.Count; i++)
                {
                    if (i != langIndex && mLanguages[i].Code.StartsWith(text) && (!skipDisabled || mLanguages[i].IsEnabled()))
                    {
                        Translation = termData.GetTranslation(i, overrideSpecialization, true);
                        if (!string.IsNullOrEmpty(Translation))
                        {
                            return true;
                        }
                    }
                }
            }
            for (int j = 0; j < mLanguages.Count; j++)
            {
                if (j != langIndex && (!skipDisabled || mLanguages[j].IsEnabled()) && (text == null || !mLanguages[j].Code.StartsWith(text)))
                {
                    Translation = termData.GetTranslation(j, overrideSpecialization, true);
                    if (!string.IsNullOrEmpty(Translation))
                    {
                        return true;
                    }
                }
            }
            Translation = null;
            return false;
        }

        public TermData AddTerm(string term)
        {
            return AddTerm(term, eTermType.Text);
        }

        public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
        {
            if (string.IsNullOrEmpty(term))
            {
                return null;
            }
            if (mDictionary.Count == 0)
            {
                UpdateDictionary();
            }
            TermData value; if (mDictionary.TryGetValue(term, out value))
            {
                return value;
            }
            TermData termData = null;
            if (allowCategoryMistmatch)
            {
                string keyFromFullTerm = GetKeyFromFullTerm(term);
                {
                    foreach (KeyValuePair<string, TermData> item in mDictionary)
                    {
                        if (item.Value.IsTerm(keyFromFullTerm, true))
                        {
                            if (termData != null)
                            {
                                return null;
                            }
                            termData = item.Value;
                        }
                    }
                    return termData;
                }
            }
            return termData;
        }

        public bool ContainsTerm(string term)
        {
            return GetTermData(term) != null;
        }

        public List<string> GetTermsList(string Category = null)
        {
            if (mDictionary.Count != mTerms.Count)
            {
                UpdateDictionary();
            }
            if (string.IsNullOrEmpty(Category))
            {
                return new List<string>(mDictionary.Keys);
            }
            List<string> list = new List<string>();
            for (int i = 0; i < mTerms.Count; i++)
            {
                TermData termData = mTerms[i];
                if (GetCategoryFromFullTerm(termData.Term) == Category)
                {
                    list.Add(termData.Term);
                }
            }
            return list;
        }

        public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
        {
            ValidateFullTerm(ref NewTerm);
            NewTerm = NewTerm.Trim();
            if (mLanguages.Count == 0)
            {
                AddLanguage("English", "en");
            }
            TermData termData = GetTermData(NewTerm);
            if (termData == null)
            {
                termData = new TermData();
                termData.Term = NewTerm;
                termData.TermType = termType;
                termData.Languages = new string[mLanguages.Count];
                termData.Flags = new byte[mLanguages.Count];
                mTerms.Add(termData);
                mDictionary.Add(NewTerm, termData);
            }
            return termData;
        }

        public void RemoveTerm(string term)
        {
            int num = 0;
            int count = mTerms.Count;
            while (true)
            {
                if (num < count)
                {
                    if (mTerms[num].Term == term)
                    {
                        break;
                    }
                    num++;
                    continue;
                }
                return;
            }
            mTerms.RemoveAt(num);
            mDictionary.Remove(term);
        }

        public static void ValidateFullTerm(ref string Term)
        {
            Term = Term.Replace('\\', '/');
            Term = Term.Trim();
            if (Term.StartsWith(EmptyCategory, StringComparison.Ordinal) && Term.Length > EmptyCategory.Length && Term[EmptyCategory.Length] == '/')
            {
                Term = Term.Substring(EmptyCategory.Length + 1);
            }
            Term = I2Utils.RemoveNonASCII(Term, true);
        }
    }
}