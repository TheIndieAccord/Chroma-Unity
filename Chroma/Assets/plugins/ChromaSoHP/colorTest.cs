using System;
using UnityEngine;
using Corale.Colore.Core;
using ColoreColor = Corale.Colore.Core.Color;

public class colorTest : MonoBehaviour { 
	void Main()	{
		Chroma.Instance.SetAll (ColoreColor.FromRgb(0x1A0000));
	}
}