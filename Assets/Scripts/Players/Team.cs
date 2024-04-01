using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    //gameflow variables
    public int teamScore = 0;
    public int Plays = 2;
    public GameObject PlaysText;
    public enum teamType { away, home};

    private void Start()
    {
        Plays = 2;
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
