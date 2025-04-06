using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAtkState : PlayerState
{
    public PlayerCounterAtkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterDuration;
        player.anim.SetBool("AtkCountered",false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.atkCheck.position, player.atkCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CheckCanBeStun()) 
                {
                    stateTimer = 10;
                    player.anim.SetBool("AtkCountered", true);
                }
            }
        }
        if (stateTimer < 0 || triggerCalled) 
        {
            stateMachine.changeState(player.idleState);
        }
    }
}
