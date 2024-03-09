using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    public Player player;
    public PlayerActions PlayerActions;


    private GameObject RightMid;
    private GameObject RightInside;
    private GameObject LeftMid;
    private GameObject LeftInside;
    private GameObject GuardedPlayer;
    private GameObject foulManager;

    //Bounds for the player to move
    private GameObject boundXL;
    private GameObject boundYU;
    private GameObject boundXR;
    private GameObject boundYD;
    private GameObject MidPoint;
    private GameObject shotPlaceR;
    private GameObject shotPlaceL;

    string GuardedPlayrTag;
   
    
    GameObject[] playersToBeGuarded;
  

    private string playerTag;

    //to simulate players moving in different locations
    public float timeToMove = 2f;
    public float moveTimer = 0;
    //to decide whether to go upfront or just in a random location
    float randomNumberForAwareness;
    float randomX;
    float randomY;
    float guardingX;
    float guardingY;

    bool isInOwnHalf = true;
    bool isInPreferredZone = false;
    bool hasArrived = false;
    bool setNewLocation = false;
    bool setFoulLocation = false;
    bool isMovingTowardPreferredZone = false;
    string preferredZone;

    

    
    Vector3 direction;
    // Target position for the player to move to when is not in own half, and when he isn't aware of his best zone
    Vector3 targetPosition;


    private float currentSpeed = 4; // Current speed for the player

    //checking playmode for movement
    string playMode;
    string Half ;
    string otherHalf;






    void Start()
    {
        int prefIndex = FindPreferredZone();

       boundXL = GameObject.FindGameObjectWithTag("BoundXL");
        boundYU = GameObject.FindGameObjectWithTag("BoundYU");
        boundXR = GameObject.FindGameObjectWithTag("BoundXR");
        boundYD = GameObject.FindGameObjectWithTag("BoundYD");
        MidPoint = GameObject.FindGameObjectWithTag("MidPoint");
        foulManager = GameObject.FindGameObjectWithTag("FoulManager");
        RightMid = GameObject.FindGameObjectWithTag("RightMid");
        RightInside = GameObject.FindGameObjectWithTag("RightInside");
        LeftMid = GameObject.FindGameObjectWithTag("LeftMid");
        LeftInside = GameObject.FindGameObjectWithTag("LeftInside");

        shotPlaceL  = GameObject.FindGameObjectWithTag("ShotPlaceL");
        shotPlaceR = GameObject.FindGameObjectWithTag("ShotPlaceR");
        switch (prefIndex)
        {
            case 0:
                {
                    if (playerTag == "Player")
                    {
                        preferredZone = "LeftInside";
                        
                    }
                    else
                    {
                        preferredZone = "RightInside";
                        
                    }
                    break;
                    
                }
            case 1:
                {
                    if (playerTag == "Player")
                    {
                        preferredZone = "LeftMid";
                    }
                    else
                    {
                        preferredZone = "RightMid";
                    }
                    break;
                }
            case 2:
                {
                    if (playerTag == "Player")
                    {
                        preferredZone = "RightHalf";
                    }
                    else
                    {
                        preferredZone = "LeftHalf";
                    }
                    break;
                }
        }
   
           setNewRandoms();
       
    }

    private void Update()
    {
        if(!PlayerActions.isPicking && !foulManager.GetComponent<FoulManager>().isFouled)
        {
            setFoulLocation = false;
             MovementLogic();
        }
        else
        {
            if(foulManager.GetComponent<FoulManager>().isFouled)
            {
                FoulShotFormation();
            }
        }
       
        

     }


    public void FoulShotFormation()
    {

        if(possessionManager.CheckPossession() ==null)
        {
            if(CompareTag("Player"))
            {
                transform.position = shotPlaceL.transform.position;
            }
            else
            {
                transform.position = shotPlaceR.transform.position;
            }
        }
        else
        {
            if (!setFoulLocation)
            {
                if (possessionManager.CheckPossession().tag == "Player")
                {
                    randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);
                    randomX = Random.Range(boundXL.transform.position.x, MidPoint.transform.position.x);

                    transform.position = new Vector3(randomX, randomY, 0);
                }

                else
                {
                    randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);
                    randomX = Random.Range(boundXR.transform.position.x, MidPoint.transform.position.x);
                    transform.position = new Vector3(randomX, randomY, 0);

                }
                setFoulLocation = true;
            }
        }
    }
    public void MovementLogic()
    {
        playMode = CheckPlay();


        if (playMode == "Offense")
        {
            // Move the player to the opposite direction (left for Offense, right for Defense)

            if (transform.position != targetPosition && !isMovingTowardPreferredZone)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            }

            else
            {
                if (!isInPreferredZone)
                {

                    if (randomNumberForAwareness < player.awareness.value)
                    {
                        isMovingTowardPreferredZone = true;

                        direction = FindPreferredDirection();
                        transform.Translate(currentSpeed * Time.deltaTime * direction);


                    }
                    else
                    {

                        isInPreferredZone = true;
                        Debug.Log(gameObject.name + " " + possessionManager.CheckPossession());


                    }

                }
                else
                {
          
                    if (possessionManager.CheckPossession() == null)
                    {
                        Debug.Log(possessionManager.CheckPossession());
                
                        StartCoroutine(PlayerActions.PickAnAction());
                

                    }

                }


                // Move the player to the opposite direction (left for Offense, right for Defense)

            }

        }

        else
        {
            isMovingTowardPreferredZone = false;
            setNewLocation = false;
            isInPreferredZone = false;
            setRandomLocation();
            if (GuardedPlayer != null)
            {

                Vector3 targetPosition = GuardedPlayer.transform.position + new Vector3(guardingX, guardingY, 0.0f); // Add a small offset
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            }

        }





    }

    public void setRandomLocation()
    {
        if(playerTag =="Player")
        {
            randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);
            randomX = Random.Range(boundXL.transform.position.x, MidPoint.transform.position.x);
            targetPosition = new Vector3(randomX, randomY, 0);
        }
        else
        {
            randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);
            randomX = Random.Range(MidPoint.transform.position.x, boundXR.transform.position.x);
        }

        targetPosition = new Vector3(randomX, randomY, 0);

    }
    public void setNewRandoms()
    {
        timeToMove = Random.Range(1, 3);
        playerTag = transform.gameObject.tag;
        setRandomLocation();
        if (playerTag == "Player")
        {
            Half = "RightHalf";
            otherHalf = "LeftHalf";
            GuardedPlayrTag = "OppPlayer";
        
            guardingX = .5f;
          
          
        }
        else
        {
            Half = "LeftHalf";
            otherHalf = "RightHalf";
            GuardedPlayrTag = "Player";
       
            guardingX = -.5f;
      
          
        }

        playersToBeGuarded = GameObject.FindGameObjectsWithTag(GuardedPlayrTag);


        while (GuardedPlayer == null)
        {
            int rand = Random.Range(0, playersToBeGuarded.Length);
            if (playersToBeGuarded[rand].GetComponent<Player>().isGuarded == false)
            {
                GuardedPlayer = playersToBeGuarded[rand];
                PlayerActions.SetOtherPlayer(GuardedPlayer.GetComponent<Player>());
                GuardedPlayer.GetComponent<Player>().isGuarded = true;
            }
        }
        randomNumberForAwareness = Random.Range(1, 99);
    }

    public Vector3 FindPreferredDirection()
    {
        int prefIndex = FindPreferredZone();
        switch (prefIndex)
        {
            case 0:
                {
                    if (playerTag == "Player")
                    {
                        return (LeftInside.transform.position - transform.position).normalized;
                    }
                    else
                    {
                        return (RightInside.transform.position - transform.position).normalized;
                    }
                    

                }
            case 1:
                {
                    if (playerTag == "Player")
                    {
                        return (LeftMid.transform.position - transform.position).normalized;
                    }
                    else
                    {
                        return (RightMid.transform.position - transform.position).normalized;
                    }
                }
            case 2:
                {
                    return new Vector3(0,0,0);
                }
            default:
                return new Vector3(0, 0, 0);
        }
    }
    public int FindPreferredZone()
    {

        int maxIndex = 0;
        int maxValue = player.zoneStyle[0];

        for (int i = 1; i < player.zoneStyle.Length; i++)
        {
            if (player.zoneStyle[i] > maxValue)
            {
                maxValue = player.zoneStyle[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }
    private string CheckPlay()
    {
        
                // Call CheckPossession function from PossessionManager
                GameObject possession = possessionManager.CheckPossession();
                
                // If possession is null, return "Defense"
                if (possession == null)
                {
                    return "Offense";
                }
                // If possession is not null, return "Offense"
                else
                {
                    if(possession.tag == gameObject.tag)
                    {
                        return "Offense";
                     }
                     else
                       {
                        return "Defense";
                         }
                }
      
    }

   
      
        void OnTriggerStay2D(Collider2D other)
        {
            string otherTag = other.gameObject.tag;
            if(possessionManager.CheckPossession() == null)
            {
                if(otherTag == "RightHalf" || otherTag == "LeftHalf")
            {
                PlayerActions.SetZoneBonus(2) ;
            }
                 else if(otherTag == "RightMid" || otherTag == "LeftMid")
                 {
                    PlayerActions.SetZoneBonus(1);
            }
                 else
                 {
                PlayerActions.SetZoneBonus(0);
                    }
            }

            if (other.gameObject.CompareTag(Half))
            {

                isInOwnHalf = true;
           
            }
            else {
                   isInOwnHalf = false;
                    
              }
        if (other.gameObject.CompareTag(preferredZone))
        {
            isInPreferredZone = true;
        }


        }

}


    
