﻿using UnityEngine;
using System;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;
using Corale.Colore.Razer.Keyboard;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;


public class ChromaBasics : MonoBehaviour
{
    private uint ambientColor = 0x1A0000;

    private KeyboardCustom keyboardGridAmb = KeyboardCustom.Create();
    private KeyboardCustom keyboardGridAct = KeyboardCustom.Create();
    private KeyboardCustom keyboardGridDyn = KeyboardCustom.Create();

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    public ChromaBasics()
    {
        keyboardGridAmb.Set(ColoreColor.FromRgb(ambientColor));
        //keyboardGridAmb.Set(ColoreColor.FromRgb(ambientColor));
        //keyboardGridAct[Key.Q] = ColoreColor.Blue;
    }

    public void AssignKey(int level = 0, string inputKey)
    {
        KeyboardCustom layer;
        switch (level)
        {
            case 1:
                layer = keyboardGridAct;
                break;
            case 2:
                layer = keyboardGridDyn;
                break;
            default:
                layer = keyboardGridAmb;
                break;
        }

        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            if (key.ToString() == inputKey)
            {
                layer[key] = ColoreColor.FromRgb(0x000000);
            }
        }
    }

    public void Update()
    {
            Chroma.Instance.Keyboard.SetCustom(keyboardGridAmb);
            //Chroma.Instance.Keyboard.SetCustom(keyboardGridAct);
            //Chroma.Instance.Keyboard.SetCustom(keyboardGridDyn);
    }

/*    public void SetKey(string inputKey)
    {
        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            if (key.ToString() == inputKey)
            {
                Chroma.Instance.Keyboard.SetKey(key, ColoreColor.FromRgb(0x001A00));
            }
        }
    }
    */

}