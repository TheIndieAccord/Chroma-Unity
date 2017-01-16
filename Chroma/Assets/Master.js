#pragma strict

function Start () {
    //var ChromaKeyboard = new ChromaKeyboard();
    //ChromaKeyboard.SetKey("Z");

    var Chroma = new ChromaBasics();
    
    //var Chroma2 = new ChromaBasics2();
    //Chroma.AssignKey("O");
    Chroma.Update();
    //Chroma2.Update();
}

function Update () {
}

function OnApplicationQuit() {
    ChromaBasics.OnApplicationQuit();
}