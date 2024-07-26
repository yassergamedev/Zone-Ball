using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public bool insideGame = false;
    private float timeSpent = 0f;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep the instance across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (insideGame)
        {
            float deltaTime = Time.deltaTime;

            // Ensure deltaTime is a reasonable value
            if (deltaTime < 0 || deltaTime > 1)
            {
                Debug.LogWarning("Unusual deltaTime detected: " + deltaTime);
            }

            timeSpent += deltaTime;
            //Debug.Log("Time.deltaTime: " + deltaTime + " | Total Time Spent: " + timeSpent);
        }
    }
    public void StartGame()
    {
        insideGame = true;
    }
    public void EndGame()
    {
        // This method should be called when the game ends
        insideGame = false;
        SaveGameTime();


    }
    private void OnApplicationQuit()
    {
        if (insideGame)
        {
            SaveGameTime();
        }
     }


    private void SaveGameTime()
    {
        FileDataHandler<CurrentGame> currHandler = new FileDataHandler<CurrentGame>(Application.persistentDataPath, "Current Game");
        CurrentGame currentG = currHandler.Load();
        currentG.timePlayed += timeSpent;
        currHandler.Save(currentG);

        FileDataHandler<Games> gamesHandler = new FileDataHandler<Games>(Application.persistentDataPath, "Games");
        Games games = gamesHandler.Load();

        for (int i = 0; i < games.games.Count; i++)
        {
            if (games.games[i].currentGame == currentG.currentGame)
            {
                games.games[i].timePlayed += timeSpent; // Add the accumulated time
                break;
            }
        }
        gamesHandler.Save(games);

        // Reset the timer
        timeSpent = 0f;
    }
}
