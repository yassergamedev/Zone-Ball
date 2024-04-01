using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject FloatingTextPrefab;
    public GameObject TeamScore;
    public GameObject OppScore;
    private GameObject CommentaryObject;
    public Player player;
    public PlayerPersistent playerPersistent;
    private PlayerPersistent otherPlayerPersistent;
    private Player otherPlayer;
    private GameObject otherPlayerObject;
    private GameObject foulManager;
   public GameObject ball;

    private GameObject teamObject;
    private SoundManager    soundManager;
    private Team team;
    private Team oppTeam;
    public PossessionManager possessionManager;
    private FlavourTexts flTexts;
    public NumberManager numberManager;
    string playerTag;
    private GameObject Net;
    private int zoneBonus;
    private int otherZoneBonus;
    private int positionalBonus = 0;
    private int accuracy;
    public int index;
    public int zoneIndex;
    public bool hasPicked = false;
    public bool isPicking = false;
    public bool hasJuked = false;
    public bool isGuarded = false;
    Text commentary;
    public PlayerStatsPersistent playerStatsPersistent;
    public PlayerStatsPersistent otherPlayerStats;
    private void Start()
    {
        
        CommentaryObject = GameObject.FindGameObjectWithTag("Commentary");
        commentary = CommentaryObject.GetComponent<Text>();
        foulManager = GameObject.FindGameObjectWithTag("FoulManager");
        TeamScore = GameObject.FindGameObjectWithTag("TeamScore");
        OppScore = GameObject.FindGameObjectWithTag("OppScore");
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        numberManager.SetPlayerNumber(playerPersistent.Number);
        flTexts = CommentaryObject.GetComponent<FlavourTexts>();
        commentary.text = "";

        playerTag = gameObject.tag;
        if (playerTag == "Player")
        {
            Net = GameObject.FindGameObjectWithTag("Left Net");
            teamObject = GameObject.FindGameObjectWithTag("Team");
            team = GameObject.FindGameObjectWithTag("Team").GetComponent<Team>();
        }
        else
        {
            Net = GameObject.FindGameObjectWithTag("Right Net");
            teamObject = GameObject.FindGameObjectWithTag("Opp");
            oppTeam = GameObject.FindGameObjectWithTag("Opp").GetComponent<Team>();
        }

       
    }

 

    public IEnumerator MoveBall()
    {
        while(ball.transform.position != Net.transform.position )
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, Net.transform.position, 0.1f);
          
            yield return null;


        }  
  
    }

    public void decPlays()
    {
        teamObject.GetComponent<Team>().Plays -=1;
        teamObject.GetComponent<Team>().decreasePlays();
    }

    public void AddScore(int score)
    {         if (playerTag == "Player")
        {
            team.teamScore += score;
            TeamScore.GetComponent<Text>().text = team.teamScore.ToString();
        }
        else
        {
            oppTeam.teamScore += score;
            OppScore.GetComponent<Text>().text = oppTeam.teamScore.ToString();
        }
    }
    //action 1 == the other player tries to steal the ball
    public IEnumerator Steal()
    {
     

        switch (FoulCheck())
        {
            case "Major Foul":   
                
                ShowFloatingTextPrefab(flTexts.chooseRandom("majorFoul"));
                soundManager.PlayeWhistle();
                CommentaryObject.transform.Translate(new Vector3(0,0.5f,0));
                // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.fouls += 1;
                playerStatsPersistent.foulShots += 2;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                
                yield return  FoulShot(3);
                
                break;
            case "Minor Foul":
                
                   
                ShowFloatingTextPrefab(flTexts.chooseRandom("minorFoul"));
                soundManager.PlayeWhistle();
                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.fouls += 1;
                playerStatsPersistent.foulShots += 3;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                yield return  FoulShot(2);
                break;
            case "No Foul":
               
                   
                ShowFloatingTextPrefab(flTexts.chooseRandom("stealSuccess"));
                // Wait until space key is pressed

                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                

                otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.steals += 1;
               // 
                decPlays();
                possessionManager.ChangePossession(otherPlayerObject.GetComponent<PlayerActions>().index, false);
                break;

        }

        // Wait until space key is pressed

       
        commentary.text = "";

    }
    //action 2 == the playerStatsPersistent tries to shoot the ball
    public IEnumerator Shoot()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        //yield return new WaitForSeconds(waitingTime);
        if (ShotCheck())
        {
            
            playerStatsPersistent.shots += 1;
            int RandShot = UnityEngine.Random.Range(0, 100);
            int shotAccuracy = 50;

            switch (zoneIndex)
            {
                case 0:
                    shotAccuracy = 70 + accuracy;
                    break;
                case 1:
                    shotAccuracy = 60 + accuracy;
                    break;
                case 2:
                    shotAccuracy = 50 + accuracy;
                    break;
            }

            if (RandShot <= shotAccuracy)
            {
             
                switch (zoneIndex)
                {
                    case 0:

                        soundManager.PlayNet();
                        ShowFloatingTextPrefab(flTexts.chooseRandom("insideMade"));

                        playerStatsPersistent.pointsScored += 4;
                        playerStatsPersistent.insideShotsMade += 1;
                        playerStatsPersistent.insideShots += 1;
                        otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.pointsAllowed += 4;
                        AddScore(4);
                        
                            // Wait until space key is pressed
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        
                        commentary.text = "";
                        break;
                    case 1:

                        soundManager.PlayNet();
                        ShowFloatingTextPrefab(flTexts.chooseRandom("midMade"));
                        playerStatsPersistent.pointsScored += 5;
                        playerStatsPersistent.midShotsMade += 1;
                        playerStatsPersistent.midShots += 1;
                        otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.pointsAllowed += 5;
                        AddScore(5);
                        
                            // Wait until space key is pressed
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        
                        commentary.text = "";
                        break;
                    case 2:

                        soundManager.PlayNet();
                        ShowFloatingTextPrefab(flTexts.chooseRandom("outsideMade"));
                        playerStatsPersistent.pointsScored += 6;
                        playerStatsPersistent.outsideShotsMade += 1;
                        playerStatsPersistent.outsideShots += 1;
                        otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.pointsAllowed += 6;
                        AddScore(6);
                        
                            // Wait until space key is pressed
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        
                        commentary.text = "";
                        break;
                }
               
            }
            else
            {
 
                soundManager.PlayMiss();

                switch (zoneIndex)
                {
                    case 0:
                        ShowFloatingTextPrefab(flTexts.chooseRandom("insideMiss"));
                        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        commentary.text = "";
                        break;
                    case 1:
                        ShowFloatingTextPrefab(flTexts.chooseRandom("midMiss"));
                        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        commentary.text = "";
                        break;
                    case 2:
                        ShowFloatingTextPrefab(flTexts.chooseRandom("outsideMiss"));
                        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        commentary.text = "";
                        break;
                }

               
            }


        }
        else {
            soundManager.PlayBlocked();
            ShowFloatingTextPrefab("Blocked!");
            otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.blocks += 1;
                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
            
           
        }
        commentary.text = "";
        hasPicked = true;
        
        decPlays();
        possessionManager.ChangePossession(otherPlayerObject.GetComponent<PlayerActions>().index, false);
    }
    //action 3 == the player tries to juke the other player
    public IEnumerator Juke()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        // Wait until space key is pressed

        hasJuked = true;
        if (JukeCheck() == "Juke")
        {
        
            ShowFloatingTextPrefab(flTexts.chooseRandom("jukeSuccess"));
            otherPlayerPersistent.isJuked = true;

            // Wait until space key is pressed
            
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
            commentary.text = "";
            yield return  Shoot();
        }
        else
        {
            if (JukeCheck() == "Steal")
            {
                ShowFloatingTextPrefab(flTexts.chooseRandom("stealSuccess"));

               
                    // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                commentary.text = "";
                otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.steals += 1;
                
                decPlays();
                possessionManager.ChangePossession(otherPlayerObject.GetComponent<PlayerActions>().index, false);
            }
            else
            {
                ShowFloatingTextPrefab("Juke Failed");

               
                    // Wait until space key is pressed
                   yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                yield return Shoot();
            }
        }

    }
        public string JukeCheck()
    {
        Debug.Log("other player : "+ playerPersistent.consistency.value);

            int playerStatSum = playerPersistent.consistency.value + playerPersistent.juking.value + playerPersistent.shooting.value + zoneBonus + positionalBonus + UnityEngine.Random.Range(1,100) + UnityEngine.Random.Range(1, 100);
            int otherPlayerStatSum = otherPlayerPersistent.consistency.value + otherPlayerPersistent.guarding.value + otherPlayerPersistent.steal.value + otherZoneBonus + UnityEngine.Random.Range(1, 100) + UnityEngine.Random.Range(1, 100);

            int disparity = playerStatSum - otherPlayerStatSum;
            
           if(disparity >= 100)
            {
                return "Juke";

            }
            else
            {
                if(disparity >= -99)
                {
                    return "Nothing";
                }
                else
                {
                    return "Steal";

                }
            }
    }

    


    public string FoulCheck()
    {

     
        int playerStatSum = playerPersistent.consistency.value + zoneBonus + playerPersistent.personality*10 + otherPlayerPersistent.personality*10+ positionalBonus + UnityEngine.Random.Range(1, 100);
        int otherPlayerStatSum = otherPlayerPersistent.consistency.value + otherPlayerPersistent.awareness.value + otherZoneBonus + UnityEngine.Random.Range(1, 100);

        int disparity = playerStatSum - otherPlayerStatSum;
        if(disparity >= 150)
        {
            return "Major Foul";
        }
        else
        {
            if(disparity >= 50)
            {
                return "Minor Foul";
            }
            else
            {
               
              
                return "No Foul";
            }
        }

    }

    public IEnumerator FoulShot(int times)
    {
      
        // Wait until space key is pressed
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        FoulShotCheck();
        for (int i = 0; i < times; i++)
        {
            ShowFloatingTextPrefab("Shot Number " + i);

            // Wait until space key is released
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));

            int RandShot = UnityEngine.Random.Range(0, 100);
            if (RandShot <= accuracy)
            {
                playerStatsPersistent.foulShotsMade += 2;
                playerStatsPersistent.pointsScored += 2;
                AddScore(2);
                soundManager.PlayNet();
                ShowFloatingTextPrefab("Shot Made");
            }
            else
            {
                soundManager.PlayMiss();
                ShowFloatingTextPrefab("Shot Missed");
            }

            // Wait until space key is pressed again
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        commentary.text = "";

        decPlays();
        possessionManager.ChangePossession(otherPlayerObject.GetComponent<PlayerActions>().index, true);


        foulManager.GetComponent<FoulManager>().isFouled = false;
        
        foulManager.GetComponent<FoulManager>().foulOver = true;
        // Wait until space key is pressed again


       
      
    }


    public void FoulShotCheck()
    {
        int offStats = playerPersistent.consistency.value + playerPersistent.awareness.value + playerPersistent.control.value +
                        playerPersistent.shooting.value;
        for(int i = 40, j = 0; i<400; i+=50, j+=5)
        {
            if(i == 40)
            {
                i += 10;
            }
            if(offStats >= i)
            {
                accuracy += j;
            }
            else
            {
                break;
            }
        }
    }
  
    
    public bool ShotCheck()
    {
        int disparity;
        int playerStatSum = playerPersistent.consistency.value +
            playerPersistent.control.value +
            playerPersistent.shooting.value +
            zoneBonus + positionalBonus + UnityEngine.Random.Range(1, 100);
        int otherPlayerStatSumw = otherPlayerPersistent.consistency.value +
           otherPlayerPersistent.guarding.value +
           otherPlayerPersistent.pressure.value +
           otherZoneBonus + UnityEngine.Random.Range(1, 100);
        if(otherPlayerPersistent.isJuked)
        {
             disparity = playerStatSum;
            otherPlayerPersistent.isJuked = false;
        }
        else
        {
             disparity = playerStatSum - otherPlayerStatSumw;
            otherPlayerPersistent.isJuked = false;
        }
        

        if(disparity >= 200)
        {
            accuracy += 10;
            playerStatsPersistent.shotsTaken += 1;
            return true;
        }
        else
        {
            if( disparity >= 100)
            {
                accuracy += 5;
                playerStatsPersistent.shotsTaken += 1;
                return true;

            }
            else
            {
                if(disparity >=0)
                {
                    playerStatsPersistent.shotsTaken += 1;
                    return true;
                }
                else
                {
                    if(disparity >= -50)
                    {
                        playerStatsPersistent.shotsTaken += 1;
                        accuracy += -5;
                        return true;
                    }
                    else
                    {
                        if(disparity>= -100)
                        {
                            playerStatsPersistent.shotsTaken += 1;
                            accuracy += -10;
                            return true;
                        }
                        else
                        {
                            otherPlayerObject.GetComponent<PlayerActions>().playerStatsPersistent.blocks += 1;
                            playerStatsPersistent.turnovers += 1;
                            return false;
                        }
                    }
                }
            }
        }
    }

    public void PositionCheck()
    {
        int playerStatSum =100+ playerPersistent.consistency.value +  zoneBonus + UnityEngine.Random.Range(1, 100);
        int otherPlayerStatSum = otherPlayerPersistent.consistency.value + otherPlayerPersistent.positioning.value + otherZoneBonus + UnityEngine.Random.Range(1, 100);

        int disparity = playerStatSum - otherPlayerStatSum;

        if (disparity >= 200)
        {
            positionalBonus = 25;
;
        }
        else
        {
            if (disparity >= 150)
            {
                positionalBonus = 20;
            }
            else
            {
                if(disparity >= 100)
                {
                    positionalBonus = 15;
                }
                else
                {
                    if(disparity >= 50)
                    {
                        positionalBonus = 10;
                    }
                    else
                    {
                        if(disparity >= 0)
                        {
                            positionalBonus = 5;
                        }
                        else
                        {
                            if(disparity >= -50)
                            {
                                positionalBonus = 0;
                            }
                            else
                            {
                                if(disparity >= -100)
                                {
                                    positionalBonus = -5;
                                }
                                else
                                {
                                    if(disparity >= -178)
                                    {
                                        positionalBonus = -10;
                                    }
                                
                                }
                            }
                        }
                    }
                }

            }
        }
    }

   

    public void SetOtherPlayer( PlayerActions player)
    {
        otherPlayerPersistent = player.playerPersistent;
       
        otherPlayerObject = player.gameObject;
        Debug.Log(playerPersistent.Name + " has set the other player " + otherPlayerPersistent.Name);
    }
    public Player GetOtherPlayer()
    {
        return otherPlayer;
    }
    public void SetZoneBonus(int zoneI)
    {
        zoneIndex = zoneI;
        if(otherPlayerPersistent != null)
        {
       
       
       switch (zoneIndex){
            case 0 :
                
                zoneBonus = playerPersistent.inside.value;
                otherZoneBonus = otherPlayerPersistent.inside.value;
               
                break;
            case 1:
                zoneBonus = playerPersistent.mid.value;
                otherZoneBonus = otherPlayerPersistent.mid.value;
                break;
            case 2:
                  zoneBonus = playerPersistent.Outside.value;
                otherZoneBonus = otherPlayerPersistent.Outside.value;
                break;
        } }
    }

    public void ShowFloatingTextPrefab(string text)
    {
       
        commentary.text =  text;
        
    }

   
    public void StartSteal()
    {
        StartCoroutine(Steal());
    }
    public IEnumerator PickAnAction()
    {
        PlayerMovement[] playerMovements = GameObject.FindObjectsOfType<PlayerMovement>();

        gameObject.GetComponent<PlayerMovement>().possessionHold = true;
        if (hasJuked)
        {
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
            hasJuked = false;
        }
     
        isPicking = true;
        int action = UnityEngine.Random.Range(1, 4);
        Debug.Log(action);
        commentary.text = "";
        switch (action)
        {
            case 1:
                ShowFloatingTextPrefab("Guarding Player going To Steal");
                // Wait until space key is pressed
                // Check if space key is not pressed

                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                // Now that space key is pressed, start the coroutine
                Debug.Log("Starting Coroutine");
                yield return Steal();


                break;
            case 2:
                ShowFloatingTextPrefab("Going To Shoot");
                // Wait until space key is pressed
               
                    // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                yield return Shoot();
                break;
            case 3:
                ShowFloatingTextPrefab("Going To Juke");
                // Wait until space key is pressed

                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                yield return Juke();
                break;
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        Debug.Log("Done Picking");
        foreach (PlayerMovement playerMovement in playerMovements)
        {
            playerMovement.possessionHold = false;
        }
        isPicking = false;

        

    }







}
