using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedStuff : MonoBehaviour,IDataPersistence
{
    public GameObject selectedTeam;
    public GameObject selectedOption;
    public GameObject selectedOption2;
    
    public ViewRoster ViewRoster;
    public DepthChart depthChart;
    public Coaching coaching;

    public TMP_Dropdown weekSelector;
    public TMP_Dropdown oc;
    public TMP_Dropdown oc2;
    public TMP_Dropdown dc;
    public TMP_Dropdown dc2;
    public TMP_Dropdown hc;
    public TMP_Dropdown hc2;
    public Text teamName, marketCap;
    public Text Home, Guest;
    public Text HomeMatchPlayed, GuestMatchPlayed, HomeScore,GuestScore;
    public GameObject playerStats;
    public GameObject weekChange;
    public GameObject teamStanding;
    public int week;

    public Transform MatchesTable;
    public Transform StandingsTable;
    public Transform Content;
    public GameObject MatchDetail;

    private GameData gameData;
    private Season currentSeason;
    private TeamPersistent selTeam;
    private SceneStuff sceneStuff;
    public void LoadData(GameData go)
    {
        gameData = go;
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent team = teamLoader.Load();
        teamName.text = team.name;
        marketCap.text = team.salaryCap.ToString();
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        Season currentSeason = seasonHandler.Load();
        week = currentSeason.week;

        Home.text = team.matchesPlayed[week].isHome ? team.name : team.matchesPlayed[week].opponent;
        Guest.text = team.matchesPlayed[week].isHome ? team.matchesPlayed[week].opponent : team.name;

        if (team.matchesPlayed[week].isPlayed)
        {
            HomeMatchPlayed.text = team.matchesPlayed[week].isHome ? team.name : team.matchesPlayed[week].opponent;
            HomeScore.text = team.matchesPlayed[week].score.ToString();
            GuestMatchPlayed.text = team.matchesPlayed[week].isHome? team.matchesPlayed[week].opponent : team.name;
            GuestScore.text = team.matchesPlayed[week].oppScore.ToString();

            for(int i = 0; i<team.players.Length; i ++)
            {
                FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/" + team.id + "/" + week.ToString()
                    , team.players[i]);
                PlayerStatsPersistent stats = statsHandler.Load();
                if(stats !=null)
                {
                    GameObject statsTable = Instantiate(playerStats, Content);
                    int rowsCount = statsTable.transform.childCount;
                    Transform[] rows = new Transform[statsTable.transform.childCount];
                    Transform roww;
                    for (int k = 0; k < rowsCount; k++)
                    {
                        roww = statsTable.transform.GetChild(k);

                        rows[k] = roww;
                    }
                    foreach (Transform row in rows)
                    {

                        if (row.gameObject.name != "Table Header" || row.gameObject.name != "Buttons Header")
                        {
                          
                            if (row.gameObject.name == "Name")
                            {


                                row.GetChild(0).gameObject.GetComponent<Text>().text = team.players[i];
                            }

                            else
                            {
                                foreach ((string statname, System.Func<int> stat) in stats.getStats())
                                {

                                    if (row.gameObject.name == statname)
                                    {

                                        Transform valueCell = row.GetChild(1);
                                        Transform textObjV = valueCell.GetChild(0);
                                        int statN = stat();
                                        Debug.Log(statname + " " + statN);
                                        textObjV.gameObject.GetComponent<Text>().text = statN.ToString();



                                    }
                                }

                            }
                        }



                    }
                }

            }
        }
        
        setStandings();
        StartCoroutine(stretch());
    }
    public void SaveData(ref GameData go) { }
    private void Start()
    {
       WeekChange wek = weekChange.GetComponent<WeekChange>();
        week = wek.currentWeek;
        weekSelector.value = week;
        // SetSelectedTeam(selectedTeam);
        SetSelectedOption(selectedOption);
        SetSelectedOption2(selectedOption2);
   
    
    }
    public IEnumerator stretch()
    {
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(true);
    }
    public void SetSelectedTeam(GameObject team)
    {
        selectedTeam = team;
        depthChart.GenerateDepthChart(team.gameObject.name);
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent teamm = teamLoader.Load();
        selTeam = teamm;
        teamName.text = teamm.name;
        marketCap.text = teamm.salaryCap.ToString();

        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        currentSeason = seasonHandler.Load();
        week = currentSeason.week;

        Vector2 sizeDelta = MatchesTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = selTeam.matchesPlayed.Count * 100;
        MatchesTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;


        Home.text = teamm.matchesPlayed[week].isHome ? teamm.name : teamm.matchesPlayed[week].opponent;
        Guest.text = teamm.matchesPlayed[week].isHome ? teamm.matchesPlayed[week].opponent : teamm.name;

        setGames();
    }
    public void setWeek()
    {
        week = weekSelector.value;
    }
    public void SetSelectedOption(GameObject option)
    { 
        selectedOption = option;
    }
    public void SetSelectedOption2(GameObject option)
    { 
        selectedOption2 = option;
    }
    public void setCoaching()
    {
        coaching.setCoaching(selectedTeam.gameObject.name,new string[] { oc.value.ToString(), oc2.value.ToString() },
            new string[] { dc.value.ToString(), dc2.value.ToString() }, new string[] { hc.value.ToString(), hc2.value.ToString() });
    }

    public void setGames()
    {
        for (int i = 0; i < MatchesTable.childCount; i++)
        {
            if (MatchesTable.GetChild(i).gameObject.name != "Header")
                Destroy(MatchesTable.GetChild(i).gameObject);
        }

        for(int i = 0; i<selTeam.matchesPlayed.Count; i++)
        {
            GameObject newGame = Instantiate(MatchDetail, MatchesTable);
            newGame.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Week " + (i + 1);   
           newGame.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
                selTeam.matchesPlayed[i].isHome ? selTeam.name : selTeam.matchesPlayed[i].opponent;
            newGame.transform.GetChild(2).GetChild(0).GetComponent<Text>().text =
               selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].opponent : selTeam.name;
           
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
        List<TeamPersistent> orderedTeamsList = new List<TeamPersistent>();

        for(int i = 0; i < west.Length; i++)
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
        foreach(TeamPersistent team in orderedTeamsList)
        {
            GameObject standing = Instantiate(teamStanding, StandingsTable);
            standing.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (k+1).ToString();
            standing.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = team.name;
            standing.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = team.Conference;
            standing.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = team.wins.ToString();
            standing.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = team.losses.ToString();
            standing.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = team.pointsScored.ToString();
            standing.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = team.pointsAllowed.ToString();
            standing.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = team.oppTurnovers.ToString();
            standing.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = team.turnovers.ToString();
            k++;
        }
    }
    public void StartGame()
    {
        FileDataHandler<TeamPersistent> otherTeam = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.matchesPlayed[week].opponent);
        TeamPersistent other = otherTeam.Load();

        if (selTeam.matchesPlayed[week].isReady && selTeam.matchesPlayed[week].isPlayed == false 
            && other.matchesPlayed[week].isReady)
        {
            FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath, "Current Game");
            CurrentGame currGame = currentGame.Load();
            currGame.week = week;
            currGame.game = selTeam.matchesPlayed[week];
            currentGame.Save(currGame);
            sceneStuff.LoadScene();
        }
        else
        {
            Debug.Log("Game not ready to start, set depth charts correctly for both teams!!");
        }
        
    }
}
