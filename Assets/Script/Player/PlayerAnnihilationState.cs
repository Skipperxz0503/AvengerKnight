using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnnihilationState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayerAnnihilationState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimFinishTrigger()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!skillUsed) 
            {
                if(player.skill.annihilation.CanUseSkill())
                    skillUsed = true;
            }

        }
        if (player.skill.annihilation.SkillCompleted())
        {
            stateMachine.changeState(player.airState);
        }

    }
}
