using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ViewRoster : MonoBehaviour, IDataPersistence
{
    public Transform Table;
    public Transform AgingTable;
    private string selectedTeam;
    public string round;
    public GameObject PlayerInfo,PlayerPot;
    public GameObject Contract;
    public GameObject ContractExtention;
    public GameObject Message, Vacancy;
    public GameObject Confirmation;
    public GameObject DraftTable;
    private GameData gameData;

    public void LoadData(GameData data)
    {
        gameData = data;
       
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
        sizeDelta.y = team.players.Length * 150;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;




        List<PlayerPersistent> teamPlayers = new List<PlayerPersistent>();

        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();
                teamPlayers.Add(player);
            }
        }
        teamPlayers = teamPlayers.OrderByDescending(player => player.ovrl).ToList();
                //Debug.Log(Table.transform.GetComponent<RectTransform>().rect.height);
        foreach (PlayerPersistent player in teamPlayers)
        {
           
               
                GameObject playerInfo = Instantiate(PlayerInfo, Table);

                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                //playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();

                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                    + "(" + (player.consistency.value - player.consistency.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                    player.consistency.value - player.consistency.prevValue > 0 ? Color.green : (player.consistency.value - player.consistency.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                      + "(" + (player.awareness.value - player.awareness.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.awareness.value - player.awareness.prevValue > 0 ? Color.green : (player.awareness.value - player.awareness.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                      + "(" + (player.juking.value - player.juking.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.juking.value - player.juking.prevValue > 0 ? Color.green : (player.juking.value - player.juking.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                      + "(" + (player.control.value - player.control.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.control.value - player.control.prevValue > 0 ? Color.green : (player.control.value - player.control.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                      + "(" + (player.shooting.value - player.shooting.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.shooting.value - player.shooting.prevValue > 0 ? Color.green : (player.shooting.value - player.shooting.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                      + "(" + (player.positioning.value - player.positioning.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.positioning.value - player.positioning.prevValue > 0 ? Color.green : (player.positioning.value - player.positioning.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                      + "(" + (player.steal.value - player.steal.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.steal.value - player.steal.prevValue > 0 ? Color.green : (player.steal.value - player.steal.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                      + "(" + (player.guarding.value - player.guarding.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.guarding.value - player.guarding.prevValue > 0 ? Color.green : (player.guarding.value - player.guarding.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                      + "(" + (player.pressure.value - player.pressure.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.pressure.value - player.pressure.prevValue > 0 ? Color.green : (player.pressure.value - player.pressure.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                      + "(" + (player.inside.value - player.inside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.inside.value - player.inside.prevValue > 0 ? Color.green : (player.inside.value - player.inside.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
                      + "(" + (player.mid.value - player.mid.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.mid.value - player.mid.prevValue > 0 ? Color.green : (player.mid.value - player.mid.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                      + "(" + (player.Outside.value - player.Outside.prevValue).ToString() + ")";
                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.Outside.value - player.Outside.prevValue > 0 ? Color.green : (player.Outside.value - player.Outside.prevValue == 0 ? Color.white : Color.red);

                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString()
                      ;
                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().color =
                     player.ovrl - player.prevOvrl > 0 ? Color.green : (player.ovrl - player.prevOvrl == 0 ? Color.white : Color.red);





                GameObject playerPotInfo = Instantiate(PlayerPot, Table);

                playerPotInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Potentials";
                //playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();

             
                playerPotInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.potential.ToString();
                 
                playerPotInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.potential.ToString();

                playerPotInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.potential.ToString();
                   
                playerPotInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.potential.ToString();
                  
                playerPotInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.potential.ToString();
                 
                playerPotInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.potential.ToString();
                    
                playerPotInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.potential.ToString();
                     
                playerPotInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.potential.ToString();
                    
                playerPotInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.potential.ToString();
                     
                playerPotInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.potential.ToString();
                     
                playerPotInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.potential.ToString();
                    
                playerPotInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.potential.ToString();

                playerPotInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.potOvrl.ToString();
                    





                GameObject playerContract = Instantiate(Contract, Table);
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();


                playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.salary.ToString();

                playerContract.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString();


            
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
                playerInfo.name = gameData.id;

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

                    if (player.contract.years == 0)
                    {
                        int demandedSalary = calcSalary(player.ovrl);
                        switch (player.resignNumber)
                        {
                            case 0:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = demandedSalary.ToString();
                                
                                break;
                            case 1:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = demandedSalary.ToString();
                                
                                break;
                            case 2:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = demandedSalary.ToString();
                                
                                break;
                            case 3:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = demandedSalary.ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 4:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = (demandedSalary + 1000).ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 5:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = (demandedSalary + 1000).ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 6:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = (demandedSalary + 1000).ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 7:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = (demandedSalary + 2000).ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 8:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = (demandedSalary + 2000).ToString();
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 9:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "0";
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 10:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "0";
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                            case 11:
                                playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "0";
                                playerContractExtention.transform.GetChild(5).GetChild(0).gameObject.GetComponent<NumberValidation>().maxValue = 3;
                                break;
                           
                        }

                    }
                    else
                    {
                        playerContractExtention.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = player.contract.salary.ToString();
                        playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString();
                    }
                    
                    GameObject MessageObject = Instantiate(Message, AgingTable);
                    MessageObject.name = player.Name;

                }

                Instantiate(Vacancy, AgingTable);
            }
        }
    }

    public int calcSalary(int ovrl)
    {
        int salary = 10000;
        if(ovrl>=90)
        {
            salary = 100000;
        }
        else
        {
            if (ovrl >= 85)
            {
                salary = 95000;

            }
            else
            {
                if (ovrl >= 80)
                {
                    salary = 90000;
                }
                else
                {
                    if (ovrl >= 75)
                    {
                        salary = 80000;
                    }
                    else
                    {
                        if (ovrl >= 70)
                        {
                            salary = 70000;
                        }
                        else
                        {
                            if (ovrl >= 65)
                            {
                                salary = 60000;
                            }
                            else
                            {
                                if (ovrl >= 60)
                                {
                                    salary = 50000;
                                }
                                else
                                {
                                    if (ovrl >= 55)
                                    {
                                        salary = 40000;
                                    }
                                    else
                                    {
                                        if (ovrl >= 50)
                                        {
                                            salary = 30000;
                                        }
                                        else if (ovrl >= 45)
                                        {
                                            salary = 20000;
                                        }
                                        else
                                        {
                                            if (ovrl >= 40)
                                            {
                                                salary = 15000;
                                            }
                                            else
                                            {
                                                if (ovrl >= 31)
                                                {
                                                    salary = 10000;
                                                }

                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                }

            }

        }

        return salary;
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
    public void Bid(GameObject playerName)
    {
        string name = playerName.name.Replace(" FA", "");
        string teamName = GameObject.FindGameObjectWithTag("TeamScore").GetComponent<Text>().text;
        GameObject FATable = GameObject.FindGameObjectWithTag("ShootingRange");
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame cg = currHandler.Load();
        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + cg.currentGame + "/Players/" , name);
        PlayerPersistent player = playerHandler.Load();
        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + cg.currentGame + "/Teams/", teamName);
        TeamPersistent team = teamHandler.Load();

        for (int i = 0; i < FATable.transform.childCount; i++)
        {
            if (FATable.transform.GetChild(i).name == name)
            {
                for (int k = 0; k < team.players.Length; k++)
                {
                    if (team.players[k] == "")
                    {
                        if (team.salaryCap >= int.Parse(FATable.transform.GetChild(i - 1).GetChild(1).GetChild(0).GetComponent<Text>().text))
                        {
                            player.team = teamName;
                            team.players[k] = player.Name;
                            player.contract.salary = int.Parse(FATable.transform.GetChild(i - 1).GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text);
                            player.contract.years = int.Parse(FATable.transform.GetChild(i - 1).GetChild(5).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text);

                            team.salaryCap -= (int)player.contract.salary;
                            FATable.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = "Player Currently Signed to " + teamName;
                            teamHandler.Save(team);
                            playerHandler.Save(player);
                        }
                        else
                        {
                            FATable.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = "Salary Cap isn't Enough! ";
                        }

                    }
                    else
                    {
                        FATable.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = " Team Roster is Full";
                    }
                }
             }
        }
  }
    public void DraftPlayer(GameObject playerName)
    {
        string name = playerName.name.Replace(" Draft","");
        //team name with the tag teamScore
        string teamName = GameObject.FindGameObjectWithTag("TeamScore").GetComponent<Text>().text;
        //draft table named to the gamedata id with the tag Opp
        GameObject DraftTable = GameObject.FindGameObjectWithTag("Opp");
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame cg = currHandler.Load();
       
        string round = GameObject.FindGameObjectWithTag("MidPoint").GetComponent<Text>().text;
        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + cg.currentGame + "/" +cg.currentSeason+"/"+ round + "/", name);
        PlayerPersistent player = playerHandler.Load();
        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + cg.currentGame + "/Teams/", teamName);
        TeamPersistent team = teamHandler.Load();

        bool isDraftedRound = false;
        if(round == "Draft R1")
        {
            isDraftedRound = team.round1;
        }
        else
        {
            isDraftedRound = team.round2;
        }
        
        int index = 0;
        bool drafted = false;
        if (player.team == "" && !isDraftedRound)
        {
            for (int i = 0; i < DraftTable.transform.childCount; i++)
            {
                if (DraftTable.transform.GetChild(i).name == name)
                {
                   
                    
                    index = i;

                    for (int k = 0; k < team.players.Length; k++)
                    {
                        if (team.players[k] == "")
                        {
                            Debug.Log("Here");
                            if(team.salaryCap>=int.Parse(DraftTable.transform.GetChild(i+1).GetChild(1).GetChild(0).GetComponent<Text>().text))
                            {
                                player.team = teamName;
                                player.draftSelections += 1;
                                
                                Debug.Log(int.Parse(DraftTable.transform.GetChild(i + 1).GetChild(1).GetChild(0).GetComponent<Text>().text));
                                team.players[k] = name;
                                if (round == "Draft R1")
                                {
                                     team.round1 = true;
                                }
                                else
                                {
                                    team.round2 = true;
                                }
                                team.salaryCap -= int.Parse(DraftTable.transform.GetChild(i + 1).GetChild(1).GetChild(0).GetComponent<Text>().text);
                                drafted = true;
                                DraftTable.transform.GetChild(i + 2).GetChild(0).GetChild(0).GetComponent<Text>().text = "Player Drafted by "+ teamName;
                                FileDataHandler<PlayerPersistent> playerDraftedHandler = new(Application.persistentDataPath + "/" + cg.currentGame + "/Players/", name);
                                playerDraftedHandler.Save(player);
                            }
                            else
                            {
                                DraftTable.transform.GetChild(i + 2).GetChild(0).GetChild(0).GetComponent<Text>().text = "Team Salary Cap not enough";
                                drafted = true;
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < DraftTable.transform.childCount; i++)
            {
                if (DraftTable.transform.GetChild(i).name == name)
                {
                    player.team = teamName;
                    index = i;

                    break;
                }
            }
            bool DraftingTeam = false;
            for (int k = 0; k < team.players.Length; k++)
            {
                if (team.players[k] == name)
                {
                    DraftTable.transform.GetChild(index + 2).GetChild(0).GetChild(0).GetComponent<Text>().text = "player Already Drafted by "+teamName;
                    DraftingTeam = true;
                    break;
                }
            }
            if (!DraftingTeam)
            {
                drafted = true;
                DraftTable.transform.GetChild(index + 2).GetChild(0).GetChild(0).GetComponent<Text>().text = "Team Or player Already Drafted";
            }
            
        }
        if(!drafted)
        {
            DraftTable.transform.GetChild(index+2).GetChild(0).GetChild(0).GetComponent<Text>().text = "Team Roster is Full";
        }
        teamHandler.Save(team);
        playerHandler.Save(player);
        
    }
   
    public void ExtendContract(GameObject PlayerName)
    {
        //We tagged Aging Roster table to Right net to be able to find when clicking on extend or terminate contract button and change it directly
        Transform AgingTable = GameObject.FindGameObjectWithTag("Right Net").transform;
        int years = 1, salary = 10000;
        int demandedSalary = 10000;
        string id = "";
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
                if (id == "")
                {
                    id = AgingTable.GetChild(i - 3).name;
                }
                if (salary >= demandedSalary)
                {
                  
                   
                    Debug.Log(PlayerName.name.Replace(" Extention", ""));
                    FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + id + "/Players/", PlayerName.name.Replace(" Extention", ""));
                    PlayerPersistent player = playerHandler.Load();
                    if(demandedSalary==0)
                    {
                        AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Player Doesn't Want to renew contract";
                        

                    }
                    else
                    {
                        int difference = (int)player.contract.salary - salary;
                        player.contract.salary = salary;
                        player.contract.years = years;
                        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + id + "/Teams/", player.team);
                        TeamPersistent team = teamHandler.Load();

                        team.salaryCap -= difference;
                        AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Conract Extended";
                        AgingTable.GetChild(i - 2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = salary.ToString();
                        Debug.Log(AgingTable.GetChild(i - 2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text);
                        Debug.Log(AgingTable.GetChild(i - 2).GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text);
                        AgingTable.GetChild(i - 2).GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = years.ToString();

                        playerHandler.Save(player);
                        teamHandler.Save(team);
                    }
                    
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
        string id = "";
        for (int i = 0; i < AgingTable.childCount; i++)
        {
            if (AgingTable.GetChild(i).name == PlayerName.name.Replace(" Extention", ""))
            {
                if (id == "")
                {
                    id = AgingTable.GetChild(i - 3).name;
                }
            }
        }

     
        
        Debug.Log(PlayerName.name.Replace(" Extention", ""));
        
        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + id + "/Players/", PlayerName.name.Replace(" Extention", ""));
        PlayerPersistent player = playerHandler.Load();
        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + id + "/Teams/", player.team);
        TeamPersistent team = teamHandler.Load();
        int teamNum = 0;
        for (int i =0;i<team.players.Length; i++)
        {
            if (team.players[i] !="")
            {
                teamNum++;
            }
        }
        if(teamNum>8)
        {
            player.prevTeam = player.team;
            player.team = "";
            player.status = "FA";


            playerHandler.Save(player);

            for (int i = 0; i < team.players.Length; i++)
            {
                if (team.players[i] == player.Name)
                {
                    team.players[i] = "";
                }
            }
            teamHandler.Save(team);
            for (int i = 0; i < AgingTable.childCount; i++)
            {
                if (AgingTable.GetChild(i).name == PlayerName.name.Replace(" Extention", ""))
                {
                    AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Contract Terminated, Player is Now Free Agent";
                }
            }
        }
        else
        {
            for (int i = 0; i < AgingTable.childCount; i++)
            {
                if (AgingTable.GetChild(i).name == PlayerName.name.Replace(" Extention", ""))
                {
                    AgingTable.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Can't Terminate Contract, Team Doesn't have enough players";
                }
            }

        }
           
    }
}