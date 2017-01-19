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
 
function Update () {
    Chrom.AssignAll(255,50,50,1);
    //Chrom.AssignKey(0,250,0,1,1,1);
    Chrom.AssignKey(0xFF0000,2,3,2);
    //Chrom.AssignKey(0,0,0,1,1,2);
    Chrom.Update();
}

function OnApplicationQuit() {
    Chrom.OnApplicationQuit();
}