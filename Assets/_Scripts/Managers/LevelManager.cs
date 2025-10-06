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

    private Dictionary<PlayerContext, int> playerScores = new Dictionary<PlayerContext, int>();

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
        player1PointsTxt.text = $"Player 1 Score: {playerScores[context]}";
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
        if (context != null && !playerScores.ContainsKey(context))
        {
            playerScores.Add(context, 0);
        }
    }

    public void UpdatePlayerScore(PlayerContext playerContext, int newScore)
    {
        if (playerContext != null && playerScores.ContainsKey(playerContext))
        {
            playerScores[playerContext] = newScore;
            UpdateScoreUI();
        }
        else
        {
            Debug.LogWarning("[LevelManager] Tried to update a player that isn't registered.");
        }
    }

    public void AddPoints(PlayerContext playerContext, int points)
    {
        if (playerContext != null && playerScores.ContainsKey(playerContext))
        {
            playerScores[playerContext] += points;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        int i = 1;
        foreach (var kvp in playerScores)
        {
            if (i == 1) player1PointsTxt.text = $"Player 1 Score: {kvp.Value}";
            if (i == 2) player2PointsTxt.text = $"Player 2 Score: {kvp.Value}";
            i++;
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

        timerText.text = "Time's up!";
    }
}