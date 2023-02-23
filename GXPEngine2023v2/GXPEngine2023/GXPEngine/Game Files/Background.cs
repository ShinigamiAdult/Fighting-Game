using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using TiledMapParser;
public class Background : AnimationSprite
{
    public Background(TiledObject obj = null) : base("Assets/Boxes/Box.png", 1, 1)
    {
        alpha = obj.GetFloatProperty("alpha", 1f);
        color = 0x000000;
    }
}