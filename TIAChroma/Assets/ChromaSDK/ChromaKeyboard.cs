// Access to Types and Utils
using ChromaSDK;
// Access to Chroma data structures
using ChromaSDK.ChromaPackage.Model;
// Access to the Chroma API
using ChromaSDK.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChromaKeyboard : MonoBehaviour
{
    private ChromaConnectionManager _mConnectionManager = null;

    //public ChromaSDKAnimation2D[] _KeyboardLayers = new ChromaSDKAnimation2D[1];
    public Color _BaseColor;

    public ChromaKeyboard()
    {
        ApplyStaticLayer();
    }

    private void ApplyStaticLayer()
    {
        if (!_mConnectionManager.Connected)
        {
            Debug.LogError("Chroma client is not yet connected!");
            return;
        }

        ChromaApi chromaApi = _mConnectionManager.ApiChromaInstance;
        int bgrInt = ChromaUtils.ToBGR(_BaseColor);
        chromaApi.PutKeyboardStatic(bgrInt);
    }

    // Use this for initialization
    void Start()
    {
        _mConnectionManager = ChromaConnectionManager.Instance;

    }
}
