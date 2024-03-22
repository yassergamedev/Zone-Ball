using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedStuff : MonoBehaviour
{
    private GameObject selectedTeam;
    private GameObject selectedOption;
    private GameObject selectedOption2;
    public ColorBlock colors;
    private ColorBlock oldColors;
    private ColorBlock oldColorsOptions;
    private ColorBlock oldColorsOptions2;


    public void SetSelectedTeam(GameObject team)
    {
        selectedTeam = team;
        //change color of deselect button
        oldColorsOptions = selectedOption.gameObject.GetComponent<Button>().colors;
        oldColorsOptions2 = selectedOption2.gameObject.GetComponent<Button>().colors;

        selectedOption.gameObject.GetComponent<Button>().colors= colors;
        selectedOption2.gameObject.GetComponent<Button>().colors = colors;

        selectedTeam.gameObject.GetComponent<Button>().colors = oldColors;

       
    }
    public void SetSelectedOption(GameObject option)
    {
        selectedOption = option;
        //change color of deselect button
        oldColors = selectedTeam.gameObject.GetComponent<Button>().colors;
        oldColorsOptions2 = selectedOption2.gameObject.GetComponent<Button>().colors;

        selectedTeam.gameObject.GetComponent<Button>().colors = colors;
        selectedOption2.gameObject.GetComponent<Button>().colors = colors;

        selectedOption.gameObject.GetComponent<Button>().colors = oldColorsOptions;

       
    }
    public void SetSelectedOption2(GameObject option)
    {
        selectedOption2 = option;
        //change color of deselect button
        oldColorsOptions = selectedOption.gameObject.GetComponent<Button>().colors;
        oldColors = selectedTeam.gameObject.GetComponent<Button>().colors;

        selectedTeam.gameObject.GetComponent<Button>().colors = colors;
        selectedOption2.gameObject.GetComponent<Button>().colors = colors;

        selectedOption2.gameObject.GetComponent<Button>().colors = oldColorsOptions2;

  
    }
}
