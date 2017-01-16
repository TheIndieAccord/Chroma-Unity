#pragma strict

function Start () {
    var ChromaKeyboard = new ChromaKeyboard();
    ChromaKeyboard.SetKey("Z");
}

function Update () {
}

function OnApplicationQuit() {
    ChromaBasics.OnApplicationQuit();
}