using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAtkState : PlayerState
{
    private int comboCounter;
    private float lastTimeAtk;
    private float comboWindow = 2;

    public PlayerAtkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAtk + comboWindow) 
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float atkDir = player.facingDir;
        if(xInput != 0)
        {
            atkDir = xInput;
        }

        player.SetVelocity(player.atkMovement[comboCounter].x * atkDir, player.atkMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter++;
        lastTimeAtk = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
        {
            player.ZeroVelocity();
        }
        if (triggerCalled) 
        {
            stateMachine.changeState(player.idleState);
        }
    }
}
