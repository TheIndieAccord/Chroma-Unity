#pragma strict

function Start() {
    var Chroma = new ChromaBasics(0x1A0000);
    //Chroma.SetKey("Z", 0xBB1A00);
}

function Update () {
}

function OnApplicationQuit() {
    ChromaBasics.OnApplicationQuit();
}