using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    public Player player;

    private string playerTag;

    bool isInOwnHalf = true;
    //true == left
    //false == right
    private float movementDirection = 0f;
    private float horizontalMovement = 0f;
    private float currentSpeed; // Current speed for the player

    string Half ;


    void Start()
    {
        playerTag = transform.gameObject.tag;
        if (playerTag == "Player")
        {
            Half = "RightHalf";
            
          
        }
        else{
            Half = "LeftHalf";
            
        }
        StartCoroutine(UpdateSpeed());
    }
    private string CheckPlay()
    {
        
                // Call CheckPossession function from PossessionManager
                GameObject possession = possessionManager.CheckPossession();
                
                // If possession is null, return "Defense"
                if (possession == null)
                {
                    return "Defense";
                }
                // If possession is not null, return "Offense"
                else
                {
                    return "Offense";
                }
        // Return an empty string if the gameObject's tag is not Player or OppPlayer
        return "";
    }

    public void MovePlayer()
    {
        // Check the play mode (Offense or Defense)
        string playMode = CheckPlay();


        Vector3 direction;

        if (playMode == "Offense")
        {
           
            // If the player is in his own half
            if (isInOwnHalf)
        {
                
               
                // Generate a random number between 0 and 99
                int randomNumber = Random.Range(0, 100);

            // If the random number is less than or equal to 90 (90% chance)
            if (randomNumber <= 90)
            {
                    horizontalMovement = gameObject.tag == "Player" ? -1f : 1f;

                    // Create a vector for movement direction
                    direction = new Vector3(horizontalMovement, 0, 0f).normalized;

                    // Move the player to the opposite direction (left for Offense, right for Defense)
                    transform.Translate(direction * currentSpeed * Time.deltaTime);
                }
            // Otherwise (10% chance)
            else
            {
                // Move the player randomly
                float randomX = Random.Range(-1f, 1f);
                float randomY = Random.Range(-1f, 1f);
                Vector3 randomDirection = new Vector3(randomX, randomY, 0f);
                transform.Translate(randomDirection *currentSpeed* Time.deltaTime);
            }
        }
        
        // If the player is not in his own half
         else
          {
            // do nothing for now
           
          }

          }
        else
        {
            
            if (isInOwnHalf)
            {
               
                // Generate a random number between 0 and 99
                int randomNumber = Random.Range(0, 100);
               
                // If the random number is less than or equal to 90 (90% chance)
                if (randomNumber <= 90)
                {
                    horizontalMovement = gameObject.tag == "Player" ? -1f : 1f;

                    // Create a vector for movement direction
                    direction = new Vector3(horizontalMovement, 0, 0f).normalized;
                   
                    // Move the player to the opposite direction (left for Offense, right for Defense)
                    transform.Translate(direction *currentSpeed* Time.deltaTime);
                }
                // Otherwise (10% chance)
                else
                {
                    Debug.Log("I just hit the 10 percent");
                    // Move the player randomly
                    float randomX = Random.Range(-1f, 1f);
                    float randomY = Random.Range(-1f, 1f);
                    Vector3 randomDirection = new Vector3(randomX, randomY, 0f);
                    transform.Translate(randomDirection *currentSpeed* Time.deltaTime);
                }
            }

            // If the player is not in his own half
            else
            {
                // do nothing for now

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
        Debug.Log(gameObject.transform.name + " " + other.tag);

        if (other.gameObject.CompareTag(Half))
        {
      
            isInOwnHalf = true;
        }
        else{
            isInOwnHalf = false;
           
        }
    }

}
