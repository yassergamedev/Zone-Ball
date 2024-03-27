using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour,IDataPersistence
{
    private GameData gameData;
    private TeamPersistent Home, Guest;
    public Text HomeText, GuestText;
    public GameObject HomeObject, GuestObject;
    public GameObject teamPlayer, oppPlayer;
    public GameObject ball;
    public void LoadData(GameData data)
    {
       gameData = data;
        FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath , "Current Game");   
        CurrentGame cg = currentGame.Load();
        Debug.Log(cg.currentGame);
        Debug.Log(cg.game.opponent);
       FileDataHandler<TeamPersistent> firstTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", cg.game.opponent);
        TeamPersistent firstTeam = firstTeamHandler.Load();
        string opp = firstTeam.matchesPlayed[cg.week].opponent;
        Debug.Log(opp);
        FileDataHandler<TeamPersistent> secondTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", opp);
        TeamPersistent secondTeam = secondTeamHandler.Load();

        Home = firstTeam.matchesPlayed[cg.week].isHome ? firstTeam : secondTeam;
        Guest = firstTeam.matchesPlayed[cg.week].isHome ? secondTeam : firstTeam;

        HomeText.text = Home.name;
        GuestText.text = Guest.name;

        for(int i = 0; i<Home.players.Length; i++)
        {
            if (Home.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", Home.players[i]);
                PlayerPersistent player = playerHandler.Load();
                Debug.Log(player.Name);
                if (player.plays > 0)
                {
                    GameObject playerObject = Instantiate(teamPlayer, HomeObject.transform);
                    playerObject.transform.name = player.Name;
                    playerObject.GetComponent<PlayerActions>().playerPersistent = player;
                    playerObject.GetComponent<PlayerMovement>().playerPersistent = player;
                }
            }
        }
        for (int i = 0; i < Guest.players.Length; i++)
        {
            if (Guest.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", Guest.players[i]);
                PlayerPersistent player = playerHandler.Load();
                if (player.plays > 0)
                {
                    GameObject playerObject = Instantiate(oppPlayer, GuestObject.transform);
                    playerObject.transform.name = player.Name;
                    playerObject.GetComponent<PlayerActions>().playerPersistent = player;
                    playerObject.GetComponent<PlayerMovement>().playerPersistent = player;
                  //  FileDataHandler<PlayerStatsPersistent> statsHandler = new(
                      //  Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/Week " + cg.week + "/" + firstTeam.id +" Vs " + secondTeam.id + "/"
                   //     , player.Name + " Stats");
                   // PlayerStatsPersistent playerStats = new();
                //  statsHandler.Save(player.stats);

                }
            }
        }
        int randomPlayer = Random.Range(0, GuestObject.transform.childCount);
        Instantiate(ball, GuestObject.transform.GetChild(randomPlayer));
    }
    public void SaveData(ref GameData data)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
