using UnityEngine;
using System;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;
using Corale.Colore.Razer.Keyboard;


class ChromaBasics {

    //uint ambientColor;

    public ChromaBasics(uint ambientColor = 0x1A0000)
    {
        Chroma.Instance.SetAll(ColoreColor.FromRgb(ambientColor));
    }

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    //Sets the color of a specific key. Color is defined in hexadecimal.
    public void SetKey(string inputKey, uint colorKey)
    {
        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            if (key.ToString() == inputKey)
            {
                Chroma.Instance.Keyboard.SetKey(key, ColoreColor.FromRgb(colorKey));
            }
        }
    }



/*

    public void colorAll(uint chosenColor)
        {
            Chroma.Instance.SetAll(ColoreColor.FromRgb(chosenColor));
        }

        //Sets the entire color of the headset. Color is defined in hexadecimal.
        public void SetHeadset(uint colorHeadset)
        {
            Chroma.Instance.Headset.SetAll(ColoreColor.FromRgb(colorHeadset));
        }

        //Sets the entire color of the mouse. Color is defined in hexadecimal.
        public void SetMouse(uint colorMouse)
        {
            Chroma.Instance.Mouse.SetAll(ColoreColor.FromRgb(colorMouse));
        }

        //Sets the entire color of the mousepad. Color is defined in hexadecimal.
        public void SetMousepad(uint colorPad)
        {
            Chroma.Instance.Mousepad.SetAll(ColoreColor.FromRgb(colorPad));
        }

        //Sets the entire color of the keyboard. Color is defined in hexadecimal.
        public void SetKeyboard(uint colorKeyboard)
        {
            Chroma.Instance.Keyboard.SetAll(ColoreColor.FromRgb(colorKeyboard));
        }

        //Strobe
        public void LowHealth(bool active)
        {
            while(true)
            {
                colorAll(0xFF0000);
            }

        }

*/


    // Update is called once per frame
    //void Update () {
    //}
}