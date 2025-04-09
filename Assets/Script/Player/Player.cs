using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{


    [Header("Atk details")]
    public Vector2[] atkMovement;
    public float counterDuration = .2f;

    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 13;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    public float dashDir {  get; private set; }

    public SkillManager skill {  get; private set; }
    public GameObject sword{ get; private set; }

    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerAtkState playerAtk { get; private set; }
    public PlayerCounterAtkState counterAtk { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerAnnihilationState annihilation {  get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");

        moveState = new PlayerMoveState(this, stateMachine, "Move");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");

        dashState = new PlayerDashState(this, stateMachine, "Dash");

        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");

        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        playerAtk = new PlayerAtkState(this, stateMachine, "Atk");

        counterAtk = new PlayerCounterAtkState(this, stateMachine, "CounterAtk");

        aimState = new PlayerAimState(this, stateMachine, "Aim");

        catchSword = new PlayerCatchSwordState(this, stateMachine, "Catch");

        annihilation = new PlayerAnnihilationState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        skill = SkillManager.instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;   


    }

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }

    protected override void Update()
    {
        base.Update();   
        stateMachine.currentState.Update();
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            skill.crystal.CanUseSkill();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;

    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        stateMachine.changeState(catchSword);
        Destroy(sword);
    }

    public void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }
            

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {
                dashDir = facingDir;
            } 

            stateMachine.changeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deadState);
    }


}
