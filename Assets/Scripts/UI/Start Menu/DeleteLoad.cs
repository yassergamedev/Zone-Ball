using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLoad : MonoBehaviour
{
    CurrentGame game;
    Games games;
  
    public void Start()
    {
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
        game = currHandler.Load();

      
        
    }
    public void DeleteLoadFunction(GameObject loadObj)
    {

        FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
        games = gamesHandler.Load();
        for (int i = 0;i<games.games.Count;i++)
        {
            if (games.games[i].currentGame == loadObj.name)
            {
                games.games.RemoveAt(i);
                break;
            }
        }
        games.games.Remove(game);
        gamesHandler.Save(games);
        Destroy(loadObj);


    }

}
