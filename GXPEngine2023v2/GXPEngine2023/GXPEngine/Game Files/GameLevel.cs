using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
public class GameLevel : Level
{
    public List<Charecter> players = new List<Charecter>();
    Charecter Hitter;
    public bool pause;
    int pausetime;
    //bool respawn = false; used for smart respawn
    public GameLevel(string filename) : base(filename)
    {
    }
    public override void CreateLevel(bool includeImageLayers = true)
    {
        loader.highQualityText = true;
        loader.rootObject = this;
        loader.autoInstance = true;
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.LoadTileLayers();
        loader.addColliders = true;
        loader.LoadObjectGroups();

        AddPlayers();
    }
    void AddPlayers()
    {
        players.AddRange(FindObjectsOfType<Charecter>());


        foreach (Charecter a in players)
        {
            a.AddGround(FindObjectsOfType<Ground>());
            a.AddWalls(FindObjectsOfType<Wall>());
            a.SetOpponent(players.Find(x => x != a));
            a.HitConfirm += Pause;
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
        Hitter = attack.parent as Charecter;
    }
    void Update()
    {
        HitPause();
    }
    protected override void OnDestroy()
    {
        foreach (Charecter a in players)
        {
            a.HitConfirm -= Pause;
        }
        base.OnDestroy();
    }
}
