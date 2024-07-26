using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeagueMenu : MonoBehaviour, IDataPersistence
{
    private GameData gameData;
    private CurrentGame currGame;
    public Text League;
    public Button Load;
    List<TeamPersistent> orderedTeamsList;
    public Transform StandingsTable;
    public GameObject teamStanding;
    public GameObject salCap, totalPlays, maxPlays, minPlays;
    public void SaveData(ref GameData gamdata)
    {

    }
    public void LoadData(GameData gamdata)
    {
    this.gameData = gamdata;
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
         currGame = currHandler.Load();
        Load.GetComponent<SceneStuff>().game = currGame;

        League.text =  currGame.currentGame+"\n "+
            currGame.currentSeason.ToString() + "\nWeek: " + currGame.week.ToString();

        setStandings();
        salCap.GetComponent<InputField>().text = currGame.SalaryCap.ToString();
        totalPlays.GetComponent<InputField>().text = currGame.gamePlays.ToString();
        maxPlays.GetComponent<InputField>().text = currGame.maxPlays.ToString();
        minPlays.GetComponent<InputField>().text = currGame.minPlays.ToString();
    }

    public void changeSettings()
    {
        FileDataHandler<CurrentGame> gameDataHandler = new(Application.persistentDataPath, "Current Game");
        
       
        currGame.prevSalaryCap = currGame.SalaryCap;
        currGame.SalaryCap = int.Parse(salCap.GetComponent<InputField>().text);
        
        currGame.gamePlays = int.Parse(totalPlays.GetComponent<InputField>().text);
        currGame.maxPlays = int.Parse(maxPlays.GetComponent<InputField>().text);
        currGame.minPlays = int.Parse(minPlays.GetComponent<InputField>().text);

        gameDataHandler.Save(currGame);
        string[] west =
      {
            "Arizona Jaguars",
            "California Lightning",
            "Kansas Coyotes",
            "Minnesota Wolves",
            "Nevada Magic",
            "New Mexico Dragons",
            "Oklahoma Stoppers",
            "Oregon Trail Makers",
            "Texas Rattlesnakes",
            "Washington Hornets"
        };
        string[] east =
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
            "Wisconsin Crows"
        };

        for (int i = 0; i < west.Length; i++)
        {
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", west[i]);
            TeamPersistent team = teamHandler.Load();
            int difference =  currGame.SalaryCap - currGame.prevSalaryCap;
            team.salaryCap += difference;
            teamHandler.Save(team);
        }
        for (int i = 0; i < east.Length; i++)
        {
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", east[i]);
            TeamPersistent team = teamHandler.Load();
            int difference = currGame.SalaryCap - currGame.prevSalaryCap;
            team.salaryCap += difference;
            teamHandler.Save(team);
        }

    }
    public void setStandings()
    {
        string[] west =
       {
            "Arizona Jaguars",
            "California Lightning",
            "Kansas Coyotes",
            "Minnesota Wolves",
            "Nevada Magic",
            "New Mexico Dragons",
            "Oklahoma Stoppers",
            "Oregon Trail Makers",
            "Texas Rattlesnakes",
            "Washington Hornets"
        };
        string[] east =
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
            "Wisconsin Crows"
        };
        orderedTeamsList = new List<TeamPersistent>();

        for (int i = 0; i < west.Length; i++)
        {
            FileDataHandler<TeamPersistent> team = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", west[i]);
            orderedTeamsList.Add(team.Load());
        }
        for (int i = 0; i < east.Length; i++)
        {
            FileDataHandler<TeamPersistent> team = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", east[i]);
            orderedTeamsList.Add(team.Load());
        }
        //order the teams according to wins, pointsscored, pointsallowed, least turnedovers
        orderedTeamsList = orderedTeamsList.OrderByDescending(team => team.wins)
                                     .ThenByDescending(team => team.pointsScored)
                                     .ThenBy(team => team.pointsAllowed)
                                     .ThenBy(team => team.turnovers)
                                     .ToList();
        Vector2 sizeDelta = StandingsTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = 20 * 50;
        StandingsTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        for (int i = 0; i < StandingsTable.childCount; i++)
        {
            if (StandingsTable.GetChild(i).gameObject.name != "Header")
                Destroy(StandingsTable.GetChild(i).gameObject);
        }
        int k = 0;
        foreach (TeamPersistent team in orderedTeamsList)
        {
            GameObject standing = Instantiate(teamStanding, StandingsTable);
            team.matchesPlayed = new();
            standing.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (k + 1).ToString();
            standing.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = team.name;
            standing.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = team.Conference;
            standing.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = team.wins.ToString();
            standing.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = team.losses.ToString();
            standing.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = team.pointsScored.ToString();
            standing.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = team.pointsAllowed.ToString();
            standing.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = team.oppTurnovers.ToString();
            standing.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = team.turnovers.ToString();
            k++;
        }
    }
}
