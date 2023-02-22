using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
public class CharacterArt : AnimationSprite
{
    public List<AnimationSprite> Image = new List<AnimationSprite>();
    public CharacterArt(TiledObject obj = null) : base("Assets/Boxes/HurtBox.png", 1, 1, -1, false, false)
    {
        alpha = 0;
        //AddImage("Assets/Character Art/player_0.png");
        AddImage("Assets/Character Art/player_1.png");
        AddImage("Assets/Character Art/player_2.png");
        AddImage("Assets/Character Art/player_3.png");
    }

    void AddImage(string filename)
    {
        AnimationSprite image = new AnimationSprite(filename, 1, 1, -1, false, false);
        image.SetOrigin(image.width / 2, image.height / 2);
        image.SetXY(width / 2, height / 2);
        image.SetScaleXY(1f);
        image.visible = false;
        Image.Add(image);
        AddChild(image);
    }
    public void ShowImage(int i)
    {
        foreach(AnimationSprite a in Image)
            a.visible = false;
        Image[i].visible = true;
    }
}

