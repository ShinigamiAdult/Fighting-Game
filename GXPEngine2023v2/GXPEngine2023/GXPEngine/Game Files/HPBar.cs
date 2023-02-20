﻿using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;


class HPBar : AnimationSprite
{
    float hp1;
    Sprite fill;
    Sprite border;
    public HPBar(TiledObject obj = null) : base("Assets/Boxes/HPBackground.png", 1, 1, -1, false, false)
    {
        bool invert = obj.GetBoolProperty("Inverted");
        fill = new Sprite("Assets/Boxes/UIPlayerHealth.png");

        if (invert)
        {
            fill.SetOrigin(0, fill.height / 2);
            fill.SetXY(width / 2 + 17, -0.5f);
        }
        else
        {
            fill.SetOrigin(0, fill.height / 2);
            fill.SetXY(-width / 2 + 17, -0.5f);
        }
        AddChild(fill);

        border = new Sprite("Assets/Boxes/hp_bar.png");
        if (invert)
        {
            border.SetOrigin(0, border.height / 2);
            border.SetXY(width / 2, -0.5f);
        }
        else
        {
            border.SetOrigin(0, border.height / 2);
            border.SetXY(-width / 2, -0.5f);
        }
        AddChild(border);
    }
    public void SetUpBar(Character player)
    {
        player.TakeDamage += UIUpdate;
        hp1 = fill.scaleX / player.GetMaxHP();
    }
    public void UIUpdate(float Chp)
    {
        fill.SetScaleXY(hp1 * Chp, 1);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
