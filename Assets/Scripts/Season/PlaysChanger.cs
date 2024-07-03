using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaysChanger : MonoBehaviour,IDataPersistence
{
    // Start is called before the first frame update
    public string playType;
    private GameData gameData;
    private CurrentGame currGame;
    public void SaveData(ref GameData gamdata)
    {

    }
    public void LoadData(GameData gameData)
    {
       
        this.gameData = gameData;
        
       
    }

    public void changePlays(GameObject player)
    {
        if (currGame == null)
        {
            FileDataHandler<CurrentGame> gameDataHandler = new(Application.persistentDataPath, "Current Game");
            currGame = gameDataHandler.Load();
        }
        string playerName = player.name;
        string id = player.transform.parent.name;

        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + id+ "/Players/", playerName);
        PlayerPersistent playerPersistent = playerHandler.Load();
        if (playerPersistent != null)
        {
           if(playType == "off")
            {
                int playerPlays = int.Parse(player.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text);
                if(playerPlays > currGame.maxPlays)
                {
                    playerPlays = currGame.maxPlays;
                }
                else
                {
                    if(playerPlays<currGame.minPlays && playerPlays!=0)
                    {
                        playerPlays = currGame.minPlays;
                    }
                }
                
                playerPersistent.plays = playerPlays;
                playerHandler.Save(playerPersistent);
              

            }
            else
            {
                int playerDeffPlays = int.Parse(player.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text);
                if (playerDeffPlays > currGame.maxPlays)
                {
                    playerDeffPlays = currGame.maxPlays;
                }
                else
                {
                    if (playerDeffPlays < currGame.minPlays && playerDeffPlays!=0)
                    {
                        playerDeffPlays = currGame.minPlays;
                    }
                }
                playerPersistent.defPlays = playerDeffPlays;
                playerHandler.Save(playerPersistent);
                
            }
        }
    }
}
