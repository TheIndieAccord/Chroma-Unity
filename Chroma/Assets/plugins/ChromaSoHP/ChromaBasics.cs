using UnityEngine;
using System;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;
using Corale.Colore.Razer.Keyboard;


public class ChromaBasics : MonoBehaviour {

    uint ambientColor;

    public ChromaBasics()
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

    // Use this for initialization
    //public static void Start () {
    //    Chroma.Instance.SetAll(ColoreColor.FromRgb(ambientColor));
    //    var color1 = ColoreColor.White;
    //}
	
    public static void OnApplicationQuit ()
    {
        Chroma.Instance.Uninitialize();
    }

	// Update is called once per frame
	//void Update () {
	
	//}
}