using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ball;
    public List<PlayerActions> playersToPlay;
    public List<PlayerActions> otherPlayersToPlay;
    private GameObject holderObject;
    private Text holderText;
    public bool isFouled = false;
    private bool ballFound = false;
    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        holderObject = GameObject.FindGameObjectWithTag("HolderText");
        holderText = holderObject.GetComponent<Text>();
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
            holderText.text = transform.name;
            return null;
            }
         else
           {
           
                 return ball.transform.parent.gameObject; // Return the GameObject that has the ball
             }
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void ChangePossession( int ind, bool isFoulShot, int teamPlays)
    {
        Debug.Log(ind);
        Debug.Log(otherPlayersToPlay.Count);
        Debug.Log("team plays "+ teamPlays);
        if (teamPlays > 0) {

            if (CompareTag("Player"))
            {
                Vector3 offset = new Vector3(-0.3f, 0, 0);
                ShuffleList(otherPlayersToPlay);  // Shuffle the list

                for (int k = 0; k < otherPlayersToPlay.Count; k++)
                {
                    Debug.Log("checking players " + otherPlayersToPlay[k].name);
                    if (otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays > 0)
                    {
                        Debug.Log("found a player " + otherPlayersToPlay[k].name);
                        ball.transform.position = otherPlayersToPlay[k].transform.position + -1 * offset;
                        ball.transform.parent = otherPlayersToPlay[k].transform;
                        if (!isFoulShot)
                        {
                            Debug.Log("Decreasing Plays");
                            otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        }
                        break;
                    }
                }
            }
            else
            {
                Vector3 offset = new Vector3(0.3f, 0, 0);
                ShuffleList(playersToPlay);  // Shuffle the list

                for (int k = 0; k < playersToPlay.Count; k++)
                {
                    Debug.Log("checking players " + playersToPlay[k].name);
                    if (playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays > 0)
                    {
                        Debug.Log("found a player " + playersToPlay[k].name);
                        ball.transform.position = playersToPlay[k].transform.position + -1 * offset;
                        ball.transform.parent = playersToPlay[k].transform;
                        if (!isFoulShot)
                        {
                            Debug.Log("Decreasing Plays");
                            playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("no team plays");
            if (CompareTag("Player"))
            {
                Vector3 offset = new Vector3(-0.3f, 0, 0);
                ShuffleList(otherPlayersToPlay);  // Shuffle the list

                for (int k = 0; k < otherPlayersToPlay.Count; k++)
                {
                    
                        ball.transform.position = otherPlayersToPlay[k].transform.position + -1 * offset;
                        ball.transform.parent = otherPlayersToPlay[k].transform;
                        if (!isFoulShot)
                        {
                            otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        }
                        break;
                    
                }
            }
            else
            {
                Vector3 offset = new Vector3(0.3f, 0, 0);
                ShuffleList(playersToPlay);  // Shuffle the list

                for (int k = 0; k < playersToPlay.Count; k++)
                {
                   
                        ball.transform.position = playersToPlay[k].transform.position + -1 * offset;
                        ball.transform.parent = playersToPlay[k].transform;
                        if (!isFoulShot)
                        {
                            playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.plays -= 1;
                        }
                        break;
                    
                }
            }
        }
        

        if (CompareTag("Player"))
        {
            if (ball.transform.parent.gameObject.GetComponent<PlayerMovement>().GuardedPlayer.GetComponent<PlayerActions>().playerPersistent.defPlays == 0)
            {
                for (int k = 0; k < playersToPlay.Count; k++)
                {
                    if (playersToPlay[k].GetComponent<PlayerActions>().playerPersistent.defPlays > 0)
                    {
                        //get the old guarded players 
                        GameObject oldGuard = ball.transform.parent.gameObject.GetComponent<PlayerMovement>().GuardedPlayer;
                        GameObject oldGuarder = playersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer;

                        //change guarded player of the old guard that no longer has def plays to the old guarded player of the player that has def plays
                        oldGuard.GetComponent<PlayerMovement>().GuardedPlayer = playersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer;
                        oldGuard.GetComponent<PlayerActions>().SetOtherPlayer(playersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer.GetComponent<PlayerActions>());

                        //set the new guarded players for both ball holder and guarder
                        playersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer = ball.transform.parent.gameObject;
                        playersToPlay[k].GetComponent<PlayerActions>().SetOtherPlayer(ball.transform.parent.gameObject.GetComponent<PlayerActions>());
                        ball.transform.parent.gameObject.GetComponent<PlayerActions>().SetOtherPlayer(playersToPlay[k].GetComponent<PlayerActions>());

                        //the the old guarded of the player that has enough def plays to the old guard of the ball holder
                        oldGuarder.GetComponent<PlayerMovement>().GuardedPlayer = oldGuard;
                        oldGuarder.GetComponent<PlayerActions>().SetOtherPlayer(oldGuard.GetComponent<PlayerActions>());
                        break;
                    }
                }
            }
        }
        else
        {
            if (ball.transform.parent.gameObject.GetComponent<PlayerMovement>().GuardedPlayer.GetComponent<PlayerActions>().playerPersistent.defPlays == 0)
            {
                for (int k = 0; k < otherPlayersToPlay.Count; k++)
                {
                    if (otherPlayersToPlay[k].GetComponent<PlayerActions>().playerPersistent.defPlays > 0)
                    {
                        //get the old guarded players 
                        GameObject oldGuard = ball.transform.parent.gameObject.GetComponent<PlayerMovement>().GuardedPlayer;
                        GameObject oldGuarder = otherPlayersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer;

                        //change guarded player of the old guard that no longer has def plays to the old guarded player of the player that has def plays
                        oldGuard.GetComponent<PlayerMovement>().GuardedPlayer = otherPlayersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer;
                        oldGuard.GetComponent<PlayerActions>().SetOtherPlayer(otherPlayersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer.GetComponent<PlayerActions>());

                        //set the new guarded players for both ball holder and guarder
                        otherPlayersToPlay[k].GetComponent<PlayerMovement>().GuardedPlayer = ball.transform.parent.gameObject;
                        otherPlayersToPlay[k].GetComponent<PlayerActions>().SetOtherPlayer(ball.transform.parent.gameObject.GetComponent<PlayerActions>());
                        ball.transform.parent.gameObject.GetComponent<PlayerActions>().SetOtherPlayer(otherPlayersToPlay[k].GetComponent<PlayerActions>());

                        //the the old guarded of the player that has enough def plays to the old guard of the ball holder
                        oldGuarder.GetComponent<PlayerMovement>().GuardedPlayer = oldGuard;
                        oldGuarder.GetComponent<PlayerActions>().SetOtherPlayer(oldGuard.GetComponent<PlayerActions>());
                        break;
                    }
                }
            }
        }
        ball.transform.parent.gameObject.GetComponent<PlayerMovement>().GuardedPlayer.GetComponent<PlayerActions>().playerPersistent.defPlays -= 1;

       
    }

   

}
