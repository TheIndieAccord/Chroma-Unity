using UnityEngine;
using System;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;
using Corale.Colore.Razer.Keyboard;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;


public class ChromaKeyboard : MonoBehaviour
{
    uint ambientColor = 0x1A0000;

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    public ChromaKeyboard()
    {
        var keyboardGrid = KeyboardCustom.Create();
        keyboardGrid.Set(ColoreColor.Red);
        keyboardGrid[Key.Z] = ColoreColor.White;
        keyboardGrid[Key.X] = ColoreColor.White;
        keyboardGrid[Key.C] = ColoreColor.White;
        Chroma.Instance.Keyboard.SetCustom(keyboardGrid);
    }

    /*    void Update()
        {
            Chroma.Instance.Keyboard.SetCustom();
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
     */
}