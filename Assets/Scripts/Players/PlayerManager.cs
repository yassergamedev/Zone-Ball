using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public NumberManager numberManager;
    public PossessionManager possessionManager;
    public PlayerMovement playerMovement;
    public int number;
       private GameObject Possessor;
    void Start()
    {
        Possessor = possessionManager.CheckPossession();
        if(Possessor == null)
        {
            Debug.Log("I am " + gameObject.name);
        }else{
            Debug.Log("the ball isn't with me");
        }
        numberManager.SetPlayerNumber(number);
    }

    void Update()
    {
        numberManager.SetPlayerNumber(number);
        playerMovement.MovePlayer();
    }
    
}
