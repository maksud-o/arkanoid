﻿//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------------------------------------------------

// Use the defines below to globally disable features:

//#define DISABLE_CAMERA_SHAKE
//#define DISABLE_LIGHTS
//#define DISABLE_CLEAR_BEHAVIOR

//--------------------------------------------------------------------------------------------------------------------------------

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CartoonFX
{
    [RequireComponent(typeof(ParticleSystem))]
    [DisallowMultipleComponent]
    public partial class CFXR_Effect : MonoBehaviour
    {
        #region Public Nested Types

        public enum ClearBehavior
        {
            None,
            Disable,
            Destroy,
        }

        [System.Serializable]
        public class AnimatedLight
        {
            #region Public Nested Types

#if UNITY_EDITOR
            [CustomPropertyDrawer(typeof(AnimatedLight))]
            public class AnimatedLightDrawer : PropertyDrawer
            {
                #region Variables

                private static readonly GUIContent ColorModeLabel = new("Color Mode");
                private static readonly GUIContent IntensityModeLabel = new("Intensity Mode");

                private static readonly GUIContent[] ModePopupLabels = { new("Curve"), new("Perlin Noise") };
                private static readonly GUIContent RangeModeLabel = new("Range Mode");

                private const float INDENT_WIDTH = 15f;
                private const float PADDING = 4f;

                private SerializedProperty animateColor;

                private SerializedProperty animateIntensity;

                private SerializedProperty animateRange;
                private SerializedProperty colorCurve;
                private SerializedProperty colorDuration;
                private SerializedProperty colorGradient;
                private SerializedProperty fadeIn;
                private SerializedProperty fadeInDuration;
                private SerializedProperty fadeOut;
                private SerializedProperty fadeOutDuration;
                private SerializedProperty intensityCurve;
                private SerializedProperty intensityDuration;
                private SerializedProperty intensityEnd;
                private SerializedProperty intensityStart;
                private SerializedProperty light;

                private SerializedProperty loop;
                private SerializedProperty perlinColor;
                private SerializedProperty perlinColorSpeed;
                private SerializedProperty perlinIntensity;
                private SerializedProperty perlinIntensitySpeed;
                private SerializedProperty perlinRange;
                private SerializedProperty perlinRangeSpeed;
                private SerializedProperty rangeCurve;
                private SerializedProperty rangeDuration;
                private SerializedProperty rangeEnd;
                private SerializedProperty rangeStart;

                #endregion

                #region Public methods

                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                    fetchProperties(property);

                    float propSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    var count = 5;

                    if (animateIntensity.boolValue)
                    {
                        count += 3;
                        count += perlinIntensity.boolValue ? 1 : 2;
                        count += 1;
                        count += fadeIn.boolValue ? 1 : 0;
                        count += 1;
                        count += fadeOut.boolValue ? 1 : 0;
                    }

                    if (animateRange.boolValue)
                    {
                        count += 3;
                        count += perlinRange.boolValue ? 1 : 2;
                    }

                    if (animateColor.boolValue)
                    {
                        count += 2;
                        count += perlinColor.boolValue ? 1 : 2;
                    }

                    return count * propSpace + PADDING * 2;
                }

                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                    fetchProperties(property);

                    Rect rect = EditorGUI.IndentedRect(position);

                    //Rect lineRect = rect;
                    //lineRect.height = 1;
                    //lineRect.y -= 2;
                    //EditorGUI.DrawRect(lineRect, Color.gray);

                    if (Event.current.type == EventType.Repaint)
                    {
                        EditorStyles.helpBox.Draw(rect, GUIContent.none, 0);
                    }

                    EditorGUIUtility.labelWidth -= INDENT_WIDTH;

                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.xMax -= PADDING;
                    rect.y += PADDING;
                    float propSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    EditorGUI.PropertyField(rect, light);
                    rect.y += propSpace;
                    EditorGUI.PropertyField(rect, loop);
                    rect.y += propSpace;

                    EditorGUI.PropertyField(rect, animateIntensity);
                    rect.y += propSpace;
                    if (animateIntensity.boolValue)
                    {
                        startIndent(ref rect);
                        {
                            EditorGUI.PropertyField(rect, intensityStart);
                            rect.y += propSpace;
                            EditorGUI.PropertyField(rect, intensityEnd);
                            rect.y += propSpace;

                            int val = EditorGUI.Popup(rect, IntensityModeLabel, perlinIntensity.boolValue ? 1 : 0,
                                ModePopupLabels);
                            rect.y += propSpace;
                            if (val == 1 && !perlinIntensity.boolValue)
                            {
                                perlinIntensity.boolValue = true;
                            }
                            else if (val == 0 && perlinIntensity.boolValue)
                            {
                                perlinIntensity.boolValue = false;
                            }

                            startIndent(ref rect);
                            {
                                if (perlinIntensity.boolValue)
                                {
                                    EditorGUI.PropertyField(rect, perlinIntensitySpeed);
                                    rect.y += propSpace;
                                }
                                else
                                {
                                    EditorGUI.PropertyField(rect, intensityDuration);
                                    rect.y += propSpace;
                                    EditorGUI.PropertyField(rect, intensityCurve);
                                    rect.y += propSpace;
                                }
                            }
                            endIndent(ref rect);

                            EditorGUI.PropertyField(rect, fadeIn);
                            rect.y += propSpace;
                            if (fadeIn.boolValue)
                            {
                                startIndent(ref rect);
                                EditorGUI.PropertyField(rect, fadeInDuration);
                                rect.y += propSpace;
                                endIndent(ref rect);
                            }

                            EditorGUI.PropertyField(rect, fadeOut);
                            rect.y += propSpace;
                            if (fadeOut.boolValue)
                            {
                                startIndent(ref rect);
                                EditorGUI.PropertyField(rect, fadeOutDuration);
                                rect.y += propSpace;
                                endIndent(ref rect);
                            }
                        }
                        endIndent(ref rect);
                    }

                    EditorGUI.PropertyField(rect, animateRange);
                    rect.y += propSpace;
                    if (animateRange.boolValue)
                    {
                        startIndent(ref rect);
                        {
                            EditorGUI.PropertyField(rect, rangeStart);
                            rect.y += propSpace;
                            EditorGUI.PropertyField(rect, rangeEnd);
                            rect.y += propSpace;

                            int val = EditorGUI.Popup(rect, RangeModeLabel, perlinRange.boolValue ? 1 : 0,
                                ModePopupLabels);
                            rect.y += propSpace;
                            if (val == 1 && !perlinRange.boolValue)
                            {
                                perlinRange.boolValue = true;
                            }
                            else if (val == 0 && perlinRange.boolValue)
                            {
                                perlinRange.boolValue = false;
                            }

                            startIndent(ref rect);
                            {
                                if (perlinRange.boolValue)
                                {
                                    EditorGUI.PropertyField(rect, perlinRangeSpeed);
                                    rect.y += propSpace;
                                }
                                else
                                {
                                    EditorGUI.PropertyField(rect, rangeDuration);
                                    rect.y += propSpace;
                                    EditorGUI.PropertyField(rect, rangeCurve);
                                    rect.y += propSpace;
                                }
                            }
                            endIndent(ref rect);
                        }
                        endIndent(ref rect);
                    }

                    EditorGUI.PropertyField(rect, animateColor);
                    rect.y += propSpace;
                    if (animateColor.boolValue)
                    {
                        startIndent(ref rect);
                        {
                            EditorGUI.PropertyField(rect, colorGradient);
                            rect.y += propSpace;

                            int val = EditorGUI.Popup(rect, ColorModeLabel, perlinColor.boolValue ? 1 : 0,
                                ModePopupLabels);
                            rect.y += propSpace;
                            if (val == 1 && !perlinColor.boolValue)
                            {
                                perlinColor.boolValue = true;
                            }
                            else if (val == 0 && perlinColor.boolValue)
                            {
                                perlinColor.boolValue = false;
                            }

                            startIndent(ref rect);
                            {
                                if (perlinColor.boolValue)
                                {
                                    EditorGUI.PropertyField(rect, perlinColorSpeed);
                                    rect.y += propSpace;
                                }
                                else
                                {
                                    EditorGUI.PropertyField(rect, colorDuration);
                                    rect.y += propSpace;
                                    EditorGUI.PropertyField(rect, colorCurve);
                                    rect.y += propSpace;
                                }
                            }
                            endIndent(ref rect);
                        }
                        endIndent(ref rect);
                    }

                    EditorGUIUtility.labelWidth += INDENT_WIDTH;

                    if (GUI.changed)
                    {
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }

                #endregion

                #region Private methods

                private void endIndent(ref Rect rect)
                {
                    EditorGUIUtility.labelWidth += INDENT_WIDTH;
                    rect.xMin -= INDENT_WIDTH;
                }

                private void fetchProperties(SerializedProperty property)
                {
                    light = property.FindPropertyRelative("light");

                    loop = property.FindPropertyRelative("loop");

                    animateIntensity = property.FindPropertyRelative("animateIntensity");
                    intensityStart = property.FindPropertyRelative("intensityStart");
                    intensityEnd = property.FindPropertyRelative("intensityEnd");
                    intensityDuration = property.FindPropertyRelative("intensityDuration");
                    intensityCurve = property.FindPropertyRelative("intensityCurve");
                    perlinIntensity = property.FindPropertyRelative("perlinIntensity");
                    perlinIntensitySpeed = property.FindPropertyRelative("perlinIntensitySpeed");
                    fadeIn = property.FindPropertyRelative("fadeIn");
                    fadeInDuration = property.FindPropertyRelative("fadeInDuration");
                    fadeOut = property.FindPropertyRelative("fadeOut");
                    fadeOutDuration = property.FindPropertyRelative("fadeOutDuration");

                    animateRange = property.FindPropertyRelative("animateRange");
                    rangeStart = property.FindPropertyRelative("rangeStart");
                    rangeEnd = property.FindPropertyRelative("rangeEnd");
                    rangeDuration = property.FindPropertyRelative("rangeDuration");
                    rangeCurve = property.FindPropertyRelative("rangeCurve");
                    perlinRange = property.FindPropertyRelative("perlinRange");
                    perlinRangeSpeed = property.FindPropertyRelative("perlinRangeSpeed");

                    animateColor = property.FindPropertyRelative("animateColor");
                    colorGradient = property.FindPropertyRelative("colorGradient");
                    colorDuration = property.FindPropertyRelative("colorDuration");
                    colorCurve = property.FindPropertyRelative("colorCurve");
                    perlinColor = property.FindPropertyRelative("perlinColor");
                    perlinColorSpeed = property.FindPropertyRelative("perlinColorSpeed");
                }

                private void startIndent(ref Rect rect)
                {
                    EditorGUIUtility.labelWidth -= INDENT_WIDTH;
                    rect.xMin += INDENT_WIDTH;
                }

                #endregion
            }
#endif

            #endregion

            #region Variables

            public static bool editorPreview = true;

            public bool animateColor;

            public bool animateIntensity;

            public bool animateRange;
            public AnimationCurve colorCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            public float colorDuration = 0.5f;
            public Gradient colorGradient;
            public bool fadeIn;
            public float fadeInDuration = 0.5f;
            public bool fadeOut;
            public float fadeOutDuration = 0.5f;
            public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            public float intensityDuration = 0.5f;
            public float intensityEnd;
            public float intensityStart = 8f;

            public Light light;

            public bool loop;
            public bool perlinColor;
            public float perlinColorSpeed = 1f;
            public bool perlinIntensity;
            public float perlinIntensitySpeed = 1f;
            public bool perlinRange;
            public float perlinRangeSpeed = 1f;
            public AnimationCurve rangeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            public float rangeDuration = 0.5f;
            public float rangeEnd;
            public float rangeStart = 8f;

            #endregion

            #region Public methods

            public void animate(float time)
            {
#if UNITY_EDITOR
                if (!editorPreview && !EditorApplication.isPlaying)
                {
                    return;
                }
#endif

                if (light != null)
                {
                    if (animateIntensity)
                    {
                        float delta = loop
                            ? Mathf.Clamp01(time % intensityDuration / intensityDuration)
                            : Mathf.Clamp01(time / intensityDuration);
                        delta = perlinIntensity
                            ? Mathf.PerlinNoise(Time.time * perlinIntensitySpeed, 0f)
                            : intensityCurve.Evaluate(delta);
                        light.intensity = Mathf.LerpUnclamped(intensityEnd, intensityStart, delta);

                        if (fadeIn && time < fadeInDuration)
                        {
                            light.intensity *= Mathf.Clamp01(time / fadeInDuration);
                        }
                    }

                    if (animateRange)
                    {
                        float delta = loop
                            ? Mathf.Clamp01(time % rangeDuration / rangeDuration)
                            : Mathf.Clamp01(time / rangeDuration);
                        delta = perlinRange
                            ? Mathf.PerlinNoise(Time.time * perlinRangeSpeed, 10f)
                            : rangeCurve.Evaluate(delta);
                        light.range = Mathf.LerpUnclamped(rangeEnd, rangeStart, delta);
                    }

                    if (animateColor)
                    {
                        float delta = loop
                            ? Mathf.Clamp01(time % colorDuration / colorDuration)
                            : Mathf.Clamp01(time / colorDuration);
                        delta = perlinColor
                            ? Mathf.PerlinNoise(Time.time * perlinColorSpeed, 0f)
                            : colorCurve.Evaluate(delta);
                        light.color = colorGradient.Evaluate(delta);
                    }
                }
            }

            public void animateFadeOut(float time)
            {
                if (fadeOut && light != null)
                {
                    light.intensity *= 1.0f - Mathf.Clamp01(time / fadeOutDuration);
                }
            }

            public void reset()
            {
                if (light != null)
                {
                    if (animateIntensity)
                    {
                        light.intensity = fadeIn || fadeOut ? 0 : intensityEnd;
                    }

                    if (animateRange)
                    {
                        light.range = rangeEnd;
                    }

                    if (animateColor)
                    {
                        light.color = colorGradient.Evaluate(1f);
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Variables

        // ================================================================================================================================

        // Globally disable features
        public static bool GlobalDisableCameraShake;
        public static bool GlobalDisableLights;
        // Change this value to easily tune the camera shake strength for all effects
        private const float GLOBAL_CAMERA_SHAKE_MULTIPLIER = 1.0f;
        [Space]
        public AnimatedLight[] animatedLights;
        [Space]
        public CameraShake cameraShake;

        // ================================================================================================================================

        [Tooltip(
            "Defines an action to execute when the Particle System has completely finished playing and emitting particles.")]
        public ClearBehavior clearBehavior = ClearBehavior.Destroy;
        [Tooltip(
            "Defines which Particle System to track to trigger light fading out.\nLeave empty if not using fading out.")]
        public ParticleSystem fadeOutReference;
        [System.NonSerialized] private MaterialPropertyBlock materialPropertyBlock;
        [System.NonSerialized] private Renderer particleRenderer;
        private ParticleSystem rootParticleSystem;

        private float time;

        #endregion

        #region Unity lifecycle

#if !DISABLE_CAMERA_SHAKE || !DISABLE_CLEAR_BEHAVIOR
        private void Awake()
        {
#if !DISABLE_CAMERA_SHAKE
            if (cameraShake != null && cameraShake.enabled)
            {
                cameraShake.fetchCameras();
            }
#endif
#if !DISABLE_CLEAR_BEHAVIOR
            startFrameOffset = GlobalStartFrameOffset++;
#endif
            // Detect if world position needs to be passed to the shader
            particleRenderer = GetComponent<ParticleSystemRenderer>();
            if (particleRenderer.sharedMaterial != null &&
                particleRenderer.sharedMaterial.IsKeywordEnabled("_CFXR_LIGHTING_WPOS_OFFSET"))
            {
                materialPropertyBlock = new MaterialPropertyBlock();
            }
        }
#endif

        private void OnEnable()
        {
            foreach (AnimatedLight animLight in animatedLights)
            {
                if (animLight.light != null)
                {
#if !DISABLE_LIGHTS
                    animLight.light.enabled = !GlobalDisableLights;
#else
					animLight.light.enabled = false;
#endif
                }
            }
        }

        private void OnDisable()
        {
            ResetState();
        }

        #endregion

        #region Public methods

#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE
        public void Animate(float time)
        {
#if !DISABLE_LIGHTS
            if (animatedLights != null && !GlobalDisableLights)
            {
                foreach (AnimatedLight animLight in animatedLights)
                {
                    animLight.animate(time);
                }
            }
#endif

#if !DISABLE_CAMERA_SHAKE
            if (cameraShake != null && cameraShake.enabled && !GlobalDisableCameraShake)
            {
#if UNITY_EDITOR
                if (!cameraShake.isShaking)
                {
                    cameraShake.fetchCameras();
                }
#endif
                cameraShake.animate(time);
            }
#endif
        }
#endif

        // ================================================================================================================================

        public void ResetState()
        {
            time = 0f;
            fadingOutStartTime = 0f;
            isFadingOut = false;

#if !DISABLE_LIGHTS
            if (animatedLights != null)
            {
                foreach (AnimatedLight animLight in animatedLights)
                {
                    animLight.reset();
                }
            }
#endif

#if !DISABLE_CAMERA_SHAKE
            if (cameraShake != null && cameraShake.enabled)
            {
                cameraShake.StopShake();
            }
#endif
        }

        #endregion

        #region Private methods

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitGlobalOptions()
        {
            AnimatedLight.editorPreview = EditorPrefs.GetBool("CFXR Light EditorPreview", true);
#if !DISABLE_CAMERA_SHAKE
            CameraShake.editorPreview = EditorPrefs.GetBool("CFXR CameraShake EditorPreview", true);
#endif
        }
#endif

        #endregion

#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE || !DISABLE_CLEAR_BEHAVIOR
        private const int CHECK_EVERY_N_FRAME = 20;
        private static int GlobalStartFrameOffset;
        private int startFrameOffset;
        private void Update()
        {
#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE
            time += Time.deltaTime;

            Animate(time);

            if (fadeOutReference != null && !fadeOutReference.isEmitting && (fadeOutReference.isPlaying || isFadingOut))
            {
                FadeOut(time);
            }
#endif
#if !DISABLE_CLEAR_BEHAVIOR
            if (clearBehavior != ClearBehavior.None)
            {
                if (rootParticleSystem == null)
                {
                    rootParticleSystem = GetComponent<ParticleSystem>();
                }

                // Check isAlive every N frame, with an offset so that all active effects aren't checked at once
                if ((Time.renderedFrameCount + startFrameOffset) % CHECK_EVERY_N_FRAME == 0)
                {
                    if (!rootParticleSystem.IsAlive(true))
                    {
                        if (clearBehavior == ClearBehavior.Destroy)
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
#endif
            if (materialPropertyBlock != null)
            {
                particleRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetVector("_GameObjectWorldPosition", transform.position);
                particleRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
#endif

#if !DISABLE_LIGHTS
        private bool isFadingOut;
        private float fadingOutStartTime;
        public void FadeOut(float time)
        {
            if (animatedLights == null)
            {
                return;
            }

            if (!isFadingOut)
            {
                isFadingOut = true;
                fadingOutStartTime = time;
            }

            foreach (AnimatedLight animLight in animatedLights)
            {
                animLight.animateFadeOut(time - fadingOutStartTime);
            }
        }
#endif

#if UNITY_EDITOR
        // Editor preview
        // Detect when the Particle System is previewing and trigger this animation too

        [System.NonSerialized] private ParticleSystem _parentParticle;

        private ParticleSystem parentParticle
        {
            get
            {
                if (_parentParticle == null)
                {
                    _parentParticle = GetComponent<ParticleSystem>();
                }

                return _parentParticle;
            }
        }

        [System.NonSerialized] public bool editorUpdateRegistered;

        [System.NonSerialized] private bool particleWasStopped;
        [System.NonSerialized] private float particleTime;
        [System.NonSerialized] private float particleTimeUnwrapped;

        private void OnDestroy()
        {
            UnregisterEditorUpdate();
        }

        public void RegisterEditorUpdate()
        {
            PrefabAssetType type = PrefabUtility.GetPrefabAssetType(gameObject);
            PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(gameObject);

            // Prefab in Project window
            if ((type == PrefabAssetType.Regular || type == PrefabAssetType.Variant) &&
                status == PrefabInstanceStatus.NotAPrefab)
            {
                return;
            }

            if (!editorUpdateRegistered)
            {
                EditorApplication.update += onEditorUpdate;
                editorUpdateRegistered = true;
            }
        }

        public void UnregisterEditorUpdate()
        {
            if (editorUpdateRegistered)
            {
                editorUpdateRegistered = false;
                EditorApplication.update -= onEditorUpdate;
            }

            ResetState();
        }

        private void onEditorUpdate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (this == null)
            {
                return;
            }

            var renderer = GetComponent<ParticleSystemRenderer>();
            if (renderer.sharedMaterial != null &&
                renderer.sharedMaterial.IsKeywordEnabled("_CFXR_LIGHTING_WPOS_OFFSET"))
            {
                if (materialPropertyBlock == null)
                {
                    materialPropertyBlock = new MaterialPropertyBlock();
                }

                renderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetVector("_GameObjectWorldPosition", transform.position);
                renderer.SetPropertyBlock(materialPropertyBlock);
            }

            // Need to track unwrapped time when playing back from Editor
            // because the parentParticle.time will be reset at each loop
            float delta = parentParticle.time - particleTime;

            if (delta < 0 && parentParticle.isPlaying)
            {
                delta = parentParticle.main.duration + delta;
                if (delta > 0.1 || delta < 0)
                {
                    // try to detect when "Restart" is pressed
                    ResetState();
                    particleTimeUnwrapped = 0;
                    delta = 0;
                }
            }

            particleTimeUnwrapped += delta;

            if (particleTime != parentParticle.time)
            {
#if !DISABLE_CAMERA_SHAKE
                if (cameraShake != null && cameraShake.enabled && parentParticle.time < particleTime &&
                    parentParticle.time < 0.05f)
                {
                    cameraShake.StartShake();
                }
#endif
#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKES
                Animate(particleTimeUnwrapped);

                if (!parentParticle.isEmitting)
                {
                    FadeOut(particleTimeUnwrapped);
                }
#endif
            }

            if (particleWasStopped != parentParticle.isStopped)
            {
                if (parentParticle.isStopped)
                {
                    ResetState();
                }

                particleTimeUnwrapped = 0;
            }

            particleWasStopped = parentParticle.isStopped;
            particleTime = parentParticle.time;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CFXR_Effect))]
    [CanEditMultipleObjects]
    public class CFXR_Effect_Editor : Editor
    {
        #region Variables

        private GUIStyle _PaddedRoundedRect;
        private bool? lightEditorPreview;
        private bool? shakeEditorPreview;

        #endregion

        #region Properties

        private GUIStyle PaddedRoundedRect
        {
            get
            {
                if (_PaddedRoundedRect == null)
                {
                    _PaddedRoundedRect = new GUIStyle(EditorStyles.helpBox);
                    _PaddedRoundedRect.padding = new RectOffset(4, 4, 4, 4);
                }

                return _PaddedRoundedRect;
            }
        }

        #endregion

        #region Unity lifecycle

        private void OnEnable()
        {
            if (targets == null)
            {
                return;
            }

            foreach (Object t in targets)
            {
                var cfxr_effect = t as CFXR_Effect;
                if (cfxr_effect != null)
                {
                    if (isPrefabSource(cfxr_effect.gameObject))
                    {
                        return;
                    }

                    cfxr_effect.RegisterEditorUpdate();
                }
            }
        }

        private void OnDisable()
        {
            if (targets == null)
            {
                return;
            }

            foreach (Object t in targets)
            {
                // Can be null if GameObject has been destroyed
                var cfxr_effect = t as CFXR_Effect;
                if (cfxr_effect != null)
                {
                    if (isPrefabSource(cfxr_effect.gameObject))
                    {
                        return;
                    }

                    cfxr_effect.UnregisterEditorUpdate();
                }
            }
        }

        #endregion

        #region Public methods

        public override void OnInspectorGUI()
        {
            GlobalOptionsGUI();

#if DISABLE_CAMERA_SHAKE
			EditorGUILayout.HelpBox("Camera Shake has been globally disabled in the code.\nThe properties remain to avoid data loss but the shaking won't be applied for any effect.", MessageType.Info);
#endif

            base.OnInspectorGUI();
        }

        #endregion

        #region Private methods

        private void GlobalOptionsGUI()
        {
            EditorGUILayout.BeginVertical(PaddedRoundedRect);
            {
                GUILayout.Label("Editor Preview:", EditorStyles.boldLabel);

                if (lightEditorPreview == null)
                {
                    lightEditorPreview = EditorPrefs.GetBool("CFXR Light EditorPreview", true);
                }

                bool lightPreview = EditorGUILayout.Toggle("Light Animations", lightEditorPreview.Value);
                if (lightPreview != lightEditorPreview.Value)
                {
                    lightEditorPreview = lightPreview;
                    EditorPrefs.SetBool("CFXR Light EditorPreview", lightPreview);
                    CFXR_Effect.AnimatedLight.editorPreview = lightPreview;
                }

#if !DISABLE_CAMERA_SHAKE
                if (shakeEditorPreview == null)
                {
                    shakeEditorPreview = EditorPrefs.GetBool("CFXR CameraShake EditorPreview", true);
                }

                bool shakePreview = EditorGUILayout.Toggle("Camera Shake", shakeEditorPreview.Value);
                if (shakePreview != shakeEditorPreview.Value)
                {
                    shakeEditorPreview = shakePreview;
                    EditorPrefs.SetBool("CFXR CameraShake EditorPreview", shakePreview);
                    CFXR_Effect.CameraShake.editorPreview = shakePreview;
                }
#endif
            }
            EditorGUILayout.EndVertical();
        }

        private static bool isPrefabSource(GameObject gameObject)
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            PrefabInstanceStatus prefabType = PrefabUtility.GetPrefabInstanceStatus(gameObject);
            return (assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant) &&
                   prefabType == PrefabInstanceStatus.NotAPrefab;
        }

        #endregion
    }
#endif
}