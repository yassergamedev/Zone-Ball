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

    public GameObject weekChange;
    public int week;
    private GameData gameData;
    public void LoadData(GameData go)
    {
        gameData = go;
        FileDataHandler<TeamPersistent> teamLoader = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam.name);
        TeamPersistent team = teamLoader.Load();
        teamName.text = team.name;
        marketCap.text = team.salaryCap.ToString();
    }
    public void SaveData(ref GameData go) { }
    private void Start()
    {
       WeekChange wek = weekChange.GetComponent<WeekChange>();
        week = wek.currentWeek;
        weekSelector.value = week;
         SetSelectedTeam(selectedTeam);
        SetSelectedOption(selectedOption);
        SetSelectedOption2(selectedOption2);

    
    }

    public void SetSelectedTeam(GameObject team)
    {
        selectedTeam = team;
        depthChart.GenerateDepthChart(team.gameObject.name);
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
}
