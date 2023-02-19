using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using TiledMapParser;

class Button : Background
{
    string mapName;
    public Button(TiledObject obj = null)
    {
        mapName = obj.GetStringProperty("map", null);
    }

    public Button(string name)
    {
        mapName = name;
    }
    void Update()
    {
        if (HitTestPoint(Input.mouseX, Input.mouseY))
        {
            alpha = 0.3f;
            if (Input.GetMouseButtonUp(0))
            {
                //if (mapName != null)
                    //((SoloKnight)game).LoadLevel(mapName + ".tmx");
                //else
                    //((SoloKnight)game).LoadCurrentLevel();
            }
        }
        else
            alpha = 0f;

    }
}

