using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LevelManager : MonoBehaviourPun
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] InteractionButton starSesionButton;
    [SerializeField] float gameTime = 90f;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text player1PointsTxt;
    [SerializeField] TMP_Text player2PointsTxt;
    [SerializeField] Spawner[] toyMachines;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    private bool gameStarted = false;
    private Coroutine gameTimer;

    void Start()
    {
        // Spawn player (local one, most likely)
        GameObject newPlayer = ConnectionManager.Instance.CreatePlayer(spawnPoint);
        PlayerContext context = newPlayer.GetComponent<PlayerContext>();

        // Register them in the dictionary
        TryAddPlayer(context);

        // Initialize UI
        player1PointsTxt.text = "Player 1 Score: 0";
        player2PointsTxt.text = "Player 2 Score: 0";
        timerText.text = "Press the button to start the work session";
    }

    void Update()
    {
        if (starSesionButton.IsOn && !gameStarted)
        {
            gameTimer = StartCoroutine(GameTimer());
            gameStarted = true;

            foreach (var machine in toyMachines)
                machine.StartSpawning();
        }
    }

    public void TryAddPlayer(PlayerContext context)
    {
        int actorNumber = context.PhotonView.Owner.ActorNumber;
        if (!playerScores.ContainsKey(actorNumber))
        {
            playerScores.Add(actorNumber, 0);
        }
    }

    public void AddPoints(PlayerContext playerContext, int points)
    {
        if (playerContext == null) return;

        int actorNumber = playerContext.PhotonView.Owner.ActorNumber;

        if (PhotonNetwork.IsMasterClient)
        {
            //  El MasterClient mantiene el estado de puntajes
            if (!playerScores.ContainsKey(actorNumber))
                playerScores.Add(actorNumber, 0);

            playerScores[actorNumber] += points;

            //  Notificamos a todos los clientes el nuevo puntaje
            photonView.RPC("RPC_UpdateScores", RpcTarget.AllBuffered, actorNumber, playerScores[actorNumber]);
        }
        else
        {
            //  Si no soy el Master, envío una solicitud para sumar puntos
            photonView.RPC("RPC_RequestAddPoints", RpcTarget.MasterClient, actorNumber, points);
        }
    }

    [PunRPC]
    private void RPC_RequestAddPoints(int actorNumber, int points)
    {
        if (!playerScores.ContainsKey(actorNumber))
            playerScores.Add(actorNumber, 0);

        playerScores[actorNumber] += points;

        photonView.RPC("RPC_UpdateScores", RpcTarget.AllBuffered, actorNumber, playerScores[actorNumber]);
    }

    // Actualiza el puntaje en todos los clientes
    [PunRPC]
    private void RPC_UpdateScores(int actorNumber, int newScore)
    {
        playerScores[actorNumber] = newScore;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        // Obtenemos los dos jugadores ordenados por ActorNumber
        List<int> sortedKeys = new List<int>(playerScores.Keys);
        sortedKeys.Sort();

        if (sortedKeys.Count > 0)
            player1PointsTxt.text = $"Player 1 Score: {playerScores[sortedKeys[0]]}";
        if (sortedKeys.Count > 1)
            player2PointsTxt.text = $"Player 2 Score: {playerScores[sortedKeys[1]]}";
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

        timerText.text = "Time's up!";
    }
}