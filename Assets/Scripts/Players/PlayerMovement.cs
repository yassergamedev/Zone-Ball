using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    public Player player;
    private string playerTag;

    //to simulate players moving in different locations
    public float timeToMove = 2f;
    public float moveTimer = 0;
    //to decide whether to go upfront or just in a random location
    float randomNumber;

    bool isInOwnHalf = true;
    bool isInPreferredZone = false;

    string[] preferredZones;
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

        timeToMove = Random.Range(1, 3);
        playerTag = transform.gameObject.tag;

        if (playerTag == "Player")
        {
            Half = "RightHalf";
            otherHalf = "LeftHalf";
            horizontalMovement = Random.Range(-100, 0);
            verticalMovement = Random.Range(-100, 100);
        }
        else{
            Half = "LeftHalf";
            otherHalf = "RightHalf";
            horizontalMovement = Random.Range(0, 100);
            verticalMovement = Random.Range(-100, 100);
        }
        StartCoroutine(UpdateSpeed());
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
                 
                        // Create a vector for movement direction
                        direction = new Vector3(horizontalMovement, verticalMovement, 0f).normalized;

                        // Move the player to the opposite direction (left for Offense, right for Defense)
                        transform.Translate(currentSpeed * Time.deltaTime * direction);
                

                }
            }

            else
            {

                if (isInOwnHalf)
                {

                  
                        // Create a vector for movement direction
                        direction = new Vector3(horizontalMovement, verticalMovement, 0f).normalized;

                        // Move the player to the opposite direction (left for Offense, right for Defense)
                        transform.Translate(direction * currentSpeed * Time.deltaTime);
               
                }

                // If the player is not in his own half
                else
                {
                   

                        // Create a vector for movement direction
                        direction = new Vector3(horizontalMovement, verticalMovement, 0f).normalized;

                        // Move the player to the opposite direction (left for Offense, right for Defense)
                        transform.Translate(direction * currentSpeed * Time.deltaTime);
                   
                }
            }
        } 
        else
        {
            timeToMove = Random.Range(1, 3);
            moveTimer = 0;
            Debug.Log(gameObject.name + " time : "+ timeToMove);
            if (playerTag == "Player")
            {


                horizontalMovement = Random.Range(-100, 0);
                verticalMovement = Random.Range(-100, 100);

                Debug.Log(gameObject.name + " horizontalMovement : " + horizontalMovement);
                Debug.Log(gameObject.name + " verticalMovement : " + verticalMovement);
            }
            else
            {

                horizontalMovement = Random.Range(0, 100);
                verticalMovement = Random.Range(-100, 100);
                Debug.Log(gameObject.name + " horizontalMovement : " + horizontalMovement);
                Debug.Log(gameObject.name + " verticalMovement : " + verticalMovement);
            }

        }

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
                    
                     }
                

            }
       
        }

    }
