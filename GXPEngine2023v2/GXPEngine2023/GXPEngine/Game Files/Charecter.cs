using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
using GXPEngine.Core;
public class Charecter : AnimationSprite
{
    public event Action<AttackClass> HitConfirm;
    public enum State { Jump, Crouch, Netural, Move, Block, Attack, Dead, Hit }
    protected State current;
    protected float gravity = 0;
    protected float force = 0.6f;
    protected bool grounded;
    protected bool attacking;
    protected bool moving;
    protected bool crouching;
    protected bool hit;
    protected bool block;
    protected bool cornered;
    protected float vy; // Update first this then use it to do Move until collision
    protected float vx; // Update first this then use it to do Move until collision
    protected int attackCode;
    protected float Horizontalspeed = 5;
    protected float VerticalSpeed = -18;
    protected float damage;
    public float knock;
    protected PlayerColl playerColl;
    protected HurtBox hurtBox;
    protected List<GameObject> grounds = new List<GameObject>();
    protected List<GameObject> walls = new List<GameObject>();
    protected List<PlayerInput> Inputs = new List<PlayerInput>();
    protected Charecter opponent;
    //Animations
    protected AnimationSprite Walk;
    protected AnimationSprite Block;
    protected AnimationSprite idle;
    protected AnimationSprite Hit;
    protected AnimationSprite Jump;
    //Attacks
    protected AttackClass LightPunch;
    protected AttackClass HardPunch;
    protected AttackClass LightKick;
    protected AttackClass HardKick;
    protected AttackClass CrouchAttack;
    protected AttackClass JumpAttack;
    protected AttackClass SpecialAttack;
    protected AttackClass UltimateAttack;

    protected AttackClass CurrentAttack;
    public Charecter(TiledObject obj = null) : base("Assets/Test Animations/Chare.png", 1, 1)
    {
        bool assign = obj.GetBoolProperty("Input", true);
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

        Initalize();
        CurrentAttack = LightPunch;
        playerColl = new PlayerColl();
        playerColl.visible = true;
        playerColl.SetScaleXY(2f, 5.5f);
        playerColl.SetXY(0, 0f);
        AddChild(playerColl);

        hurtBox = new HurtBox();
        hurtBox.visible = true;
        BaseHurtBox();
        AddChild(hurtBox);
    }

    protected void GetCurrentAttack(AttackClass attack)
    {
        CurrentAttack = attack;
    }
    protected virtual void Initalize()
    {
        idle = new AnimationSprite("Assets/Test Animations/idle.png", 2, 1, 2, false, false);
        idle.SetOrigin(idle.width / 2, idle.height / 2);
        idle.SetCycle(0, 2, 16);
        idle.SetXY(0, -8);
        AddChild(idle);
        idle.visible = false;

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
    protected void SetHurBox(float x, float y, float Sx, float Sy)
    {
        hurtBox.rotation = 0;
        hurtBox.SetXY(x, y);
        hurtBox.SetScaleXY(Sx, Sy);
    }
    protected void BaseHurtBox()
    {
        hurtBox.SetXY(0, 0);
        hurtBox.SetScaleXY(0, 0);
    }

    protected void BasePlayerColl()
    {
        playerColl.SetScaleXY(2f, 5.5f);
        playerColl.SetXY(0, 0f);
    }

    protected void SetAttackDmg(float dmg, float kn)
    {
        damage = dmg;
        knock = kn;
    }
    protected virtual void ResetAttacks()
    {
        LightPunch.currentFrame = 0;
        HardPunch.currentFrame = 0;
    }
    protected void GotHit()
    {
        attacking = false;
        if ((scaleX > 0 && LastInput() == 1) || (scaleX < 0 && LastInput() == 5))
        {
            block = true;
        }
        else
            hit = true;
    }
    public void KnockBack()
    {
            vx = 0;
        if (current == State.Block)
        {
            vx = scaleX < 0 ? opponent.knock / 2 : -opponent.knock / 2;
            //opponent.knock -= vx;
        }
        else
        {
            vx = scaleX < 0 ? opponent.knock : -opponent.knock;
                //opponent.knock -= vx;
        }
    }
    protected virtual void SetVisible(AnimationSprite vis)
    {
        //Sets the current sprite based on state and animates it
        if (!vis.visible)
        {
            Walk.visible = false;
            Block.visible = false;
            Hit.visible = false;
            LightPunch.visible = false;
            HardPunch.visible = false;
            idle.visible = false;
            vis.visible = true;
        }
        // some sprites should not repeat 
        //if (true)
        // {
        //if (vis.currentFrame != vis.frameCount - 1)
        //vis.Animate();
        //}
        //else
        if ((vis == LightPunch || vis == HardPunch || vis == Hit || vis == Block) && vis.currentFrame == vis.frameCount - 1)
        {
            attacking = false;
            hit = false;
            block = false;
            vis.currentFrame = 0;
            BaseHurtBox();
        }
        //else
        vis.Animate();
    }
    protected virtual void SetState()
    {
        //Setes the player state
        switch (current)
        {
            case State.Jump:
                {
                    if (Jump != null)
                        SetVisible(Jump);
                }
                break;
            case State.Crouch:
                {
                    //SetVisible(crouch);
                }
                break;
            case State.Move:
                {
                    if(opponent.hurtBox.HitTest(playerColl) && (scaleX > 0 && LastInput() == 1) || (scaleX < 0 && LastInput() == 5) && opponent.GetCurrentAttack().GetState() == AttackClass.State.StartUp)
                    {
                        block = true;
                    }
                    SetVisible(Walk);
                }
                break;
            case State.Attack:
                {

                }
                break;
            case State.Netural:
                {
                    SetVisible(idle);
                }
                break;
            case State.Block:
                {
                    if (Block.currentFrame == Block.frameCount -2)
                    {
                        KnockBack();
                    }
                    SetVisible(Block);

                }
                break;
            case State.Hit:
                {
                    if (Hit.currentFrame == Hit.frameCount - 2)
                    {
                        KnockBack();
                    }
                    SetVisible(Hit);

                }
                break;
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
    protected void MoveInputs()
    {
        //Left
        if (LastInput() == 1 && !block)
        {
            if (DashLeft())
            {
                vx = -Horizontalspeed * 8;
                current = State.Move;
                moving = true;
            }
            else
            {
                vx = -Horizontalspeed;
                current = State.Move;
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
                current = State.Jump;
                grounded = false;
            }
            //UP
            if (LastInput() == 3)
            {

                gravity = VerticalSpeed;
                current = State.Jump;
                grounded = false;
            }
            //RightUP
            if (LastInput() == 4)
            {
                gravity = VerticalSpeed;
                vx = Horizontalspeed * 1.2f;
                current = State.Jump;
                grounded = false;
            }
        }
        //Right
        if (LastInput() == 5 && !block)
        {
            if (DashRight())
            {
                vx = Horizontalspeed * 8;
                current = State.Move;
                moving = true;
            }
            else
            {
                vx = Horizontalspeed;
                current = State.Move;
                moving = true;
            }
        }
        if (LastInput() == 6 && !block)
        {
            current = State.Crouch;
            crouching = true;
        }
        if (LastInput() == 7)
        {
            current = State.Crouch;
            crouching = true;
        }
        if (LastInput() == 8 && !block)
        {
            current = State.Crouch;
            crouching = true;
        }
    }
    protected virtual void LPAttack()
    {
        LightPunch.ResetAttack();
        GetCurrentAttack(LightPunch);
        SetHurBox(60, -35, 2f, 1f);
        SetAttackDmg(20, 3);
        attackCode = 1;
    }
    protected virtual void HPAttack()
    {
        HardPunch.ResetAttack();
        GetCurrentAttack(HardPunch);
        SetHurBox(60, -35, 2.5f, 1f);
        SetAttackDmg(30, 5);
        attackCode = 2;
    }
    protected virtual void LKAttack()
    {
        attackCode = 3;
    }
    protected virtual void HKAttack()
    {
        attackCode = 4;
    }
    protected virtual void AttackInputs()
    {
        //Console.WriteLine(ZMotionRight());
        //To achive a motion input with attack use LastInput for attack
        //Add hitbox changes
        if (ValidInput(9))
        {
            LPAttack();
            //if(current == State.Jump)
            //if(!grounded) jump attack (insert here)
            current = State.Attack;
            attacking = true;
        }
        else if (ValidInput(10))
        {
            HPAttack();
            current = State.Attack;
            attacking = true;
        }
        /*else if (ValidInput(11))
        {
            LKAttack();
            current = State.Attack;
            attacking = true;
        }
        else if (ValidInput(12))
        {
            HKAttack();
            current = State.Attack;
            attacking = true;
        }*/

    }

    protected virtual void OnAttack()
    {
        if (attackCode == 1 && !hit && !block)
        {
            if (LightPunch.FrameValidation())
            {
                AttackCollision(LightPunch);
            }

            if (LightPunch.HitConfirm() && LightPunch.GetState() == AttackClass.State.Recovery && ValidInput(10))
            {
                HPAttack();
                HardPunch.currentFrame = 1;
            }
            else
                SetVisible(LightPunch);
        }
        if (attackCode == 2 && !hit && !block)
        {
            if (HardPunch.FrameValidation())
            {
                AttackCollision(HardPunch);
            }
            SetVisible(HardPunch);
        }
    }

    protected void AttackCollision(AttackClass attack)
    {
        if (hurtBox.HitTest(opponent.playerColl) && opponent.CurrentAttack.GetState() != AttackClass.State.Active)
        {
            attack.Hit();
            opponent.GotHit();
            if (HitConfirm != null)
            {
                HitConfirm(CurrentAttack);
            }
        }
    }
    protected void UpdatePosition()
    {
        //Update position on Y
        Collision coll = MoveUntilCollision(0, vy, grounds);

        if (coll != null)
        {
            grounded = true;
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
        if (grounded && !hit)
        {
            vx = 0;
            moving = false;
            crouching = false;
        }
        if(!attacking)
        LookAtOpponent();
    }
    protected void LookAtOpponent()
    {
        if (grounded && opponent.grounded && !hit && !hit)
            scaleX = opponent.x > x ? Math.Abs(scaleX) : -Math.Abs(scaleX);
    }
    public State GetState()
    {
        return current;
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
        Console.WriteLine(LastInput());
        if (Inputs.Count == 20)
            Inputs.RemoveAt(0);
        if (parent != null && !opponent.attacking)
            parent.AddChildAt(this, parent.GetChildCount() - 1);
    }

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
    public void SetOpponent(Charecter player)
    {
        opponent = player;
        walls.Add(player.playerColl);
    }

    public AttackClass GetCurrentAttack()
    {
        return CurrentAttack;
    }
    public Charecter GetOpponent()
    {
        return opponent;
    }
    protected virtual void Update()
    {
        if (!((MyGame)game).GameLevel.pause)
        {
            UpdateInput();

            if (grounded && !attacking && !hit && !block)
                MoveInputs();

            if (!attacking && !hit && !block)
                AttackInputs();

            if (attacking && !hit && !block)
                OnAttack();

            if (!moving && !crouching && grounded && !attacking && !hit)
                current = State.Netural;
            if (hit)
                current = State.Hit;
            if (block)
                current = State.Block;

            if (Input.GetKeyDown(Key.ONE))
                playerColl.visible = !playerColl.visible;
            if (Input.GetKeyDown(Key.TWO))
                hurtBox.visible = !hurtBox.visible;
            //Update Position, Ground physics also and SetState should be ran last
            GroundCheck();
            UpdatePosition();
            SetState();
        }
    }

    protected override void OnDestroy()
    {
        ((MyGame)game).controller1.ControllerInput1 -= AddInput;
        ((MyGame)game).controller2.ControllerInput2 -= AddInput;
        base.OnDestroy();
    }
}