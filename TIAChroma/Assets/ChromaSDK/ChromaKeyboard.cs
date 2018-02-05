// Access to Types and Utils
using ChromaSDK;
// Access to Chroma data structures
using ChromaSDK.ChromaPackage.Model;
// Access to the Chroma API
using ChromaSDK.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChromaKeyboard
{
    private ChromaConnectionManager _mConnectionManager = null;

    public Color _BaseColor;

    //Layers
    private EffectArray2dInput keyboardActive = new EffectArray2dInput();
    private EffectArray2dInput keyboardDynamic = new EffectArray2dInput();
    private EffectArray2dInput keyboardStatic = new EffectArray2dInput();

    public ChromaKeyboard()
    {
        //AssignStaticLayer(Color.black);
        ApplyStaticLayer();
    }

    public void AssignStaticLayer(Color col)
    {
        //Required for each Assign.//
        //while (null == _mConnectionManager) { }
        //ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;
        //Required for each Assign.//

        //Avoids Slowdowns
        //ChromaUtils.RunOnThread(() =>
        //{
           // Color color;
            //ColorUtility.TryParseHtmlString(col, out color);
            //EffectInput input = GetEffectChromaStatic(color);
            //keyboardActive = ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keyboard);
            ////keyboardActive[2][2] = ChromaUtils.ToBGR(Color.green);
            //chromaApi.PutKeyboardCustom(keyboardActive);

            //chromaApi.PutKeyboard(input);
        //});
    }

    public void ApplyStaticLayer()
    {
        //Required for each Assign.//
        while (null == _mConnectionManager) { }
        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;

        ChromaUtils.RunOnThread(() =>
        {
            //EffectInput input = GetEffectChromaStatic(_StaticColor);
            keyboardActive = ChromaUtils.CreateRandomColors2D(ChromaDevice2DEnum.Keyboard);
            //keyboardActive[2][2] = ChromaUtils.ToBGR(Color.green);
            chromaApi.PutKeyboardCustom(keyboardActive);

            //chromaApi.PutKeyboard(input);
        });
    }

    // Use this for initialization
    void Start()
    {
        _mConnectionManager = ChromaConnectionManager.Instance;

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
}
