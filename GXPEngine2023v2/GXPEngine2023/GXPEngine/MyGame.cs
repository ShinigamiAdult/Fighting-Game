using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Collections.Generic;
public class MyGame : Game
{
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

        LoadLevel("TestMap.tmx");
    }

    public void LoadLevel(string filename)
    {
        if (GameLevel != null)
            GameLevel.Destroy();
        GameLevel = new GameLevel(filename);
        AddChild(GameLevel);
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(GameLevel.currentlevelName);
    }

    void Update()
    {
    }

    static void Main()
    {
        new MyGame().Start();
    }


}