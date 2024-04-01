
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PossessionManager possessionManager;
    public PlayerPersistent playerPersistent;
    public Player player;
    public PlayerActions PlayerActions;


    private GameObject RightMid;
    private GameObject RightInside;
    private GameObject LeftMid;
    private GameObject LeftInside;
    public GameObject GuardedPlayer;
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
   int dontMoveLong = 0;
    
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
    public bool ballHolderGuard = false;
    public bool possessionHold = false;
    string preferredZone;
    public bool foulOver = false;
    int lol = 0;

    
    Vector3 direction;
    // Target position for the player to move to when is not in own half, and when he isn't aware of his best zone
    Vector3 targetPosition;


    private float currentSpeed = 4; // Current speed for the player

    //checking playmode for movement
    string playMode;
    string Half ;
    string otherHalf;



    public bool fouled = false;


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
       // StartCoroutine(RegUpdate());
    }

    private void Update()
    {
        if (!PlayerActions.isPicking && !foulManager.GetComponent<FoulManager>().isFouled)
        {
            setFoulLocation = false;
            if (!possessionHold)
            {
                StartCoroutine(MovementLogic());
            }else
            {
                Debug.Log(gameObject.name + " still holding");
            }

        }
        else
        {
            if (foulManager.GetComponent<FoulManager>().isFouled)
            {
                StopCoroutine(MovementLogic());
                FoulShotFormation();


            }
        }
    }
    private IEnumerator RegUpdate()
    {
        lol++;
        Debug.Log("lol value: " + lol);
        fouled = false;
        while (true)
        {

        
        if(!PlayerActions.isPicking && !foulManager.GetComponent<FoulManager>().isFouled)
        {
            setFoulLocation = false;
            if(!possessionHold)
            {
                yield return MovementLogic();
            }
            
        }
        else
        {
            if(foulManager.GetComponent<FoulManager>().isFouled)
            {
                FoulShotFormation();
                    
                    break;
                }
              
        }

        Debug.Log(gameObject.name + " still updating");
        }
    }

    public bool RangeCheck()
    {
        if (transform.position.x <= boundXL.transform.position.x
            || transform.position.x >= boundXR.transform.position.x
            || transform.position.y >= boundYU.transform.position.y
            || transform.position.y >= boundYD.transform.position.y)
            return false;
        else
            return true;
    }


    public void FoulShotFormation()
    {
        GameObject ballPossessor = possessionManager.CheckPossession();

        float distance = 1f;
        if (ballPossessor ==null)
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
                if (ballPossessor.CompareTag("Player"))
                {
                    // Calculate random Y position within the vertical bounds
                    float randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);

                    

                    // Calculate the X position to maintain the specified distance
                    float randomX = shotPlaceL.transform.position.x + distance;

                    // Set the position of the player
                    transform.position = new Vector3(randomX, randomY, 0);
                }

                else
                {
                    // Calculate random Y position within the vertical bounds
                    float randomY = Random.Range(boundYD.transform.position.y, boundYU.transform.position.y);



                    // Calculate the X position to maintain the specified distance
                    float randomX = shotPlaceR.transform.position.x - distance;

                    // Set the position of the player
                    transform.position = new Vector3(randomX, randomY, 0);

                }
                setFoulLocation = true;
            }
        }
    }
    public IEnumerator MovementLogic()
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
                if (!isInPreferredZone && RangeCheck())
                {
                   
                    if (randomNumberForAwareness < playerPersistent.awareness.value)
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
                        Debug.Log("Picked an action");
                
                        yield return PlayerActions.PickAnAction();
                        Debug.Log("didn't pause");

                    }else
                    {
                        possessionHold = true;
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
            if (transform.position == targetPosition)
            {
                possessionHold = true;
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
        
            guardingX = .3f;
          
          
        }
        else
        {
            Half = "LeftHalf";
            otherHalf = "RightHalf";
            GuardedPlayrTag = "Player";
       
            guardingX = -.3f;
      
          
        }

        playersToBeGuarded = GameObject.FindGameObjectsWithTag(GuardedPlayrTag);


        int guardIndex = Random.Range(1, 9);

        if (ballHolderGuard)
        {
            while(GuardedPlayer == null)
            {
            foreach (GameObject player in playersToBeGuarded)
            {
                if (player.transform.childCount == 3)
                {
                        Debug.Log(this.transform.name + " has set the guarding player");
                    GuardedPlayer = player;
                    PlayerActions.SetOtherPlayer(GuardedPlayer.GetComponent<PlayerActions>());
                    GuardedPlayer.GetComponent<PlayerActions>().isGuarded = true;
                }
            }

            }
            ballHolderGuard = false;
        }
        else
        {


            StartCoroutine(AssignGuardCoroutine());
        }
        randomNumberForAwareness = Random.Range(1, 99);
    }
    private IEnumerator AssignGuardCoroutine()
    {
        // Repeat indefinitely
        while (true)
        {
            // Check each player
            foreach (GameObject player in playersToBeGuarded)
            {
                // Skip this player if it's already guarded or has max children

                if (!player.GetComponent<PlayerActions>().isGuarded)
                {
                    // Set this player as guarded
                    GuardedPlayer = player;
                    player.GetComponent<PlayerActions>().isGuarded = true;
                    PlayerActions.SetOtherPlayer(player.GetComponent<PlayerActions>());
                    Debug.Log(gameObject.name + " is guarding " + player.gameObject.name);
                    break;
                }

                // Wait for a short duration before checking again
                yield return new WaitForSeconds(0.1f);
            }

            // Wait for the next iteration
            yield return null;
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
        int maxValue = playerPersistent.zoneStyle[0];

        for (int i = 1; i < playerPersistent.zoneStyle.Length; i++)
        {
            if (playerPersistent.zoneStyle[i] > maxValue)
            {
                maxValue = playerPersistent.zoneStyle[i];
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
    void OnTriggerEnter2D(Collider2D other)
    {
       
            if (other.CompareTag( "RightHalf") || other.CompareTag("LeftHalf"))
            {

                Debug.Log("In Outside");
                PlayerActions.SetZoneBonus(2);
                PlayerActions.zoneIndex = 2;
            }
             if (other.CompareTag("RightMid") || other.CompareTag("LeftMid"))
           {

                Debug.Log("In Mid");
                PlayerActions.SetZoneBonus(1);
                PlayerActions.zoneIndex = 1;
            }
              if (other.CompareTag("RightInside") || other.CompareTag("LeftInside"))
               {

                Debug.Log("In Inside");
                PlayerActions.SetZoneBonus(0);
                PlayerActions.zoneIndex = 0;
            }
        

        if (other.gameObject.CompareTag(Half))
        {

            isInOwnHalf = true;

        }
        else
        {
            isInOwnHalf = false;

        }
        if (other.gameObject.CompareTag(preferredZone))
        {
            isInPreferredZone = true;
        }


    }

}


    
