using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]  protected LayerMask whatIsPlayer;


    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    [Header("Attack info")]
    public float atkDistance;
    public float atkCooldown;
    [HideInInspector]  public float lastTimeAtk;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
    public virtual void AnimFinishTrigger() => stateMachine.currentState.AnimFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right *facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + atkDistance *facingDir, transform.position.y));
    }
}
