//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CartoonFX
{
    public class CFXR_ParticleTextFontAsset : ScriptableObject
    {
        #region Public Nested Types

        public enum LetterCase
        {
            Both,
            Upper,
            Lower,
        }

        [System.Serializable]
        public class Kerning
        {
            #region Variables

            public string name = "A";
            public float post;
            public float pre;

            #endregion
        }

        #endregion

        #region Variables

        public Kerning[] CharKerningOffsets;

        public string CharSequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?-.#@$ ";
        public Sprite[] CharSprites;
        public LetterCase letterCase = LetterCase.Upper;

        #endregion

        #region Unity lifecycle

        private void OnValidate()
        {
            hideFlags = HideFlags.None;

            if (CharKerningOffsets == null || CharKerningOffsets.Length != CharSequence.Length)
            {
                CharKerningOffsets = new Kerning[CharSequence.Length];
                for (var i = 0; i < CharKerningOffsets.Length; i++)
                {
                    CharKerningOffsets[i] = new Kerning { name = CharSequence[i].ToString() };
                }
            }
        }

        #endregion

        #region Public methods

        public bool IsValid()
        {
            bool valid = !string.IsNullOrEmpty(CharSequence) && CharSprites != null &&
                         CharSprites.Length == CharSequence.Length && CharKerningOffsets != null &&
                         CharKerningOffsets.Length == CharSprites.Length;

            if (!valid)
            {
                Debug.LogError(string.Format("Invalid ParticleTextFontAsset: '{0}'\n", name), this);
            }

            return valid;
        }

        #endregion

        #region Private methods

#if UNITY_EDITOR
        // [MenuItem("Tools/Create font asset")]
        private static void CreateFontAsset()
        {
            var instance = CreateInstance<CFXR_ParticleTextFontAsset>();
            AssetDatabase.CreateAsset(instance, "Assets/Font.asset");
        }
#endif

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CFXR_ParticleTextFontAsset))]
    public class ParticleTextFontAssetEditor : Editor
    {
        #region Public methods

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Export Kerning"))
            {
                var ptfa = target as CFXR_ParticleTextFontAsset;
                string path = EditorUtility.SaveFilePanel("Export Kerning Settings", Application.dataPath,
                    ptfa.name + " kerning", ".txt");
                if (!string.IsNullOrEmpty(path))
                {
                    var output = "";
                    foreach (CFXR_ParticleTextFontAsset.Kerning k in ptfa.CharKerningOffsets)
                    {
                        output += k.name + "\t" + k.pre + "\t" + k.post + "\n";
                    }

                    System.IO.File.WriteAllText(path, output);
                }
            }

            if (GUILayout.Button("Import Kerning"))
            {
                string path = EditorUtility.OpenFilePanel("Import Kerning Settings", Application.dataPath, "txt");
                if (!string.IsNullOrEmpty(path))
                {
                    string text = System.IO.File.ReadAllText(path);
                    string[] split = text.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                    var ptfa = target as CFXR_ParticleTextFontAsset;
                    Undo.RecordObject(ptfa, "Import Kerning Settings");
                    var kerningList = new List<CFXR_ParticleTextFontAsset.Kerning>(ptfa.CharKerningOffsets);
                    for (var i = 0; i < split.Length; i++)
                    {
                        string[] data = split[i].Split('\t');

                        foreach (CFXR_ParticleTextFontAsset.Kerning cko in kerningList)
                        {
                            if (cko.name == data[0])
                            {
                                cko.pre = float.Parse(data[1]);
                                cko.post = float.Parse(data[2]);
                                break;
                            }
                        }
                    }

                    ptfa.CharKerningOffsets = kerningList.ToArray();
                }
            }

            GUILayout.EndHorizontal();
        }

        #endregion
    }
#endif
}