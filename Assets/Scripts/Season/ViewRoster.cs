using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewRoster : MonoBehaviour, IDataPersistence
{
    public  Transform Table;
    private string selectedTeam;
    public GameObject PlayerInfo;
    public GameObject Contract;
    
    private GameData gameData;

    public void LoadData(GameData data)
    {
        gameData = data;
    }
    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateList(Transform selected)
    {
        for(int i = 0; i < Table.childCount; i++)
        {
            if(Table.GetChild(i).gameObject.name != "Header")
            Destroy(Table.GetChild(i).gameObject);
        }   
        selectedTeam = selected.name;
        FileDataHandler<TeamPersistent> _teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        TeamPersistent team = _teamHandler.Load();
       

        Vector2 sizeDelta = Table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.players.Length * 100;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;



      
        Debug.Log(Table.transform.GetComponent<RectTransform>().rect.height);
        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();

                GameObject playerInfo = Instantiate(PlayerInfo, Table);

                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                
                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString() 
                    + "("+ (player.consistency.value- player.consistency.prevValue).ToString()+")";
                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                    player.consistency.value - player.consistency.prevValue>0?Color.green : (player.consistency.value - player.consistency.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                      + "(" + (player.awareness.value - player.awareness.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.awareness.value - player.awareness.prevValue > 0 ? Color.green : (player.awareness.value - player.awareness.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                      + "(" + (player.juking.value - player.juking.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.juking.value - player.juking.prevValue > 0 ? Color.green : (player.juking.value - player.juking.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                      + "(" + (player.control.value - player.control.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.control.value - player.control.prevValue > 0 ? Color.green : (player.control.value - player.control.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                      + "(" + (player.shooting.value - player.shooting.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.shooting.value - player.shooting.prevValue > 0 ? Color.green : (player.shooting.value - player.shooting.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                      + "(" + (player.positioning.value - player.positioning.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.positioning.value - player.positioning.prevValue > 0 ? Color.green : (player.positioning.value - player.positioning.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                      + "(" + (player.steal.value - player.steal.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.steal.value - player.steal.prevValue > 0 ? Color.green : (player.steal.value - player.steal.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                      + "(" + (player.guarding.value - player.guarding.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.guarding.value - player.guarding.prevValue > 0 ? Color.green : (player.guarding.value - player.guarding.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                      + "(" + (player.pressure.value - player.pressure.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.pressure.value - player.pressure.prevValue > 0 ? Color.green : (player.pressure.value - player.pressure.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                      + "(" + (player.inside.value - player.inside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.inside.value - player.inside.prevValue > 0 ? Color.green : (player.pressure.value - player.pressure.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
                      + "(" + (player.mid.value - player.mid.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.mid.value - player.mid.prevValue > 0 ? Color.green : (player.mid.value - player.mid.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                      + "(" + (player.Outside.value - player.Outside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.Outside.value - player.Outside.prevValue > 0 ? Color.green :(player.Outside.value - player.Outside.prevValue == 0?Color.white:Color.red);

                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString()
                      + "(" + (player.ovrl - player.prevOvrl).ToString() + ")";
                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.ovrl - player.prevOvrl > 0 ? Color.green : (player.ovrl - player.prevOvrl == 0 ? Color.white : Color.red);

                GameObject playerContract = Instantiate(Contract, Table);
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.salary.ToString();
                player.contract.years-=1;

                playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString() ;
                
          
            }
        }
    }

    public void GenerateAgingList(Transform selected)
    {
        for (int i = 0; i < Table.childCount; i++)
        {
            if (Table.GetChild(i).gameObject.name != "Header")
                Destroy(Table.GetChild(i).gameObject);
        }
        selectedTeam = selected.name;
        FileDataHandler<TeamPersistent> _teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        TeamPersistent team = _teamHandler.Load();


        Vector2 sizeDelta = Table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.players.Length * 100;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;




        Debug.Log(Table.transform.GetComponent<RectTransform>().rect.height);
        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();

                GameObject playerInfo = Instantiate(PlayerInfo, Table);

                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                player.Age += 1;
                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                    + "(" + (player.consistency.value - player.consistency.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                    player.consistency.value - player.consistency.prevValue > 0 ? Color.green : (player.consistency.value - player.consistency.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                      + "(" + (player.awareness.value - player.awareness.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.awareness.value - player.awareness.prevValue > 0 ? Color.green : (player.awareness.value - player.awareness.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                      + "(" + (player.juking.value - player.juking.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.juking.value - player.juking.prevValue > 0 ? Color.green : (player.juking.value - player.juking.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                      + "(" + (player.control.value - player.control.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.control.value - player.control.prevValue > 0 ? Color.green : (player.control.value - player.control.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                      + "(" + (player.shooting.value - player.shooting.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.shooting.value - player.shooting.prevValue > 0 ? Color.green : (player.shooting.value - player.shooting.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                      + "(" + (player.positioning.value - player.positioning.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.positioning.value - player.positioning.prevValue > 0 ? Color.green : (player.positioning.value - player.positioning.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                      + "(" + (player.steal.value - player.steal.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.steal.value - player.steal.prevValue > 0 ? Color.green : (player.steal.value - player.steal.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                      + "(" + (player.guarding.value - player.guarding.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.guarding.value - player.guarding.prevValue > 0 ? Color.green : (player.guarding.value - player.guarding.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                      + "(" + (player.pressure.value - player.pressure.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.pressure.value - player.pressure.prevValue > 0 ? Color.green : (player.pressure.value - player.pressure.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                      + "(" + (player.inside.value - player.inside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.inside.value - player.inside.prevValue > 0 ? Color.green : (player.pressure.value - player.pressure.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
                      + "(" + (player.mid.value - player.mid.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.mid.value - player.mid.prevValue > 0 ? Color.green : (player.mid.value - player.mid.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                      + "(" + (player.Outside.value - player.Outside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.Outside.value - player.Outside.prevValue > 0 ? Color.green : (player.Outside.value - player.Outside.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString()
                      + "(" + (player.ovrl - player.prevOvrl).ToString() + ")";
                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.ovrl - player.prevOvrl > 0 ? Color.green : (player.ovrl - player.prevOvrl == 0 ? Color.white : Color.red);

                GameObject playerContract = Instantiate(Contract, Table);
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.salary.ToString();
                player.contract.years -= 1;
                if (player.contract.years == 0)
                {
                    playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Expired";
                }
                else
                {
                    playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString();
                }


            }
        }
    }

    public void AgePlayers()
    {

        string[] teamsPlay =
  {
            "Alabama Alligators",
            "Florida Dolphins",
            "Georgia Bears",
            "Maryland Sharks",
            "Michigan Warriors",
            "New York Owls",
            "Ohio True Frogs", 
            "Pennsylvania Rush", 
            "Virginia Bobcats", 
            "Wisconsin Crows",
            "Arizona Jaguars", 
           "California Lightning", 
          
           "Minnesota Wolves",
         "Nevada Magic",
          "New Mexico Dragons", 
            "Oklahoma Stoppers", 
         "Washington Hornets",
             "Kansas Coyotes",

           "Oregon Trail Makers", 
            "Texas Rattlesnakes", 
        
        };

        for(int i = 0; i< teamsPlay.Length; i++) {

            FileDataHandler<TeamPersistent> teamHanlder = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamsPlay[i]);
            TeamPersistent team = teamHanlder.Load();

            for(int k = 0; k<team.players.Length; k++)
            {
                FileDataHandler<PlayerPersistent> playerHanlder = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = playerHanlder.Load();

                player.Age += 1;
                player.contract.years -= 1;

                if(player.contract.years == 0)
                {
                    player.team = "";
                    player.prevTeam = team.name;
                    for(int j = 0; j<team.players.Length; j++)
                    {
                        if (team.players[j] == player.Name)
                        {
                            team.players[j] = "";
                            team.salaryCap -= (int)player.contract.salary;
                        }
                    }
                }
                playerHanlder.Save(player);
            }
            teamHanlder.Save(team);
        }
    }
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }
}
