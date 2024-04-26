using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewRoster : MonoBehaviour, IDataPersistence
{
    public Transform Table;
    public Transform AgingTable;
    private string selectedTeam;
    public GameObject PlayerInfo;
    public GameObject Contract;
    public GameObject ContractExtention;
    public GameObject Message, Vacancy;
    public GameObject Confirmation;
    private GameData gameData;

    public void LoadData(GameData data)
    {
        gameData = data;
        Debug.Log(data);
    }
    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateList(Transform selected)
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

                playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString();


            }
        }
    }

    public void GenerateAgingList(Transform selected)
    {
        for (int i = 0; i < AgingTable.childCount; i++)
        {
            if (AgingTable.GetChild(i).gameObject.name != "Header")
                Destroy(AgingTable.GetChild(i).gameObject);
        }
        selectedTeam = selected.name;
        FileDataHandler<TeamPersistent> _teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        TeamPersistent team = _teamHandler.Load();


        Vector2 sizeDelta = AgingTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.players.Length * 250;
        AgingTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;




        Debug.Log(AgingTable.transform.GetComponent<RectTransform>().rect.height);
        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();

                GameObject playerInfo = Instantiate(PlayerInfo, AgingTable);

                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                player.Age += 1;
                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString() + ((player.Age + player.longevity >= 32 + player.longevity) ? "(Retired)" : "");

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

                GameObject playerContract = Instantiate(Contract, AgingTable);
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.salary.ToString();
                player.contract.years -= 1;
                playerContract.name = player.Name + " Contract";
                if (player.contract.years == 0)
                {
                    playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Expired";

                }
                else
                {
                    playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString();
                }

                if (player.hasExtended)
                {
                    GameObject MessageObject = Instantiate(Message, AgingTable);
                    MessageObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Player Has Extended";
                    MessageObject.name = player.Name;
                }
                else
                {

                    GameObject playerContractExtention = Instantiate(ContractExtention, AgingTable);
                    playerContractExtention.name = player.Name + " Extention";

                    playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = player.contract.salary.ToString();
                    GameObject MessageObject = Instantiate(Message, AgingTable);
                    MessageObject.name = player.Name;

                }

                Instantiate(Vacancy, AgingTable);
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

        for (int i = 0; i < teamsPlay.Length; i++)
        {

            FileDataHandler<TeamPersistent> teamHanlder = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamsPlay[i]);
            TeamPersistent team = teamHanlder.Load();

            for (int k = 0; k < team.players.Length; k++)
            {
                FileDataHandler<PlayerPersistent> playerHanlder = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = playerHanlder.Load();

                player.Age += 1;
                player.contract.years -= 1;

                if (player.contract.years == 0)
                {
                    player.team = "";
                    player.prevTeam = team.name;
                    player.status = "FA";
                    for (int j = 0; j < team.players.Length; j++)
                    {
                        if (team.players[j] == player.Name)
                        {
                            team.players[j] = "";
                            team.salaryCap += (int)player.contract.salary;
                        }
                    }
                }
                playerHanlder.Save(player);
            }
            teamHanlder.Save(team);
        }
    }

    public void anothFunction()
    {

    }

    public void ExtendContract(GameObject PlayerName)
    {
        //We tagged Aging Roster table to Right net to be able to find when clicking on extend or terminate contract button and change it directly
        Transform AgingTable = GameObject.FindGameObjectWithTag("Right Net").transform;
        int years = 1, salary = 10000;
        int demandedSalary = 10000;
        for (int i = 0; i < AgingTable.childCount; i++)
        {
            if (AgingTable.GetChild(i).name == PlayerName.name)
            {
                salary = int.Parse(AgingTable.GetChild(i).GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text);
                years = int.Parse(AgingTable.GetChild(i).GetChild(5).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text);

                demandedSalary = int.Parse(AgingTable.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text);

            }

            if (AgingTable.GetChild(i).name == PlayerName.name.Replace(" Extention", ""))
            {
                if (salary >= demandedSalary)
                {
                    Debug.Log(gameData.id);
                    Debug.Log(PlayerName.name.Replace(" Extention", ""));
                    FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", PlayerName.name.Replace(" Extention", ""));
                    PlayerPersistent player = playerHandler.Load();
                    int difference = (int)player.contract.salary - salary;
                    player.contract.salary = salary;
                    player.contract.years = years;
                    FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", player.team);
                    TeamPersistent team = teamHandler.Load();

                    team.salaryCap -= difference;
                    AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Conract Extended";
                    AgingTable.GetChild(i - 2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = salary.ToString();
                    Debug.Log(AgingTable.GetChild(i - 2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text);
                    Debug.Log(AgingTable.GetChild(i - 2).GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text);
                    AgingTable.GetChild(i - 2).GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = years.ToString();

                    // playerHandler.Save(player);
                    //teamHandler.Save(team);
                }
                else
                {
                    AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Player Demands More in Salary";
                }

            }
        }
    }
    public void TerminateContract(GameObject PlayerName)
    {
        //We tagged Aging Roster table to Right net to be able to find when clicking on extend or terminate contract button and change it directly
        Transform AgingTable = GameObject.FindGameObjectWithTag("Right Net").transform;

        Debug.Log(gameData.id);
        Debug.Log(PlayerName.name.Replace(" Extention", ""));
        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", PlayerName.name.Replace(" Extention", ""));
        PlayerPersistent player = playerHandler.Load();
        player.prevTeam = player.team;
        player.team = "";
        player.status = "FA";

        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", player.team);
        TeamPersistent team = teamHandler.Load();
       // playerHandler.Save(player);

        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] == player.Name)
            {
                team.players[i] = "";
            }
        }
        //teamHandler.Save(team);
        for (int i = 0; i < AgingTable.childCount; i++)
        {
            if (AgingTable.GetChild(i).name == PlayerName.name.Replace(" Extention", ""))
            {
                AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Contract Terminated, Player is Now Free Agent";
            }
        }
    }
}