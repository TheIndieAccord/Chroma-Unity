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

    //Constants
	private readonly static int KEYBOARD_ROWS = ChromaUtils.GetMaxRow(ChromaDevice2DEnum.Keyboard);
	private readonly static int KEYBOARD_COLS = ChromaUtils.GetMaxColumn(ChromaDevice2DEnum.Keyboard);

    //Static Layer. Assignable through Unity UI.
    public Color _StaticColor;

    //Dynamic Layers
    public ChromaSDKAnimation2D _LowHealthEffect = null;
    public ChromaSDKAnimation2D _CutsceneEffect = null;

    //Keyboard Layers
    //private Color[ , ] topLayer = new Color[KEYBOARD_ROWS,KEYBOARD_COLS];
    private ChromaSDKAnimation2D[ , ] dynamicLayer = new ChromaSDKAnimation2D[KEYBOARD_ROWS, KEYBOARD_COLS];
    private Color[ , ] topLayer = new Color[KEYBOARD_ROWS,KEYBOARD_COLS];
    private Color[ , ] baseLayer = new Color[KEYBOARD_ROWS,KEYBOARD_COLS];

    //Keyboard Grid
    private EffectArray2dInput keyboardGrid = new EffectArray2dInput();

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

    ///// <summary>
    ///// Show status label
    ///// </summary>
    //private string _mTextStatus;

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

        InitializeLayers();
        AssignBaseLayer(Color.red);
        AssignTopLayer(2, 2, Color.cyan);

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

    void InitializeLayers()
    {
        keyboardGrid = ChromaUtils.CreateColors2D(ChromaDevice2DEnum.Keyboard);
        for (int r = 0; r < KEYBOARD_ROWS; r++)
        {
            for (int c = 0; c < KEYBOARD_COLS; c++)
            {
                baseLayer[r, c] = Color.black;
                topLayer[r,c] = Color.black;
            }
        }

    }

    /// <summary>
    /// Assigns a single static color to the Static/Base/Ambient layer of the system. If no color is set, defaults to off.
    /// </summary>
    /// <param name="col">Color to be applied.</param>
    void AssignBaseLayer(Color col)
    {
    	for (int r = 0; r < KEYBOARD_ROWS; r++)
    	{
    		for (int c = 0; c < KEYBOARD_COLS; c++)
    		{
        		baseLayer[r, c] = col;
    		}
    	}
    }

    /// <summary>
    /// Assigns a color to the specified keyboard location. 0,0 is top left.
    /// </summary>
    /// <param name="r">Row. Begins at 0.</param>
    /// <param name="c">Column. Begins at 0.</param>
    /// <param name="col">Color to be applied. Black by default.</param>
    void AssignTopLayer(int r, int c, Color col)
    {
        topLayer[r, c] = col;
    }

    /// <summary>
    /// Iterates through the layers and checks for depth priority. Applies the uppermost layer to the keyboardGrid to be used.
    /// </summary>
    void ApplyLayers()
    {
        while (null == _mConnectionManager)
        {
            GUILayout.Label("Waiting for Chroma to start...");
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        ChromaUtils.RunOnThread(() =>
        {
            if (_mPlayAnimation)
            {
                //Play animation then apply top layer

                for (int r = 0; r < KEYBOARD_ROWS; r++)
                {
                    for (int c = 0; c < KEYBOARD_COLS; c++)
                    {
                        if (topLayer[r, c].Equals(Color.black))
                            keyboardGrid[r][c] = ChromaUtils.ToBGR(topLayer[r, c]);
                    }
                }
            }
            else
            {
                //Loops through all of the rows & columns
                for (int r = 0; r < KEYBOARD_ROWS; r++)
                {
                    for (int c = 0; c < KEYBOARD_COLS; c++)
                    {
                        //if (topLayer[r, c].Equals(Color.black))
                        //if (topLayer.Colors.Equals(Color.black))
                        {
                            if (topLayer[r, c].Equals(Color.black))
                                keyboardGrid[r][c] = ChromaUtils.ToBGR(baseLayer[r, c]);
                            else
                                keyboardGrid[r][c] = ChromaUtils.ToBGR(topLayer[r, c]);
                        }
                        //else
                        //keyboardGrid[r][c] = ChromaUtils.ToBGR(Color.yellow);
                        //keyboardGrid[r][c] = topLayer.Frames[r][c].;
                        //  keyboardGrid[r][c] = ChromaUtils.ToBGR(topLayer[r, c]);
                    }
                }
            }
            chromaApi.PutKeyboardCustom(keyboardGrid);
        });
    }

    void Update()
    {
        if (_mConnectionManager != null)
        {
            ApplyLayers();
        }
    }
}
