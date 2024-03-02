using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ball;
    GameObject[] playersToPlay;
    private void Start()
    {
         ball = GameObject.FindGameObjectWithTag("Ball");

    }
    public GameObject CheckPossession()
    {
 

    // Check if the ball is in possession
    if (ball != null && ball.transform.IsChildOf(transform))
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
        Vector3 position;

        GameObject team;
        playersToPlay = GameObject.FindGameObjectsWithTag(teamTag);

        Player player = parent.GetComponent<Player>();
        if(player.plays >= player.maxPlays)
        {
            while(!assignedPlayer)
            {
     
                int rand = Random.Range(0, playersToPlay.Length);
                if (playersToPlay[rand].GetComponent<Player>().plays <
                    playersToPlay[rand].GetComponent<Player>().maxPlays)
                {
                    
                    ball.transform.parent = playersToPlay[rand].transform;
                    playersToPlay[rand].GetComponent<Player>().plays += 1;
                    assignedPlayer = true;

                }
              
            }
        }
        else
        {
            switch (teamTag)
            {
                case "Player":
                    ball.transform.position = parent.transform.position + new Vector3(-0.5f, 0, 0);
                    break;
                case "OppPlayer":
                    ball.transform.position = parent.transform.position + new Vector3(0.5f, 0, 0);
                    break;
            }
            parent.GetComponent<Player>().plays += 1;
            
            team = GameObject.FindGameObjectWithTag(teamTag);
            ball.transform.parent = parent.transform;
            
        }
        
      
    }

}
