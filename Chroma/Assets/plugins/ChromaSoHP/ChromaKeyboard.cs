using UnityEngine;
using System;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;
using Corale.Colore.Razer.Keyboard;


public class ChromaKeyboard : MonoBehaviour
{

    uint ambientColor;

    void Start()
    {

    }

    void Update()
    {

    }

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    public ChromaKeyboard()
    {
        ambientColor = 0x1A0000;
        Chroma.Instance.SetAll(ColoreColor.FromRgb(ambientColor));
    }

    public void SetKey(string inputKey)
    {
        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            if (key.ToString() == inputKey)
            {
                Chroma.Instance.Keyboard.SetKey(key, ColoreColor.FromRgb(0x001A00));
            }
        }
    }
}