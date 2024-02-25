using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public NumberManager numberManager;
    public int number;
    
    void Start()
    {
        CheckPossession();
        numberManager.SetPlayerNumber(number);
    }

    void Update()
    {
        numberManager.SetPlayerNumber(number);
    }
    void CheckPossession()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");

        // Check if the ball is in possession
        if (ball != null && ball.transform.IsChildOf(transform))
        {
             Debug.Log("I am " + gameObject.name + " from team "+ gameObject.transform.parent + " and i have possession of the ball");
            
        }
        else
        {
           Debug.Log("I am " + gameObject.name + " from team "+ gameObject.transform.parent + 
           "and i know that " +
           ball.transform.parent + " from team " + ball.transform.parent.transform.parent.name + " is in possession of the ball");
        }
    }
}
