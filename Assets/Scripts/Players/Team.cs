using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    //gameflow variables
    public int teamScore = 0;
    public int Plays = 40;
    public GameObject PlaysText;
    public enum teamType { away, home};

    private void Start()
    {
        FileDataHandler<CurrentGame> currGameHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame currentGame = currGameHandler.Load();
        Plays = currentGame.gamePlays;
       
        PlaysText.GetComponent<Text>().text = currentGame.gamePlays.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void decreasePlays()
    {
        PlaysText.GetComponent<Text>().text =  Plays.ToString();
    }
}
