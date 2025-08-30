using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;
    private PlayerContext playerContext;

    // PLAYER STATES
    public IdleState idleState;
    public WalkState walkState;
    public AttackState attackState;
    public RunState runningState;

    void Awake()
    {
        playerContext = GetComponent<PlayerContext>();

        // Initialize states
        idleState = new IdleState(this, playerContext);
        walkState = new WalkState(this, playerContext);
        runningState = new RunState(this, playerContext);
        attackState = new AttackState(this, playerContext);
    }

    void Start()
    {
        ChangeState(idleState); // Starting State
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void ResetAnimations()
    {
       if (playerContext.HandleInputs.GetMoveVector2() != Vector2.zero) // Check for player movement
        {
            ChangeState(walkState);
        }
        else
            ChangeState(idleState);
    }
}
