using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedStuff : MonoBehaviour
{
    public GameObject selectedTeam;
    public GameObject selectedOption;
    public GameObject selectedOption2;

    public ViewRoster ViewRoster;
    public DepthChart depthChart;
    public TMP_Dropdown weekSelector;
    public GameObject weekChange;
    public int week;
    private void Start()
    {
       WeekChange wek = weekChange.GetComponent<WeekChange>();
        week = wek.currentWeek;
        weekSelector.value = week;
       //  SetSelectedTeam(selectedTeam);
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
}
