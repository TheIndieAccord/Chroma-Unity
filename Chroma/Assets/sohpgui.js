import UnityEngine.SceneManagement;
var guiBG : Texture2D;
var guiEmpty : Texture2D;
var guiFull : Texture2D;
var font : GUIStyle;
var guiComboEmpty : Texture2D;
var guiComboFull : Texture2D;
var guiPaused : Texture2D;
private var quit_prompt = false;
var guiHurt : Texture;
private var hp = 0.0;
private var hurt_timer = 0.0;

var zSKill1Texture: Texture;
var zSKill2Texture: Texture;
var zSKill3Texture: Texture;
var xSKill1Texture: Texture;
var xSKill2Texture: Texture;
var xSKill3Texture: Texture;
var cSKill1Texture: Texture;
var cSKill2Texture: Texture;
var cSKill3Texture: Texture;

var guiSkillOutline : Texture2D;
var guiSkillZ : Texture2D;
var guiSkillX : Texture2D;
var guiSkillC : Texture2D;

public var cursorTexture: Texture2D;
public var cursorMode: CursorMode = CursorMode.Auto;
public var hotSpot: Vector2 = Vector2.zero;
private var Chrom : ChromaBasics;
private var alph : float = 0.0;
private var rw : float = 0.0;

function Start () {
    Chrom = gameObject.AddComponent.<ChromaBasics>();
    hp = herm_script.hp;
    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
}

function Update(){
    if (Input.GetKeyDown(KeyCode.Escape)) {
        quit_prompt = !quit_prompt;
    }

    //The base ambient layer
        // Chrom.AssignAll(0x190000,1);

        Chrom.AssignKeyboardHorizontal(0xCC0000, 3, 1);
        Chrom.AssignKeyboardHorizontal(0x990000, 3, 2);
        Chrom.AssignKeyboardHorizontal(0x660000, 3, 3);
        Chrom.AssignKeyboardHorizontal(0x330000, 3, 4);
        Chrom.AssignKeyboardHorizontal(0x100000, 3, 5);
        Chrom.AssignKeyboardVertical(0x00FF00, 1, 0);
        // Chrom.AssignKey(0xFF0000,1,1,1);
        // Chrom.AssignKey(0xB00000,1,2,1);
        // Chrom.AssignKey(0xA00000,1,3,1);
        // Chrom.AssignKey(0x300000,1,4,1);
        // Chrom.AssignKey(0x100000,1,5,1);

        // Chrom.AssignKey(0xFF0000,1,1,2);
        // Chrom.AssignKey(0xB00000,1,2,2);
        // Chrom.AssignKey(0xA00000,1,3,2);
        // Chrom.AssignKey(0x300000,1,4,2);
        // Chrom.AssignKey(0x100000,1,5,2);

        // Chrom.AssignKey(0xFF0000,1,1,3);
        // Chrom.AssignKey(0xB00000,1,2,3);
        // Chrom.AssignKey(0xA00000,1,3,3);
        // Chrom.AssignKey(0x300000,1,4,3);
        // Chrom.AssignKey(0x100000,1,5,3);

        // Chrom.AssignKey(0xFF0000,1,1,4);
        // Chrom.AssignKey(0xB00000,1,2,4);
        // Chrom.AssignKey(0xA00000,1,3,4);
        // Chrom.AssignKey(0x300000,1,4,4);
        // Chrom.AssignKey(0x100000,1,5,4);

    //Transitionary Layer
        // if (hurt==true)
        if (hurt_timer >= 0.0)
            alph = alph + 1.5*Time.deltaTime;
        else
            alph = 0.0;

    //Static Layer
        Chrom.AssignKey(0xFFFFFF,2,0,1);
        // Chrom.AssignKey(1,250,250,2,4,3);
        // Chrom.AssignKey(250,250,1,2,4,4);
        // Chrom.AssignKey(250,1,250,2,4,5);

        //Combo Bar. Falls on Static Layer
        hor_bar((herm_script.combo)%10, 10, 1, 1, 15, 1);
        if (rw<5.5)
            rw=rw+10.0*Time.deltaTime;
        else
          rw=0;
        if (rw>5.5){
           rw=5.5;
             row(150,150,0,rw,1);
        }
        if(alph<1.0)
            hurt(alph);
        vert_bar(herm_script.hp, 100, 1, 0, 5, 0);

    // Chrom.AssignLayerAll(0x900000,1);
    // Chrom.AllFd(255,255,255,2,true);
    // Chrom.AssignKey(100,0,160,1,1,0);
    // Chrom.AssignAll(0,0,0,1);
    // Chrom.AssignAll(0,0,0,2);

  //  Chrom.AllFade(255,0,0,1,false);
    Chrom.Update();

}

function OnGUI() {

    if (quit_prompt==true){
        GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), guiPaused);
        GUI.color=Color(1.0,1.0,1.0,0.0);
        if (GUI.Button(new Rect(Screen.width*.38,Screen.height*.602,Screen.width*.24,Screen.height*.07),"QUIT TO MENU")){
            SceneManager.LoadScene("equip_scene");
        }
        if (GUI.Button(new Rect(Screen.width*.38,Screen.height*.335,Screen.width*.24,Screen.height*.07),"NVM")){
            quit_prompt = false;;
        }
        GUI.color=Color(1.0,1.0,1.0,1.0);
    }

  //  GUI.Label(Rect(0,50,100,100),herm_script.combo.ToString());

    if (herm_script.hp<hp){
        hp = herm_script.hp;
        hurt_timer = 1.5;
        alph = 0.0;
    }

    if (hurt_timer >= 0.0){
        GUI.color.a = hurt_timer/2.0f;
        GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), guiHurt);
        hurt_timer -= Time.deltaTime;
    }
    GUI.color.a = 0.8f;

	GUI.DrawTexture(Rect(-Screen.width*0.027,Screen.height-0.25*Screen.width,Screen.width/2,Screen.width*.25), guiBG);
	GUI.color.a = 1.0f;
	
//	GUI.DrawTexture(Rect(-Screen.width*0.02,Screen.height-0.23*Screen.width,Screen.width*.25,Screen.width*.25), guiEmpty);
//	Graphics.DrawTexture(Rect(0,0,100,100), guiEmpty, new Rect(0, 0, 100, 100), 100,100,100,100);
	var texture = guiComboEmpty;
	var position = new Vector2( -0.005*Screen.width+0.09*Screen.width, Screen.height-0.21*Screen.width+0.01*Screen.width );
	
	GUI.DrawTexture(Rect( position.x, position.y, guiComboFull.width * 0.17*Screen.width/1600, guiComboFull.height * 0.16 *Screen.width/1600), guiComboFull);
	var textureCrop = new Rect( 0.0f, 0.0f, 1.0f, /*0.16*.25+*/0.16*(1-herm_script.combo_timer/5.0) );
	GUI.BeginGroup( new Rect( position.x, position.y, texture.width * textureCrop.width, texture.height * textureCrop.height*Screen.width/1600 ) );
	//GUI.DrawTexture(Rect( position.x+0.09*Screen.width, position.y+0.01*Screen.width, guiComboEmpty.width * 0.17*Screen.width/1600, guiComboEmpty.height * 0.16 *Screen.width/1600), guiComboEmpty);
	    GUI.DrawTexture( new Rect( -texture.width * textureCrop.x, -texture.height * textureCrop.y, texture.width*0.17*Screen.width/1600 , texture.height*0.16*Screen.width/1600  ), guiComboEmpty );
	GUI.EndGroup();

	position = new Vector2( -0.005*Screen.width, Screen.height-0.21*Screen.width );
	
	texture = guiEmpty;
	textureCrop = new Rect( 0.0f, 0.0f, 1.0f, 0.17*.25+0.17*0.5*(1-herm_script.hp/100.0) );
    

    GUI.DrawTexture(Rect( position.x, position.y, texture.width * 0.17*Screen.width/1600, texture.height * 0.17 *Screen.width/1600), guiFull);
	
    GUI.BeginGroup( new Rect( position.x, position.y, texture.width * textureCrop.width, texture.height * textureCrop.height*Screen.width/1600 ) );
    GUI.DrawTexture( new Rect( -texture.width * textureCrop.x, -texture.height * textureCrop.y, texture.width*0.17*Screen.width/1600 , texture.height*0.17*Screen.width/1600  ), texture );
    GUI.EndGroup();

	if (herm_script.attack1==0)
		guiSkillZ = zSKill1Texture;
	else if (herm_script.attack1==1)
		guiSkillZ = zSKill2Texture;
	else 
		guiSkillZ = zSKill3Texture;
		
	if (herm_script.attack2==0)
		guiSkillX = xSKill1Texture;
	else if (herm_script.attack2==1)
		guiSkillX = xSKill2Texture;
	else 
		guiSkillX = xSKill3Texture;
	
	if (herm_script.attack3==0)
		guiSkillC = cSKill1Texture;
	else if (herm_script.attack3==1)
		guiSkillC = cSKill2Texture;
	else 
		guiSkillC = cSKill3Texture;
	
	
	GUI.DrawTexture(Rect(Screen.width*0.78,Screen.height-0.07*Screen.width,Screen.width*0.05,Screen.width*0.05), guiSkillZ);
	GUI.DrawTexture(Rect(Screen.width*0.77,Screen.height-0.08*Screen.width,Screen.width*0.07,Screen.width*0.07), guiSkillOutline);
	
	GUI.DrawTexture(Rect(Screen.width*0.78+Screen.width*0.05*1.5,Screen.height-0.07*Screen.width,Screen.width*0.05,Screen.width*0.05), guiSkillX);
	GUI.DrawTexture(Rect(Screen.width*0.77+Screen.width*0.05*1.5,Screen.height-0.08*Screen.width,Screen.width*0.07,Screen.width*0.07), guiSkillOutline);
	
	GUI.DrawTexture(Rect(Screen.width*0.78+Screen.width*0.05*3,Screen.height-0.07*Screen.width,Screen.width*0.05,Screen.width*0.05), guiSkillC);
	GUI.DrawTexture(Rect(Screen.width*0.77+Screen.width*0.05*3,Screen.height-0.08*Screen.width,Screen.width*0.07,Screen.width*0.07), guiSkillOutline);

//	GUI.Label(Rect(Screen.width*.032,Screen.height*.57,Screen.width*.1,Screen.width*.1),herm_script.combo.ToString(),font);
}

function OnApplicationQuit() {
    Chrom.OnApplicationQuit();
}

function hor_bar(hp:float, maxhp:float, layer:int, ystart:int, yend:int, xstart:int){
    for (var i:int=ystart;i<=yend;i++){
        if ( (hp/maxhp) > (parseFloat(i-1)/(parseFloat(yend-ystart))))
            Chrom.AssignKey(250*(1.0-herm_script.combo_timer/5.0),250*(1.0-herm_script.combo_timer/5.0),0,layer,xstart,ystart+i);
    else
            Chrom.AssignKey(0,0,0,layer,xstart,ystart+i);
    }
}

function vert_bar(hp:float, maxhp:float, layer:int, xstart:int, xend:int, ystart:int){
    for (var i:int=xstart;i<=xend;i++){
        if ( (hp/maxhp) > (parseFloat(i)/(parseFloat(xend-xstart))))
            Chrom.AssignKey(150,0,0,layer,xend-i,ystart);
    else
            Chrom.AssignKey(0,0,0,layer,xend-i,ystart);
    }
}

function hurt(fd:float){
    row(255.0*fd,0,0,0,1);
    row(255.0*fd,0,0,5,1);
    col(255.0*fd,0,0,0,1);
    col(255.0*fd,0,0,21,1);
}


    function row(r:int, g:int, b:int, row:int, layer:int){
    

        var width = 0;
        if (row == 0)
            width = 17;
        else if (row == 1)
            width = 21;
        else if (row == 2)
            width = 21;
        else if (row == 3)
            width = 21;
        else if (row == 4)
            width = 21;
        else if (row == 5)
            width = 21;

    
        for (var i:int=0;i<=width;i++){
            Chrom.AssignKey(r,g,b,layer,row,i);
        }
    }

    function col(r:int, g:int, b:int, row:int, layer:int){
    

        var width = 0;
        if (row == 0)
            width = 4;
        else if (row == 21)
            width = 5;
        else if (row == 2)
            width = 21;
        else if (row == 3)
            width = 21;
        else if (row == 4)
            width = 21;
        else if (row == 5)
            width = 21;

    
        for (var i:int=0;i<=width;i++){
            Chrom.AssignKey(r,g,b,layer,i,row);
        }
    }