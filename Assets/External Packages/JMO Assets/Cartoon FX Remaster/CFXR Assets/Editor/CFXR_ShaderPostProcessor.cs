using System;
using UnityEditor;
using UnityEngine;

namespace CartoonFX
{
    namespace CustomShaderImporter
    {
        public class CFXR_ShaderPostProcessor : AssetPostprocessor
        {
            #region Unity lifecycle

            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                string[] movedAssets, string[] movedFromAssetPaths)
            {
                CleanCFXRShaders(importedAssets);
            }

            #endregion

            #region Private methods

            private static void CleanCFXRShaders(string[] paths)
            {
                foreach (string assetPath in paths)
                {
                    if (!assetPath.EndsWith(CFXR_ShaderImporter.FILE_EXTENSION,
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var shader = AssetDatabase.LoadMainAssetAtPath(assetPath) as Shader;
                    if (shader != null)
                    {
                        ShaderUtil.ClearShaderMessages(shader);
                        if (!ShaderUtil.ShaderHasError(shader))
                        {
                            ShaderUtil.RegisterShader(shader);
                        }
                    }
                }
            }

            #endregion
        }
    }
}