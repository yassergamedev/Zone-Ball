using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Profiling;

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
    public void LoadData(GameData data)
    {
        gameData = data;
    }
   
    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateDepthChart(string selectedTeam)
    {
        off.text = "40";
        def.text = "40";
        for (int i = 0; i < Table.childCount; i++)
        {
            
                Destroy(Table.GetChild(i).gameObject);
        }
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        team = teamLoader.Load();

        Vector2 sizeDelta = Table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.players.Length * 50;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();

                GameObject playerDepth = Instantiate(PlayerDepth, Table);
                playerDepth.gameObject.name = player.Name;
                PlayerDepth.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player.Name;
                PlayerDepth.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = player.Number.ToString();
                PlayerDepth.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = player.ovrl.ToString();
                PlayerDepth.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text = player.defPlays.ToString();
                PlayerDepth.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text = player.plays.ToString();
                notice.text = player.Name;
            }
            
        }
    }

    public void SetPlays()
    {
       

     
        int totalPlays = 0,totalDefPlays = 0;
        List <PlayerPersistent > Playerlist = new List<PlayerPersistent>(); notice.text = "trying to save";
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
        if (totalPlays == 40 && totalDefPlays == 40) {
            foreach(PlayerPersistent player in Playerlist)
            {
                FileDataHandler<PlayerPersistent> handler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                    player.Name);
                notice.text = player.Name;
                handler.Save(player);
            }
            notice.text= "Save Successful";
            team.matchesPlayed[selectedStuff.week].isReady = true;
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", team.name);
            teamHandler.Save(team);
            selectedStuff.MatchHistory();

        }
        else
        {
            notice.text = "Total Number of Plays for players must be 40, number of off plays: "+ totalPlays+ " number of def plays: "+ totalDefPlays;
          }
        
    }
}
