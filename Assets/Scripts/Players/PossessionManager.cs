using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ball;
    public List<PlayerActions> playersToPlay;
    public List<PlayerActions> otherPlayersToPlay;

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

    public void ChangePossession( int ind)
    {
        Debug.Log(ind);
        Debug.Log(otherPlayersToPlay.Count);
        if(CompareTag("Player"))
        {
            if (otherPlayersToPlay[ind].GetComponent<PlayerActions>().playerPersistent.plays < 0)
            {
                for (int k = 0; k < otherPlayersToPlay.Count; k++)
                {
                    if (otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays > 0)
                    {
                        ball.transform.position = otherPlayersToPlay[k].transform.position + new Vector3(-0.3f, 0, 0);
                        ball.transform.parent = otherPlayersToPlay[k].transform;
                        otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        break;
                    }
                }
            }
            else
            {
                ball.transform.position = otherPlayersToPlay[ind].transform.position + new Vector3(-0.3f, 0, 0);
                ball.transform.parent = otherPlayersToPlay[ind].transform;
                otherPlayersToPlay[ind].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
            }
            
           
        }
        else
        {
            if (playersToPlay[ind].GetComponent<PlayerActions>().playerPersistent.plays < 0)
            {
                for (int k = 0; k < playersToPlay.Count; k++)
                {
                    if (playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays > 0)
                    {
                        ball.transform.position = playersToPlay[k].transform.position + new Vector3(-0.3f, 0, 0);
                        ball.transform.parent = playersToPlay[k].transform;
                        playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        break;
                    }
                }
            }
            else
            {
                ball.transform.position = playersToPlay[ind].transform.position + new Vector3(-0.3f, 0, 0);
                ball.transform.parent = playersToPlay[ind].transform;
                playersToPlay[ind].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
            }
        }



        
    }

   

}
