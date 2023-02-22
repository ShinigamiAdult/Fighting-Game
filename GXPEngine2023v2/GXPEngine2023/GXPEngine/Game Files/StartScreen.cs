using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
public class StartScreen : Level
{
    //bool respawn = false; used for smart respawn
    public StartScreen(string filename) : base(filename, 0,0)
    {
    }

   /* public override void CreateLevel(int i = 0, int y = 0)
    {
        loader.highQualityText = true;
        loader.rootObject = this;
        loader.autoInstance = true;
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.LoadObjectGroups();
    }*/

    void Update()
    {
        if(Input.GetKeyDown(Key.P))
        {
            ((MyGame)game).LoadCharecterSelect("CharecterSelect.tmx");
        }
    }
}