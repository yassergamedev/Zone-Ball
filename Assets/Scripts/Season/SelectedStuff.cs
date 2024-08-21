using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

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
    public Text weekText, isGameReady, SeasonPhase;
    public GameObject playerStats;
    public GameObject TeamSeasonStats;
    public GameObject matchStats, matchPlayed;
    public GameObject weekChange;
    public GameObject teamStanding;
    public int week;
    public int playOffRound, oldPlayOffRound;
    public Transform MatchesTable;
    public Transform StandingsTable;
    public Transform Content;
    public Transform TeamSeasonStatsContent;
    public Transform PlayersStatsContent;
    public GameObject DraftPlayer, DraftSalary, FAbid, DraftTeam, Vacancy, DraftTable,SeasonDraft,FATable;
    public GameObject Contract;
    public RandomNameGenerator rng;
    public GameObject MatchDetail;
    public bool isActiveTable = false;
    public bool isGamePlayed =false;
    public bool noGameToBePlayed = false;
    private GameData gameData;
    public Season currentSeason;
    private TeamPersistent selTeam;
    public SceneStuff sceneStuff;
    public GameObject seasonRecords;
    public GameObject careerRecords;
    public GameObject gameRecordsObject;
    public GameObject AgingTeams, Roster,advanceAgingPhase, DraftTeams,DraftRound2;
    public GameObject nextWeek, prevWeek, startNewWeek,startNextRound,finishPlayOffs,startFAphase,startNewSeason, NoGamesTab;
    public GameObject playOffsTab,StartPlayOffs;
    public GameObject sortableCareerContent, FullStats;
    public bool isNextWeekReady = true, isSeasonFinished = false;
    public GameObject teams;
    public GameObject hasPlayedObj;
    public GameObject[] tabs;
    private CurrentGame currGame;
    public Transform Table;
    public GameObject PlayerDepth;
    public GameObject coachingTable;
    public GameObject advanceTraining,StartNewGame;
    public TeamPersistent team;
    public Text off, def, notice;
    public Playfs playOffs;
    private bool isClickedBack = false, isComing = false;
    private List<(string, PlayerStatsPersistent,string)> allPlayerStats = new();
    private List<(string,string, PlayerStatsPersistent)> allTimePlayerStats = new();
    private List<(string, int)> teamsStats = new();
    List<TeamPersistent> orderedTeamsList;
    List<PlayerPersistent> players = new();
    public void LoadData(GameData go)
    {
        FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath, "Current Game");
        currGame = currentGame.Load();
        gameData = go;
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent team = teamLoader.Load();
        teamName.text = team.name;
        marketCap.text = team.salaryCap.ToString();
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
         currentSeason = seasonHandler.Load();
        week = currentSeason.week;
        playOffRound = currentSeason.PlayOffRound;
        oldPlayOffRound = playOffRound;
        setStandings();
        switch (currentSeason.phase)
        {
            case "Season":
                int weekNum = week + 1;
                SeasonPhase.text = currentSeason.id;
                weekText.text = "Week " + weekNum;
                Home.text = team.matchesPlayed[week].isHome ? team.name : team.matchesPlayed[week]?.opponent;
                Guest.text = team.matchesPlayed[week].isHome ? team.matchesPlayed[week]?.opponent : team.name;
                Vector2 sizeDelta = SeasonDraft.transform.GetComponent<RectTransform>().sizeDelta;
                sizeDelta.y = 2000;
                SeasonDraft.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                SeasonDraft.name = gameData.id;
                FileDataHandler<PlayerPersistent> playerHandlerr = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R1/", "");
                List<string> playersDrafted = playerHandlerr.GetAllFiles();

                List<PlayerPersistent> draftPlayers = new();
                
                for(int i = 0; i < 20; i++)
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R1/", playersDrafted[i]);
                    PlayerPersistent player = playerHandler.Load();
                    draftPlayers.Add(player);
                }
                draftPlayers = draftPlayers.OrderByDescending(player => player.ovrl).ToList();
                
                foreach (PlayerPersistent player in draftPlayers)
                {
                   
                    GameObject playerInfo = Instantiate(DraftPlayer, SeasonDraft.transform);

                    playerInfo.name = player.Name;
                    playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                    //playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                    playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                    playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                    ;

                    playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                    ;

                    playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                 ;

                    playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                  ;

                    playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                   ;

                    playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                      ;

                    playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                ;

                    playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                   ;

                    playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                   ;

                    playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                 ;

                    playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
              ;

                    playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                  ;

                    playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                    
                   



                   // GameObject draftTeam = Instantiate(DraftTeam, SeasonDraft.transform);

                }
                break;
            case "PlayOffs":
                SeasonPhase.text = "PlayOffs";
                playOffsTab.SetActive(true);
                switch (playOffRound)
                {
                    case 0:
                        {
                            weekText.text = "Round Of 16";
                            for (int i = 0, k = 15; i < playOffs.Round16.Length; i++, k--)
                            {
                                playOffs.Round16[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[i].name;
                                playOffs.Round16[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[k].name;
                            }
                            break;
                        }
                    case 1:
                        {
                            weekText.text = "QF";
                            for (int i = 0, k = 15; i < playOffs.Round16.Length; i++, k--)
                            {
                                playOffs.Round16[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[i].name;
                                playOffs.Round16[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[k].name;
                            }
                            for (int i = 0, k = 0; k < playOffs.Quarters.Length; i += 2, k++)
                            {
                                playOffs.Quarters[k].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i];
                                playOffs.Quarters[k].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i + 1];
                            }
                            break;
                        }
                    case 2:
                        {
                            weekText.text = "Semis";
                            for (int i = 0, k = 15; i < playOffs.Round16.Length; i++, k--)
                            {
                                playOffs.Round16[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[i].name;
                                playOffs.Round16[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[k].name;
                            }
                            for (int i = 0; i < playOffs.Quarters.Length; i++)
                            {
                                playOffs.Quarters[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i];
                                playOffs.Quarters[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i + 1];
                            }
                            for (int i = 0, j = 0; i < 2; i++, j++)
                            {
                                playOffs.Semis[0].transform.GetChild(0).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Semis[i];
                                playOffs.Semis[1].transform.GetChild(0).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Semis[i + 2];
                            }
                            break;
                        }
                    case 3:
                        {
                            weekText.text = "Final";
                            for (int i = 0, k = 15; i < playOffs.Round16.Length; i++, k--)
                            {
                                playOffs.Round16[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[i].name;
                                playOffs.Round16[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[k].name;
                            }
                            for (int i = 0, k = 0; i < playOffs.Quarters.Length * 2; i += 2, k++)
                            {
                                playOffs.Quarters[k].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i];
                                playOffs.Quarters[k].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Quarters[i + 1];
                            }
                            for (int i = 0, j = 0; i < 2; i++, j++)
                            {
                                playOffs.Semis[0].transform.GetChild(0).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Semis[i];
                                playOffs.Semis[1].transform.GetChild(0).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Semis[i + 2];
                            }
                            playOffs.Final.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Final[0];
                            playOffs.Final.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.Final[1];

                            break;
                        }
                        
                }
                break;
            case "Aging":
                SeasonPhase.text = "Aging/Resign phase";
                isSeasonFinished = true;
                for(int i = 0; i<tabs.Length; i++)
                {
                    tabs[i].SetActive(false);
                }
                prevWeek.SetActive(false);
                finishPlayOffs.SetActive(false);
                AgingTeams.SetActive(true);
                Roster.SetActive(true);
                 advanceAgingPhase.SetActive(true);

                break;
            case "Draft R1":
                prevWeek.SetActive(false);
                for (int i = 0; i < tabs.Length; i++)
                {
                    tabs[i].SetActive(false);
                }
                advanceAgingPhase.SetActive(false);
                SeasonPhase.text = "Draft R1";
                Roster.SetActive(false);
                DraftTable.SetActive(true);
                prevWeek.SetActive(false);
                AgingTeams.SetActive(false);
                DraftTeams.SetActive(true);
                DraftRound2.SetActive(true);
                Vector2 sizeDelta1 = DraftTable.transform.GetComponent<RectTransform>().sizeDelta;
                sizeDelta.y = 4000;
                DraftTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta1;
                DraftTable.name = gameData.id;
                FileDataHandler<PlayerPersistent> playerHandlerr1 = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R1/", "");
                List<string> playersDrafted1 = playerHandlerr1.GetAllFiles();
                for (int i = 0; i < 20; i++)
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R1/", playersDrafted1[i]);
                    PlayerPersistent player = playerHandler.Load();
                    GameObject playerInfo = Instantiate(DraftPlayer, DraftTable.transform);

                    playerInfo.name = playersDrafted1[i];
                    playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                    playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                    playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                    playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                       ;

                    playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                       ;

                    playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                    ;

                    playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                     ;

                    playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                      ;

                    playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                         ;

                    playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                   ;

                    playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                      ;

                    playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                      ;

                    playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                    ;

                    playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
                 ;

                    playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                     ;

                    playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                    GameObject playerContract = Instantiate(DraftSalary, DraftTable.transform);
                    playerContract.name = player.Name + " Draft";
                   
                        playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "10000";
                    
                     

                    

                    GameObject draftTeam = Instantiate(DraftTeam, DraftTable.transform);
                  
                }
                break;
            case "Draft R2":
                prevWeek.SetActive(false);
                for (int i = 0; i < tabs.Length; i++)
                {
                    tabs[i].SetActive(false);
                }
                SeasonPhase.text = "Draft R2";
                Roster.SetActive(false);
                DraftTable.SetActive(true);
                AgingTeams.SetActive(false);
                DraftTeams.SetActive(true);
                prevWeek.SetActive(false);
                startFAphase.SetActive(true);
                Vector2 sizeDeltaa = DraftTable.transform.GetComponent<RectTransform>().sizeDelta;
                sizeDeltaa.y = 4000;
                DraftTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDeltaa;
                DraftTable.name = gameData.id;
                FileDataHandler<PlayerPersistent> playerHandlerrr = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R2/", "");
                List<string> playersDraftedd = playerHandlerrr.GetAllFiles();
                for (int i = 0; i < 20; i++)
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/Draft R2/", playersDraftedd[i]);
                    PlayerPersistent player = playerHandler.Load();
                    GameObject playerInfo = Instantiate(DraftPlayer, DraftTable.transform);

                    playerInfo.name = playersDraftedd[i];
                    playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                    playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                    playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                    playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                   ;

                    playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                   ;

                    playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                ;

                    playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                 ;

                    playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                  ;

                    playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                     ;

                    playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
               ;

                    playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                  ;

                    playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                  ;

                    playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                ;

                    playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
             ;

                    playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                 ;

                    playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                    GameObject playerContract = Instantiate(DraftSalary, DraftTable.transform);
                    playerContract.name = player.Name + " Draft";

                    playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "5000";

                    GameObject draftTeam = Instantiate(DraftTeam, DraftTable.transform);
                }
                break;
            case "FA":
                prevWeek.SetActive(false);
                for (int i = 0; i < tabs.Length; i++)
                {
                    tabs[i].SetActive(false);
                }
                SeasonPhase.text = "FA phase";
                startNewSeason.SetActive(true);
                DraftTeams.SetActive(true) ;
                FileDataHandler<PlayerPersistent> playersHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", "");
                List<string> allPlayers = playersHandler.GetAllFiles();

                Vector2 sizeDeltaaa = DraftTable.transform.GetComponent<RectTransform>().sizeDelta;
                sizeDeltaaa.y = 150* allPlayers.Count;
                DraftTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDeltaaa;
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", allPlayers[i]);
                    PlayerPersistent player = playerHandler.Load();
                    if (player.status == "FA")
                    {
                        GameObject playerInfo = Instantiate(DraftPlayer, FATable.transform);


                        playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                        playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                        playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                        playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString();

                        playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString();

                        playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString();

                        playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString();

                        playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString();

                        playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString();

                        playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString();

                        playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString();

                        playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString();

                        playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString();

                        playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString();

                        playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString();

                        playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                        GameObject playerContract = Instantiate(FAbid, FATable.transform);
                        playerContract.name = player.Name + " FA";

                        playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = calcSalary(player.ovrl).ToString();


                        GameObject draftTeam = Instantiate(DraftTeam, FATable.transform);
                        draftTeam.name = player.Name;
                    }
                }
                break;

        }

       
            
        
       
        selTeam = team;
        depthChart.team = team;
        //selectedTeam.GetComponent<Button>().onClick.Invoke();
     
      if(week==0)
        {
            prevWeek.SetActive(false);
            
        }
            MatchHistory();

     
            TeamStats();
            StartCoroutine(stretch(Content));
            StartCoroutine(stretch(Content));
            SeasonRecords();
            CareerRecords();
        GameRecords();
        SortableStats();
            CheckWeek();
            setProgress();
        
    }

    public void onClickStartNewSeason()
    {
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame current = currHandler.Load();
       
        current.week = 1;

        int year = int.Parse(current.currentSeason.Replace("Season ", "")) + 1;
        current.currentSeason = "Season " + year;
        currHandler.Save(current);
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Season " + year + "/", "Season " + year);
        Season season = new("Season " + year, year.ToString(), "League " + year);
        seasonHandler.Save(season);

        FileDataHandler<TeamPersistent> teamsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", "");
        List<string> teams = teamsHandler.GetAllFiles();
        FileDataHandler<GameData> gameHandler = new(Application.persistentDataPath + "/" + gameData.id, gameData.id);
        GameData gamedata = gameHandler.Load();
        gamedata.seasons.Add("Season " + year);
        gamedata.currentSeason = "Season " + year;
        gameHandler.Save(gamedata);
        for (int i = 0;i<teams.Count;i++)
        {
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teams[i]);
            TeamPersistent team = teamHandler.Load();

            for(int k = 0;k<team.matchesPlayed.Count;k++)
            {
                team.matchesPlayed[k].isPlayed = false;
                team.matchesPlayed[k].isReady = false;

            }
            team.playOffMatches = new();
            teamHandler.Save(team);
        }
        SceneManager.LoadScene("Season");
    }

    public void LoadCoaching()
    {
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
                  "Steal",
            "Guarding",
            "Pressure",
            "Inside",
                  "Mid",
            "Outside",

    };
    int hc1=0, hc2 = 0, oc1 = 0, oc2 = 0, dc1 = 0, dc2 = 0;
        PlayerPersistent player = new();
        for(int i =0;i<player.getStats().Count; i++)
        {
            if (selTeam.HC[0] == player.getStats()[i].Item1)
            {
                hc1 = i;
            }
        }
        for (int i = 0; i < player.getStats().Count; i++)
        {
            if (selTeam.HC[1] == player.getStats()[i].Item1)
            {
                hc2 = i;
            }
        }
        for (int i = 0; i < DC.Length; i++)
        {
            if (selTeam.DC[0] == DC[i])
            {
                dc1 = i;
            }
        }
        for (int i = 0; i < DC.Length; i++)
        {
            if (selTeam.DC[1] == DC[i])
            {
                dc2 = i;
            }
        }
        for (int i = 0; i < OC.Length; i++)
        {
            if (selTeam.OC[0] == OC[i])
            {
                oc1 = i;
            }
        }
        for (int i = 0; i < OC.Length; i++)
        {
            if (selTeam.OC[1] == OC[i])
            {
                oc2 = i;
            }
        }

        coachingTable.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = hc1;
        coachingTable.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Dropdown>().value = hc2;
        coachingTable.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = oc1;
        coachingTable.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_Dropdown>().value = oc2;
        coachingTable.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value = dc1;
        coachingTable.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_Dropdown>().value = dc2;
    }
    public void onClickStartFA()
    {
        SeasonPhase.text = "FA Phase";
        DraftTable.SetActive(false);
        DraftRound2.SetActive(false);
        startFAphase.SetActive(false);
        startNewSeason.SetActive(true);

        
        DraftTable.name = gameData.id;
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);

        currentSeason.phase = "FA";
        seasonHandler.Save(currentSeason);

        FileDataHandler<PlayerPersistent> playersHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", "");
        List<string> allPlayers = playersHandler.GetAllFiles();

        Vector2 sizeDelta = DraftTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = 150* allPlayers.Count;
        DraftTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        for (int i = 0; i<allPlayers.Count; i++)
        {
            FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", allPlayers[i]);
            PlayerPersistent player = playerHandler.Load();
            if(player.status == "FA")
            {
                GameObject playerInfo = Instantiate(DraftPlayer, FATable.transform);

              
                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString();

                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString();

                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString();

                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString();

                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString();

                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString();

                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString();

                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString();

                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString();

                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString();

                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString();

                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString();

                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                GameObject playerContract = Instantiate(FAbid, FATable.transform);
                playerContract.name = player.Name + " FA";

                 playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = calcSalary(player.ovrl).ToString();


                GameObject draftTeam = Instantiate(DraftTeam, FATable.transform);
            }
        }
    }

    public int calcSalary(int ovrl)
    {
        int salary = 10000;
        if (ovrl >= 90)
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
    public void SaveData(ref GameData go) { }
    public void MatchHistory()
    {
        for(int i = 0; i < Content.childCount; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
        if(currentSeason.phase == "Season")
        {

        
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
                FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.name);
                
                    int totalPlays = 0;
                    int totalDefPlays = 0;
                    int startingPlayers = 0;
                    for (int i = 0; i < selTeam.players.Length; i++)
                    {
                        if(selTeam.players[i]!="")
                        {
                            FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", selTeam.players[i]);
                            PlayerPersistent playerPersistent = playerHandler.Load();

                            totalPlays += playerPersistent.plays;
                            totalDefPlays += playerPersistent.defPlays;
                            if(playerPersistent.plays>0 || playerPersistent.defPlays>0)
                            {
                                startingPlayers += 1;
                            }
                        }
                        
                    }
                int totalOtherTeamsPlays = 0;
                int totalOtherTeamDefPlays = 0;
                int otherStartingPlayers = 0;
                FileDataHandler<TeamPersistent> otherTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.matchesPlayed[week].opponent);
                TeamPersistent otherTeam = otherTeamHandler.Load();
                for (int i = 0; i < otherTeam.players.Length; i++)
                {
                    if (otherTeam.players[i] != "")
                    {
                        FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", otherTeam.players[i]);
                        PlayerPersistent playerPersistent = playerHandler.Load();
                        totalOtherTeamsPlays += playerPersistent.plays;
                        totalOtherTeamDefPlays += playerPersistent.defPlays;
                        if (playerPersistent.plays > 0 || playerPersistent.defPlays > 0)
                        {
                            otherStartingPlayers += 1;
                        }
                    }
                }
                if (totalPlays == currGame.gamePlays && totalDefPlays == currGame.gamePlays && startingPlayers==8 &&
                    totalOtherTeamsPlays == currGame.gamePlays && totalOtherTeamDefPlays == currGame.gamePlays && otherStartingPlayers == 8 
                    )
                    {
                        Debug.Log("True");
                        Debug.Log("Ready : " + totalPlays);
                        Debug.Log("Ready : " + totalDefPlays);
                        Debug.Log("Ready : " + startingPlayers);
                        selTeam.matchesPlayed[week].isReady = true;
                        
                        teamHandler.Save(selTeam);

                    otherTeam.matchesPlayed[week].isReady = true;
                    
                    otherTeamHandler.Save(otherTeam);

                }
                    else
                    {
                        Debug.Log("False");
                        Debug.Log("Ready : " + totalPlays);
                        Debug.Log("Ready : " + totalDefPlays);
                        Debug.Log("Ready : " + startingPlayers);
                        selTeam.matchesPlayed[week].isReady = false;

                        teamHandler.Save(selTeam);
                    otherTeam.matchesPlayed[week].isReady = false;

                    otherTeamHandler.Save(otherTeam);
                }
                
               

                isGameReady.text = selTeam.matchesPlayed[week].isReady ? "Ready" : "Not Ready";
        }

        }
        else
        {
            if(selTeam.playOffMatches.Count < playOffRound+1)
            {
                Debug.Log("trying with team: " + team);

                noGameToBePlayed = true;
            }
            else
            {
                if (selTeam.playOffMatches[playOffRound].isPlayed)
                {
                    isGamePlayed = true;
                    HomeMatchPlayed.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.name : selTeam.playOffMatches[playOffRound].opponent;
                    HomeScore.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.playOffMatches[playOffRound].score.ToString() : selTeam.playOffMatches[playOffRound].oppScore.ToString();
                    GuestMatchPlayed.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.playOffMatches[playOffRound].opponent : selTeam.name;
                    GuestScore.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.playOffMatches[playOffRound].oppScore.ToString() : selTeam.playOffMatches[playOffRound].score.ToString();

                    for (int i = 0; i < selTeam.players.Length; i++)
                    {
                        FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + currentSeason.id + "/"
                            + selTeam.id + "/" + "Round "+ playOffRound.ToString()
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
                                        foreach ((string statname, System.Func<int> stat, Action<int> b) in stats.getStats())
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
                    Home.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.name : selTeam.playOffMatches[playOffRound]?.opponent;
                    Guest.text = selTeam.playOffMatches[playOffRound].isHome ? selTeam.playOffMatches[playOffRound]?.opponent : selTeam.name;
                    isGamePlayed = false;
                    
                        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.name);
                        int totalPlays = 0;
                        int totalDefPlays = 0;
                        int startingPlayers = 0;
                        for (int i = 0; i < selTeam.players.Length; i++)
                        {
                        if (selTeam.players[i] != "")
                        {
                            FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", selTeam.players[i]);
                            PlayerPersistent playerPersistent = playerHandler.Load();
                            totalPlays += playerPersistent.plays;
                            totalDefPlays += playerPersistent.defPlays;
                            if (playerPersistent.plays > 0 || playerPersistent.defPlays > 0)
                            {
                                startingPlayers += 1;
                            }
                        }
                        }
                        int totalOtherTeamsPlays = 0;
                        int totalOtherTeamDefPlays = 0;
                        int otherStartingPlayers = 0;
                        FileDataHandler<TeamPersistent> otherTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.matchesPlayed[playOffRound].opponent);
                        TeamPersistent otherTeam = otherTeamHandler.Load();
                        for (int i = 0; i < otherTeam.players.Length; i++)
                        {
                        if (otherTeam.players[i] != "")
                        {
                            FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", otherTeam.players[i]);
                            PlayerPersistent playerPersistent = playerHandler.Load();
                            totalOtherTeamsPlays += playerPersistent.plays;
                            totalOtherTeamDefPlays += playerPersistent.defPlays;
                            if (playerPersistent.plays > 0 || playerPersistent.defPlays > 0)
                            {
                                otherStartingPlayers += 1;
                            }
                        }
                        }
                        if (totalPlays == currGame.gamePlays && totalDefPlays == currGame.gamePlays &&
                            totalOtherTeamsPlays == currGame.gamePlays && totalOtherTeamDefPlays == currGame.gamePlays
                            && startingPlayers == 8 && otherStartingPlayers == 8)
                        {
                            selTeam.playOffMatches[playOffRound].isReady = true;
                            otherTeam.playOffMatches[playOffRound].isReady = true;
                            teamHandler.Save(selTeam);
                            otherTeamHandler.Save(otherTeam);
                        }
                        else
                        {
                            selTeam.playOffMatches[playOffRound].isReady = false;
                            otherTeam.playOffMatches[playOffRound].isReady = false;
                            teamHandler.Save(selTeam);
                            otherTeamHandler.Save(otherTeam);
                        }
                    
                    isGameReady.text = selTeam.playOffMatches[playOffRound].isReady ? "Ready" : "Not Ready";
                }
            }
            

            
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


                            row.GetChild(0).gameObject.GetComponent<Text>().text = east[i];
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
        Array.Sort(teamsPlay, (team1, team2) => team1.Item1.CompareTo(team2.Item1));
        if (currentSeason.phase == "Season")
        {
            for (int i = 0; i<teamsPlay.Length;i++)
            {
                Debug.Log(teamsPlay[i].Item1);
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamsPlay[i].Item1);
            TeamPersistent teamPersistent = teamHandler.Load();
                Debug.Log("week: " + teamPersistent.name);
                if (teamPersistent.matchesPlayed[week].isPlayed)
                {
                    Debug.Log("team " + teamPersistent.name + "'s match is played");
                    teamsPlay[i].Item2 = true;
                    for (int c = 0; c < teams.transform.childCount; c++)
                    {
                        if (teams.transform.GetChild(c).gameObject.name == teamsPlay[i].Item1)
                        {
                            if (teams.transform.GetChild(i).transform.childCount == 2)
                                {
                       
                                Destroy(teams.transform.GetChild(c).GetChild(1).gameObject);
                                break;
                            }
                            
                        }
                       
                    }
                }
                else
                {
                    isNextWeekReady = false;
                    for (int c = 0; c < teams.transform.childCount; c++)
                    {
                        if (teams.transform.GetChild(c).gameObject.name == teamsPlay[i].Item1)
                        {
                            Instantiate(hasPlayedObj, teams.transform.GetChild(c).transform);
                            break;
                        }
                    }
                            
                   
                }
            }
        }
        else
        {
            for (int i = 0; i < teamsPlay.Length; i++)
            {
                FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamsPlay[i].Item1);
                TeamPersistent teamPersistent = teamHandler.Load();

                Debug.Log(teamPersistent.name + " number of playoff matches: "+teamPersistent.playOffMatches.Count);

                if (teamPersistent.playOffMatches.Count == oldPlayOffRound+1)
                {
                   

                    if (teamPersistent.playOffMatches[playOffRound].isPlayed)
                    {
                        Debug.Log("team " + teamPersistent.name + "'s match is played");
                        teamsPlay[i].Item2 = true;
                        if (teams.transform.GetChild(i).transform.childCount == 2)
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
               
            }
            isComing = false;
            isClickedBack = false;
        }
        if(isNextWeekReady)
        {
            if(currentSeason.phase == "Season")
            {
                Debug.Log("the week is :" + week);
                nextWeek.SetActive(false);
                if (week == 27)
                {
                    StartPlayOffs.SetActive(true);
                }
                else
                {
                    startNewWeek.SetActive(true);
                }
            }
            else
            {
                if(playOffRound == 3)
                {
                    finishPlayOffs.SetActive(true);
                }
                else
                {
                    startNextRound.SetActive(true);
                }
            }
           
            
        }
        StartCoroutine(stretch(teams.transform));
    }

    public void onClickStartDraftingPhase(string round)
    {
      for(int i = 0; i < DraftTable.transform.childCount;i++)
        {
            if(DraftTable.transform.GetChild(i).name != "Header")
            {
                Destroy(DraftTable.transform.GetChild(i).gameObject);
            }
        }

            SeasonPhase.text = round;
        advanceAgingPhase.SetActive(false);
        Roster.SetActive(false);
        DraftTable.SetActive(true);
        AgingTeams.SetActive(false);
        DraftTeams.SetActive(true);
        if(round == "Draft R1")
        {
            DraftRound2.SetActive(true);
        }
        else
        {
            startFAphase.SetActive(true) ;
        }
      
        Vector2 sizeDelta = DraftTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = 4000;
        DraftTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        DraftTable.name = gameData.id;
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        
        currentSeason.phase = round;
        seasonHandler.Save(currentSeason);
       

        for (int i = 0;i<20;i++)
        {

            string playerName = rng.GenerateRandomPlayerName();
            FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/"+ currentSeason.id +"/"+round+"/", playerName);
            string type = "";
            int randomNum = UnityEngine.Random.Range(0, 10);
            if (randomNum < 5)
            {
                type = "off";
            }
            else
            {
                type = "def";
            }
            int ovrl = 31;
            if (round == "Draft R1")
            {
                ovrl = UnityEngine.Random.Range(35, 40);
            }
            else
            {
                 
                ovrl = UnityEngine.Random.Range(31, 46);

            }
            
            int age = UnityEngine.Random.Range(18, 21);
            PlayerPersistent player = new PlayerPersistent(playerName, i, playerName, type, 0, age, ovrl, "Signed");



            GameObject playerInfo = Instantiate(DraftPlayer, DraftTable.transform);

            playerInfo.name = playerName;
            playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
            playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
            playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

            playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString()
                + "(" + (player.consistency.potential).ToString() + ")";

            playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString()
                  + "(" + (player.awareness.potential).ToString() + ")";

            playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString()
                  + "(" + (player.juking.potential).ToString() + ")";

            playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString()
                  + "(" + (player.control.potential).ToString() + ")";

            playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString()
                  + "(" + (player.shooting.potential).ToString() + ")";

            playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString()
                  + "(" + (player.positioning.potential).ToString() + ")";

            playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString()
                  + "(" + (player.steal.potential).ToString() + ")";

            playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString()
                  + "(" + (player.guarding.potential).ToString() + ")";

            playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString()
                  + "(" + (player.pressure.potential).ToString() + ")";

            playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString()
                  + "(" + (player.inside.potential ).ToString() + ")";

            playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString()
                  + "(" + (player.mid.potential).ToString() + ")";
     
            playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString()
                  + "(" + (player.Outside.potential ).ToString() + ")";

            playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

            GameObject playerContract = Instantiate(DraftSalary, DraftTable.transform);
            playerContract.name = player.Name + " Draft";
            if(round == "Draft R1")
            {
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "10000";
            }
            else
            {
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "5000";
            }
            
            playerHandler.Save(player);

            GameObject draftTeam = Instantiate(DraftTeam, DraftTable.transform);

        }
    }
   
 public void onClickFinishPlayOffs()
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

        for(int i =0;i<west.Length;i++)
        {
            FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", west[i]);
            TeamPersistent team = teamHandler.Load();
            for(int k = 0; k<team.players.Length;k++)
            {
                if (team.players[k]!="")
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[k]);
                    PlayerPersistent player = playerHandler.Load();

                    player.resignNumber = UnityEngine.Random.Range(1, 7) + player.personality;
                    playerHandler.Save(player);
                }
            }
        }
        SeasonPhase.text = "Aging/Resign phase";
        currentSeason.phase = "Aging";
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        seasonHandler.Save(currentSeason);
        isSeasonFinished = true;
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }

        finishPlayOffs.SetActive(false);
        AgingTeams.SetActive(true);
        prevWeek.SetActive(false);
        Roster.SetActive(true);
        advanceAgingPhase.SetActive(true);

    }
    //CHANGE THIS LATER
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
                        playerTotalStats.jukes += stats.jukes;

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
               
                for(int s = 0; s< gameData.seasons.Count; s++)
                {

                
                if (selTeam.players[p] != "")
                {

                    PlayerStatsPersistent playerTotalStats = new(selTeam.players[p]);
                    
                    for (int k = 0; k <= week; k++)
                    {
                        FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.seasons[s] + "/" + selTeam.name + "/" + k.ToString(),
                            selTeam.players[p]);

                        PlayerStatsPersistent stats = statsHandler.Load();
                        
                        if (stats != null)
                        {
                            playerTotalStats.id = stats.id;
                            playerTotalStats.team = stats.team;
                            playerTotalStats.pressures += stats.pressures;
                            playerTotalStats.plays += stats.plays;
                            

                            playerTotalStats.defPlays += stats.defPlays;
                            playerTotalStats.blocks += stats.blocks;
                            playerTotalStats.steals += stats.steals;
                            playerTotalStats.fouls += stats.fouls;
                                playerTotalStats.jukes += stats.jukes;
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
                    allPlayerStats.Add(new(selTeam.players[p], playerTotalStats, gameData.seasons[s]));
                }

            }
            }
        }

        List<(string statName, int statVal, string player,string team,string season)> records = new();
        PlayerStatsPersistent record = new("record");
        for(int i = 0; i< record.getStats().Count; i++)
        {
            (string statName, int statVal,string player,string team,string season) rec = new();
          

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
                            
                            rec.team = allPlayerStats[k].Item2.team;
                            rec.season = allPlayerStats[k].Item3;
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
                Debug.Log("some team : " +records[b].Item4);
                row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item4;
                row.GetChild(4).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item5;
                FileDataHandler<PlayerPersistent> plHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", records[b].Item3.ToString());
                    PlayerPersistent p = plHandler.Load();
                  //  row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = p.team!=null?p.team:"";
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

            int teamMemebers = 0, ovrlTotal = 0;
            for (int p = 0; p < selTeam.players.Length; p++)
            {

                List<(string, PlayerStatsPersistent)> playersAll = new List<(string, PlayerStatsPersistent)>();
                if (selTeam.players[p] != "")
                {
                    FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" +
                    gameData.id + "/Players/", selTeam.players[p]);
                    PlayerPersistent selPlayer = playerHandler.Load();
                    teamMemebers++;
                    ovrlTotal += selPlayer.ovrl;
                    selPlayer.stats.team = selPlayer.team;
                    Debug.Log(selPlayer.stats.id + " " + selPlayer.stats.fouls);
                    allTimePlayerStats.Add(new(selTeam.players[p],selTeam.id, selPlayer.stats));
                }

            }
            int teamOvrl = (int)ovrlTotal / teamMemebers;

            teamsStats.Add((selTeam.name, teamOvrl));
        }
        List<(string statName, int statVal, string player,string team)> records = new();
        PlayerStatsPersistent record = new("record");
        for (int i = 0; i < record.getStats().Count; i++)
        {
            (string statName, int statVal, string player,string team) rec = new();

            int max = 0;
            for (int k = 1; k < allTimePlayerStats.Count; k++)
            {
               
                for (int l = 0; l < allTimePlayerStats[k].Item3.getStats().Count; l++)
                {
                    if (allTimePlayerStats[k].Item3.getStats()[l].Item1 == record.getStats()[i].Item1)
                    {
                        Debug.Log("current points of " + allTimePlayerStats[k].Item3.getStats()[l].Item1 + " of "+ allTimePlayerStats[k].Item1+ ": " + allTimePlayerStats[k].Item3.getStats()[l].Item2());

                        if (allTimePlayerStats[k].Item3.getStats()[l].Item2() >= max)
                        {
                            

                            max = allTimePlayerStats[k].Item3.getStats()[l].Item2();
                            if(max != 0)
                            {
                                Debug.Log("current max of " + allTimePlayerStats[k].Item3.getStats()[l].Item1 + ": " + max);
                            }
                            rec.statName = allTimePlayerStats[k].Item3.getStats()[l].Item1;
                            rec.player = allTimePlayerStats[k].Item1;
                            rec.statVal = allTimePlayerStats[k].Item3.getStats()[l].Item2();
                            rec.team = allTimePlayerStats[k].Item3.team;
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
                row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = records[b].Item4;
                FileDataHandler<PlayerPersistent> plHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", records[b].Item3.ToString());
                PlayerPersistent p = plHandler.Load();
                //row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = p.team;
                b++;
            }

        }

    }

  
    public void SortableStats()
    {

        FileDataHandler<PlayerPersistent> playersHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", "");
        List<string> playerNames = playersHandler.GetAllFiles();
       
        foreach(string p in playerNames)
        {
            FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players", p);
            PlayerPersistent player = playerHandler.Load();
            players.Add(player);
        }
     
            Vector2 sizeDelta = StandingsTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = players.Count * 50;
        sortableCareerContent.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;

       
        foreach(PlayerPersistent player in players)
        {
            GameObject fullStats = Instantiate(FullStats, sortableCareerContent.transform);

            fullStats.gameObject.name = player.Name;
            fullStats.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player.Name;
            fullStats.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = player.Age.ToString();
            if (player.team == "New York Owls")
            {
                fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "NY";
            }
            else
            {
                if(player.team == "New Mexico Dragons")
                {
                    fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "NM";
                }
                else
                {
                    fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = player.team;
                }
            }
            

            for(int k = 3,s=0; s<player.stats.getStats().Count; k++,s++)
            {
                fullStats.transform.GetChild(k).GetChild(0).GetComponent<Text>().text = player.stats.getStats()[s].Item2().ToString();
            }
        }
    }

    public void SortTable(GameObject stat)
    {
        string statName = stat.name;
        PlayerStatsPersistent p = new("");
        int i;
        for ( i = 0; i < p.getStats().Count; i++)
        {
            if (p.getStats()[i].Item1 == statName)
                break;
        }

        switch (statName){
            case "Name":
                players = players.OrderBy(player => player.Name).ToList();
                break;
            case "Age":
                players = players.OrderByDescending(player => player.Age).ToList();
                break;
            case "Team":
                players = players.OrderBy(player => player.team).ToList();
                break;
            default:
                players = players.OrderByDescending(player => player.stats.getStats()[i].Item2()).ToList();
                break;
        }

       


        for(int k = 0; k< sortableCareerContent.transform.childCount; k++)
        {
            Destroy(sortableCareerContent.transform.GetChild(k).gameObject);
        }


        Vector2 sizeDelta = StandingsTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = players.Count * 50;
        sortableCareerContent.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;


        foreach (PlayerPersistent player in players)
        {
            GameObject fullStats = Instantiate(FullStats, sortableCareerContent.transform);

            fullStats.gameObject.name = player.Name;
            fullStats.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player.Name;
            fullStats.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = player.Age.ToString();
            fullStats.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = player.Age.ToString();
            if (player.team == "New York Owls")
            {
                fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "NY";
            }
            else
            {
                if (player.team == "New Mexico Dragons")
                {
                    fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "NM";
                }
                else
                {
                    fullStats.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = player.team;
                }
            }

            for (int k = 3, s = 0; s < player.stats.getStats().Count; k++, s++)
            {
                fullStats.transform.GetChild(k).GetChild(0).GetComponent<Text>().text = player.stats.getStats()[s].Item2().ToString();
            }
        }
    }
    public void GameRecords()
    {
        List<(string statName, int statVal, string player, string team, string game, int week, string season)> overallRecords = new();
        List<(string statName, int statVal, string player, string team, string game, int week, string season)> gameRecords = new();
        List<(string statName, int statVal, string player, string team, string game, int week, string season)> localRecords = new();
        PlayerStatsPersistent iterableStat = new("record");
        List<string> seasons = gameData.seasons;

        for (int s = 0; s < seasons.Count; s++) // Loop through each season
        {
            string season = seasons[s];

            for (int w = 0; w <= week; w++)
            {
                Debug.Log($"Processing week {w}");
                FileDataHandler<MatchPlayed> matchesPlayedHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + season + "/Matches Played/" + w, "");
                List<string> matchesPlayed = matchesPlayedHandler.GetAllFiles();
                foreach (string match in matchesPlayed)
                {
                    Debug.Log($"Processing match {match}");
                    (string statName, int statVal, string player, string team, string game, int week, string season) homeGameRec = new();
                    (string statName, int statVal, string player, string team, string game, int week, string season) guestGameRec = new();
                    string[] teams = match.Split(new string[] { " vs " }, StringSplitOptions.None);
                    string homeTeam = "", guestTeam = "";
                    if (teams.Length == 2)
                    {
                        homeTeam = teams[0];
                        guestTeam = teams[1];
                    }
                    else
                    {
                        Debug.LogError("Invalid match format: " + match);
                        continue;
                    }

                    FileDataHandler<MatchPlayed> matchHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + season + "/Matches Played/" + w, match);
                    MatchPlayed p = matchHandler.Load();

                    if (p == null)
                    {
                        Debug.LogError("Failed to load match: " + match);
                        continue;
                    }

                    List<PlayerStatsPersistent> homeStats = p.homeStatsPersistent;
                    List<PlayerStatsPersistent> guestStats = p.guestStatsPersistent;

                    for (int k = 0; k < iterableStat.getStats().Count; k++)
                    {
                        int max = 0;
                        for (int i = 0; i < homeStats.Count; i++)
                        {
                            Debug.Log("big stat coming :" + homeStats[i].id);
                            for (int j = 0; j < iterableStat.getStats().Count; j++)
                            {
                                if (homeStats[i].getStats()[j].Item1 == iterableStat.getStats()[k].Item1)
                                {
                                    if (homeStats[i].getStats()[j].Item2() >= max)
                                    {
                                        max = homeStats[i].getStats()[j].Item2();
                                        homeGameRec.statName = homeStats[i].getStats()[j].Item1;
                                        homeGameRec.statVal = max;
                                        homeGameRec.player = homeStats[i].id;
                                        homeGameRec.team = homeTeam;
                                        homeGameRec.game = match;
                                        homeGameRec.week = w;
                                        homeGameRec.season = season; // Register the season
                                    }
                                }
                            }
                        }
                        localRecords.Add(homeGameRec);
                        max = 0;
                        for (int i = 0; i < guestStats.Count; i++)
                        {
                            for (int j = 0; j < iterableStat.getStats().Count; j++)
                            {
                                if (guestStats[i].getStats()[j].Item1 == iterableStat.getStats()[k].Item1)
                                {
                                    if (guestStats[i].getStats()[j].Item2() >= max)
                                    {
                                        max = guestStats[i].getStats()[j].Item2();
                                        guestGameRec.statName = guestStats[i].getStats()[j].Item1;
                                        guestGameRec.statVal = max;
                                        guestGameRec.player = guestStats[i].id;
                                        guestGameRec.team = guestTeam;
                                        guestGameRec.game = match;
                                        guestGameRec.week = w;
                                        guestGameRec.season = season; // Register the season
                                    }
                                }
                            }
                        }
                        localRecords.Add(guestGameRec);
                    }
                }

                // Find top stats for each statName in a single game (localRecords)
                foreach (var stat in iterableStat.getStats())
                {
                    var topStat = localRecords
                        .Where(r => r.statName == stat.Item1 && r.week == w)
                        .OrderByDescending(r => r.statVal)
                        .FirstOrDefault();

                    if (topStat != default)
                    {
                        gameRecords.Add(topStat);
                    }
                    else
                    {
                        Debug.LogWarning($"No records found for stat {stat.Item1} in week {w}");
                    }
                }

                localRecords.Clear();
            }
        }

        // Find top stats for each statName overall (gameRecords)
        foreach (var stat in iterableStat.getStats())
        {
            var topStat = gameRecords
                .Where(r => r.statName == stat.Item1)
                .OrderByDescending(r => r.statVal)
                .FirstOrDefault();

            if (topStat != default)
            {
                overallRecords.Add(topStat);
            }
            else
            {
                Debug.LogWarning($"No overall records found for stat {stat.Item1}");
            }
        }

        // Output results (for demonstration purposes)
        Console.WriteLine("Overall Records:");
        foreach (var record in overallRecords)
        {
            Console.WriteLine($"Stat: {record.statName}, Value: {record.statVal}, Player: {record.player}, Team: {record.team}, Game: {record.game}, Week: {record.week}, Season: {record.season}");
        }

        // Update UI
        int rowsCount = gameRecordsObject.transform.childCount;
        Transform[] rows = new Transform[rowsCount];
        for (int k = 0; k < rowsCount; k++)
        {
            rows[k] = gameRecordsObject.transform.GetChild(k);
        }

        int b = 0;
        foreach (Transform row in rows)
        {
            if (row.gameObject.name != "Table Header")
            {
                if (b < overallRecords.Count)
                {
                    row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = overallRecords[b].statVal.ToString();
                    row.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = overallRecords[b].player;
                    row.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = overallRecords[b].team;
                    row.GetChild(4).GetChild(0).gameObject.GetComponent<Text>().text = overallRecords[b].game;
                    row.GetChild(5).GetChild(0).gameObject.GetComponent<Text>().text = (overallRecords[b].week + 1).ToString();
                    row.GetChild(6).GetChild(0).gameObject.GetComponent<Text>().text = overallRecords[b].season; 
                    b++;
                }
                else
                {
                    Debug.LogWarning("Not enough overall records to fill all rows");
                    break;
                }
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
            NoGamesTab.SetActive(false);
        }
        else
        {
            if(noGameToBePlayed)
            {
                matchStats.SetActive(false);
                matchPlayed.SetActive(false);
               
                NoGamesTab.SetActive(true);
            }
            else
            {
                matchStats.SetActive(false);
                matchPlayed.SetActive(true);
                NoGamesTab.SetActive(false);
            }
            
        }
    }
    public void setTableInActive()
    {

            matchStats.SetActive(false);
            matchPlayed.SetActive(false);
            NoGamesTab.SetActive(false);
       

    }

    public void OnClickPrevWeek()
    {
        if(currentSeason.phase == "Season")
        {
            if (week > 0)
            {
                week -= 1;
                int weekNum = week + 1;
                weekText.text = "Week " + weekNum;
                nextWeek.SetActive(true);
                if (week == 0)
                {
                    prevWeek.SetActive(false);
                }

                CheckWeek();
            }

        }
        else
        {
            if (playOffRound > 0)
            {
                playOffRound -= 1;
                isClickedBack = true;
                
                switch (playOffRound)
                {
                    case 0:
                        weekText.text = "Round of 16";
                        break;
                    case 1:
                        weekText.text = "QF";
                        break;
                    case 2:
                        weekText.text = "Semis";
                        break;
                    case 3:
                        weekText.text = "Final";
                        break;
                }

                nextWeek.SetActive(true);
                if (playOffRound == 0)
                {
                    prevWeek.SetActive(false);
                }
         

                CheckWeek();
            }
        }

    }
    public void OnClickStartPlayOffs()
    {
        
        currentSeason.phase = "PlayOffs";
        weekText.text = "Round of 16";
        StartPlayOffs.SetActive(false);
        prevWeek.SetActive(false);
        SeasonPhase.text = "PlayOffs";
        playOffsTab.SetActive(true);
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);

        currentSeason.finalTeamStandings = orderedTeamsList;
        for (int i = orderedTeamsList.Count-1, j = 0; i>=1;i--)
        {
         
            if(j<8 && i<16)
            {
                 string teamName = orderedTeamsList[j].id;
                string otherTeam = orderedTeamsList[i].id;
                Debug.Log("the lovely team is: " + teamName);
                FileDataHandler<TeamPersistent> anotherTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", teamName);
                FileDataHandler<TeamPersistent> otherTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", otherTeam);
                TeamPersistent anotherTeam = anotherTeamHandler.Load();
                TeamPersistent teamm = otherTeamHandler.Load();
                anotherTeam.playOffMatches.Add(new(teamm.name, false));
                teamm.playOffMatches.Add(new(anotherTeam.name, true));

                otherTeamHandler.Save(teamm);
                anotherTeamHandler.Save(anotherTeam);

                j++;
            }

        }
        seasonHandler.Save(currentSeason);

        for (int i = 0, k=15; i < playOffs.Round16.Length; i++,k--)
        {
            playOffs.Round16[i].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[i].name;
            playOffs.Round16[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = currentSeason.finalTeamStandings[k].name;
        }
        CheckWeek();
    }
    public void OnClickNextWeek()
    {
       if(currentSeason.phase == "Season")
        {
            if (week < currentSeason.week)
            {
                week += 1;
                int weekNum = week + 1;
                weekText.text = "Week " + weekNum;
                prevWeek.SetActive(true);
                if (week == currentSeason.week)
                {
                    nextWeek.SetActive(false);
                }


                CheckWeek();

            }
        }
        else
        {
            if (playOffRound < currentSeason.PlayOffRound)
            {
                playOffRound += 1;
                switch (playOffRound)
                {
                    case 0:
                        weekText.text = "Round of 16";
                        break;
                    case 1:
                        weekText.text = "QF";
                        break;
                    case 2:
                        weekText.text = "Semis";
                        break;
                    case 3:
                        weekText.text = "Final";
                        break;
                }
                prevWeek.SetActive(true);
                if (playOffRound == currentSeason.PlayOffRound)
                {
                    nextWeek.SetActive(false);
                }


                CheckWeek();

            }

        }
       
       
    }
   public void OnClickNewRound()
    {
        playOffRound++;
        oldPlayOffRound++;
        currentSeason.PlayOffRound = playOffRound;
        switch (playOffRound)
        {
            case 1:
                {
                    weekText.text = "QF";
                    for (int i = 0, j=0; i < 8; i+=2,j++)
                    {
                        FileDataHandler<TeamPersistent> qTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.finalTeamStandings[i].id);
                        TeamPersistent qTeam = qTeamHandler.Load();
                        
                        playOffs.Quarters[j].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = 
                            qTeam.playOffMatches[playOffRound-1].result? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                        FileDataHandler<TeamPersistent> qTeamHandler2 = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.finalTeamStandings[i+1].id);
                        TeamPersistent qTeam2 = qTeamHandler2.Load();
                        playOffs.Quarters[j].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                           qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                        string firstWinner = qTeam.playOffMatches[playOffRound - 1].result ? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                        string secondWinner = qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                        FileDataHandler<TeamPersistent> firstWinnderHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", firstWinner);
                        TeamPersistent fWinner = firstWinnderHandler.Load();
                        FileDataHandler<TeamPersistent> secondWinnerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", secondWinner);
                        TeamPersistent sWinner = secondWinnerHandler.Load();

                        fWinner.playOffMatches.Add(new(sWinner.id, true));
                        sWinner.playOffMatches.Add(new(fWinner.id, false));

                        firstWinnderHandler.Save(fWinner);
                        secondWinnerHandler.Save(sWinner);
                        currentSeason.Quarters.Add(fWinner.id);
                        currentSeason.Quarters.Add(sWinner.id);

                    }
                    
                    break;
                }
            case 2:
                {
                    weekText.text = "Semis";

                    for (int i = 0, j = 0; i <= 2; i += 2,j++)
                    {
                        FileDataHandler<TeamPersistent> qTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.Quarters[i]);
                        TeamPersistent qTeam = qTeamHandler.Load();

                        playOffs.Semis[j].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                            qTeam.playOffMatches[playOffRound - 1].result ? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                        FileDataHandler<TeamPersistent> qTeamHandler2 = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.Quarters[i + 4]);
                        TeamPersistent qTeam2 = qTeamHandler2.Load();
                        playOffs.Semis[j].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                           qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                        string firstWinner = qTeam.playOffMatches[playOffRound - 1].result ? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                        string secondWinner = qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                        FileDataHandler<TeamPersistent> firstWinnderHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", firstWinner);
                        TeamPersistent fWinner = firstWinnderHandler.Load();
                        FileDataHandler<TeamPersistent> secondWinnerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", secondWinner);
                        TeamPersistent sWinner = secondWinnerHandler.Load();
                       
                        fWinner.playOffMatches.Add(new(sWinner.id, true));
                        sWinner.playOffMatches.Add(new(fWinner.id, false));

                        firstWinnderHandler.Save(fWinner);
                        secondWinnerHandler.Save(sWinner);
                        currentSeason.Semis.Add(fWinner.id);
                        currentSeason.Semis.Add(sWinner.id);
      
                    }
                    break;
                }
            case 3:
                {
                    weekText.text = "Final";
                    FileDataHandler<TeamPersistent> qTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.Semis[0]);
                    TeamPersistent qTeam = qTeamHandler.Load();

                    playOffs.Final.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                        qTeam.playOffMatches[playOffRound - 1].result ? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                    FileDataHandler<TeamPersistent> qTeamHandler2 = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", currentSeason.Semis[2]);
                    TeamPersistent qTeam2 = qTeamHandler2.Load();
                    playOffs.Final.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text =
                       qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                    string firstWinner = qTeam.playOffMatches[playOffRound - 1].result ? qTeam.name : qTeam.playOffMatches[playOffRound - 1].opponent;
                    string secondWinner = qTeam2.playOffMatches[playOffRound - 1].result ? qTeam2.name : qTeam2.playOffMatches[playOffRound - 1].opponent;

                    FileDataHandler<TeamPersistent> firstWinnderHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", firstWinner);
                    TeamPersistent fWinner = firstWinnderHandler.Load();
                    FileDataHandler<TeamPersistent> secondWinnerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", secondWinner);
                    TeamPersistent sWinner = secondWinnerHandler.Load();

                    fWinner.playOffMatches.Add(new(sWinner.id, true));
                    sWinner.playOffMatches.Add(new(fWinner.id, false));

                    firstWinnderHandler.Save(fWinner);
                    secondWinnerHandler.Save(sWinner);
                    currentSeason.Final.Add(fWinner.id);
                    currentSeason.Final.Add(sWinner.id);
                    break;
                }
        }

        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        seasonHandler.Save(currentSeason);
        startNextRound.SetActive(false);
        CheckWeek();

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
        FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
        Games games = gamesHandler.Load();

        for (int i = 0; i < games.games.Count; i++)
        {
            if (games.games[i].currentGame == currentSeason.id)
            {
                games.games[i].week = week;
                break;
            }
        }
        gamesHandler.Save(games);
        CheckWeek();
        setProgress();
       }

    public void setProgress()
    {

        if((week+1)  ==1 || (week + 1) == 5 || (week + 1) == 9 || (week + 1) == 13 || (week + 1) == 17 || (week + 1) == 21 || (week + 1) == 21 || (week + 1) == 25)
        {
           
            for(int i = 0; i<currentSeason.progress.Length;i++)
            {
                if(week+1 == currentSeason.progress[i].week)
                {
                    if(!currentSeason.progress[i].hasProgressed)
                    {
                        advanceTraining.SetActive(true);
                        StartNewGame.SetActive(false);
                        
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
           
        }
       
    }

    public void ProgressPlayers()
    {
        for (int i = 0; i < currentSeason.progress.Length; i++)
        {
            if (week + 1 == currentSeason.progress[i].week)
            {
                if (!currentSeason.progress[i].hasProgressed)
                {
                    currentSeason.progress[i].setHasProgressed(true);
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
    public void callStretch(Transform Content)
    {
        StartCoroutine(stretch(Content));
    }
    public IEnumerator stretch(Transform Content)
    {
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Content.gameObject.SetActive(true);
    }
    public void SetSelectedTeamName(GameObject team)
    {
        teamName.text = team.name;
        selectedTeam = team;
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent teamm = teamLoader.Load();
        marketCap.text = teamm.salaryCap.ToString();
    }
        public void SetSelectedTeam(GameObject team)
    {
        noGameToBePlayed = false;
        selectedTeam = team;
        MatchHistory();
        setTableActive();
        string teamNameString = team.name;
        Debug.Log(teamNameString);
 
       
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent teamm = teamLoader.Load();
        List<PlayerPersistent> players = new();
        for (int i = 0; i < teamm.players.Length; i++)
        {
            if (teamm.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", teamm.players[i]);
                PlayerPersistent player = _playerHandler.Load();
                Debug.Log(player.Name);
                players.Add(player);
                Debug.Log(players[i].Name);
            }
        }
        depthChart.team = teamm;
        depthChart.players = players;
        selTeam = teamm;
       depthChart.GenerateDepthChart(teamNameString);
    
        teamName.text = teamm.name;
        marketCap.text = teamm.salaryCap.ToString();

        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        currentSeason = seasonHandler.Load();
        

        Vector2 sizeDelta = MatchesTable.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = selTeam.matchesPlayed.Count * 50;
        MatchesTable.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        switch (currentSeason.phase)
        {
            case "Season":
                Debug.Log("week " + week);
                Home.text = teamm.matchesPlayed[week].isHome ? teamm.name : teamm.matchesPlayed[week]?.opponent;
                Guest.text = teamm.matchesPlayed[week].isHome ? teamm.matchesPlayed[week]?.opponent : teamm.name;
                break;
            case "Playoffs":
                if (teamm.playOffMatches.Count < playOffRound + 1)
                {
                    noGameToBePlayed = true;
                }
                else
                {
                    Home.text = teamm.playOffMatches[playOffRound].isHome ? teamm.name : teamm.playOffMatches[playOffRound]?.opponent;
                    Guest.text = teamm.playOffMatches[playOffRound].isHome ? teamm.playOffMatches[playOffRound]?.opponent : team.name;
                }
                break;
            case "Draft":
                break;
            case "FA":
                break;

        }

    

        setGames();
        PlayerStatsAllTime();
        LoadCoaching();
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
            "Steal",
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
        int teamOverall = 0;
        for(int i = 0; i<teamsStats.Count; i++)
        {
        
            if (teamsStats[i].Item1 == selTeam.name)
            {
                teamOverall = teamsStats[i].Item2;
                break;
            }
        }
        for(int i = 0; i<selTeam.matchesPlayed.Count; i++)
        {
            int oppOverall = 0;
            for (int k = 0; k < teamsStats.Count; k++)
            {
                if (teamsStats[k].Item1 == selTeam.matchesPlayed[i].opponent)
                {
                    oppOverall = teamsStats[k].Item2;
                    break;
                }
            }
            GameObject newGame = Instantiate(MatchDetail, MatchesTable);
            
            newGame.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Week " + (i + 1);   
           newGame.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
                selTeam.matchesPlayed[i].isHome ? selTeam.name + " ("+ teamOverall + ")": selTeam.matchesPlayed[i].opponent + " (" + oppOverall + ")";
            newGame.transform.GetChild(2).GetChild(0).GetComponent<Text>().text =
                selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].score.ToString() : selTeam.matchesPlayed[i].oppScore.ToString();
            newGame.transform.GetChild(3).GetChild(0).GetComponent<Text>().text =
            selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].oppScore.ToString() : selTeam.matchesPlayed[i].score.ToString();

            newGame.transform.GetChild(4).GetChild(0).GetComponent<Text>().text =
               selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].opponent + " (" + oppOverall + ")" : selTeam.name + " (" + teamOverall + ")";
            newGame.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
                selTeam.matchesPlayed[i].extraTime.ToString();
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
            team.matchesPlayed = new();
            standing.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (k+1).ToString();
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
    public void StartGame()
    {
        if(currentSeason.phase == "Season")
        {
            FileDataHandler<TeamPersistent> otherTeam = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.matchesPlayed[week].opponent);
            TeamPersistent other = otherTeam.Load();

            if (selTeam.matchesPlayed[week].isReady && selTeam.matchesPlayed[week].isPlayed == false
                && other.matchesPlayed[week].isReady)
            {
                FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath, "Current Game");
                 currGame = currentGame.Load();
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
        else
        {
            FileDataHandler<TeamPersistent> otherTeam = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selTeam.playOffMatches[currentSeason.PlayOffRound].opponent);
            TeamPersistent other = otherTeam.Load();

            if (selTeam.playOffMatches[currentSeason.PlayOffRound].isReady && selTeam.playOffMatches[currentSeason.PlayOffRound].isPlayed == false
                && other.playOffMatches[currentSeason.PlayOffRound].isReady)
            {
                FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath, "Current Game");
                CurrentGame currGame = currentGame.Load();
                currGame.week = week;
                currGame.game = selTeam.playOffMatches[currentSeason.PlayOffRound];
                currentGame.Save(currGame);
                sceneStuff.LoadScene();
            }
            else
            {
                Debug.Log("Game not ready to start, set depth charts correctly for both teams!!");
            }
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
                PlayerPersistent p =new("",1,"","",1,1,1,"");
               
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
            if (totalPlays == currGame.maxPlays && totalDefPlays == currGame.maxPlays)
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
