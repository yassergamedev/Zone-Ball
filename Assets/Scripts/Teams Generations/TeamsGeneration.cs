using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeamsGeneration : MonoBehaviour,IDataPersistence
{
    private GameData gameData;
    public Transform Teamstable;
    public Transform Playerstable;
    public RandomNameGenerator rng;
    private Transform[] TeamsRows;
    private Transform[] PlayersRows;
    private TeamPersistent selectedTeam;

    // Start is called before the first frame update
    void Start()
    {
        
        int TeamsRowsCount = Teamstable.childCount;
        TeamsRows = new Transform[Teamstable.childCount];
        Transform row;
        for (int i = 0; i < TeamsRowsCount; i++)
        {
            row = Teamstable.GetChild(i);
            TeamsRows[i] = row;
            
        }
        int PlayersRowsCount = Playerstable.childCount;
        PlayersRows = new Transform[Playerstable.childCount];
        
        for (int i = 0; i < PlayersRowsCount; i++)
        {
            row = Playerstable.GetChild(i);
            PlayersRows[i] = row;

        }


    }
    public void LoadData(GameData data)
    {
        gameData = data;
        Debug.Log(gameData.id);
    }
    public void SaveData(ref GameData data)
    {

    }
    public void GenerateTeams()
    {
        List<(string, int, string)> teams = new List<(string, int, string)>();

        foreach (Transform row in TeamsRows)
        {
            Debug.Log(row.gameObject.name);

            if (row.gameObject.name != "Header" && row.gameObject.name != "West" && row.gameObject.name != "East")
            {
                string name = row.gameObject.name;
                Debug.Log(name);    
                FileDataHandler<TeamPersistent> teamHandler = new FileDataHandler<TeamPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Teams/", name);
                
                TeamPersistent team = teamHandler.Load();

             
        

                Transform teamN = row.gameObject.transform;
                Transform playstyle = teamN.GetChild(1);
                Transform playstyleSlider = playstyle.GetChild(0);
                int playstyleValue = (int)playstyleSlider.gameObject.GetComponent<Slider>().value;
                Debug.Log(playstyleValue);
                Transform experience = teamN.GetChild(2);
                Transform experienceOption = experience.GetChild(0);
                Transform experienceOptionText = experienceOption.GetChild(0);
                string experienceOptionChoice = experienceOptionText.GetComponent<TextMeshProUGUI>().text;

                Debug.Log(experienceOptionChoice);
                team.playstyle = playstyleValue;
                team.start = experienceOptionChoice;

                for (int i = 0; i < 9; i++)
                {
                    string type;
                    int ovrl = 0;
                    int years = 0, age = 0;
                    string playerName = rng.GenerateRandomPlayerName();
                    FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Players/", playerName);

                    int randomNum = UnityEngine.Random.Range(0, playstyleValue);
                    if (randomNum < playstyleValue)
                    {
                        type = "off";
                    }
                    else
                    {
                        type = "def";
                    }
                    switch (experienceOptionChoice)
                    {
                        case "Young":
                            ovrl = Random.Range(31, 40);
                            years = Random.Range(1, 3);
                            age = Random.Range(18, 21);
                            break;
                        case "Average":
                            ovrl = Random.Range(41, 50);
                            years = Random.Range(3, 6);
                            age = Random.Range(22, 25);
                            break;
                        case "Experienced":
                            ovrl = Random.Range(51, 60);
                            years = Random.Range(6, 9);
                            age = Random.Range(26, 29);
                            break;
                    }
                    PlayerPersistent player = new PlayerPersistent(playerName, playerName, type, years, age, ovrl);
                    playerHandler.Save(player);
                    team.players[i] = playerName;
                    team.salaryCap -= (int)player.contract.salary;
                }
                teamHandler.Save(team);


            }
        }
    }

    public void setSelectedTeam(GameObject selected)
    {
        FileDataHandler<TeamPersistent> teamHandler = new FileDataHandler<TeamPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Teams/", selected.name);
        selectedTeam = teamHandler.Load();
        Debug.Log(selectedTeam.name);
        ShowPlayers();
    }

    public void ShowPlayers()
    {
        string[] teamplayers = selectedTeam.players;

        int i = 0;
            foreach(Transform row in PlayersRows)
            {

                FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/Players/", teamplayers[i]);
                PlayerPersistent player = playerHandler.Load();
                Debug.Log(player.id);
                if (row.gameObject.name != "Header")
                {
                    
                    int rowChildren = row.childCount;

                    for(int j = 0, s = 0; j<rowChildren; j++)
                    {
                        if (row.GetChild(j).gameObject.name == "Name")
                        {

                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.Name;
                            

                        }else if (row.GetChild(j).gameObject.name == "Age")
                        {
                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.Age.ToString();
                        }
                        else 
                        {
                           
                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.getStats()[s].Item2().value.ToString();
                        s++;
                        }

                    }
                }
                i++;
            }
        
    }

}
