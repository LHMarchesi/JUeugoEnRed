using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerStateMachine stateMachine, PlayerContext playerContext)
        : base(stateMachine, playerContext) { }

    private float attackDuration = 0.6f;
    private float timer = 0f;

    public override void Enter()
    {
        playerContext.HandleAnimations.ChangeAnimationState("AttackWithHammer");
        timer = 0f;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackDuration)
        {
            stateMachine.ResetAnimations();
        }
    }
}
