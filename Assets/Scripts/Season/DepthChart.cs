using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

using UnityEngine;
using UnityEngine.UI;

public class DepthChart : MonoBehaviour, IDataPersistence
{

    public Transform Table;
    public GameObject PlayerDepth;
    private GameData gameData;
    public TeamPersistent team;
    public SelectedStuff selectedStuff;
    public Text off, def,notice;
    public SceneStuff sceneStuff;
    public List<PlayerPersistent> players;
    CurrentGame currGame;
    public void LoadData(GameData data)
    {
        gameData = data;
        Table.name = gameData.id;
        FileDataHandler<CurrentGame> gameDataHandler = new(Application.persistentDataPath, "Current Game");
         currGame = gameDataHandler.Load();

        notice.text = "Players Total of plays must equal " + currGame.gamePlays.ToString();
        off.text = currGame.gamePlays.ToString();
        def.text = currGame.gamePlays.ToString();

    }

    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateDepthChart(string selectedTeam)
    {
       int defPlays = currGame.gamePlays, offPlays = currGame.gamePlays;
        for (int i = 0; i < Table.childCount; i++)
        {
            
                Destroy(Table.GetChild(i).gameObject);
        }
       
       
        Vector2 sizeDelta = Table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = (players.Count+1) * 50;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        
        
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Name != "")
            {
               
                GameObject playerDepth = Instantiate(PlayerDepth, Table);
                playerDepth.gameObject.name = players[i].Name;
                Transform p1 = playerDepth.transform.GetChild(0);
                Transform p2 = p1.GetChild(0);
                Text name = p2.gameObject.GetComponent<Text>();
                name.text = players[i].Name;

                Transform p0 = playerDepth.transform.GetChild(1);
                Transform p3 = p0.GetChild(0);
                Text name1 = p3.gameObject.GetComponent<Text>();
                name1.text = players[i].Number.ToString();

                Transform p4 = playerDepth.transform.GetChild(2);
                Transform p5 = p4.GetChild(0);
                Text name2 = p5.gameObject.GetComponent<Text>();
                name2.text = players[i].ovrl.ToString();

                Transform p6 = playerDepth.transform.GetChild(3);
                Transform p7 = p6.GetChild(0);
                InputField name3 = p7.gameObject.GetComponent<InputField>();
                name3.text = players[i].defPlays.ToString();
                defPlays -= players[i].defPlays;

                Transform p8 = playerDepth.transform.GetChild(4);
                Transform p9 = p8.GetChild(0);
                InputField name4 = p9.gameObject.GetComponent<InputField>();
                name4.text = players[i].plays.ToString();
                offPlays -= players[i].plays;
                
              
            }
            
        }
        off.text = offPlays.ToString();
        def.text = defPlays.ToString();
    }

    public void SetPlays()
    {
        int totalPlays = 0,totalDefPlays = 0;
        List <PlayerPersistent > Playerlist = new List<PlayerPersistent>();
        notice.text = "trying to save";
        for (int i = 0; i <Table.childCount; i++)
        {
            string playerName = Table.GetChild(i).gameObject.name;
            notice.text = playerName;
                FileDataHandler <PlayerPersistent> player = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                playerName);
            PlayerPersistent p = player.Load();
               if(p==null)
                {
                    notice.text = Application.persistentDataPath + "/" + gameData.id + "/Players/" + playerName;
                }

            if (p != null) { 
            int playerDeffPlays = int.Parse(Table.GetChild(i).GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text);
            int playerPlays = int.Parse(Table.GetChild(i).GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text);
          p.plays  = playerPlays;
            p.defPlays = playerDeffPlays;
            totalPlays += playerPlays;
            totalDefPlays += playerDeffPlays;
            Playerlist.Add(p);
            }
        }
        notice.text = "finished going through table";
        if (totalPlays == currGame.gamePlays && totalDefPlays == currGame.gamePlays) {
            foreach(PlayerPersistent player in Playerlist)
            {
                FileDataHandler<PlayerPersistent> handler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                    player.Name);
                notice.text = player.Name;
                handler.Save(player);
            }
            if(selectedStuff.currentSeason.phase == "Season")
            {
               
                team.matchesPlayed[selectedStuff.week].isReady = true;
            }
            else
            {
                team.playOffMatches[selectedStuff.currentSeason.PlayOffRound].isReady = true;
            }
            notice.text = "Save Successful";
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", team.name);
            teamHandler.Save(team);
            selectedStuff.MatchHistory();

        }
        else
        {
            notice.text = "Total Number of Plays for players must be "+ currGame.gamePlays+", number of off plays: "+ totalPlays+ " number of def plays: "+ totalDefPlays;
          }
        
    }
}
