using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAtkState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonAtkState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAtk = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
