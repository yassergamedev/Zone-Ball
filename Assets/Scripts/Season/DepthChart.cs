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
    public void LoadData(GameData data)
    {
        gameData = data;
    }
   
    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateDepthChart(string selectedTeam)
    {
        for (int i = 0; i < Table.childCount; i++)
        {
            if (Table.GetChild(i).gameObject.name != "Header")
                Destroy(Table.GetChild(i).gameObject);
        }
  
        FileDataHandler<TeamPersistent> _teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        TeamPersistent team = _teamHandler.Load();

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

                PlayerDepth.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                PlayerDepth.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                PlayerDepth.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();


            }
        }
    }

    public void SetPlays()
    {
        int totalPlays = 0;
        List <PlayerPersistent > Playerlist = new List<PlayerPersistent>();   
        for (int i = 1; i < Table.childCount; i++)
        {
            FileDataHandler<PlayerPersistent> player = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                Table.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text);
            PlayerPersistent p = player.Load();
            Debug.Log(p.Name);
            int playerPlays = int.Parse(Table.GetChild(i).GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text);
          p.plays  = playerPlays;
            totalPlays += playerPlays;
            Playerlist.Add(p);
        }
        if(totalPlays == 40) {
            foreach(PlayerPersistent player in Playerlist)
            {
                FileDataHandler<PlayerPersistent> handler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                    player.Name);
                handler.Save(player);
            }
            Debug.Log("Save Successful");
        }
        else
        {
            Debug.Log("Total Number of Plays for players must be 40");
        }
    }
}
