﻿using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
using GXPEngine.Core;
public class Character : AnimationSprite
{
    public event Action<AttackClass> HitConfirm;
    public event Action<float> TakeDamage;
    public event Action<float> BarFill;
    public event Action<bool> Super;
    public enum State { Jump, Crouch, Netural, Move, Block, Attack, Dead, Hit, DashForward, DashBackward, Knockdown }
    protected State current;
    //Stats
    protected float gravity = 0;
    protected float force = 0.6f;
    protected float Horizontalspeed = 5;
    protected float VerticalSpeed = -18;
    protected float MaxHealth = 1;
    protected float currentHealth = 1;
    protected float MaxPower;
    protected float currentPower;
    protected float damage;
    public float knock;
    public float knockY;
    //Checkings
    protected bool grounded;
    protected bool attacking;
    protected bool moving;
    protected bool crouching;
    protected bool hit;
    protected bool block;
    protected bool dashLeft;
    protected bool dashRight;
    protected bool knockdown;
    protected bool cornered;
    protected int attackCode;
    protected Sound specialSound;
    protected Sound ultSound;
    protected Sound[] sHit = new Sound[9];

    //Positoning
    protected float vy; // Update first this then use it to do Move until collision
    protected float vx; // Update first this then use it to do Move until collision
    //Collisions and Colliders
    protected PlayerColl playerColl;
    protected HurtBox hurtBox;
    protected Character opponent;
    protected List<GameObject> grounds = new List<GameObject>();
    protected List<GameObject> walls = new List<GameObject>();
    //Inputs
    protected List<PlayerInput> Inputs = new List<PlayerInput>();
    //Animations
    protected AnimationSprite Crouch;
    protected AnimationSprite Walk;
    protected AnimationSprite Block;
    protected AnimationSprite Idle;
    protected AnimationSprite Hit;
    protected AnimationSprite Jump;
    protected AnimationSprite DashForward;
    protected AnimationSprite DashBackwards;
    protected AnimationSprite KnockDown;
    protected AnimationSprite Dead;
    public AnimationSprite spark;
    //Attacks
    protected AttackClass LightPunch;
    protected AttackClass HardPunch;
    protected AttackClass LightKick;
    protected AttackClass HardKick;
    protected AttackClass CrouchAttack;
    protected AttackClass JumpAttack;
    protected AttackClass SpecialAttack;
    protected AttackClass SpecialAttack2;
    protected AttackClass UltimateAttack;

    protected AttackClass CurrentAttack;
    public GameLevel level;
    public Character(bool assign) : base("Assets/Test Animations/Chare.png", 1, 1, -1)
    {
        if (assign)
            ((MyGame)game).controller1.ControllerInput1 += AddInput;
        else
            ((MyGame)game).controller2.ControllerInput2 += AddInput;
        for (int i = 0; i < 10; i++)
        {
            AddInput(0);
        }
        SetOrigin(width / 2, height / 2);
        alpha = 0;
        SetStats();
        Initalize();
        CurrentAttack = LightPunch;
        playerColl = new PlayerColl();
        playerColl.visible = false;
        playerColl.SetScaleXY(2f, 5.5f);
        playerColl.SetXY(0, 0f);
        AddChild(playerColl);

        hurtBox = new HurtBox();
        hurtBox.visible = false;
        BaseHurtBox();
        AddChild(hurtBox);

        spark = new AnimationSprite("Assets/VFX/Hit.png", 7, 1, 7, false, false);
        spark.SetOrigin(spark.width / 2, spark.height / 2);
        spark.SetCycle(0, 7, 2);
        AddChild(spark);
        spark.visible = false;

        sHit[0] = new Sound("Assets/Sound/SFX/SE_00007.wav");
        sHit[1] = new Sound("Assets/Sound/SFX/SE_00008.wav");
        sHit[2] = new Sound("Assets/Sound/SFX/SE_00009.wav");
        sHit[3] = new Sound("Assets/Sound/SFX/SE_00010.wav");
        sHit[4] = new Sound("Assets/Sound/SFX/SE_00011.wav");
        sHit[5] = new Sound("Assets/Sound/SFX/SE_00012.wav");
        sHit[6] = new Sound("Assets/Sound/SFX/SE_00013.wav");
        sHit[7] = new Sound("Assets/Sound/SFX/SE_00014.wav");
        sHit[8] = new Sound("Assets/Sound/SFX/SE_00015.wav");
    }
    //Base Functionality
    protected void SetState()
    {
        //Setes the player state
        switch (current)
        {
            case State.Jump:
                {
                    SetVisible(Jump);
                    SetPlayerColl(0, -20, 2f,4f);
                }
                break;
            case State.Knockdown:
                {
                    SetVisible(KnockDown);
                    //if(!grounded)
                    //KnockBack();
                }
                break;
            case State.Crouch:
                {
                    SetVisible(Crouch);
                    Crouching();
                }
                break;
            case State.Move:
                {
                    SetVisible(Walk);
                }
                break;
            case State.Attack:
                {
                    moving = false;
                    dashLeft = false;
                    dashRight = false;
                    AttackAddition();
                    SetVisible(CurrentAttack);
                    if (CurrentAttack.FrameValidation())
                        AttackCollision(CurrentAttack);
                }
                break;
            case State.Netural:
                {
                    SetVisible(Idle);
                    BasePlayerColl();
                }
                break;
            case State.Block:
                {
                    if (Block.currentFrame == 0)
                        KnockBack();
                    SetVisible(Block);
                }
                break;
            case State.Hit:
                {
                    if (Hit.currentFrame == 0)
                        KnockBack();
                    SetVisible(Hit);
                }
                break;
            case State.Dead:
                {
                    SetVisible(Dead);
                }
                break;
            case State.DashForward:
                {
                    SetVisible(DashForward);
                }
                break;
            case State.DashBackward:
                {
                    SetVisible(DashBackwards);
                }
                break;
        }
    }
    protected void MoveInputs()
    {
        //Left
        if (LastInput() == 1)
        {
            if (DashLeft())
            {
                vx = -Horizontalspeed * 15;
                dashLeft = true;
            }
            else
            {
                vx = -Horizontalspeed;
                moving = true;
            }

        }
        if (grounded)
        {
            //LeftUP
            if (LastInput() == 2)
            {
                gravity = VerticalSpeed;
                vx = -Horizontalspeed * 1.2f;
                grounded = false;
            }
            //UP
            if (LastInput() == 3)
            {

                gravity = VerticalSpeed;
                grounded = false;
            }
            //RightUP
            if (LastInput() == 4)
            {
                gravity = VerticalSpeed;
                vx = Horizontalspeed * 1.2f;
                grounded = false;
            }
        }
        //Right
        if (LastInput() == 5)
        {
            if (DashRight())
            {
                vx = Horizontalspeed * 15;
                dashRight = true;
            }
            else
            {
                vx = Horizontalspeed;
                moving = true;
            }
        }
        if (LastInput() == 6)
        {
            crouching = true;
        }
        if (LastInput() == 7)
        {
            crouching = true;
        }
        if (LastInput() == 8)
        {
            crouching = true;
        }
    }
    protected void UpdatePosition()
    {
        //Update position on Y
        Collision coll = MoveUntilCollision(0, vy, grounds);
        if (coll != null)
        {
            grounded = true;
            knockdown = false;
        }

        if (playerColl.HitTest(opponent) && !grounded)
        {
            opponent.vx = scaleX > 0 ? -20 : 20;
        }

        if (playerColl.HitTest(opponent) && grounded && opponent.grounded)
        {
            vx = scaleX > 0 ? -20 : 20;
        }

        //Update position on x
        coll = MoveUntilCollision(vx, 0, walls);

        if (coll != null)
        {
            if (coll.other is PlayerColl)
            {
                if ((coll.normal.x > 0 || coll.normal.x < 0) && opponent.GetState() == State.Netural && grounded)
                {
                    opponent.vx += vx / 5;
                }
            }
        }

        //ground Check
        if (grounded && !hit && !block && !dashLeft && !dashRight)
        {
            vx = 0;
            moving = false;
        }
        if (LastInput() != 6 && LastInput() != 7 && LastInput() != 8)
            crouching = false;
        LookAtOpponent();
    }
    protected void Update()
    {
        if (!level.pause)
        {
            UpdateInput();

            if (!level.gameEnd && !level.roundBegin && !level.roundEnd)
            {
                if (grounded && (!attacking || Cancelabel()) && !hit && !block && !knockdown)
                    MoveInputs();


                if (!knockdown && !hit && !block && (!attacking || Cancelabel()))
                    AttackInputs();
            }

            if (hit)
            {
                current = State.Hit;
            }
            else if (knockdown)
            {
                current = State.Knockdown;
            }
            else if (currentHealth <= 0)
            {
                current = State.Dead;
            }
            else if (block)
            {
                current = State.Block;
            }
            else if (attacking)
            {
                current = State.Attack;
            }
            else if (!grounded)
            {
                current = State.Jump;
            }
            else if (crouching)
            {
                current = State.Crouch;
            }
            else if (dashLeft)
            {
                if (scaleX < 0)
                    current = State.DashForward;
                else
                    current = State.DashBackward;
            }
            else if (dashRight)
            {
                if (scaleX > 0)
                    current = State.DashForward;
                else
                    current = State.DashBackward;
            }
            else if (moving)
            {
                current = State.Move;
            }
            else
                current = State.Netural;
            GroundCheck();
            UpdatePosition();
            SetState();
        }
        if (Input.GetKeyDown(Key.ONE))
            playerColl.visible = !playerColl.visible;
        if (Input.GetKeyDown(Key.TWO))
            hurtBox.visible = !hurtBox.visible;
        if (spark.currentFrame < spark.frameCount - 1)
            spark.Animate();
    }
    //Shared
    protected virtual void SetStats()
    {
        MaxHealth = 700;
        currentHealth = MaxHealth;
        MaxPower = 500;
        currentPower = 0;
    }
    protected virtual void Initalize()
    {
        Idle = new AnimationSprite("Assets/Test Animations/idle.png", 2, 1, 2, false, false);
        Idle.SetOrigin(Idle.width / 2, Idle.height / 2);
        Idle.SetCycle(0, 2, 16);
        Idle.SetXY(0, -8);
        AddChild(Idle);
        Idle.visible = false;

        LightPunch = new AttackClass("Assets/Test Animations/punch2.png", 5, 1, 2, 2, 8, false);
        LightPunch.SetOrigin(LightPunch.width / 2, LightPunch.height / 2);
        LightPunch.SetCycle(0, 5, 8);
        LightPunch.SetXY(0, -8);
        AddChild(LightPunch);
        LightPunch.visible = false;

        HardPunch = new AttackClass("Assets/Test Animations/punch1.png", 5, 1, 2, 2, 12, true);
        HardPunch.SetOrigin(HardPunch.width / 2, HardPunch.height / 2);
        HardPunch.SetCycle(0, 5, 8);
        HardPunch.SetXY(0, -8);
        AddChild(HardPunch);
        HardPunch.visible = false;

        Hit = new AnimationSprite("Assets/Charecter 1/C1 Block.png", 3, 1, 3, false, false);
        Hit.SetOrigin(Hit.width / 2, Hit.height / 2);
        Hit.SetCycle(0, 3, 4);
        Hit.SetXY(0, -8);
        AddChild(Hit);
        Hit.visible = false;

        Block = new AnimationSprite("Assets/Charecter 1/C1 Block.png", 3, 1, 3, false, false);
        Block.SetOrigin(Block.width / 2, Block.height / 2);
        Block.SetCycle(0, 3, 4);
        Block.SetXY(0, -8);
        AddChild(Block);
        Block.visible = false;

        Walk = new AnimationSprite("Assets/Charecter 1/C1 Walk.png", 4, 1, 4, false, false);
        Walk.SetOrigin(Walk.width / 2, Walk.height / 2);
        Walk.SetCycle(0, 4, 4);
        Walk.SetXY(0, -8);
        AddChild(Walk);
        Walk.visible = false;
    }
    protected virtual void AttackInputs()
    {


    }
    protected virtual void SetVisible(AnimationSprite vis)
    {
        //Sets the current sprite based on state and animates it
        if (!vis.visible)
        {
            Dead.visible = false;
            UltimateAttack.visible = false;
            KnockDown.visible = false;
            SpecialAttack.visible = false;
            HardKick.visible = false;
            DashBackwards.visible = false;
            DashForward.visible = false;
            JumpAttack.visible = false;
            CrouchAttack.visible = false;
            Crouch.visible = false;
            Walk.visible = false;
            Block.visible = false;
            Hit.visible = false;
            LightPunch.visible = false;
            HardPunch.visible = false;
            LightKick.visible = false;
            Jump.visible = false;
            Idle.visible = false;
            vis.visible = true;
        }
        // some sprites should not repeat 
        if (vis.currentFrame == vis.frameCount - 1 && ((vis is AttackClass && (vis != JumpAttack || grounded)) || vis == Hit || vis == Block || vis == DashBackwards || vis == DashForward || vis == Dead))
        {
            if (vis is AttackClass)
            {
                if (vis == UltimateAttack)
                    TriggerSuper(false);
                if (grounded)
                    BaseHurtBox();
                attacking = false;
                vis.currentFrame = 0;
                if (vis == CrouchAttack)
                    crouching = true;
                AttackClass temp = vis as AttackClass;
                temp.ResetAttack();
            }
            if (vis == Hit || vis == Block)
            {
                Hit.currentFrame = 0;
                hit = false;
                Block.currentFrame = 0;
                block = false;
            }
            if (vis == DashBackwards || vis == DashForward)
            {
                vis.currentFrame = 0;
                dashLeft = false;
                dashRight = false;
            }
        }
        else
            vis.Animate();
    }
    protected virtual void AttackAddition()
    {

    }
    protected void TriggerSuper(bool f)
    {
        if (Super != null)
            Super(f);
    }
    //Stats
    public void ResetHealth()
    {
        attacking = false;
        grounded = false;
        moving = false;
        crouching = false;
        hit = false;
        block = false;
        LookAtOpponent();
        currentHealth = MaxHealth;
        if (TakeDamage != null)
            TakeDamage(currentHealth);
    }
    protected void BaseHurtBox()
    {
        hurtBox.rotation = 0;
        hurtBox.SetXY(0, 0);
        hurtBox.SetScaleXY(0, 0);
    }
    protected void BasePlayerColl()
    {
        playerColl.rotation = 0;
        playerColl.SetScaleXY(2f, 5.5f);
        playerColl.SetXY(0, 0f);
    }
    protected void SetAttackDmg(float dmg, float kn, float y = 0)
    {
        damage = dmg;
        knock = kn;
        knockY = y;
    }
    protected void Crouching()
    {
        playerColl.SetScaleXY(2f, 3f);
        playerColl.SetXY(0, 35f);
    }
    //Input Checks
    protected bool ValidInput(int a)
    {
        PlayerInput c = Inputs.FindLast(x => x.GetKey() == a);
        if (c != null)
        {
            if (c.GetTime() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    protected int LastInput()
    {
        if (Inputs[Inputs.Count - 1].GetTime() != 0)
            return Inputs[Inputs.Count - 1].GetKey();
        else
            return 0;
    }
    protected int BeforeLastInput()
    {
        if (Inputs[Inputs.Count - 2].GetTime() != 0)
            return Inputs[Inputs.Count - 2].GetKey();
        else
            return 0;
    }
    protected int ThirdLastInput()
    {
        if (Inputs[Inputs.Count - 3].GetTime() != 0)
            return Inputs[Inputs.Count - 3].GetKey();
        else
            return 0;
    }
    protected void UpdateInput()
    {
        foreach (PlayerInput a in Inputs)
            a.Update();
    }
    protected bool QuarterCircleLeft()
    {
        bool down = false;
        bool downside = false;
        bool side = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
            {
                if ((Inputs[i].GetKey() == 8 && Inputs[i].GetTime() != 0) || downside)
                {
                    if ((Inputs[i].GetKey() == 1 && Inputs[i].GetTime() != 0) || downside)
                        side = true;

                    downside = true;
                }
                down = true;
            }
        }
        if (down && downside && side)
            return true;
        else
            return false;
    }
    protected bool DoubleQuarterCircleLeft()
    {
        bool down = false;
        bool downside = false;
        bool side = false;
        bool down2 = false;
        bool downside2 = false;
        bool side2 = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
            {
                if ((Inputs[i].GetKey() == 8 && Inputs[i].GetTime() != 0) || downside)
                {
                    if ((Inputs[i].GetKey() == 1 && Inputs[i].GetTime() != 0) || downside)
                    {
                        if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down2)
                        {
                            if ((Inputs[i].GetKey() == 8 && Inputs[i].GetTime() != 0) || downside2)
                            {
                                if ((Inputs[i].GetKey() == 1 && Inputs[i].GetTime() != 0) || downside2)
                                    side2 = true;

                                downside2 = true;
                            }

                            down2 = true;
                        }
                        side = true;
                    }

                    downside = true;
                }
                down = true;
            }
        }
        if (down && downside && side && down2 && downside2 && side2)
            return true;
        else
            return false;
    }
    protected bool QuarterCircleRight()
    {
        bool down = false;
        bool downside = false;
        bool side = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
            {
                if ((Inputs[i].GetKey() == 6 && Inputs[i].GetTime() != 0) || downside)
                {
                    if ((Inputs[i].GetKey() == 5 && Inputs[i].GetTime() != 0) || downside)
                    {
                        side = true;
                    }
                    downside = true;

                }
                down = true;

            }
        }
        if (down && downside && side)
            return true;
        else
            return false;
    }
    protected bool DoubleQuarterCircleRight()
    {
        bool down = false;
        bool downside = false;
        bool side = false;
        bool down2 = false;
        bool downside2 = false;
        bool side2 = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
            {
                if ((Inputs[i].GetKey() == 6 && Inputs[i].GetTime() != 0) || downside)
                {
                    if ((Inputs[i].GetKey() == 5 && Inputs[i].GetTime() != 0) || downside)
                    {
                        if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down2)
                        {
                            if ((Inputs[i].GetKey() == 6 && Inputs[i].GetTime() != 0) || downside2)
                            {
                                if ((Inputs[i].GetKey() == 5 && Inputs[i].GetTime() != 0) || downside2)
                                {
                                    side2 = true;
                                }
                                downside2 = true;
                            }
                            down2 = true;
                        }
                        side = true;
                    }
                    downside = true;
                }
                down = true;
            }
        }
        if (down && downside && side && down2 && downside2 && side2)
            return true;
        else
            return false;
    }
    protected bool ZMotionLeft()
    {
        bool side1 = false;
        bool down = false;
        bool downside = false;
        bool side = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 1 && Inputs[i].GetTime() != 0) || side1)
            {
                if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
                {
                    if ((Inputs[i].GetKey() == 8 && Inputs[i].GetTime() != 0) || downside)
                    {
                        if ((Inputs[i].GetKey() == 1 && Inputs[i].GetTime() != 0) || downside)
                        {
                            side = true;
                        }
                        downside = true;

                    }
                    down = true;

                }
                side1 = true;
            }
        }
        if (down && downside && side && side1)
            return true;
        else
            return false;
    }
    protected bool ZMotionRight()
    {
        bool side1 = false;
        bool down = false;
        bool downside = false;
        bool side = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 5 && Inputs[i].GetTime() != 0) || side1)
            {
                if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
                {
                    if ((Inputs[i].GetKey() == 6 && Inputs[i].GetTime() != 0) || downside)
                    {
                        if ((Inputs[i].GetKey() == 5 && Inputs[i].GetTime() != 0) || downside)
                        {
                            side = true;
                        }
                        downside = true;

                    }
                    down = true;

                }
                side1 = true;
            }
        }
        if (down && downside && side && side1)
            return true;
        else
            return false;
    }
    protected bool QuarterCircleForward()
    {
        if (scaleX > 0)
            return QuarterCircleRight();
        else
            return QuarterCircleLeft();
    }
    protected bool DoubleQuarterCircleForward()
    {
        if (scaleX > 0)
            return DoubleQuarterCircleRight();
        else
            return DoubleQuarterCircleLeft();
    }
    protected bool DoubleQuarterCircleBackwards()
    {
        if (scaleX < 0)
            return DoubleQuarterCircleRight();
        else
            return DoubleQuarterCircleLeft();
    }
    protected bool QuarterCircleBackwards()
    {
        if (scaleX < 0)
            return QuarterCircleRight();
        else
            return QuarterCircleLeft();
    }
    protected bool ZMotionBackwards()
    {
        if (scaleX < 0)
            return ZMotionRight();
        else
            return ZMotionLeft();
    }
    protected bool ZMotionForward()
    {
        if (scaleX > 0)
            return ZMotionRight();
        else
            return ZMotionLeft();
    }
    protected bool DashLeft()
    {
        bool side = false;
        bool side1 = false;
        for (int i = 0; i < Inputs.Count - 1; i++)
        {
            if (ThirdLastInput() == 1 || side)
            {
                if (BeforeLastInput() == 0)
                    side1 = true;
                side = true;
            }
        }
        if (side1 && side)
            return true;
        else
            return false;
    }
    protected bool DashRight()
    {
        bool side = false;
        bool side1 = false;
        for (int i = 0; i < Inputs.Count - 1; i++)
        {
            if (ThirdLastInput() == 5 || side)
            {
                if (BeforeLastInput() == 0)
                    side1 = true;
                side = true;
            }
        }
        if (side1 && side)
            return true;
        else
            return false;
    }
    protected bool DoubleDown()
    {
        bool down = false;
        bool downside = false;
        bool side = false;
        for (int i = 0; i < Inputs.Count; i++)
        {
            if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || down)
            {
                if ((Inputs[i].GetKey() == 0 && Inputs[i].GetTime() != 0) || downside)
                {
                    if ((Inputs[i].GetKey() == 7 && Inputs[i].GetTime() != 0) || downside)
                    {
                        side = true;
                    }
                    downside = true;

                }
                down = true;

            }
        }
        if (down && downside && side)
            return true;
        else
            return false;
    }
    //Battle Checks
    //if knock causes bug you can implement the knock from here
    protected void GotHit()
    {
        attacking = false;
        CurrentAttack.ResetAttack();
        if (Blocking() && !hit && !knockdown)
        {
            block = true;
            Block.currentFrame = 0;
            if (currentHealth - opponent.damage / 3 > 0)
                currentHealth -= opponent.damage / 3;
            else
                while (currentHealth >= 0)
                {
                    currentHealth -= 0.1f;
                }
            if (currentPower + opponent.damage < MaxPower)
                currentPower += opponent.damage;
            else
                while (currentPower < 500)
                {
                    currentPower += 0.1f;
                }
        }
        else
        {
            hit = true;
            Hit.currentFrame = 0;
            if (currentHealth - opponent.damage > 0)
                currentHealth -= opponent.damage;
            else
                while (currentHealth >= 0)
                {
                    currentHealth -= 0.1f;
                }
            if (currentPower + opponent.damage / 1.5f < MaxPower)
                currentPower += opponent.damage / 1.5f;
            else
                while (currentPower < MaxPower)
                {
                    currentPower += 0.1f;
                }
            //Vector2 a = new Vector2(0, -playerColl.height / 4);
            //spark.SetXY(a.x, a.y);
            spark.currentFrame = 0;
            spark.visible = true;
        }
        if (TakeDamage != null)
            TakeDamage(currentHealth);
        if (BarFill != null)
            BarFill(currentPower);
    }
    public bool Blocking()
    {
        if ((scaleX > 0 && (LastInput() == 1 || LastInput() == 8)) || (scaleX < 0 && (LastInput() == 5 || LastInput() == 6)))
            return true;
        else
            return false;
    }
    protected void AttackCollision(AttackClass attack)
    {
        if (hurtBox.HitTest(opponent.playerColl) && opponent.CurrentAttack.GetState() != AttackClass.State.Active)
        {
            attack.Hit();
            opponent.GotHit();
            if (CurrentAttack != UltimateAttack)
            {
                if (currentPower + damage * 2 < MaxPower)
                    currentPower += damage * 2;
                else
                    while (currentPower < MaxPower)
                    {
                        currentPower += 0.1f;
                    }
            }
            if (HitConfirm != null)
                HitConfirm(CurrentAttack);

            if (BarFill != null)
                BarFill(currentPower);
            int i = Utils.Random(0,9);
            sHit[i].Play(false,0,0.4f);
        }
    }
    protected void GroundCheck()
    {
        //if (gravity < 10)
        gravity += force;
        if (grounded)
            gravity = 0;
        vy = gravity;
    }
    protected void LookAtOpponent()
    {
        if (grounded && !hit && !block && !attacking)
            scaleX = opponent.x > x ? Math.Abs(scaleX) : -Math.Abs(scaleX);
    }
    protected bool Cancelabel()
    {
        if ((CurrentAttack.Cancel() && CurrentAttack.GetState() == AttackClass.State.Recovery))
            return true;
        else
            return false;
    }
    //Getters
    public AttackClass GetCurrentAttack()
    {
        return CurrentAttack;
    }
    public Character GetOpponent()
    {
        return opponent;
    }
    public State GetState()
    {
        return current;
    }
    public float GetMaxHP()
    {
        return MaxHealth;
    }
    public float GetMaxSP()
    {
        return MaxPower;
    }
    public float GetcurrentHP()
    {
        return currentHealth;
    }
    //Setters
    protected void SetCurrentAttack(AttackClass attack)
    {
        CurrentAttack = attack;
    }
    public void SetState(State state)
    {
        current = state;
    }
    public void AddGround(GameObject[] ground)
    {
        grounds.AddRange(ground);
    }
    public void AddWalls(GameObject[] wall)
    {
        walls.AddRange(wall);
    }
    public void AddInput(int i)
    {
        Inputs.Add(new PlayerInput(i));
        if (Inputs.Count == 20)
            Inputs.RemoveAt(0);
        if (parent != null && !opponent.attacking)
            parent.AddChildAt(this, parent.GetChildCount() - 1);
    }
    public void SetOpponent(Character player)
    {
        opponent = player;
        walls.Add(player.playerColl);
    }
    protected void ResetSpBar()
    {
        currentPower = 0;
        if (BarFill != null)
            BarFill(currentPower);
    }
    protected void SetPlayerColl(float x, float y, float Sx = 0, float Sy = 0)
    {
        playerColl.rotation = 0;
        playerColl.SetXY(x, y);
        if(Sx == 0 && Sy == 0)
        {
            playerColl.SetScaleXY(2f, 5.5f);
        }
        else
        playerColl.SetScaleXY(Sx, Sy);
    }
    protected void SetHurBox(float x, float y, float Sx, float Sy)
    {
        hurtBox.rotation = 0;
        hurtBox.SetXY(x, y);
        hurtBox.SetScaleXY(Sx, Sy);
    }
    //Destroy
    protected override void OnDestroy()
    {
        ((MyGame)game).controller1.ControllerInput1 -= AddInput;
        ((MyGame)game).controller2.ControllerInput2 -= AddInput;
        base.OnDestroy();
    }
    //Other
    public void KnockBack()
    {
        //vy = 0;
        vx = 0;
        if (block)
        {
            vx = scaleX < 0 ? opponent.knock / 2 : -opponent.knock / 2;
        }
        else
        {
            vx = scaleX < 0 ? opponent.knock : -opponent.knock;
            if (!grounded && opponent.knockY == 0)
            {
                gravity = -5;
                knockdown = true;
            }
            else
            {
                gravity = -opponent.knockY;
                knockdown = true;
            }

            grounded = false;
        }
    }
}