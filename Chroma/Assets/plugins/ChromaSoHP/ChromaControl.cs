/**
using UnityEngine;
using System.Collections;
using Corale.Colore;
using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;

using ColoreColor = Corale.Colore.Core.Color;
using ColoreKeyboard = Corale.Colore.Razer.Keyboard;

public class ChromaControl : MonoBehaviour
{
    public int ambientColor;

    public static void Start (int ambient)
    {
        ambientColor = ambient;
        colorRefresh();
    }

    public static void colorRefresh()
    {
        var keyboardGrid = KeyboardCustom.Create();
        keyboardGrid.Set(ColoreColor.FromRgb(0x1A0000));
        //keyboardGrid[Key.Z] = ColoreColor.White;
        //keyboardGrid[Key.X] = ColoreColor.White;
        //keyboardGrid[Key.C] = ColoreColor.White;
        Chroma.Instance.Keyboard.SetCustom(keyboardGrid);
        Chroma.Instance.Unregister();
    }
}

/*
    public static void chroma_update()
    {

    }

    public static void chroma_set()
    {

    }

    public static void chroma_damage()
    {

    }

    protected void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }




    public static void chroma_init(int skill1, int skill2, int skill3)
    {
        var color1 = ColoreColor.White;
        var color2 = ColoreColor.White;
        var color3 = ColoreColor.White;

        switch (skill1)
        {
            case 0:
                //Red
                color1 = ColoreColor.FromRgb(0xFF0000);
                break;
            case 1:
                //Yellow
                color1 = ColoreColor.FromRgb(0xFFAA00);
                break;
            case 2:
                color1 = ColoreColor.White;
                break;
        }

        switch (skill2)
        {
            case 0:
                //Blue
                color2 = ColoreColor.FromRgb(0x0066CC);
                break;
            case 1:
                //Blue
                color2 = ColoreColor.FromRgb(0x0066CC);
                break;
            case 2:
                //Green
                color2 = ColoreColor.FromRgb(0x0073E6);
                break;
        }

        switch (skill3)
        {
            case 0:
                //Green
                color3 = ColoreColor.FromRgb(0x009900);
                break;
            case 1:
                //Yellow
                color3 = ColoreColor.FromRgb(0xFFAA00);
                break;
            case 2:
                //Red
                color3 = ColoreColor.FromRgb(0xFF0000);
                break;
        }}

        //if (skill1 == 0)
        //{
        //    color1 = ColoreColor.Red;
        //}
        //else if (skill1 == 1)
        //{
        //    color1 = ColoreColor.Blue;
        //}
        //else if (skill1 == 2)
        //{
        //    color1 = ColoreColor.Yellow;
        //}
        //else
        //{
        //    color1 = ColoreColor.White;
        //}

        /*
        var keyboardGrid = KeyboardCustom.Create();
        // Set the whole Grid to Red
        keyboardGrid.Set(ColoreColor.Red);
        keyboardGrid.Set(ColoreColor.FromRgb(0x1A0000));
        keyboardGrid[Key.Z] = color1;
        keyboardGrid[Key.X] = color2;
        keyboardGrid[Key.C] = color3;
        Chroma.Instance.Keyboard.SetCustom(keyboardGrid);
        Chroma.Instance.Unregister();
    } }
    */

/*(public static void chroma_init(int skill1)
{
    var color1 = ColoreColor.White;
    var color2 = ColoreColor.White;
    var color3 = ColoreColor.White;

    if (skill1==0)
    {
        color1 = ColoreColor.Red;
    } else if (skill1 == 1)
    {
        color1 = ColoreColor.Blue;
    } else if (skill1 == 2)
    {
        color1 = ColoreColor.Yellow;
    } else
    {
        color1 = ColoreColor.White;
    }

    var keyboardGrid = KeyboardCustom.Create();
    // Set the whole Grid to Red
    keyboardGrid.Set(ColoreColor.Red);
    keyboardGrid.Set(ColoreColor.FromRgb(0x1A0000));
    keyboardGrid[Key.Z] = color1;
    keyboardGrid[Key.X] = color2;
    keyboardGrid[Key.C] = color3;
    Chroma.Instance.Keyboard.SetCustom(keyboardGrid);
    Chroma.Instance.Unregister();
}
}
*/
