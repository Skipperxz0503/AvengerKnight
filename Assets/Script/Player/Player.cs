using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Atk details")]
    public Vector2[] atkMovement;
    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 13;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }


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
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
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
    }

    public void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }
            
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if(dashDir == 0)
            {
                dashDir = facingDir;
            } 

            stateMachine.changeState(dashState);
        }
    }


}
