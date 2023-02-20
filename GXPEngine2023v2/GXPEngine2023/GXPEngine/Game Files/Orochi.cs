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
        idle = new AnimationSprite("Assets/Charecter 1/C1 Idle.png", 2, 1, 2, false, false);
        idle.SetOrigin(idle.width / 2, idle.height / 2);
        idle.SetCycle(0, 2, 16);
        idle.SetXY(0, -8);
        AddChild(idle);
        idle.visible = false;

        LightPunch = new AttackClass("Assets/Charecter 1/C1 LightPunch.png", 5, 1, 2, 2, 10, false);
        LightPunch.SetOrigin(LightPunch.width / 2, LightPunch.height / 2);
        LightPunch.SetCycle(0, 5, 6);
        LightPunch.SetXY(0, -8);
        AddChild(LightPunch);
        LightPunch.visible = false;

        HardPunch = new AttackClass("Assets/Charecter 1/C1 HardPunch.png", 6, 1, 2, 3, 14, true);
        HardPunch.SetOrigin(HardPunch.width / 2, HardPunch.height / 2);
        HardPunch.SetCycle(0, 6, 8);
        HardPunch.SetXY(0, -8);
        AddChild(HardPunch);
        HardPunch.visible = false;

        LightKick = new AttackClass("Assets/Charecter 1/C1 LightKick.png", 4, 1, 2, 2, 8, false);
        LightKick.SetOrigin(LightKick.width / 2, LightKick.height / 2);
        LightKick.SetCycle(0, 5, 5);
        LightKick.SetXY(0, -8);
        AddChild(LightKick);
        LightKick.visible = false;
        //Remember to change the hit sprite
        Hit = new AnimationSprite("Assets/Charecter 1/C1 Block.png", 3, 1, 3, false, false);
        Hit.SetOrigin(Hit.width / 2, Hit.height / 2);
        Hit.SetCycle(0, 3, 8);
        Hit.SetXY(0, -8);
        AddChild(Hit);
        Hit.visible = false;

        Block = new AnimationSprite("Assets/Charecter 1/C1 Block.png", 3, 1, 3, false, false);
        Block.SetOrigin(Block.width / 2, Block.height / 2);
        Block.SetCycle(0, 2, 8);
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
    }

    protected override void SetVisible(AnimationSprite vis)
    {
        //Sets the current sprite based on state and animates it
        if (!vis.visible)
        {
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
        if ((vis == LightPunch || vis == LightKick || vis == HardPunch || vis == Block || vis == Hit) && vis.currentFrame == vis.frameCount - 1)
        {
            if ((vis == LightPunch || vis == LightKick || vis == HardPunch))
            {
                BaseHurtBox();
                attacking = false;
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


        }
        else
            vis.Animate();
    }

    protected override void LPAttack()
    {
        LightPunch.ResetAttack();
        GetCurrentAttack(LightPunch);
        SetHurBox(60, -35, 2f, 1f);
        SetAttackDmg(20, 4);
        attackCode = 1;
    }

    protected override void HPAttack()
    {
        HardPunch.ResetAttack();
        GetCurrentAttack(HardPunch);
        SetHurBox(60, -35, 2.5f, 1f);
        SetAttackDmg(30, 6);
        attackCode = 2;
    }
    protected override void LKAttack()
    {
        LightKick.ResetAttack();
        GetCurrentAttack(LightKick);
        SetHurBox(70, -30, 2.8f, 1f);
        hurtBox.rotation = -30;
        SetAttackDmg(10, 3);
        attackCode = 3;
    }

    protected override void AttackInputs()
    {
        //Console.WriteLine(ZMotionRight());
        //To achive a motion input with attack use LastInput for attack
        //Add hitbox changes
        if (ValidInput(9))
        {
            LPAttack();
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
        else if (ValidInput(11))
        {
            LKAttack();
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
        if (CurrentAttack.Cancel() && CurrentAttack.GetState() == AttackClass.State.Recovery)
        {

        }
    }

    protected override void ResetAttacks()
    {
        LightPunch.currentFrame = 0;
        HardPunch.currentFrame = 0;
        LightKick.currentFrame = 0;
    }

}