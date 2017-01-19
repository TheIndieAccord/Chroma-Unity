#pragma strict
private var Chrom : ChromaBasics;
function Start () {
    Chrom = new ChromaBasics();
}
 
function Update () {
    Chrom.AssignKey(255,50,50,2,1,1);
    Chrom.AssignKey(0,250,0,1,1,1);
    Chrom.AssignKey(250,0,0,2,1,2);
    Chrom.AssignKey(0,0,0,1,1,2);
    Chrom.Update();
}

function OnApplicationQuit() {
    Chrom.OnApplicationQuit();
}