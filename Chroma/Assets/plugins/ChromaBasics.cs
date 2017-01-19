using System;
using UnityEngine;

using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;

using ColoreColor = Corale.Colore.Core.Color;
using KeyboardCustom = Corale.Colore.Razer.Keyboard.Effects.Custom;

public class ChromaBasics : MonoBehaviour
{
    private uint ambientColor = 0xFFFF0000;
    private ColoreColor[,] layer1 = new ColoreColor[Constants.MaxRows, Constants.MaxColumns];
    private ColoreColor[,] layer2 = new ColoreColor[Constants.MaxRows, Constants.MaxColumns];

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    public ChromaBasics()
    {

    }

    public void AssignKey(int r, int g, int b, int layer, int x, int y)
    {
        ColoreColor col = new ColoreColor(r, g, b);
        // Chroma.Instance.Keyboard[x, y] =col;
        layer1[x, y] = col;

        switch (layer)
        {
            case 1:
                layer1[x, y] = col;
                break;
            case 2:
                layer2[x, y] = col;
                break;
            default:
                layer1[x, y] = col;
                break;
        }
    }

    public void Update()
    {
        for (var r = 0; r < Constants.MaxRows; r++)
        {
            //Loop through all Columns
            for (var c = 0; c < Constants.MaxColumns; c++)
            {
                // Set the current row and column to the random color
                ColoreColor empty = new ColoreColor(0, 0, 0);
                if (layer1[r, c] == empty)
                    Chroma.Instance.Keyboard[r, c] = layer2[r, c];
                else
                    Chroma.Instance.Keyboard[r, c] = layer1[r, c];
            }
        }
    }

}