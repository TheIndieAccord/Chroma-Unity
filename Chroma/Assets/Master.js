#pragma strict

function Start () {
    var Chroma = new ChromaBasics();
    Chroma.SetKey("Z");
}

function Update () {
}

function OnApplicationQuit() {
    ChromaBasics.OnApplicationQuit();
}