using System.Collections;
using UnityEngine;
using TMPro;
public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] InteractionButton starSesionButton;
    [SerializeField] float gameTime = 90f;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Spawner[] toyMachines;

    bool gameStarted = false;
    Coroutine gameTimer;
    void Start()
    {
        ConnectionManager.Instance.CreatePlayer(spawnPoint);
        timerText.text = " Press the button to start the work session";
    }

   public void Update()
    {
        if (starSesionButton.IsOn && !gameStarted)
        {
            gameTimer = StartCoroutine(GameTimer());
            gameStarted = true;
           
            foreach (var machine in toyMachines)
            {
                machine.StartSpawning();
            }   
        }
    }


    IEnumerator GameTimer()
    {
        float timeLeft = gameTime;
        timeLeft += 1; // Ajuste para compensar el primer decremento inmediato
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = " Work session ends in " + string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        timerText.text = "Time's up!";
    }
}
