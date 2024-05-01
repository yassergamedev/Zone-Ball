using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaysChanger : MonoBehaviour,IDataPersistence
{
    // Start is called before the first frame update
    public string playType;
    private GameData gameData;

    public void SaveData(ref GameData gamdata)
    {

    }
    public void LoadData(GameData gameData)
    {
        Debug.Log("hahahaha "+ gameData.id);
        this.gameData = gameData;
    }

    public void changePlays(GameObject player)
    {
       string playerName = player.name;
        string id = player.transform.parent.name;

        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + id+ "/Players/", playerName);
        PlayerPersistent playerPersistent = playerHandler.Load();
        if (playerPersistent != null)
        {
           if(playType == "off")
            {
                int playerPlays = int.Parse(player.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text);
                if(playerPlays > 8)
                {
                    playerPlays = 8;
                }
                else
                {
                    if(playerPlays<4 && playerPlays!=0)
                    {
                        playerPlays = 4;
                    }
                }
                
                playerPersistent.plays = playerPlays;
                playerHandler.Save(playerPersistent);
              

            }
            else
            {
                int playerDeffPlays = int.Parse(player.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text);
                if (playerDeffPlays > 8)
                {
                    playerDeffPlays = 8;
                }
                else
                {
                    if (playerDeffPlays < 4&&playerDeffPlays!=0)
                    {
                        playerDeffPlays = 4;
                    }
                }
                playerPersistent.defPlays = playerDeffPlays;
                playerHandler.Save(playerPersistent);
                
            }
        }
    }
}
