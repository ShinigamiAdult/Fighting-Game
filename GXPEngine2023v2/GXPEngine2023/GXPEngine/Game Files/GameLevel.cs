using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
public class GameLevel : Level
{
    public List<Character> players = new List<Character>();
    Character Hitter;
    public bool pause;
    int pausetime;
    //bool respawn = false; used for smart respawn
    public GameLevel(string filename, int i, int y) : base(filename, i, y)
    {
    }
    public override void CreateLevel(int i, int y)
    {
        loader.highQualityText = true;
        loader.rootObject = this;
        loader.autoInstance = true;
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.AddManualType();
        loader.LoadObjectGroups();

        AddPlayers(i,y);
    }
    void AddPlayers(int i, int y)
    {

        players.Add(AssignChareter(i,true));
        players.Add(AssignChareter(y, false));
        players[0].SetXY(324, 330);
        players[1].SetXY(544, 330);
        foreach (Character a in players)
        {
            a.AddGround(FindObjectsOfType<Ground>());
            a.AddWalls(FindObjectsOfType<Wall>());
            a.SetOpponent(players.Find(x => x != a));
            a.HitConfirm += Pause;
            AddChild(a);
        }
    }
    void HitPause()
    {
        if (pause)
        {
            pausetime--;
            if (pausetime == 0)
                pause = false;
        }
    }
    void Pause(AttackClass attack)
    {
        pausetime = attack.HitStop();
        pause = true;
        this.AddChildAt(attack.parent, this.GetChildCount() - 1);
        Hitter = attack.parent as Character;
    }
    void Update()
    {
        HitPause();
    }
    protected override void OnDestroy()
    {
        foreach (Character a in players)
        {
            a.HitConfirm -= Pause;
        }
        base.OnDestroy();
    }

    Character AssignChareter(int b, bool assign)
    {
        if (b == 0)
            return new Character(assign);
        else if(b == 1)
            return new Orochi(assign);
        else
            return new Character(assign);
    }
}
