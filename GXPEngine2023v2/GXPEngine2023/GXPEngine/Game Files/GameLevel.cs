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
    public bool roundEnd = false;
    public bool roundBegin = false;
    public bool gameEnd = false;
    int pausetime = 0;
    int RoundCount = 0;
    int P1Round;
    int P2Round;
    Level Announcer;
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
        AddPlayers(i, y);
        RoundKO(120);
    }
    void AddPlayers(int i, int y)
    {
        HPBar[] hPbars = FindObjectsOfType<HPBar>();

        players.Add(AssignChareter(i, true));
        players.Add(AssignChareter(y, false));
        hPbars[0].SetUpBar(players[0]);
        hPbars[1].SetUpBar(players[1]);
        Reset();
        foreach (Character a in players)
        {
            a.AddGround(FindObjectsOfType<Ground>());
            a.AddWalls(FindObjectsOfType<Wall>());
            a.SetOpponent(players.Find(x => x != a));
            a.HitConfirm += Pause;
            //a.TakeDamage += CheckPlayer;
            a.level = this;
            AddChild(a);
        }
    }
    void HitPause()
    {
        pausetime--;
        if (pausetime == 0)
            pause = false;
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
        if (!roundEnd && !roundBegin && !gameEnd)
        {
            if (!pause)
                CheckPlayer();
            if (pause)
                HitPause();
        }
        if (roundEnd)
            RoundEnd();
        if (roundBegin)
            RoundBegin();
        if (gameEnd)
            GameEnd();
    }
    protected override void OnDestroy()
    {
        foreach (Character a in players)
        {
            a.HitConfirm -= Pause;
            //a.TakeDamage -= CheckPlayer;
        }
        base.OnDestroy();
    }

    Character AssignChareter(int b, bool assign)
    {
        if (b == 0)
            return new Character(assign);
        else if (b == 1)
            return new Orochi(assign);
        else
            return new Character(assign);
    }
    void CheckPlayer()
    {
        foreach (Character a in players)
        {
            if (a.GetState() == Character.State.Dead)
            {
                RoundKO(180);
                if (a.GetOpponent() == players[0] && P1Round != 2)
                    P1Round++;
                if (a.GetOpponent() == players[1] && P2Round != 2)
                    P2Round++;
            }
        }
    }
    void RoundEnd()
    {
        if (pausetime == 120)
        {
            RemoveChild(Announcer);
            Announcer.Destroy();
            if (P1Round != 2 && P2Round != 2)
            {
                Announcer = new Level("Announcment Maps/Round" + RoundCount + ".tmx", 0, 0);
                roundBegin = true;
                Reset();
            }
            if (P1Round == 2)
            {
                Announcer = new Level("Announcment Maps/P1wins.tmx", 0, 0);
                gameEnd = true;
            }
            if (P2Round == 2)
            {
                Announcer = new Level("Announcment Maps/P2wins.tmx", 0, 0);
                gameEnd = true;
            }
            AddChild(Announcer);
            roundEnd = false;
        }
        if (pausetime > 120)
            pausetime--;
    }

    void GameEnd()
    {
        if (pausetime == 0)
        {
            ((MyGame)game).LoadPlayAgainLevel("PlayAgain.tmx");
        }
        if (pausetime > 0)
            pausetime--;
    }
    void RoundBegin()
    {
        if (pausetime == 30)
        {
            RemoveChild(Announcer);
            Announcer.Destroy();
            Announcer = new Level("Announcment Maps/Fight.tmx", 0, 0);
            AddChild(Announcer);
        }
        if (pausetime == 0)
        {
            RemoveChild(Announcer);
            Announcer.Destroy();
            roundBegin = false;
        }
        if (pausetime > 0)
            pausetime--;
    }

    void Reset()
    {
        players[0].SetXY(256, 420);
        players[0].ResetHealth();
        players[0].SetState(Character.State.Netural);
        players[1].SetXY(672, 420);
        players[1].ResetHealth();

    }

    void RoundKO(int i)
    {
        RoundCount++;
        pausetime = i;
        Announcer = new Level("Announcment Maps/KORound.tmx", 0, 0);
        AddChild(Announcer);
        roundEnd = true;
    }
}
