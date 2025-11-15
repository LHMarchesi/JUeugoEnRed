using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] InteractionButton startSesionButton;
    [SerializeField] float gameTime = 90f;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text player1PointsTxt;
    [SerializeField] TMP_Text player2PointsTxt;
    [SerializeField] Spawner[] toyMachines;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();
    private bool gameStarted = false;
    private Coroutine gameTimer;
    private List<int> actornumbers = new List<int>();
    private Coroutine gameLeave;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        GameObject newPlayer = ConnectionManager.Instance.CreatePlayer(spawnPoint);
        PlayerContext context = newPlayer.GetComponent<PlayerContext>();
        TryAddPlayer(context);

        player1PointsTxt.gameObject.SetActive(false);
        player2PointsTxt.gameObject.SetActive(false);
        timerText.text = "Press the button to start the work session";

        ConnectionManager.Instance.OnPlayerLeftRoom += WinByDisconection;
    }


    void Update()
    {
        if (startSesionButton.IsOn && !gameStarted)
        {
            gameTimer = StartCoroutine(GameTimer());
            gameStarted = true;

            foreach (var machine in toyMachines)
                machine.StartSpawning();

            player1PointsTxt.gameObject.SetActive(true);
            player2PointsTxt.gameObject.SetActive(true);
            SyncScoresToAll();
        }

    }

    IEnumerator GameTimer()
    {
        float timeLeft = gameTime + 1f;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;

            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = $"Work session ends in {minutes:00}:{seconds:00}";
        }

        timerText.text = "Time's up!";

        if (PhotonNetwork.IsMasterClient)
            DetermineWinner();
    }

    public void TryAddPlayer(PlayerContext context)
    {
        int actorNumber = context.PhotonView.Owner.ActorNumber;

        if (!playerScores.ContainsKey(actorNumber))
        {
            playerScores.Add(actorNumber, 0);
            actornumbers.Add(actorNumber);
        }
    }

    public void AddPoints(PlayerContext playerContext, int points)
    {
        int actorNumber = playerContext.PhotonView.Owner.ActorNumber;

        if (PhotonNetwork.IsMasterClient)
        {
            if (!playerScores.ContainsKey(actorNumber))
                playerScores.Add(actorNumber, 0);

            playerScores[actorNumber] += points;
            SyncScoresToAll();
        }
        else
        {
            photonView.RPC("RPC_RequestAddPoints", RpcTarget.MasterClient, actorNumber, points);
        }
    }

    [PunRPC]
    private void RPC_RequestAddPoints(int actorNumber, int points)
    {
        if (!playerScores.ContainsKey(actorNumber))
            playerScores.Add(actorNumber, 0);

        playerScores[actorNumber] += points;
        SyncScoresToAll();
    }

    //  Solo el Master llama a esto
    private void SyncScoresToAll()
    {
        List<string> names = new List<string>();
        List<int> scores = new List<int>();

        foreach (var p in PhotonNetwork.PlayerList)
        {
            int actor = p.ActorNumber;
            int score = playerScores.ContainsKey(actor) ? playerScores[actor] : 0;
            names.Add(p.NickName);
            scores.Add(score);
        }

        photonView.RPC("RPC_SyncScores", RpcTarget.All, names.ToArray(), scores.ToArray());
    }

    [PunRPC]
    private void RPC_SyncScores(string[] playerNames, int[] scores)
    {
        // Se actualiza la UI con los datos exactos enviados por el Master
        if (playerNames.Length > 0)
            player1PointsTxt.text = $"{playerNames[0]}: {scores[0]}";

        if (playerNames.Length > 1)
            player2PointsTxt.text = $"{playerNames[1]}: {scores[1]}";
    }

    private void DetermineWinner()
    {
        if (playerScores.Count == 0)
        {
            Debug.Log("No players found.");
            return;
        }

        List<KeyValuePair<int, int>> sortedScores = new List<KeyValuePair<int, int>>(playerScores);
        sortedScores.Sort((a, b) => b.Value.CompareTo(a.Value));

        int highestScore = sortedScores[0].Value;
        List<int> winners = new List<int>();

        foreach (var pair in sortedScores)
        {
            if (pair.Value == highestScore)
                winners.Add(pair.Key);
            else
                break;
        }


        if (winners.Count > 1)
        {
            photonView.RPC("RPC_ShowWinnerMessage", RpcTarget.All, $"Empate entre {winners.Count} jugadores con {highestScore} puntos.");
        }
        else
        {
            string winnerName = PhotonNetwork.CurrentRoom.GetPlayer(winners[0]).NickName;
            photonView.RPC("RPC_ShowWinnerMessage", RpcTarget.All, $"{winnerName} ganó con {highestScore} puntos.");
        }
    }

    [PunRPC]
    private void RPC_ShowWinnerMessage(string message)
    {
        Debug.Log(message);
        timerText.text = message;
    }

    public void WinByDisconection(Player otherPlayer)
    {
        Debug.LogError(otherPlayer.NickName + " disconnecteeeed!!");
        StopAllCoroutines();
        int winner = -1;
        string nickname = null;
        foreach (var p in PhotonNetwork.PlayerList)
        {
            winner = p.ActorNumber;
            nickname = p.NickName;
            if (winner != otherPlayer.ActorNumber)
                break;
        }

        UIPlayerManager.Instance.ShowWinScreen(true, nickname + " won by disconection");
        gameLeave = StartCoroutine(WaitAndLeaveGame(5f));
        Debug.Log(nickname + " player id " + winner + " won by disconection");
    }

    IEnumerator WaitAndLeaveGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LeaveGame();
    }

    void LeaveGame()
    {
        gameLeave = null;
        TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 0);
        ConnectionManager.Instance.photonPunManager.LeaveRoom();
        GameManager.Instance.ChangeGameState(new GameState());
    }

}
