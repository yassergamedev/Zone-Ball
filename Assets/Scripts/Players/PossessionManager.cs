using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ball;
    GameObject[] playersToPlay;

    public bool isFouled = false;
    private void Start()
    {
         ball = GameObject.FindGameObjectWithTag("Ball");

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
      
       playersToPlay = GameObject.FindGameObjectsWithTag(teamTag);

        Player player = parent.GetComponent<Player>();
        if(player.maxPlays<=0)
        {
            while(!assignedPlayer)
            {
             
                int rand = Random.Range(0, playersToPlay.Length);
                if (playersToPlay[rand].GetComponent<Player>().maxPlays >0)
                {
                    
                    ball.transform.parent = playersToPlay[rand].transform;
                    
                    playersToPlay[rand].GetComponent<Player>().plays -= 1;
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
            parent.GetComponent<Player>().maxPlays -= 1;
            
            ball.transform.parent = parent.transform;
            
        }
        
      
    }

}
