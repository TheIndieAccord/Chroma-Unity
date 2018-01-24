// Access to Types and Utils
using ChromaSDK;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The purpose of this class is to manage the api connection
/// </summary>
public class ChromaSDKAnimationBaseEditor : Editor
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    protected const string KEY_FOLDER_ANIMATIONS = "folder/animations";
    protected const string KEY_FOLDER_CHROMA = "folder/chroma";
    protected const string KEY_FOLDER_IMAGES = "folder/images";
    protected const string CONTROL_DURATION = "control-duration";
    protected const string CONTROL_OVERRIDE = "control-override";
    protected readonly Color ORANGE = new Color(1f, 0.5f, 0f, 1f);
    protected readonly Color PURPLE = new Color(1f, 0f, 1f, 1f);
    protected const int LAYOUT_PADDING = 5;
    protected const int ANIMATION_VERSION = 1;

    protected static Texture2D _sTextureClear = null;

    protected float _mOverrideFrameTime = 0.1f;

    protected Color _mColor = Color.red;

    protected int _mCurrentFrame = 0;

    protected static bool _sGoToFirstFrame = false;
    protected static bool _sGoToLastFrame = false;

    private static List<IUpdate> _sTargets = new List<IUpdate>();

    protected string GetAnimationFolder()
    {
        if (EditorPrefs.HasKey(KEY_FOLDER_ANIMATIONS))
        {
            return EditorPrefs.GetString(KEY_FOLDER_ANIMATIONS);
        }
        return "Assets/ChromaSDK/Examples/Textures";
    }
    protected string GetChromaFolder()
    {
        if (EditorPrefs.HasKey(KEY_FOLDER_CHROMA))
        {
            return EditorPrefs.GetString(KEY_FOLDER_CHROMA);
        }
        return "Assets/ChromaSDK/Examples/Textures";
    }
    protected string GetImageFolder()
    {
        if (EditorPrefs.HasKey(KEY_FOLDER_IMAGES))
        {
            return EditorPrefs.GetString(KEY_FOLDER_IMAGES);
        }
        return "Assets/ChromaSDK/Examples/Textures";
    }
    protected string GetAnimationExtensions()
    {
        return "GIF Animation;*.gif";
    }
    protected string GetChromaExtensions()
    {
        return "Chroma Animation;*.chroma";
    }
    protected string GetImageExtensions()
    {
        return "Image File;*.bmp;*.jpg;*.png";
    }

    protected virtual int GetFrameCount()
    {
        return 0;
    }
    protected virtual ChromaSDKBaseAnimation GetBaseAnimation()
    {
        return null;
    }
    protected virtual void OnClickPreviewButton()
    {
    }

    public static void GoToFirstFrame()
    {
        _sGoToFirstFrame = true;
    }
    public static void GoToLastFrame()
    {
        _sGoToLastFrame = true;
    }

    protected static void SetupBlankTexture()
    {
        if (null == _sTextureClear)
        {
            _sTextureClear = new Texture2D(1, 1, TextureFormat.RGB24, false);
            _sTextureClear.SetPixel(0, 0, Color.white);
            _sTextureClear.Apply();
        }
    }

    public static Texture2D GetBlankTexture()
    {
        SetupBlankTexture();
        return _sTextureClear;
    }

    #region Move to a connection manager class

    #region Handle Coroutines in edit-mode

    /// <summary>
    /// Keep track of initialization
    /// </summary>
    private static bool _sHasEditorUpdates = false;

    /// <summary>
    /// Start editor updates
    /// </summary>
    public static void StartEditorUpdates()
    {
        if (!_sHasEditorUpdates)
        {
            //Debug.Log("StartEditorUpdates:");
            _sHasEditorUpdates = true;
            EditorApplication.update += EditorUpdate;
            ChromaConnectionManager.Instance.Connect();
        }
    }

    //Stop editor updates
    public static void StopEditorUpdates()
    {
        if (_sHasEditorUpdates)
        {
            //Debug.Log("StopEditorUpdates:");
            _sHasEditorUpdates = false;
            EditorApplication.update -= EditorUpdate;
        }
    }

    public override void OnInspectorGUI()
    {
        bool doPreview = false;
        if (_sGoToFirstFrame)
        {
            _sGoToFirstFrame = false;
            _mCurrentFrame = 0;
        }
        else if (_sGoToLastFrame)
        {
            _sGoToLastFrame = false;
            if (GetFrameCount() > 0)
            {
                _mCurrentFrame = GetFrameCount() - 1;
            }
            doPreview = true;
        }

        if (doPreview)
        {
            ChromaSDKBaseAnimation animation = GetBaseAnimation();
            if (animation)
            {
                animation.RefreshCurve();
            }
            OnClickPreviewButton();
        }

        // Add the target to a list of targets that will be updated
        IUpdate targetUpdater = (IUpdate)target;
        int i = 0;
        bool found = false;
        while (i < _sTargets.Count)
        {
            IUpdate updater = _sTargets[i];
            if (null != updater)
            {
                if (updater == targetUpdater)
                {
                    found = true;
                    break;
                }
                ++i;
            }
            else
            {
                _sTargets.RemoveAt(i);
            }
        }
        if (!found)
        {
            _sTargets.Add(targetUpdater);
        }
    }

    private static void UnloadPrefabAnimations()
    {
        //Debug.Log("UnloadPrefabAnimations:");

        for (int i = 0; i < _sTargets.Count; ++i)
        {
            IUpdate updater = _sTargets[i];
            if (null == updater)
            {
                continue;
            }
            else if (updater is ChromaSDKAnimation1D)
            {
                (updater as ChromaSDKAnimation1D).Unload();
            }
            else if (updater is ChromaSDKAnimation2D)
            {
                (updater as ChromaSDKAnimation2D).Unload();
            }
        }
    }

    public static void EditorUpdate()
    {
        // stop editor updates if compiling and disconnect
        if (EditorApplication.isCompiling)
        {
            StopEditorUpdates();
            UnloadPrefabAnimations();
            ChromaConnectionManager.Instance.Disconnect();
        }
        else
        {
            // Connection Manager updates aren't needed in PlayMode,
            // if the ConnectionManager is in the Scene
            if (!Application.isPlaying)
            {
                // keep updates happening in edit mode
                ChromaConnectionManager.Instance.Update();
            }
            
            // Update the targets being inspected
            int i = 0;
            while (i < _sTargets.Count)
            {
                IUpdate updater = _sTargets[i];
                if (null != updater)
                {
                    updater.Update();
                    ++i;
                }
                else
                {
                    _sTargets.RemoveAt(i);
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// OnEnable is invoked on play, or while playing after compile
    /// </summary>
    private void OnEnable()
    {
        //Debug.Log("ChromaSDKAnimationBaseEditor: OnEnable");
        StartEditorUpdates();
    }

    #endregion
	
#endif
}
