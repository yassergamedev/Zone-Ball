using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ball;
    GameObject[] playersToPlay;

    public bool isFouled = false;
    private bool ballFound = false;
    private void Start()
    {
         ball = GameObject.FindGameObjectWithTag("Ball");

    }
    private void Update()
    {
        if(!ballFound)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
        }
        else
        {
            ballFound = true;
        }
       
    }
    public GameObject CheckPossession()
    {
    // Check if the ball is in possession
         if (ball.transform.IsChildOf(transform))
         {
         return null;
            }
         else
           {
                 return ball.transform.parent.gameObject; // Return the GameObject that has the ball
             }
    }

    public void ChangePossession(GameObject parent)
    {
        bool assignedPlayer = false;
        string teamTag = parent.tag;
        string otherTeamTag = teamTag == "Player" ? "OppPlayer" : "Player";
      
       playersToPlay = GameObject.FindGameObjectsWithTag(teamTag);
    
       GameObject[] otherPlayersToPlay = GameObject.FindGameObjectsWithTag(otherTeamTag);
        PlayerPersistent player = parent.GetComponent<PlayerActions>().playerPersistent;
        if(player.plays<=0)
        {
            while(!assignedPlayer)
            {
             
                int rand = Random.Range(0, playersToPlay.Length);

                if (playersToPlay[rand].GetComponent<PlayerActions>().playerPersistent.plays >0)
                {
             
                    ball.transform.parent = playersToPlay[rand].transform;

                   
                    playersToPlay[rand].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                   
                    assignedPlayer = true;

                }
              
            }
        }
        else
        {
            switch (teamTag)
            {
                case "Player":
                    ball.transform.position = parent.transform.position + new Vector3(-0.3f, 0, 0);
                    break;
                case "OppPlayer":
                    ball.transform.position = parent.transform.position + new Vector3(0.3f, 0, 0);
                    break;
            }
            parent.GetComponent<PlayerActions>().playerPersistent.plays -= 1;
            
            ball.transform.parent = parent.transform;
            
        }

        int guardingNum = Random.Range(0, otherPlayersToPlay.Length);

        otherPlayersToPlay[guardingNum].GetComponent<PlayerMovement>().ballHolderGuard = true;
        otherPlayersToPlay[guardingNum].GetComponent<PlayerActions>().playerPersistent.defPlays -= 1;
    }

}
