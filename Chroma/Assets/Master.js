#pragma strict

function Start () {
    var Chroma = new ChromaBasics();
    //ChromaKeyboard.SetKey("Z");
    //Chroma.AssignKey("O");
}

function Update () {
    ChromaBasics.Update();
}

function OnApplicationQuit() {
    ChromaBasics.OnApplicationQuit();
}