using System.Diagnostics;

public class RunState : PlayerState
{
    public RunState(PlayerStateMachine stateMachine, PlayerContext playerContext)
        : base(stateMachine, playerContext) { }
    public override void Enter()
    {
        Debug.WriteLine("Entering Run State");
 //       playerContext.HandleAnimations.ChangeAnimationState("Running");
        playerContext.PlayerController.ChangeSpeed(playerContext.PlayerController.RunningSpeed);
    }
    public override void Update()
    {
        stateMachine.ResetAnimations();
        if (playerContext.HandleInputs.IsAttacking())
            stateMachine.ChangeState(stateMachine.attackState);
        if (!playerContext.HandleInputs.IsRunning())
            stateMachine.ChangeState(stateMachine.walkState);
    }
}