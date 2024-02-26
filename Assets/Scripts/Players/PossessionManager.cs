using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    // Start is called before the first frame update
  
 
   public GameObject CheckPossession()
{
    GameObject ball = GameObject.FindGameObjectWithTag("Ball");

    // Check if the ball is in possession
    if (ball != null && ball.transform.IsChildOf(transform))
    {
        return null;
    }
    else
    {
        // Debug.Log("I am " + gameObject.name + " from team " + gameObject.transform.parent.name + 
        // " and I know that " +
        // ball.transform.parent.name + " from team " + ball.transform.parent.transform.parent.name + " is in possession of the ball");

        return ball.transform.parent.gameObject; // Return the GameObject that has the ball
    }
}

}
