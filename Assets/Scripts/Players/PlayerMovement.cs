using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    string playerTag = gameObject.tag;
    bool isInOwnHalf = false;
    //true == left
    //false == right
    private float movementDirection = 0f;
    string Half ;


    void Start()
    {
        if(playerTag == "Player")
        {
            Half = "RightHalf";
          
        }
        else{
            Half = "LeftHalf";
        
        }
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

    private void MovePlayer()
    {
        // Check the play mode (Offense or Defense)
        string playMode = CheckPlay();

       
    if(playMode == "Offense")
    {

    
        // If the player is in his own half
        if (isInOwnHalf)
        {
            // Generate a random number between 0 and 99
            int randomNumber = Random.Range(0, 100);

            // If the random number is less than or equal to 90 (90% chance)
            if (randomNumber <= 90)
            {
                // Move the player to the opposite direction (left for Offense, right for Defense)
                transform.Translate(Vector3.left * Time.deltaTime);
            }
            // Otherwise (10% chance)
            else
            {
                // Move the player randomly
                float randomX = Random.Range(-1f, 1f);
                float randomY = Random.Range(-1f, 1f);
                Vector3 randomDirection = new Vector3(randomX, randomY, 0f);
                transform.Translate(randomDirection * Time.deltaTime);
            }
        }
        
        // If the player is not in his own half
        else
        {
            // Move the player to the opposite direction (right for Offense, left for Defense)
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
    }

     void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.compareTag(Half))
        {
            isInOwnHalf = true;
        }
        else{
            isInOwnHalf = false;
        }
    }

}
