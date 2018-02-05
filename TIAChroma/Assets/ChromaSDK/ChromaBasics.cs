// Access to Types and Utils
using ChromaSDK;
// Access to Chroma data structures
using ChromaSDK.ChromaPackage.Model;
// Access to the Chroma API
using ChromaSDK.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChromaBasics : MonoBehaviour
{
    public Color _StaticColor;

    //Testing Keyboard Layers
    private EffectArray2dInput keyboardStatic = new EffectArray2dInput();
    private EffectArray2dInput keyboardActive = new EffectArray2dInput();

    private bool ChromaLive = false;

    /// <summary>
    /// 1D animation assets
    /// </summary>
    public ChromaSDKAnimation1D[] _mAnimations1D = null;

    /// <summary>
    /// 2D animation assets
    /// </summary>
    public ChromaSDKAnimation2D[] _mAnimations2D = null;

    /// <summary>
    /// Connection manager maintains REST connection
    /// </summary>
    private ChromaConnectionManager _mConnectionManager = null;

    /// <summary>
    /// Show status label
    /// </summary>
    private string _mTextStatus;

    /// <summary>
    /// Keep animation playing
    /// </summary>
    private bool _mPlayAnimation = false;

    /// <summary>
    /// Actions to run on the main thread
    /// </summary>
    private List<Action> _mMainActions = new List<Action>();

    /// <summary>
    /// Deactivate on non-windows platforms
    /// </summary>
    public void Awake()
    {
        if (!ChromaUtils.IsPlatformSupported())
        {
            ChromaUtils.SetActive(gameObject, false);
            return;
        }
    }

    /// <summary>
    /// UI interaction needs to execute on the main thread
    /// </summary>
    private void FixedUpdate()
    {
        if (_mMainActions.Count > 0)
        {
            Action action = _mMainActions[0];
            _mMainActions.RemoveAt(0);
            action.Invoke();
        }
    }

    /// <summary>
    /// Run on the main thread
    /// </summary>
    /// <param name="action"></param>
    void RunOnMainThread(Action action)
    {
        _mMainActions.Add(action);
    }

    #region Helpers

    /// <summary>
    /// Display result with label prefix
    /// </summary>
    /// <param name="label"></param>
    /// <param name="result"></param>
    private static void LogResult(string label, EffectResponse result)
    {
        if (null == result)
        {
            Debug.LogError(string.Format("{0} Result was null!", label));
        }
        else
        {
            Debug.Log(string.Format("{0} {1}", label, result));
        }
    }

    /// <summary>
    /// Get Effect: CHROMA_STATIC
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static EffectInput GetEffectChromaStatic(Color color)
    {
        var input = new EffectInput();
        input.Effect = EffectType.CHROMA_STATIC;
        int bgrInt = ChromaUtils.ToBGR(color);
        input.Param = new EffectInputParam(bgrInt);
        return input;
    }

    #endregion

    /// <summary>
    /// Clear effect on all devices using PUT
    /// </summary>
    void SetEffectNoneOnAll()
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        LogResult("PutChromaLinkNone:", chromaApi.PutChromaLinkNone());
        LogResult("PutHeadsetNone:", chromaApi.PutHeadsetNone());
        LogResult("PutKeyboardNone:", chromaApi.PutKeyboardNone());
        LogResult("PutKeypadNone:", chromaApi.PutKeypadNone());
        LogResult("PutMouseNone:", chromaApi.PutMouseNone());
        LogResult("PutMousepadNone:", chromaApi.PutMousepadNone());
    }

    /// <summary>
    /// Set static effect on all devices using PUT
    /// </summary>
    /// <param name="color"></param>
    void SetEffectStaticOnAll(Color color)
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;
        int bgrInt = ChromaUtils.ToBGR(color);

        LogResult("PutChromaLinkStatic:", chromaApi.PutChromaLinkStatic(bgrInt));
        LogResult("PutHeadsetStatic:", chromaApi.PutHeadsetStatic(bgrInt));
        LogResult("PutKeyboardStatic:", chromaApi.PutKeyboardStatic(bgrInt));
        LogResult("PutKeypadStatic:", chromaApi.PutKeypadStatic(bgrInt));
        LogResult("PutMouseStatic:", chromaApi.PutMouseStatic(bgrInt));
        LogResult("PutMousepadStatic:", chromaApi.PutMousepadStatic(bgrInt));
    }

    /// <summary>
    /// Set effect on all devices using PUT
    /// </summary>
    /// <param name="input"></param>
    void SetEffectOnAll(EffectInput input)
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        LogResult("PutChromaLink:", chromaApi.PutChromaLink(input));
        LogResult("PutHeadset:", chromaApi.PutHeadset(input));
        LogResult("PutKeyboard:", chromaApi.PutKeyboard(input));
        LogResult("PutKeypad:", chromaApi.PutKeypad(input));
        LogResult("PutMouse:", chromaApi.PutMouse(input));
        LogResult("PutMousepad:", chromaApi.PutMousepad(input));
    }

    /// <summary>
    /// Use the API to set the CHROMA_CUSTOM effect
    /// </summary>
    void SetKeyboardCustomEffect()
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        LogResult("PutChromaLinkCustom:", chromaApi.PutChromaLinkCustom(ChromaUtils.CreateRandomColors1D(ChromaDevice1DEnum.ChromaLink)));
        LogResult("PutHeadsetCustom:", chromaApi.PutHeadsetCustom(ChromaUtils.CreateRandomColors1D(ChromaDevice1DEnum.Headset)));
        LogResult("PutKeyboardCustom:", chromaApi.PutKeyboardCustom(ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keyboard)));
        LogResult("PutKeypadCustom:", chromaApi.PutKeypadCustom(ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keypad)));
        LogResult("PutMouseCustom:", chromaApi.PutMouseCustom(ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Mouse)));
        LogResult("PutMousepadCustom:", chromaApi.PutMousepadCustom(ChromaUtils.CreateRandomColors1D(ChromaDevice1DEnum.Mousepad)));
    }

    /// <summary>
    /// Loop 1D animation using complete callback
    /// </summary>
    /// <param name="animation"></param>
    void LoopAnimation1D(ChromaSDKAnimation1D animation)
    {
        if (_mPlayAnimation)
        {
            animation.PlayWithOnComplete(LoopAnimation1D);
        }
    }

    /// <summary>
    /// Loop 2D animation using complete callback
    /// </summary>
    /// <param name="animation"></param>
    void LoopAnimation2D(ChromaSDKAnimation2D animation)
    {
        if (_mPlayAnimation)
        {
            animation.PlayWithOnComplete(LoopAnimation2D);
        }
    }

    /// <summary>
    /// Verify the animation loaded,
    /// failure indicates the device was not connected
    /// </summary>
    /// <param name="animation"></param>
    void ValidateAnimation(ChromaSDKBaseAnimation animation)
    {
        // validate animation loaded
        List<EffectResponseId> effects = animation.GetEffects();
        if (null == effects ||
            effects.Count == 0)
        {
            Debug.LogError("Animation failed to create effects!");
        }
        else
        {
            for (int i = 0; i < effects.Count; ++i)
            {
                EffectResponseId effect = effects[i];
                if (null == effect ||
                    effect.Result != 0)
                {
                    Debug.LogError("Failed to create effect!");
                }
            }
        }
    }

    /// <summary>
    /// Create and play an animation, run from the main thread
    /// </summary>
    void DoAnimations()
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        _mPlayAnimation = true;

        //Debug.Log(string.Format("Load 1D animations Length={0}", _mAnimations1D.Length));

        foreach (ChromaSDKAnimation1D animation in _mAnimations1D)
        {
            // unload in case animation was playing in editor
            if (animation.IsLoaded())
            {
                animation.Unload();
            }
            // load the animation
            animation.Load();

            // validate the animation loaded
            ValidateAnimation(animation);
        }

        //Debug.Log(string.Format("Load 2D animations Length={0}", _mAnimations2D.Length));

        foreach (ChromaSDKAnimation2D animation in _mAnimations2D)
        {
            // unload in case animation was playing in editor
            if (animation.IsLoaded())
            {
                animation.Unload();
            }
            // load the animation
            animation.Load();

            // validate the animation loaded
            ValidateAnimation(animation);
        }

        //Debug.Log("Play animations looping...");

        foreach (ChromaSDKAnimation1D animation in _mAnimations1D)
        {
            LoopAnimation1D(animation);
        }

        foreach (ChromaSDKAnimation2D animation in _mAnimations2D)
        {
            LoopAnimation2D(animation);
        }
    }

    /// <summary>
    /// Stop the animations, run from the main thread
    /// </summary>
    void StopAnimations()
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        // unload 1D animations
        foreach (ChromaSDKAnimation1D animation in _mAnimations1D)
        {
            if (animation.IsLoaded())
            {
                animation.Unload();
            }
        }

        // unload 2D animations
        foreach (ChromaSDKAnimation2D animation in _mAnimations2D)
        {
            if (animation.IsLoaded())
            {
                animation.Unload();
            }
        }
    }

    /// <summary>
    /// Get the connection manager on start
    /// </summary>
    private void Start()
    {
        _mConnectionManager = ChromaConnectionManager.Instance;

       AssignStaticLayer();

        // Make instances of animations in play mode for update events to work
        if (Application.isPlaying)
        {
            // instantiate 1D animations
            for (int i = 0; i < _mAnimations1D.Length; ++i)
            {
                _mAnimations1D[i] = (ChromaSDKAnimation1D)Instantiate(_mAnimations1D[i]);
            }

            // instantiate 2D animations
            for (int i = 0; i < _mAnimations2D.Length; ++i)
            {
                _mAnimations2D[i] = (ChromaSDKAnimation2D)Instantiate(_mAnimations2D[i]);
            }
        }
    }


    void AssignStaticLayer()
    {
        EffectInput input = GetEffectChromaStatic(_StaticColor);
        //Assigns Random
        keyboardActive = ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keyboard);
        //Sets Q to green
        keyboardActive[2][2] = ChromaUtils.ToBGR(Color.green);
    }


    void ApplyStaticLayer()
    {
        while (null == _mConnectionManager)
        {
            GUILayout.Label("Waiting for start...");
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        ChromaUtils.RunOnThread(() =>
        {
            EffectInput input = GetEffectChromaStatic(_StaticColor);
            //keyboardActive = ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keyboard);
            chromaApi.PutKeyboardCustom(keyboardActive);
        });
    }

    void Update()
    {
        if (!ChromaLive && _mConnectionManager != null)
        {
            ApplyStaticLayer();
        }
    }
}
