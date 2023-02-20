using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;

public class CharacterSelectManager : AnimationSprite
{
    List<CharacterIcon> characters = new List<CharacterIcon>();
    int index1;
    int index2;
    bool select1;
    bool select2;
    public CharacterSelectManager(TiledObject obj = null) : base("Assets/Boxes/HurtBox.png", 1, 1, -1, false, false)
    {
        visible = false;
        ((MyGame)game).controller1.SecondaryControllerInput1 += AddInput1;
        ((MyGame)game).controller2.SecondaryControllerInput2 += AddInput2;
    }

    public void AddCharaters(CharacterIcon[] charecter)
    {
        characters.AddRange(charecter);
    }

    public void AddCharater(CharacterIcon charecter)
    {
        characters.Add(charecter);
    }

    public int GetCharecterCount()
    {
        return characters.Count - 1;
    }

    void Update()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].Reset();
            if (index1 == i)
            {
                characters[i].P1Vis();
            }

            if (index2 == i)
            {
                characters[i].P2Vis();
            }
        }
        if (select1 && select2)
            ((MyGame)game).LoadGameLevel("TestMap.tmx");
    }

    void AddInput1(int i)
    {
        if (i == 1)
        {
            if (index1 > 0)
                index1--;
            else
                index1 = GetCharecterCount();
        }
        else if (i == 2)
            if (index1 < GetCharecterCount())
                index1++;
            else
                index1 = 0;
        if (i == 3)
            select1 = true;
        if (i == 4)
            select1 = false;
    }

    void AddInput2(int i)
    {
        if (i == 1)
        {
            if (index2 > 0)
                index2--;
            else
                index2 = GetCharecterCount();
        }
        else if (i == 2)
            if (index2 < GetCharecterCount())
                index2++;
            else
                index2 = 0;
        if (i == 3)
            select2 = true;
        if (i == 4)
            select2 = false;
    }

    public int GetIndex1()
    {
        return index1;
    }

    public int GetIndex2()
    {
        return index2;
    }
}