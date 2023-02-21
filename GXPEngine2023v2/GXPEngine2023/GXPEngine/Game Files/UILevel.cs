﻿using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
public class UILevel : Level
{
    //bool respawn = false; used for smart respawn
    public UILevel(string filename) : base(filename, 0, 0)
    {
        //Console.WriteLine("Game Width: " + game.width);
    }

    public override void CreateLevel(int i = 0, int y = 0)
    {
        loader.highQualityText = true;
        loader.rootObject = this;
        loader.autoInstance = true;
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.LoadObjectGroups();

        List<Character> players = ((MyGame)game).GameLevel.players;
        SPBar[] sPbars = FindObjectsOfType<SPBar>();
        sPbars[0].SetUpBar(players[0]);
        sPbars[1].SetUpBar(players[1]);
    }
}
