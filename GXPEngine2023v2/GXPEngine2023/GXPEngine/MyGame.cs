using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
public class MyGame : Game
{
    public CharacterSelectLevel CharecterSelect;
    public GameLevel GameLevel;
    public Controller1 controller1;
    public Controller2 controller2;
    public MyGame() : base(1158, 670, true, false, 1920, 1080, true)
    {
        game.targetFps = 60;
        controller1 = new Controller1();
        AddChild(controller1);

        controller2 = new Controller2();
        AddChild(controller2);

        //LoadGameLevel("TestMap.tmx");
        LoadCharecterSelect("CharecterSelect.tmx");
    }

    public void LoadGameLevel(string filename)
    {
        int char1 = CharecterSelect.manager.GetIndex1();
        int char2 = CharecterSelect.manager.GetIndex2();
        CharecterSelect.Destroy();
        if (GameLevel != null)
            GameLevel.Destroy();
        GameLevel = new GameLevel(filename,char1,char2);
        AddChild(GameLevel);
    }
    public void LoadCharecterSelect(string filename)
    {
        if (CharecterSelect != null)
            CharecterSelect.Destroy();
        CharecterSelect = new CharacterSelectLevel(filename);
        AddChild(CharecterSelect);
    }
    public void LoadCurrentLevel()
    {
        LoadGameLevel(GameLevel.currentlevelName);
    }

    void Update()
    {
    }

    static void Main()
    {
        new MyGame().Start();
    }


}