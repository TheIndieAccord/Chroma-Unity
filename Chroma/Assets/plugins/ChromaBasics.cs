using System;
using UnityEngine;

using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;

using ColoreColor = Corale.Colore.Core.Color;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;


public class ChromaBasics : MonoBehaviour
{
    private uint ambientColor = 0xFFFF0000;

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
        keyboardGridAct[Key.A] = ColoreColor.FromRgb(0x00000000);
        //keyboardGridAmb.Set(ColoreColor.FromRgb(ambientColor));
        //keyboardGridAct[Key.Q] = ColoreColor.Blue;

    }

    public void AssignKey(string inputKey, int level = 0)
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

    public static void Update()
    {
        Chroma.Instance.Keyboard.SetBreathing(ColoreColor.Red, ColoreColor.White);
        Chroma.Instance.Keyboard.SetKey(Key.A, ColoreColor.White, true);
        //Chroma.Instance.Keyboard.SetCustom(keyboardGridAmb);
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