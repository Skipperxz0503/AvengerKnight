using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            stateMachine.changeState(player.playerAtk);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.changeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) 
        { 
            stateMachine.changeState(player.jumpState);
        }
    }
}
