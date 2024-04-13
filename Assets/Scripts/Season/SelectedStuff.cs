using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    public Text weekText, isGameReady;
    public GameObject playerStats;
    public GameObject TeamSeasonStats;
    public GameObject matchStats, matchPlayed;
    public GameObject weekChange;
    public GameObject teamStanding;
    public int week;

    public Transform MatchesTable;
    public Transform StandingsTable;
    public Transform Content;
    public Transform TeamSeasonStatsContent;
    public Transform PlayersStatsContent;
    public GameObject MatchDetail;
    public bool isActiveTable = false;
    public bool isGamePlayed =false;
    private GameData gameData;
    private Season currentSeason;
    private TeamPersistent selTeam;
    public SceneStuff sceneStuff;
    public GameObject seasonRecords;
    public GameObject careerRecords;
    public GameObject nextWeek, prevWeek, startNewWeek;
    public bool isNextWeekReady = true;
    public GameObject teams;
    public GameObject hasPlayedObj;
    public Transform Table;
    public GameObject PlayerDepth;
    public TeamPersistent team;
    public Text off, def, notice;
    private List<(string, PlayerStatsPersistent)> allPlayerStats = new();
    private List<(string, PlayerStatsPersistent)> allTimePlayerStats = new();
    public void LoadData(GameData go)
    {
        gameData = go;
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent team = teamLoader.Load();
        teamName.text = team.name;
        marketCap.text = team.salaryCap.ToString();
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
         currentSeason = seasonHandler.Load();
        week = currentSeason.week;

        Home.text = team.matchesPlayed[week].isHome ? team.name : team.matchesPlayed[week].opponent;
        Guest.text = team.matchesPlayed[week].isHome ? team.matchesPlayed[week].opponent : team.name;
        selTeam = team;
        depthChart.team = team;
        //selectedTeam.GetComponent<Button>().onClick.Invoke();
     
      if(week==0)
        {
            prevWeek.SetActive(false);
            
        }
        int weekNum = week + 1;
        weekText.text = "Week " + weekNum;
        MatchHistory();
        
        setStandings();
        TeamStats();
        StartCoroutine(stretch(Content));
        StartCoroutine(stretch(TeamSeasonStatsContent));
        SeasonRecords();
        CareerRecords();
        CheckWeek();
        setProgress();
    }
    public void SaveData(ref GameData go) { }
    public void MatchHistory()
    {
        for(int i = 0; i < Content.childCount; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
        if (selTeam.matchesPlayed[week].isPlayed)
        {
            isGamePlayed = true;
            HomeMatchPlayed.text = selTeam.matchesPlayed[week].isHome ? selTeam.name : selTeam.matchesPlayed[week].opponent;
            HomeScore.text = selTeam.matchesPlayed[week].isHome? selTeam.matchesPlayed[week].score.ToString() : selTeam.matchesPlayed[week].oppScore.ToString();
            GuestMatchPlayed.text = selTeam.matchesPlayed[week].isHome ? selTeam.matchesPlayed[week].opponent : selTeam.name;
            GuestScore.text = selTeam.matchesPlayed[week].isHome ? selTeam.matchesPlayed[week].oppScore.ToString() : selTeam.matchesPlayed[week].score.ToString();

            for (int i = 0; i < selTeam.players.Length; i++)
            {
                FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/"
                    + selTeam.id + "/" + week.ToString()
                    , selTeam.players[i]);
                PlayerStatsPersistent stats = statsHandler.Load();
                if (stats != null)
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


                                row.GetChild(0).gameObject.GetComponent<Text>().text = selTeam.players[i];
                            }

                            else
                            {
                                foreach ((string statname, System.Func<int> stat,  Action<int> b) in stats.getStats())
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
        else
        {
            isGamePlayed = false;
            isGameReady.text = selTeam.matchesPlayed[week].isReady ? "Ready" : "Not Ready";
        }
        Content.gameObject.SetActive(false);
        Content.gameObject.SetActive(true);
    }
    public void TeamStats( )
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
        for (int i = 0; i < west.Length; i++)
            {
            Debug.Log(gameData.id);
                FileDataHandler<TeamPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/"
                    , west[i]);
                TeamPersistent team = statsHandler.Load();
                
                if (team != null)
                {
                    GameObject statsTable = Instantiate(TeamSeasonStats, TeamSeasonStatsContent);
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


                            row.GetChild(0).gameObject.GetComponent<Text>().text = west[i];
                        }

                       else if (row.gameObject.name == "conference")
                        {


                            row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "West";
                        }
                    
                    else
                    {
                        foreach ((string statname, System.Func<int> stat) in team.getStats())
                        {

                            if (row.gameObject.name == statname)
                            {

                                Transform valueCell = row.GetChild(1);
                                Transform textObjV = valueCell.GetChild(0);
                                int statN = stat();
                         
                                textObjV.gameObject.GetComponent<Text>().text = statN.ToString();



                            }
                        }

                    }
                        }



                    }
                }

            }
        for (int i = 0; i < east.Length; i++)
        {
            FileDataHandler<TeamPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/"
                , east[i]);
            TeamPersistent team = statsHandler.Load();
            if (team != null)
            {
                GameObject statsTable = Instantiate(TeamSeasonStats, TeamSeasonStatsContent);
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


                            row.GetChild(0).gameObject.GetComponent<Text>().text = west[i];
                        }

                        else if (row.gameObject.name == "conference")
                        {


                            row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "East";
                        }

                        else
                        {
                            foreach ((string statname, System.Func<int> stat) in team.getStats())
                            {

                                if (row.gameObject.name == statname)
                                {

                                    Transform valueCell = row.GetChild(1);
                                    Transform textObjV = valueCell.GetChild(0);
                                    int statN = stat();
                               
                                    textObjV.gameObject.GetComponent<Text>().text = statN.ToString();



                                }
                            }

                        }
                    }



                }
            }

        }
    }

    public void CheckWeek()
    {
        
   

        (string,bool)[] teamsPlay =
  {
            ("Alabama Alligators",false),
            ("Florida Dolphins",false),
            ("Georgia Bears",false),
            ("Maryland Sharks",false),
            ("Michigan Warriors",false),
            ("New York Owls",false),
            ("Ohio True Frogs",false),
            ("Pennsylvania Rush",false),
            ("Virginia Bobcats",false),
            ("Wisconsin Crows",false),
            ("Arizona Jaguars",false),
            ("California Lightning",false),
          
           ( "Minnesota Wolves",false),
                       ("Nevada Magic",false),
                                  ( "New Mexico Dragons",false),
                                             ( "Oklahoma Stoppers",false),
                                                ( "Washington Hornets",false),
             ("Kansas Coyotes",false),



           ( "Oregon Trail Makers",false),
            ("Texas Rattlesnakes",false),
        
        };
       
        for(int i = 0; i<teamsPlay.Length;i++)
        {
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamsPlay[i].Item1);
            TeamPersistent teamPersistent = teamHandler.Load();
            if (teamPersistent.matchesPlayed[week].isPlayed)
            {
                Debug.Log("team " + teamPersistent.name + "'s match is played");
                teamsPlay[i].Item2= true;
                 if(teams.transform.GetChild(i).transform.childCount == 2)
                {
                    Destroy(teams.transform.GetChild(i).GetChild(1).gameObject);
                }
            }
            else
            {
                isNextWeekReady = false;
                Instantiate(hasPlayedObj, teams.transform.GetChild(i).transform);
            }
        }
        if(isNextWeekReady)
        {
            nextWeek.SetActive(false);
            startNewWeek.SetActive(true);
        }
        StartCoroutine(stretch(teams.transform));
    }

  
    public void PlayerStatsAllTime()
    {

        List<(string, PlayerStatsPersistent)> playersAll = new List<(string, PlayerStatsPersistent)>();
        for(int p = 0; p<selTeam.players.Length;p++ )
        {
            if (selTeam.players[p] != "")
            {


                PlayerStatsPersistent playerTotalStats = new(selTeam.players[p]);

                for (int k = 0; k <= week; k++)
                {
                    FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/" + selTeam.name + "/" + k.ToString(),
                        selTeam.players[p]);

                    PlayerStatsPersistent stats = statsHandler.Load();

                    if (stats != null)
                    {
                        playerTotalStats.pressures += stats.pressures;
                        playerTotalStats.plays += stats.plays;
                        playerTotalStats.defPlays += stats.defPlays;
                        playerTotalStats.blocks += stats.blocks;
                        playerTotalStats.steals += stats.steals;
                        playerTotalStats.fouls += stats.fouls;
                        playerTotalStats.pointsAllowed += stats.pointsAllowed;
                        playerTotalStats.shots += stats.shots;
                        playerTotalStats.shotsTaken += stats.shotsTaken;
                        playerTotalStats.pointsScored += stats.pointsScored;
                        playerTotalStats.foulShots += stats.foulShots;
                        playerTotalStats.foulShotsMade += stats.foulShotsMade;
                        playerTotalStats.foulPointsScored += stats.foulShotsMade;
                        playerTotalStats.turnovers += stats.turnovers;
                        playerTotalStats.turnOn += stats.turnOn;
                        playerTotalStats.insideShots += stats.insideShots;
                        playerTotalStats.insideShotsMade += stats.insideShotsMade;
                        playerTotalStats.midShots += stats.midShots;
                        playerTotalStats.midShotsMade += stats.midShotsMade;
                        playerTotalStats.outsideShots += stats.outsideShots;
                        playerTotalStats.outsideShotsMade += stats.outsideShotsMade;
                    }
                }
                playersAll.Add(new(selTeam.players[p], playerTotalStats));
            }
      }

        for (int i = 0; i < PlayersStatsContent.childCount; i++)
        {
            Destroy(PlayersStatsContent.GetChild(i).gameObject);
        }
        if(playersAll.Count == 0)
        {
            Debug.Log("No Stats Yet");
        }
        else
        {

        for(int i = 0; i<playersAll.Count; i++)
        {
            GameObject statsTable = Instantiate(playerStats, PlayersStatsContent);
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


                        row.GetChild(0).gameObject.GetComponent<Text>().text = playersAll[i].Item1;
                    }

                    else
                    {
                        foreach ((string statname, System.Func<int> stat,  Action<int> b) in playersAll[i].Item2.getStats())
                        {

                            if (row.gameObject.name == statname)
                            {

                                Transform valueCell = row.GetChild(1);
                                Transform textObjV = valueCell.GetChild(0);
                                int statN = stat();
                             
                                textObjV.gameObject.GetComponent<Text>().text = statN.ToString();



                            }
                        }

                    }
                }



            }

            }
        }
        StartCoroutine(stretch(PlayersStatsContent));
    }

    public void SeasonRecords()
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
            "Washington Hornets",
     
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
            TeamPersistent selTeam = teamHandler.Load();
            for (int p = 0; p < selTeam.players.Length; p++)
            {
               
                if (selTeam.players[p] != "")
                {

                    PlayerStatsPersistent playerTotalStats = new(selTeam.players[p]);

                    for (int k = 0; k <= week; k++)
                    {
                        FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/" + selTeam.name + "/" + k.ToString(),
                            selTeam.players[p]);

                        PlayerStatsPersistent stats = statsHandler.Load();

                        if (stats != null)
                        {
                            playerTotalStats.pressures += stats.pressures;
                            playerTotalStats.plays += stats.plays;
                            

                            playerTotalStats.defPlays += stats.defPlays;
                            playerTotalStats.blocks += stats.blocks;
                            playerTotalStats.steals += stats.steals;
                            playerTotalStats.fouls += stats.fouls;
                            playerTotalStats.pointsAllowed += stats.pointsAllowed;
                            playerTotalStats.shots += stats.shots;
                            playerTotalStats.shotsTaken += stats.shotsTaken;
                            playerTotalStats.pointsScored += stats.pointsScored;
                            playerTotalStats.foulShots += stats.foulShots;
                            playerTotalStats.foulShotsMade += stats.foulShotsMade;
                            playerTotalStats.foulPointsScored += stats.foulShotsMade;
                            playerTotalStats.turnovers += stats.turnovers;
                            playerTotalStats.turnOn += stats.turnOn;
                            playerTotalStats.insideShots += stats.insideShots;
                            playerTotalStats.insideShotsMade += stats.insideShotsMade;
                            playerTotalStats.midShots += stats.midShots;
                            playerTotalStats.midShotsMade += stats.midShotsMade;
                            playerTotalStats.outsideShots += stats.outsideShots;
                            playerTotalStats.outsideShotsMade += stats.outsideShotsMade;
                        }
                    }
                    allPlayerStats.Add(new(selTeam.players[p], playerTotalStats));
                }
            }
        }

        List<(string statName, int statVal, string player)> records = new();
        PlayerStatsPersistent record = new("record");
        for(int i = 0; i< record.getStats().Count; i++)
        {
            (string statName, int statVal,string player) rec = new();
          

           int max = 0;
            for(int k = 0; k<allPlayerStats.Count; k++)
            {
                for(int l =0; l < allPlayerStats[i].Item2.getStats().Count; l++)
                {
                    if (allPlayerStats[k].Item2.getStats()[l].Item1 == record.getStats()[i].Item1)
                    {
                      
                        if (allPlayerStats[k].Item2.getStats()[l].Item2() >= max)
                        {
                            max = allPlayerStats[k].Item2.getStats()[l].Item2();
                            rec.statName = allPlayerStats[k].Item2.getStats()[l].Item1;
                            rec.player = allPlayerStats[k].Item1;
                            rec.statVal = allPlayerStats[k].Item2.getStats()[l].Item2();
                        }
                    }
                }
               
            }
            records.Add(rec);
        }
    
            int rowsCount = seasonRecords.transform.childCount;
            Transform[] rows = new Transform[seasonRecords.transform.childCount];
            Transform roww;
            for (int k = 0; k < rowsCount; k++)
            {
                roww = seasonRecords.transform.GetChild(k);

                rows[k] = roww;
            }
            int b = 0;
            
            foreach (Transform row in rows)
            {
               
                if (row.gameObject.name != "Table Header" )
                {
               Debug.Log(records[b]+ b.ToString());
                        row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item2.ToString();
                    row.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item3.ToString();

                    FileDataHandler<PlayerPersistent> plHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", records[b].Item3.ToString());
                    PlayerPersistent p = plHandler.Load();
                    row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = p.team;
                b++;
            }

            }
        
    }
    public void CareerRecords()
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
            "Washington Hornets",

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
            TeamPersistent selTeam = teamHandler.Load();
            for (int p = 0; p < selTeam.players.Length; p++)
            {
                if (selTeam.players[p] != "")
                {
                    PlayerStatsPersistent playerTotalStats = new(selTeam.players[p]);
                    foreach (string currentSeason in gameData.seasons)
                    {
                    
                        for (int k = 0; k <= week; k++)
                    {
                        FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason + "/" + selTeam.name + "/" + k.ToString(),
                            selTeam.players[p]);

                        PlayerStatsPersistent stats = statsHandler.Load();

                        if (stats != null)
                        {
                            playerTotalStats.pressures += stats.pressures;
                            playerTotalStats.plays += stats.plays;
                            playerTotalStats.defPlays += stats.defPlays;
                            playerTotalStats.blocks += stats.blocks;
                            playerTotalStats.steals += stats.steals;
                            playerTotalStats.fouls += stats.fouls;
                            playerTotalStats.pointsAllowed += stats.pointsAllowed;
                            playerTotalStats.shots += stats.shots;
                            playerTotalStats.shotsTaken += stats.shotsTaken;
                            playerTotalStats.pointsScored += stats.pointsScored;
                            playerTotalStats.foulShots += stats.foulShots;
                            playerTotalStats.foulShotsMade += stats.foulShotsMade;
                            playerTotalStats.foulPointsScored += stats.foulShotsMade;
                            playerTotalStats.turnovers += stats.turnovers;
                            playerTotalStats.turnOn += stats.turnOn;
                            playerTotalStats.insideShots += stats.insideShots;
                            playerTotalStats.insideShotsMade += stats.insideShotsMade;
                            playerTotalStats.midShots += stats.midShots;
                            playerTotalStats.midShotsMade += stats.midShotsMade;
                            playerTotalStats.outsideShots += stats.outsideShots;
                            playerTotalStats.outsideShotsMade += stats.outsideShotsMade;
                        }
                    }
                    
                }
                    allPlayerStats.Add(new(selTeam.players[p], playerTotalStats));
                }
                
            }
        }
        List<(string statName, int statVal, string player)> records = new();
        PlayerStatsPersistent record = new("record");
        for (int i = 0; i < record.getStats().Count; i++)
        {
            (string statName, int statVal, string player) rec = new();

            int max = 0;
            for (int k = 0; k < allPlayerStats.Count; k++)
            {
                for (int l = 0; l < allPlayerStats[k].Item2.getStats().Count; l++)
                {
                    if (allPlayerStats[k].Item2.getStats()[l].Item1 == record.getStats()[i].Item1)
                    {

                        if (allPlayerStats[k].Item2.getStats()[l].Item2() >= max)
                        {
                            max = allPlayerStats[k].Item2.getStats()[l].Item2();
                            rec.statName = allPlayerStats[k].Item2.getStats()[l].Item1;
                            rec.player = allPlayerStats[k].Item1;
                            rec.statVal = allPlayerStats[k].Item2.getStats()[l].Item2();
                        }
                    }
                }

            }
            records.Add(rec);
        }

        int rowsCount = careerRecords.transform.childCount;
        Transform[] rows = new Transform[careerRecords.transform.childCount];
        Transform roww;
        for (int k = 0; k < rowsCount; k++)
        {
            roww = careerRecords.transform.GetChild(k);

            rows[k] = roww;
        }
        int b = 0;

        foreach (Transform row in rows)
        {

            if (row.gameObject.name != "Table Header")
            {
                Debug.Log(records[b] + b.ToString());
                row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item2.ToString();
                row.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item3.ToString();

                FileDataHandler<PlayerPersistent> plHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", records[b].Item3.ToString());
                PlayerPersistent p = plHandler.Load();
                row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = p.team;
                b++;
            }

        }

    }
    public void GameRecords()
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
            "Washington Hornets",

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
            TeamPersistent selTeam = teamHandler.Load();
            for (int p = 0; p < selTeam.players.Length; p++)
            {
                foreach (string currentSeason in gameData.seasons)
                {
                    List<(string, PlayerStatsPersistent)> playersAll = new List<(string, PlayerStatsPersistent)>();
                    if (selTeam.players[p] != "")
                    {

                        PlayerStatsPersistent playerTotalStats = new(selTeam.players[p]);

                        for (int k = 0; k < week; k++)
                        {
                            FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason + "/" + selTeam.name + "/" + k.ToString(),
                                selTeam.players[p]);

                            PlayerStatsPersistent stats = statsHandler.Load();

                            if (stats != null)
                            {
                                playerTotalStats.pressures += stats.pressures;
                                playerTotalStats.plays += stats.plays;
                                playerTotalStats.defPlays += stats.defPlays;
                                playerTotalStats.blocks += stats.blocks;
                                playerTotalStats.steals += stats.steals;
                                playerTotalStats.fouls += stats.fouls;
                                playerTotalStats.pointsAllowed += stats.pointsAllowed;
                                playerTotalStats.shots += stats.shots;
                                playerTotalStats.shotsTaken += stats.shotsTaken;
                                playerTotalStats.pointsScored += stats.pointsScored;
                                playerTotalStats.foulShots += stats.foulShots;
                                playerTotalStats.foulShotsMade += stats.foulShotsMade;
                                playerTotalStats.foulPointsScored += stats.foulShotsMade;
                                playerTotalStats.turnovers += stats.turnovers;
                                playerTotalStats.turnOn += stats.turnOn;
                                playerTotalStats.insideShots += stats.insideShots;
                                playerTotalStats.insideShotsMade += stats.insideShotsMade;
                                playerTotalStats.midShots += stats.midShots;
                                playerTotalStats.midShotsMade += stats.midShotsMade;
                                playerTotalStats.outsideShots += stats.outsideShots;
                                playerTotalStats.outsideShotsMade += stats.outsideShotsMade;
                            }
                        }
                        allPlayerStats.Add(new(selTeam.players[p], playerTotalStats));
                    }
                }
            }
        }
        List<(string statName, int statVal, string player)> records = new();
        PlayerStatsPersistent record = new("record");
        for (int i = 0; i < record.getStats().Count; i++)
        {
            (string statName, int statVal, string player) rec = new();

            int max = 0;
            for (int k = 1; k < allPlayerStats.Count; k++)
            {
                for (int l = 0; l < allPlayerStats[i].Item2.getStats().Count; l++)
                {
                    if (allPlayerStats[i].Item2.getStats()[l].Item1 == record.getStats()[i].Item1)
                    {

                        if (allPlayerStats[i].Item2.getStats()[l].Item2() >= max)
                        {
                            rec.statName = allPlayerStats[i].Item2.getStats()[l].Item1;
                            rec.player = allPlayerStats[i].Item1;
                            rec.statVal = allPlayerStats[i].Item2.getStats()[l].Item2();
                        }
                    }
                }

            }
            records.Add(rec);
        }

        int rowsCount = careerRecords.transform.childCount;
        Transform[] rows = new Transform[careerRecords.transform.childCount];
        Transform roww;
        for (int k = 0; k < rowsCount; k++)
        {
            roww = careerRecords.transform.GetChild(k);

            rows[k] = roww;
        }
        int b = 0;

        foreach (Transform row in rows)
        {

            if (row.gameObject.name != "Table Header")
            {
                Debug.Log(records[b] + b.ToString());
                row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item2.ToString();
                row.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item3.ToString();

                FileDataHandler<PlayerPersistent> plHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", records[b].Item3.ToString());
                PlayerPersistent p = plHandler.Load();
                row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = p.team;
                b++;
            }

        }

    }
    public void setTableActive()
    {
        MatchHistory();
        if(isGamePlayed)
        {
            matchStats.SetActive(true);
            matchPlayed.SetActive(false);
        }
        else
        {
            matchStats.SetActive(false);
            matchPlayed.SetActive(true);
        }
    }
    public void setTableInActive()
    {

        if (isGamePlayed)
        {
            matchStats.SetActive(false);
            matchPlayed.SetActive(false);
        }
        else
        {
            matchPlayed.SetActive(false);
            matchStats.SetActive(false);
        }

    }

    public void OnClickPrevWeek()
    {
        if (week > 0)
        {
            week-=1;
            int weekNum = week + 1;
            weekText.text = "Week " + weekNum;
            nextWeek.SetActive(true);
            if(week == 0)
            {
            prevWeek.SetActive(false);
            }
           
            CheckWeek();
        }
        
    }
    public void OnClickNextWeek()
    {
       
       if (week < currentSeason.week)
        {
            week+=1;
            int weekNum = week + 1;
            weekText.text = "Week " + weekNum;
            prevWeek.SetActive(true);
            if( week == currentSeason.week)
            {
                nextWeek.SetActive(false);
            }
           
          
            CheckWeek();
        }
       
    }
   
  public void OnClickNewWeek()
    {
        week++;
        currentSeason.week = week;
        int weekNum = week + 1;
        weekText.text = "Week " + weekNum;
        startNewWeek.SetActive(false);
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
       seasonHandler.Save(currentSeason);
        CheckWeek();
        setProgress();

    }

    public void setProgress()
    {

        if((week+1) %4 ==0)
        {
            Debug.Log("Players Progressing");
            for(int i = 0; i<currentSeason.progress.Length;i++)
            {
                if(week == currentSeason.progress[i].week)
                {
                    if(!currentSeason.progress[i].hasProgressed)
                    {
                        ProgressPlayers();
                        currentSeason.progress[i].setHasProgressed(true) ;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
            seasonHandler.Save(currentSeason);
        }
    }

    public void ProgressPlayers()
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
            "Washington Hornets",

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

        for(int i = 0; i<west.Length;i++)
        {
            FileDataHandler<TeamPersistent> teamHanlder = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", west[i]); ;
            TeamPersistent t = teamHanlder.Load();

            for(int k = 0; k<t.players.Length; k++)
            {

                if (t.players[k] != "") { 
                FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", t.players[k]);
                PlayerPersistent player = playerHandler.Load();
                    Debug.Log(player.Name);
                for(int j = 0; j<player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.HC[0])
                    {
                        int points = 1;
                           
                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                            player.getStats()[j].Item4(player.getStats()[j].Item2().value +1);
                            points++;
                            }
                            
                            break;
                    }
                }

                for (int j = 0; j < player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.HC[1])
                    {
                            int points = 1;

                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                                player.getStats()[j].Item4(player.getStats()[j].Item2().value + 1);
                                points++;
                            }

                            break;
                        }
                }
                for (int j = 0; j < player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.OC[0])
                    {
                            int points = 1;

                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                                player.getStats()[j].Item4(player.getStats()[j].Item2().value + 1);
                                points++;
                            }

                            break;
                        }
                }
                for (int j = 0; j < player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.OC[1])
                    {
                            int points = 1;

                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                                player.getStats()[j].Item4(player.getStats()[j].Item2().value + 1);
                                points++;
                            }

                            break;
                        }
                }
                for (int j = 0; j < player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.DC[0])
                    {
                            int points = 1;

                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                                player.getStats()[j].Item4(player.getStats()[j].Item2().value + 1);
                                points++;
                            }

                            break;
                        }
                }
                for (int j = 0; j < player.getStats().Count; j++)
                {
                    if (player.getStats()[j].Item1 == t.DC[1])
                    {
                            int points = 1;

                            player.getStats()[j].Item3(player.getStats()[j].Item2().value);
                            while ((player.getStats()[j].Item2().value < player.getStats()[j].Item2().potential) && points <= player.learning)
                            {

                                player.getStats()[j].Item4(player.getStats()[j].Item2().value + 1);
                                points++;
                            }

                            break;
                        }
                }
                    player.prevOvrl = player.ovrl;
                    player.ovrl =

                             ( player.consistency.value
                                 + player.awareness.value

                                + player.juking.value
                             + player.control.value
                                + player.shooting.value

                            + player.positioning.value
                             + player.steal.value
                             + player.guarding.value
                             + player.pressure.value

                                + player.inside.value
                             + player.mid.value
                             + player.Outside.value)/12;
    playerHandler.Save(player);
                }
            }
        }
    }

   
    private void Start()
    {
   
        // SetSelectedTeam(selectedTeam);
        SetSelectedOption(selectedOption);
        SetSelectedOption2(selectedOption2);

    }
    public IEnumerator stretch(Transform Content)
    {
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(true);
    }
    public void SetSelectedTeam(GameObject team)
    {
        selectedTeam = team;
        MatchHistory();
        setTableActive();
        string teamNameString = team.name;
 
       
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent teamm = teamLoader.Load();
        depthChart.team = teamm;
        selTeam = teamm;
        depthChart.GenerateDepthChart(teamNameString);
        teamName.text = teamm.name;
        marketCap.text = teamm.salaryCap.ToString();

        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        currentSeason = seasonHandler.Load();
        

        Vector2 sizeDelta = MatchesTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = selTeam.matchesPlayed.Count * 100;
        MatchesTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;


        Home.text = teamm.matchesPlayed[week].isHome ? teamm.name : teamm.matchesPlayed[week].opponent;
        Guest.text = teamm.matchesPlayed[week].isHome ? teamm.matchesPlayed[week].opponent : teamm.name;

        setGames();
        PlayerStatsAllTime();
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
        string[] HC =
        {
            "Consistency",
            "Awareness",
            "Juking",
            "Control",
            "Shooting",
            "Positioning",
            "Steals",
            "Guarding",
            "Pressure",
            "Inside",
            "Mid",
            "Outside",
        };
        string[] OC =
        {
            
            "Juking",
            "Control",
            "Shooting",
            "Inside",
            "Mid",
            "Outside",
        };
        string[] DC =
        {
            
            "Positioning",
            "Steals",
            "Guarding",
            "Pressure",
            "Inside",
            "Mid",
            "Outside",
        };



        coaching.setCoaching(selectedTeam.gameObject.name,new string[] { OC[oc.value], OC[oc2.value] },
            new string[] { DC[dc.value], DC[dc2.value] }, new string[] { HC[hc.value], HC[hc2.value] });
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
                selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].score.ToString() : selTeam.matchesPlayed[i].oppScore.ToString();
            newGame.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
            selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].oppScore.ToString() : selTeam.matchesPlayed[i].score.ToString();

            newGame.transform.GetChild(4).GetChild(0).GetComponent<Text>().text =
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
    public IEnumerator SetPlays()
    {
        


            int totalPlays = 0, totalDefPlays = 0;
            List<PlayerPersistent> Playerlist = new List<PlayerPersistent>(); notice.text = "trying to save";
            off.text = Table.childCount.ToString();
            for (int i = 0; i < Table.childCount; i++)
            {
                notice.text = "started at " + i;
                string playerName = Table.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text;
                PlayerPersistent p =new("",1,"","",1,1,1);
               
                    FileDataHandler<PlayerPersistent> player = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                    playerName);
                     p = player.Load();
                    yield return new WaitUntil(()=>p.Name!="");
                
                if (p.id!="")
                {
                    notice.text = p.Name;
                    
                    int playerDeffPlays = int.Parse(Table.GetChild(i).GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text);
                    notice.text = "player def plays";
                    int playerPlays = int.Parse(Table.GetChild(i).GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text);

                    notice.text = "player off plays";
                    p.plays = playerPlays;
                    p.defPlays = playerDeffPlays;
                    totalPlays += playerPlays;
                    totalDefPlays += playerDeffPlays;
                    Playerlist.Add(p);
                }
                notice.text = "stopped at " + i;
            }
            notice.text = "finished going through table";
            if (totalPlays == 40 && totalDefPlays == 40)
            {
                foreach (PlayerPersistent player in Playerlist)
                {
                    FileDataHandler<PlayerPersistent> handler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/",
                        player.Name);
                    notice.text = player.Name;
                    handler.Save(player);
                }
                notice.text = "Save Successful";
                team.matchesPlayed[week].isReady = true;
                FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", team.name);
                teamHandler.Save(team);
                MatchHistory();

            }
            else
            {
                notice.text = "Total Number of Plays for players must be 40";
            }
        
    }
}
