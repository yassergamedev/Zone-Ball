using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    public Player player;
    private GameObject RightMid;
    private GameObject RightInside;
    private GameObject LeftMid;
    private GameObject LeftInside;
    private GameObject GuardedPlayer;
    private GameObject boundX;
    private GameObject boundY;
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
    string preferredZone;
    
    

    
    Vector3 direction;
    

    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private float currentSpeed; // Current speed for the player

    //checking playmode for movement
    string playMode;
    string Half ;
    string otherHalf;


    void Start()
    {
        int prefIndex = FindPreferredZone();

       boundX = GameObject.FindGameObjectWithTag("BoundX");
        boundY = GameObject.FindGameObjectWithTag("BoundY");
        RightMid = GameObject.FindGameObjectWithTag("RightMid");
        RightInside = GameObject.FindGameObjectWithTag("RightInside");
        LeftMid = GameObject.FindGameObjectWithTag("LeftMid");
        LeftInside = GameObject.FindGameObjectWithTag("LeftInside");

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
        timeToMove = Random.Range(1, 3);
        playerTag = transform.gameObject.tag;
        
        if (playerTag == "Player")
        {
            Half = "RightHalf";
            otherHalf = "LeftHalf";
            GuardedPlayrTag = "OppPlayer";
            horizontalMovement = Random.Range(-100, 0);
            verticalMovement = Random.Range(-100, 100);
            guardingX = Random.Range(1,1.5f );
            guardingY = Random.Range(0,.5f );
            randomY = Random.Range(-boundY.transform.position.y, boundY.transform.position.y);
            randomX = Random.Range(-boundX.transform.position.x, 0);
        }
        else{
            Half = "LeftHalf";
            otherHalf = "RightHalf";
            GuardedPlayrTag = "Player";
            horizontalMovement = Random.Range(0, 100);
            verticalMovement = Random.Range(-100, 100);
            guardingX = Random.Range(-1.5f, -1);
            guardingY = Random.Range(-.5f, 0) ;
            randomY = Random.Range(-boundY.transform.position.y, boundY.transform.position.y);
            randomX = Random.Range(0, boundX.transform.position.x);
        }

        playersToBeGuarded = GameObject.FindGameObjectsWithTag(GuardedPlayrTag);
        Debug.Log(playersToBeGuarded.Length);
        


        while (GuardedPlayer == null)
        {
            int rand = Random.Range(0, playersToBeGuarded.Length);
            if (playersToBeGuarded[rand].GetComponent<Player>().isGuarded ==false)
            {
                GuardedPlayer = playersToBeGuarded[rand];
                GuardedPlayer.GetComponent<Player>().isGuarded = true;
            }
        }
        StartCoroutine(UpdateSpeed());


        randomNumberForAwareness = Random.Range(1, 99);

       
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer < timeToMove)
        {
            playMode = CheckPlay();
            if (playMode == "Offense")
            {

                // If the player is in his own half
                if (isInOwnHalf)
                {
                        // Create a vector for movement direction
                        direction = new Vector3(horizontalMovement, verticalMovement, 0f).normalized;

                        // Move the player to the opposite direction (left for Offense, right for Defense)
                        transform.Translate(currentSpeed * Time.deltaTime * direction);
                }

                // If the player is not in his own half
                else
                {

                    if(!isInPreferredZone)
                    {
                        
                        if(randomNumberForAwareness < player.awareness.value)
                        {
                            direction = FindPreferredDirection();
                            
                            transform.Translate(currentSpeed * Time.deltaTime * direction);
                            

                        }
                        else
                        {
                            //random target position

                            Vector3 targetPosition =  new Vector3(randomX, randomY, 0.0f);
                            Debug.Log(gameObject.name + " targetPosition : " + targetPosition);
                            // rancom movement and distance
                            if(transform.position != targetPosition)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
                            }
                            else
                            {
                                isInPreferredZone  = true;
                            }
                           
                        }
                        
                    }
                  
                      
                        // Move the player to the opposite direction (left for Offense, right for Defense)
                        

                }
            }

            else
            {



                if (GuardedPlayer != null)
                {
                    
                    Vector3 targetPosition = GuardedPlayer.transform.position + new Vector3(guardingX, guardingY, 0.0f); // Add a small offset
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
                }

            }
        } 
        else
        {
            timeToMove = Random.Range(1, 3);
            moveTimer = 0;
            //Debug.Log(gameObject.name + " time : "+ timeToMove);
            if (playerTag == "Player")
            {


                horizontalMovement = Random.Range(-100, 0);
                verticalMovement = Random.Range(-100, 100);

                //Debug.Log(gameObject.name + " horizontalMovement : " + horizontalMovement);
                //Debug.Log(gameObject.name + " verticalMovement : " + verticalMovement);
            }
            else
            {

                horizontalMovement = Random.Range(0, 100);
                verticalMovement = Random.Range(-100, 100);
                //Debug.Log(gameObject.name + " horizontalMovement : " + horizontalMovement);
                //Debug.Log(gameObject.name + " verticalMovement : " + verticalMovement);
            }

        }

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
            default:
                {
                    return new Vector3(0,0,0);
                }
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

   
        private IEnumerator UpdateSpeed()
        {
            while (true)
            {
                // Wait for 1 to 3 seconds
                yield return new WaitForSeconds(Random.Range(1f, 3f));

                // Update current speed to a new random value within the specified range
                currentSpeed = Random.Range(2, 5);
            }
        }
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Half))
            {

                isInOwnHalf = true;
           
            }
            else {
                 if(other.gameObject.CompareTag(otherHalf))
                 {
                     isInOwnHalf = false;
                    
                 }
                  else
                  {
                        if (other.gameObject.CompareTag(preferredZone))
                        {
                            isInPreferredZone = true;
                        }
                        
                     
                   }
                
            }
       
        }


    }
