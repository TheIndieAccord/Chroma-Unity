/**
 * Name: Master.js
 * Author: Hunter Dubel
 * Description: Sample script to test functionality of ChromaBasics.cs
 * Updated: January 19, 2017 00:39
 **/

#pragma strict
private var Chrom : ChromaBasics;
function Start () {
    Chrom = new ChromaBasics();
}

public void Update()
{
    for (var r = 0; r < Constants.MaxRows; r++)
    {
        //Loop through all Columns
        for (var c = 0; c < Constants.MaxColumns; c++)
        {
            // Set the current row and column to the random color
            Chroma.Instance.Keyboard[r, c] = new ColoreColor(UnityEngine.Random.Range(0.0f, 255.0f), UnityEngine.Random.Range(0.0f, 255.0f), UnityEngine.Random.Range(0.0f, 255.0f));
      /*      if (layer1[r, c]==empty)
                Chroma.Instance.Keyboard[r, c] = layer2[r,c];
            else
                Chroma.Instance.Keyboard[r, c] = layer1[r, c];
        */}
    }
}

function OnApplicationQuit() {
    Chrom.OnApplicationQuit();
}
