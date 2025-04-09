using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]  protected LayerMask whatIsPlayer;
    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDir;
    public bool canBeStun;
    [SerializeField] protected GameObject counterImg;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float atkDistance;
    public float atkCooldown;
    [HideInInspector]  public float lastTimeAtk;

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;

        }
    }

    public virtual void ImmobilizedFor(float _duration) => StartCoroutine(FreezeTimeFor(_duration));

    protected virtual IEnumerator FreezeTimeFor(float _second)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_second);
        FreezeTime(false);
    }

    #region Counter Atk Window
    public virtual void OpenCounterAtkWindow()
    {
        canBeStun = true;
        counterImg.SetActive(true);

    }
    public virtual void CloseCounterAtkWindow()
    {
        canBeStun = false;
        counterImg.SetActive(false);
    }
    #endregion
    public virtual bool CheckCanBeStun()
    {
        if (canBeStun) 
        { 
            CloseCounterAtkWindow();
            return true;
        }
        return false; 
    }
    public virtual void AnimFinishTrigger() => stateMachine.currentState.AnimFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right *facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + atkDistance *facingDir, transform.position.y));
    }
}
