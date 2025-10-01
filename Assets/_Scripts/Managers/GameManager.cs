using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameStates
{
    MainMenu, Pause, Game, Win, Lose
}
public interface IGameState
{
    public void Enter();
    public void Update();
    public void Exit();
}

public class GameStateMachine
{
    public IGameState CurrentState { get => currentState; private set { } }

    private IGameState currentState;

    public void ChangeState(IGameState state)
    {
        currentState?.Exit();
        currentState = state;
        UnityEngine.Debug.Log(state);
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}

public class GameManager : Singleton<GameManager>
{
    GameStateMachine gameStateMachine;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        gameStateMachine = new GameStateMachine();
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        switch (currentBuildIndex)
        {
            case 0:
                gameStateMachine.ChangeState(new MainMenuState());
                break;
            case 1:
                gameStateMachine.ChangeState(new GameState());
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        gameStateMachine.Update();
    }

    public void ChangeGameState(IGameState state)
    {
        gameStateMachine.ChangeState(state);
    }
}

public class MainMenuState : IGameState
{
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}

public class GameState : IGameState
{
    public void Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ChangeGameState(new PauseState());
        }
    }
}

public class PauseState : IGameState
{
    PlayerContext playerContext;
    public void Enter()
    {
        playerContext = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContext>();
        playerContext.HandleInputs.SetPaused(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Exit()
    {
        playerContext.HandleInputs.SetPaused(false);
        Cursor.visible = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ChangeGameState(new GameState());
        }
    }
}
public class LoseState : IGameState
{
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}

public class WinState : IGameState
{
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}