/**
 * Name: CromaBasics.cs
 * Author: Hunter Dubel
 * Description: Basic functions for Chroma in Unity with layer functionality.
 * Updated: January 19, 2017 00:39
 **/

using UnityEngine;

using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;

using ColoreColor = Corale.Colore.Core.Color;

public class ChromaBasics : MonoBehaviour
{
    private ColoreColor[,] layer1 = new ColoreColor[Constants.MaxRows, Constants.MaxColumns];
    private ColoreColor[,] layer2 = new ColoreColor[Constants.MaxRows, Constants.MaxColumns];

    public static void OnApplicationQuit()
    {
        Chroma.Instance.Uninitialize();
    }

    public ChromaBasics()
    {

    }

    public void AssignAll(int r, int g, int b, int layer)
    {
        ColoreColor col = new ColoreColor(r, g, b);
   
        switch (layer)
        {
            case 1:
                //Loop through all Rows
                for (var rows = 0; rows < Constants.MaxRows; rows++)
                {
                    //Loop through all Columns
                    for (var c = 0; c < Constants.MaxColumns; c++)
                    {
                        layer1[rows, c] = col;
                    }
                }
                break;
            case 2:
                //Loop through all Rows
                for (var rows = 0; rows < Constants.MaxRows; rows++)
                {
                    //Loop through all Columns
                    for (var c = 0; c < Constants.MaxColumns; c++)
                    {
                        layer2[rows, c] = col;
                    }
                }
                break;
            default:
                //Loop through all Rows
                for (var rows = 0; rows < Constants.MaxRows; rows++)
                {
                    //Loop through all Columns
                    for (var c = 0; c < Constants.MaxColumns; c++)
                    {
                        layer1[rows, c] = col;
                    }
                }
                break;
        }
    }

    public void AssignKey(int r, int g, int b, int layer, int x, int y)
    {
        ColoreColor col = new ColoreColor(r, g, b);
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

    public void AssignKey(uint hexcol, int layer, int x, int y)
    {
        ColoreColor col = new ColoreColor(ColoreColor.FromRgb(hexcol));
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
        //Loop through all Rows
        for (var r = 0; r < Constants.MaxRows; r++)
        {
            //Loop through all Columns
            for (var c = 0; c < Constants.MaxColumns; c++)
            {
                ColoreColor empty = new ColoreColor(0, 0, 0);
                if (layer1[r, c] == empty)
                    Chroma.Instance.Keyboard[r, c] = layer2[r, c];
                else
                    Chroma.Instance.Keyboard[r, c] = layer1[r, c];
            }
        }
    }

}