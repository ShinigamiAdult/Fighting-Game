using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
using GXPEngine.Core;

class Orochi : Character
{
    public Orochi(TiledObject obj = null) : base(obj)
    {

    }

    public Orochi(bool assign) : base(assign)
    {

    }

    protected override void SetStats()
    {
        base.SetStats();
    }
    protected override void Initalize()
    {
        //Attacks
        LightPunch = new AttackClass("Assets/Charecter 1/C1 LightPunch.png", 5, 1, 5, 2, 10, false);
        LightPunch.SetOrigin(LightPunch.width / 2, LightPunch.height / 2);
        LightPunch.SetCycle(0, 5, 6);
        LightPunch.SetXY(0, -8);
        AddChild(LightPunch);
        LightPunch.visible = false;

        HardPunch = new AttackClass("Assets/Charecter 1/C1 HardPunch.png", 6, 1, 6, 3, 14, true);
        HardPunch.SetOrigin(HardPunch.width / 2, HardPunch.height / 2);
        HardPunch.SetCycle(0, 6, 6);
        HardPunch.SetXY(0, -8);
        AddChild(HardPunch);
        HardPunch.visible = false;

        LightKick = new AttackClass("Assets/Charecter 1/C1 LightKick.png", 5, 1, 5, 2, 8, false);
        LightKick.SetOrigin(LightKick.width / 2, LightKick.height / 2);
        LightKick.SetCycle(0, 5, 6);
        LightKick.SetXY(0, -8);
        AddChild(LightKick);
        LightKick.visible = false;

        HardKick = new AttackClass("Assets/Charecter 1/C1 LightKick.png", 7, 1, 7, 5, 8, false);
        HardKick.SetOrigin(HardKick.width / 2, HardKick.height / 2);
        HardKick.SetCycle(0, 7, 6);
        HardKick.SetXY(0, -8);
        AddChild(HardKick);
        HardKick.visible = false;

        CrouchAttack = new AttackClass("Assets/Charecter 1/C1 CrouchAttack.png", 5, 1, 5, 2, 8, false);
        CrouchAttack.SetOrigin(CrouchAttack.width / 2, CrouchAttack.height / 2);
        CrouchAttack.SetCycle(0, 5, 6);
        CrouchAttack.SetXY(0, -8);
        AddChild(CrouchAttack);
        CrouchAttack.visible = false;

        JumpAttack = new AttackClass("Assets/Charecter 1/C1 JumpAttack.png", 3, 1, 1, 0, 8, false);
        JumpAttack.SetOrigin(JumpAttack.width / 2, JumpAttack.height / 2);
        JumpAttack.SetCycle(0, 1, 4);
        JumpAttack.SetXY(0, -8);
        AddChild(JumpAttack);
        JumpAttack.visible = false;
        //States
        idle = new AnimationSprite("Assets/Charecter 1/C1 Idle.png", 2, 1, 2, false, false);
        idle.SetOrigin(idle.width / 2, idle.height / 2);
        idle.SetCycle(0, 2, 16);
        idle.SetXY(0, -8);
        AddChild(idle);
        idle.visible = false;

        Crouch = new AnimationSprite("Assets/Charecter 1/C1 Crouch.png", 2, 1, 2, false, false);
        Crouch.SetOrigin(Crouch.width / 2, Crouch.height / 2);
        Crouch.SetCycle(0, 2, 16);
        Crouch.SetXY(0, -8);
        AddChild(Crouch);
        Crouch.visible = false;

        Hit = new AnimationSprite("Assets/Charecter 1/C1 Hit.png", 3, 1, 3, false, false);
        Hit.SetOrigin(Hit.width / 2, Hit.height / 2);
        Hit.SetCycle(0, 3, 8);
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
        Walk.SetCycle(0, 4, 6);
        Walk.SetXY(0, -8);
        AddChild(Walk);
        Walk.visible = false;

        Jump = new AnimationSprite("Assets/Charecter 1/C1 Jump.png", 1, 1, 1, false, false);
        Jump.SetOrigin(Jump.width / 2, Jump.height / 2);
        Jump.SetCycle(0, 4, 4);
        Jump.SetXY(0, -8);
        AddChild(Jump);
        Jump.visible = false;

        DashForward = new AnimationSprite("Assets/Charecter 1/C1 DashForward.png", 3, 1, 3, false, false);
        DashForward.SetOrigin(DashForward.width / 2, DashForward.height / 2);
        DashForward.SetCycle(0, 3, 4);
        DashForward.SetXY(0, -8);
        AddChild(DashForward);
        DashForward.visible = false;

        DashBackwards = new AnimationSprite("Assets/Charecter 1/C1 DashBackwards.png", 3, 1, 3, false, false);
        DashBackwards.SetOrigin(DashBackwards.width / 2, DashBackwards.height / 2);
        DashBackwards.SetCycle(0, 3, 4);
        DashBackwards.SetXY(0, -8);
        AddChild(DashBackwards);
        DashBackwards.visible = false;
    }

    protected override void SetVisible(AnimationSprite vis)
    {
        //Sets the current sprite based on state and animates it
        if (!vis.visible)
        {
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
            idle.visible = false;
            vis.visible = true;
        }
        // some sprites should not repeat 
        if (vis.currentFrame == vis.frameCount - 1 && (vis is AttackClass || vis == Hit || vis == Block || vis == DashBackwards || vis == DashForward))
        {
            if (vis is AttackClass)
            {
                BaseHurtBox();
                attacking = false;
                vis.currentFrame = 0;
                if (vis == CrouchAttack)
                    crouching = true;
            }
            if (vis == Hit)
            {
                Hit.currentFrame = 0;
                hit = false;
            }
            if (vis == Block && opponent.GetCurrentAttack().GetState() == AttackClass.State.Recovery)
            {
                Block.currentFrame = 0;
                block = false;
            }
            if(vis == DashBackwards || vis == DashForward)
            {
                vis.currentFrame = 0;
                dashLeft = false;
                dashRight = false;
            }

        }
        else
            vis.Animate();
    }
    protected override void SetState()
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

                }
                break;
            case State.Netural:
                {
                    SetVisible(idle);
                    BasePlayerColl();
                }
                break;
            case State.Block:
                {
                    if (Block.currentFrame == Block.frameCount - 2)
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
            case State.Dead:
                {
                    SetVisible(idle);
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

    protected override void MoveInputs()
    {
        //Left
        if (LastInput() == 1 && !block)
        {
            if (DashLeft())
            {
                vx = -Horizontalspeed * 15;
                dashLeft = true;
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
                vx = Horizontalspeed * 15;
                dashRight = true;
            }
            else
            {
                vx = Horizontalspeed;
                current = State.Move;
                moving = true;
            }
        }
        if ((LastInput() == 6) && !block)
        {
            current = State.Crouch;
            crouching = true;
        }
        if (LastInput() == 7)
        {
            current = State.Crouch;
            crouching = true;
        }
        if ((LastInput() == 8) && !block)
        {
            current = State.Crouch;
            crouching = true;
        }
    }

    protected override void LPAttack()
    {
        LightPunch.ResetAttack();
        GetCurrentAttack(LightPunch);
        SetHurBox(60, -25, 2f, 1f);
        SetAttackDmg(20, 4);
        attackCode = 1;
    }

    protected override void HPAttack()
    {
        HardPunch.ResetAttack();
        GetCurrentAttack(HardPunch);
        SetHurBox(60, -25, 2.5f, 1f);
        SetAttackDmg(30, 6);
        attackCode = 2;
        if (scaleX > 0)
            vx += Horizontalspeed * 10;
        else
            vx -= Horizontalspeed * 10;
    }
    protected override void LKAttack()
    {
        LightKick.ResetAttack();
        GetCurrentAttack(LightKick);
        SetHurBox(80, -50, 1.5f, 0.7f);
        hurtBox.rotation = -40;
        SetAttackDmg(10, 3);
        attackCode = 3;
    }

    void CrouchingAttack()
    {
        CrouchAttack.ResetAttack();
        GetCurrentAttack(CrouchAttack);
        SetHurBox(70, 50, 1.5f, 1f);
        SetAttackDmg(10, 3);
        attackCode = 5;
    }

    void JumpingAttack()
    {
        JumpAttack.ResetAttack();
        GetCurrentAttack(JumpAttack);
        SetHurBox(20, 80, 5f, 2f);
        SetAttackDmg(10, 3);
        attackCode = 6;
    }
    protected override void AttackInputs()
    {
        //Console.WriteLine(ZMotionRight());
        //To achive a motion input with attack use LastInput for attack
        //Add hitbox changes
        if (ValidInput(9) && grounded)
        {
            LPAttack();
            //if(!grounded) jump attack (insert here)
            current = State.Attack;
            attacking = true;
        }
        else if (ValidInput(10) && grounded)
        {
            HPAttack();
            current = State.Attack;
            attacking = true;
        }
        else if (ValidInput(11))
        {
            if (current != State.Crouch && grounded)
            {
                LKAttack();
            }
            else if (grounded && current == State.Crouch)
            {
                CrouchingAttack();
            }
            else if (!grounded && !crouching)
            {
                JumpingAttack();
            }
            current = State.Attack;
            attacking = true;
        }
        /*else if (ValidInput(12))
        {
            HKAttack();
            current = State.Attack;
            attacking = true;
        }*/

    }

    protected override void OnAttack()
    {
        if (attackCode == 1)
        {
            if (LightPunch.FrameValidation())
            {
                AttackCollision(LightPunch);
            }

            if (LightPunch.HitConfirm() && LightPunch.GetState() == AttackClass.State.Recovery && ValidInput(10))
            {
                HPAttack();
                //HardPunch.currentFrame = 1;
            }
            else
                SetVisible(LightPunch);
        }
        if (attackCode == 2)
        {
            if (HardPunch.FrameValidation())
            {
                AttackCollision(HardPunch);
            }
            SetVisible(HardPunch);
        }

        if (attackCode == 3)
        {
            if (LightKick.FrameValidation())
            {
                AttackCollision(LightKick);
            }
            SetVisible(LightKick);
        }
        if(attackCode == 5)
        {
            if (CrouchAttack.FrameValidation())
            {
                AttackCollision(CrouchAttack);
            }
            SetVisible(CrouchAttack);
        }
        if (attackCode == 6)
        {
            if (JumpAttack.FrameValidation())
            {
                AttackCollision(JumpAttack);
            }
            SetVisible(JumpAttack);
        }
        if (CurrentAttack.Cancel() && CurrentAttack.GetState() == AttackClass.State.Recovery)
        {

        }
    }

}