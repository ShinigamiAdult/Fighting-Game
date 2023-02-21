using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
public class MyGame : Game
{
    public CharacterSelectLevel CharacterSelect;
    public UILevel UILevel;
    public GameLevel GameLevel;
    public Controller1 controller1;
    public Controller2 controller2;
    //1158, 670
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
        int char1 = CharacterSelect.manager.GetIndex1();
        int char2 = CharacterSelect.manager.GetIndex2();
        CharacterSelect.Destroy();
        if (GameLevel != null)
            GameLevel.Destroy();
        GameLevel = new GameLevel(filename,char1,char2);
        AddChild(GameLevel);

        LoadUILevel("UI.tmx");
    }
    public void LoadCharecterSelect(string filename)
    {
        if (CharacterSelect != null)
            CharacterSelect.Destroy();
        CharacterSelect = new CharacterSelectLevel(filename);
        AddChild(CharacterSelect);
    }

    public void LoadUILevel(string filename)
    {
        if (UILevel != null)
            UILevel.Destroy();
        UILevel = new UILevel(filename);
        AddChild(UILevel);
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