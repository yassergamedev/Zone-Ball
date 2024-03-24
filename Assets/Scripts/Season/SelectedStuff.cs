using System.Collections;
using System.Collections.Generic;
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
    public TMP_Dropdown dc;
    public TMP_Dropdown hc;
    public Text teamName, marketCap;
    public Text Home, Guest;
    public GameObject weekChange;
    public int week;

    public Transform MatchesTable;

    public GameObject MatchDetail;
    private GameData gameData;
    private Season currentSeason;
    private TeamPersistent selTeam;
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
        coaching.setCoaching(selectedTeam.gameObject.name,oc.value.ToString(), dc.value.ToString(), hc.value.ToString());
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
            MatchDetail.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                selTeam.matchesPlayed[i].isHome ? selTeam.name : selTeam.matchesPlayed[i].opponent;
            MatchDetail.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
               selTeam.matchesPlayed[i].isHome ? selTeam.matchesPlayed[i].opponent : selTeam.name;
            Instantiate(MatchDetail, MatchesTable);
        }
    }
}
