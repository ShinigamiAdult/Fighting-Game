using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;
using GXPEngine.Core;

class Momo : Character
{
    public Momo(TiledObject obj = null) : base(obj)
    {

    }

    public Momo(bool assign) : base(assign)
    {

    }

    protected override void SetStats()
    {
        base.SetStats();
    }
    protected override void Initalize()
    {
        //Attacks
        LightPunch = new AttackClass("Assets/Charecter 2/C2 LightPunch.png", 5, 1, 5, 2, 10, true);
        LightPunch.SetOrigin(LightPunch.width / 2, LightPunch.height / 2);
        LightPunch.SetCycle(0, 5, 6);
        LightPunch.SetXY(0, -8);
        AddChild(LightPunch);
        LightPunch.visible = false;

        HardPunch = new AttackClass("Assets/Charecter 2/C2 HardPunch.png", 5, 1, 5, 3, 14, false);
        HardPunch.SetOrigin(HardPunch.width / 2, HardPunch.height / 2);
        HardPunch.SetCycle(0, 6, 6);
        HardPunch.SetXY(0, -8);
        AddChild(HardPunch);
        HardPunch.visible = false;

        /*LightKick = new AttackClass("Assets/Charecter 2/C2 LightKick.png", 5, 1, 5, 2, 8, false);
        LightKick.SetOrigin(LightKick.width / 2, LightKick.height / 2);
        LightKick.SetCycle(0, 5, 6);
        LightKick.SetXY(0, -8);
        AddChild(LightKick);
        LightKick.visible = false;*/

        //Edit hard kick could become cool special and another hard kick

        /*HardKick = new AttackClass("Assets/Charecter 2/C2 HardKick.png", 11, 1, 11, 6, 14, true);
        HardKick.SetOrigin(HardKick.width / 2, HardKick.height / 2);
        HardKick.SetCycle(0, 11, 6);
        HardKick.SetXY(0, -8);
        AddChild(HardKick);
        HardKick.visible = false;*/

        /*SpecialAttack = new AttackClass("Assets/Charecter 2/C2 SpecialAttack.png", 12, 1, 12, 4, 20, true);
        SpecialAttack.SetOrigin(SpecialAttack.width / 2, SpecialAttack.height / 2);
        SpecialAttack.SetCycle(0, 12, 6);
        SpecialAttack.SetXY(100, -200);
        AddChild(SpecialAttack);
        SpecialAttack.visible = false;*/

        CrouchAttack = new AttackClass("Assets/Charecter 2/C2 CrouchAttack.png", 5, 1, 5, 2, 8, false);
        CrouchAttack.SetOrigin(CrouchAttack.width / 2, CrouchAttack.height / 2);
        CrouchAttack.SetCycle(0, 5, 6);
        CrouchAttack.SetXY(0, -8);
        AddChild(CrouchAttack);
        CrouchAttack.visible = false;

        /*JumpAttack = new AttackClass("Assets/Charecter 2/C2 JumpAttack.png", 3, 1, 1, 0, 8, false);
        JumpAttack.SetOrigin(JumpAttack.width / 2, JumpAttack.height / 2);
        JumpAttack.SetCycle(0, 1, 20);
        JumpAttack.SetXY(0, -8);
        AddChild(JumpAttack);
        JumpAttack.visible = false;*/

        //States
        idle = new AnimationSprite("Assets/Charecter 2/C2 Idle.png", 2, 1, 2, false, false);
        idle.SetOrigin(idle.width / 2, idle.height / 2);
        idle.SetCycle(0, 2, 16);
        idle.SetXY(0, -8);
        AddChild(idle);
        idle.visible = false;

        Crouch = new AnimationSprite("Assets/Charecter 2/C2 Crouch.png", 1, 1, 2, false, false);
        Crouch.SetOrigin(Crouch.width / 2, Crouch.height / 2);
        Crouch.SetCycle(0, 1, 16);
        Crouch.SetXY(0, -8);
        AddChild(Crouch);
        Crouch.visible = false;

        /*Hit = new AnimationSprite("Assets/Charecter 2/C2 Hit.png", 3, 1, 3, false, false);
        Hit.SetOrigin(Hit.width / 2, Hit.height / 2);
        Hit.SetCycle(0, 3, 10);
        Hit.SetXY(0, -8);
        AddChild(Hit);
        Hit.visible = false;*/

        Block = new AnimationSprite("Assets/Charecter 2/C2 Block.png", 3, 1, 3, false, false);
        Block.SetOrigin(Block.width / 2, Block.height / 2);
        Block.SetCycle(0, 3, 6);
        Block.SetXY(0, -8);
        AddChild(Block);
        Block.visible = false;

        Walk = new AnimationSprite("Assets/Charecter 2/C2 Walk.png", 4, 1, 4, false, false);
        Walk.SetOrigin(Walk.width / 2, Walk.height / 2);
        Walk.SetCycle(0, 4, 6);
        Walk.SetXY(0, -8);
        AddChild(Walk);
        Walk.visible = false;

        Jump = new AnimationSprite("Assets/Charecter 2/C2 Jump.png", 1, 1, 1, false, false);
        Jump.SetOrigin(Jump.width / 2, Jump.height / 2);
        Jump.SetCycle(0, 1, 4);
        Jump.SetXY(0, -8);
        AddChild(Jump);
        Jump.visible = false;

        /*KnockDown = new AnimationSprite("Assets/Charecter 2/C2 KnockDown.png", 1, 1, 1, false, false);
        KnockDown.SetOrigin(KnockDown.width / 2, KnockDown.height / 2);
        KnockDown.SetCycle(0, 1, 4);
        KnockDown.SetXY(0, -8);
        AddChild(KnockDown);
        KnockDown.visible = false;*/

        /*DashForward = new AnimationSprite("Assets/Charecter 2/C2 DashForward.png", 3, 1, 3, false, false);
        DashForward.SetOrigin(DashForward.width / 2, DashForward.height / 2);
        DashForward.SetCycle(0, 3, 4);
        DashForward.SetXY(0, -8);
        AddChild(DashForward);
        DashForward.visible = false;*/

        DashBackwards = new AnimationSprite("Assets/Charecter 2/C2 DashBackwards.png", 3, 1, 3, false, false);
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
            //KnockDown.visible = false;
            //SpecialAttack.visible = false;
            //HardKick.visible = false;
            DashBackwards.visible = false;
            //DashForward.visible = false;
            //JumpAttack.visible = false;
            CrouchAttack.visible = false;
            Crouch.visible = false;
            Walk.visible = false;
            Block.visible = false;
            //Hit.visible = false;
            LightPunch.visible = false;
            HardPunch.visible = false;
            //LightKick.visible = false;
            Jump.visible = false;
            idle.visible = false;
            vis.visible = true;
        }
        // some sprites should not repeat 
        if (vis.currentFrame == vis.frameCount - 1 && ((vis is AttackClass && (vis != JumpAttack || grounded)) || vis == Hit || vis == Block || vis == DashBackwards || vis == DashForward))
        {
            if (vis is AttackClass)
            {
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
    void LPAttack()
    {
        LightPunch.ResetAttack();
        SetCurrentAttack(LightPunch);
        SetHurBox(60, -25, 2f, 1f);
        SetAttackDmg(20, 4);
        CurrentAttack = LightPunch;
        attacking = true;
    }
    void HPAttack()
    {
        HardPunch.ResetAttack();
        SetCurrentAttack(HardPunch);
        SetHurBox(60, -35, 2.5f, 1f);
        SetAttackDmg(30, 5);
        CurrentAttack = HardPunch;
        attacking = true;
    }
    void LKAttack()
    {
        LightKick.ResetAttack();
        SetCurrentAttack(LightKick);
        SetHurBox(80, -50, 1.5f, 0.7f);
        hurtBox.rotation = -40;
        SetAttackDmg(10, 1);
        CurrentAttack = LightKick;
        attacking = true;
    }
    void HKAttack()
    {
        CurrentAttack = HardKick;
        CurrentAttack.ResetAttack();
        SetCurrentAttack(CurrentAttack);
        SetHurBox(80, -50, 1.5f, 0.7f);
        hurtBox.rotation = -40;
        SetAttackDmg(10, 5, 10);
        attacking = true;

    }
    void CrouchingAttack()
    {
        CrouchAttack.ResetAttack();
        SetCurrentAttack(CrouchAttack);
        SetHurBox(70, 50, 1.5f, 1f);
        SetAttackDmg(10, 3);
        CurrentAttack = CrouchAttack;
        attacking = true;
    }
    void JumpingAttack()
    {
        JumpAttack.ResetAttack();
        SetCurrentAttack(JumpAttack);
        SetHurBox(15, -10, 5f, 1.5f);
        SetAttackDmg(10, 3);
        CurrentAttack = JumpAttack;
        attacking = true;
    }
    void SPAttack()
    {
        CurrentAttack = SpecialAttack;
        CurrentAttack.ResetAttack();
        CurrentAttack.ResetHit(4);
        SetCurrentAttack(CurrentAttack);
        SetHurBox(80, -50, 4f, 8f);
        SetAttackDmg(10, 0, 14);
        attacking = true;
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
        }
        else if (ValidInput(10) && grounded)
        {
            HPAttack();
        }
        else if (ValidInput(11))
        {
            if (!ValidInput(6) && !ValidInput(7) && !ValidInput(8) && grounded)
            {
                LKAttack();
            }
            else if (!grounded && !crouching)
            {
                JumpingAttack();
            }
            else if (grounded && (ValidInput(6) || ValidInput(7) || ValidInput(8)))
            {
                CrouchingAttack();
            }
        }
        else if (ValidInput(12))
        {
            HKAttack();
        }
    }

    protected override void AttackAddition()
    {
        if(CurrentAttack == LightPunch)
            if(CurrentAttack.currentFrame == 1)
            {
                if (scaleX > 0)
                    vx += Horizontalspeed * 3;
                else
                    vx -= Horizontalspeed * 3;
            }
        if (CurrentAttack == HardPunch)
            if (CurrentAttack.currentFrame == 1 || CurrentAttack.currentFrame == 2)
            {
                if (scaleX > 0)
                    vx += Horizontalspeed * 2;
                else
                    vx -= Horizontalspeed * 2;
            }
    }
}
