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
       
        }
        numberManager.SetPlayerNumber(number);
    }

    void Update()
    {
        numberManager.SetPlayerNumber(number);
       
    }
    
}
