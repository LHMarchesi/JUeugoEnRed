public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine, PlayerContext playerContext)
        : base(stateMachine, playerContext) { }

    public override void Enter()
    {
        //  if (playerContext.Weapon.IsHeld())
        //   {
        //     playerContext.HandleAnimations.ChangeAnimationState("Idle");
        //      }
        //      else
        //  {
        //        playerContext.HandleAnimations.ChangeAnimationState("IdleWithOutHammer");
        //  }
    }

    public override void Update()
    {
        stateMachine.ResetAnimations();

        if (playerContext.HandleInputs.IsAttacking())
            stateMachine.ChangeState(stateMachine.attackState);
    }
}
