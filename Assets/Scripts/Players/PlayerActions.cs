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
    private GameObject newInstanceOfText;
    public Player player;
    private Player otherPlayer;
    private GameObject otherPlayerObject;
    private GameObject foulManager;
   public GameObject ball;

    private GameObject teamObject;
    private SoundManager    soundManager;
    private Team team;
    private Team oppTeam;
    public PossessionManager possessionManager;

    string playerTag;
    private GameObject Net;
    private int zoneBonus;
    private int otherZoneBonus;
    private int positionalBonus;
    private int accuracy;
    public int zoneIndex;
    public bool hasPicked = false;
    public bool isPicking = false;
    public bool hasJuked = false;
    Text commentary;
    void Update()
    {
        
      
    }
    private void Start()
    {
        
        newInstanceOfText = GameObject.FindGameObjectWithTag("Commentary");
        commentary = newInstanceOfText.GetComponent<Text>();
        foulManager = GameObject.FindGameObjectWithTag("FoulManager");
        TeamScore = GameObject.FindGameObjectWithTag("TeamScore");
        OppScore = GameObject.FindGameObjectWithTag("OppScore");
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
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
        if (!Input.GetKeyUp(KeyCode.Space))
        {
            // Wait until space key is pressed
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        switch (FoulCheck())
        {
            case "Major Foul":   
                
                ShowFloatingTextPrefab("Major Foul");
                soundManager.PlayeWhistle();
                newInstanceOfText.transform.Translate(new Vector3(0,0.5f,0));
                // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                otherPlayer.fouls += 1;
                player.foulShots += 2;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                
                StartCoroutine(FoulShot(3));
                
                break;
            case "Minor Foul":
                
                   
                ShowFloatingTextPrefab("Minor Foul");
                soundManager.PlayeWhistle();
                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                otherPlayer.fouls += 1;
                player.foulShots += 3;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                StartCoroutine(FoulShot(2));
                break;
            case "No Foul":
               
                   
                ShowFloatingTextPrefab("Ball Stolen");
                 // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                

                otherPlayer.steals += 1;
               // 
                decPlays();
                possessionManager.ChangePossession(otherPlayerObject);
                break;

        }
        
            // Wait until space key is pressed
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        
        commentary.text = "";

    }
    //action 2 == the player tries to shoot the ball
    public IEnumerator Shoot()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        //yield return new WaitForSeconds(waitingTime);
        if (ShotCheck())
        {
            player.isShooting = true;
            player.shots += 1;
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
                        ShowFloatingTextPrefab("Inside Shot Made!");

                        player.pointsScored += 4;
                        player.insideShotsMade += 1;
                        player.insideShots += 1;
                        otherPlayer.pointsAllowed += 4;
                        AddScore(4);
                        
                            // Wait until space key is pressed
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        
                        commentary.text = "";
                        break;
                    case 1:

                        soundManager.PlayNet();
                        ShowFloatingTextPrefab("Mid Shot Made!");
                        player.pointsScored += 5;
                        player.midShotsMade += 1;
                        player.midShots += 1;
                        otherPlayer.pointsAllowed += 5;
                        AddScore(5);
                        
                            // Wait until space key is pressed
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                        
                        commentary.text = "";
                        break;
                    case 2:

                        soundManager.PlayNet();
                        ShowFloatingTextPrefab("Outside Shot Made!");
                        player.pointsScored += 6;
                        player.outsideShotsMade += 1;
                        player.outsideShots += 1;
                        otherPlayer.pointsAllowed += 6;
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
                ShowFloatingTextPrefab("Missed Shot");
               
                    // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
               
              
               
            }


        }
        else {
            soundManager.PlayBlocked();
            ShowFloatingTextPrefab("Blocked!");
            otherPlayer.blocks += 1;
                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
            
           
        }
        commentary.text = "";
        hasPicked = true;
        
        decPlays();
        possessionManager.ChangePossession(otherPlayerObject);
    }
    //action 3 == the player tries to juke the other player
    public IEnumerator Juke()
    {
        
            // Wait until space key is pressed
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        
        hasJuked = true;
        if (JukeCheck() == "Juke")
        {
        
            ShowFloatingTextPrefab("Juke Successful");
            otherPlayer.isJuked = true;
            
                // Wait until space key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
            commentary.text = "";
            yield return  Shoot();
        }
        else
        {
            if (JukeCheck() == "Steal")
            {
                ShowFloatingTextPrefab("Ball Stolen");

               
                    // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                commentary.text = "";
                otherPlayer.steals += 1;
                
                decPlays();
                possessionManager.ChangePossession(otherPlayerObject);
            }
            else
            {
                ShowFloatingTextPrefab("Juke Failed");

                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    // Wait until space key is pressed
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                }
                yield return PickAnAction();
            }
        }

    }
        public string JukeCheck()
    {
            int playerStatSum = player.consistency.value + player.juking.value + player.shooting.value + zoneBonus + positionalBonus;
            int otherPlayerStatSum = otherPlayer.consistency.value + otherPlayer.guarding.value + otherPlayer.steal.value + otherZoneBonus;

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


        int playerStatSum = player.consistency.value + zoneBonus + player.personality*10 + otherPlayer.personality*10+ positionalBonus;
        int otherPlayerStatSum = otherPlayer.consistency.value + otherPlayer.awareness.value + otherZoneBonus;

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
                player.foulShotsMade += 2;
                player.pointsScored += 2;
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

        foulManager.GetComponent<FoulManager>().isFouled = false;

        // Wait until space key is pressed again
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        commentary.text = "";
        
        decPlays();
        possessionManager.ChangePossession(otherPlayerObject);
    }


    public void FoulShotCheck()
    {
        int offStats = player.consistency.value + player.awareness.value + player.control.value +
                        player.shooting.value;
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
        int playerStatSum = player.consistency.value +
            player.control.value +
            player.shooting.value +
            zoneBonus + positionalBonus;
        int otherPlayerStatSumw = player.consistency.value +
           player.guarding.value +
           player.pressure.value +
           otherZoneBonus;
        if(otherPlayer.isJuked)
        {
             disparity = playerStatSum;
            otherPlayer.isJuked = false;
        }
        else
        {
             disparity = playerStatSum - otherPlayerStatSumw;
            otherPlayer.isJuked = false;
        }
        

        if(disparity >= 200)
        {
            accuracy += 10;
            player.shotsTaken += 1;
            return true;
        }
        else
        {
            if( disparity >= 100)
            {
                accuracy += 5;
                player.shotsTaken += 1;
                return true;

            }
            else
            {
                if(disparity >=0)
                {
                    player.shotsTaken += 1;
                    return true;
                }
                else
                {
                    if(disparity >= -50)
                    {
                        player.shotsTaken += 1;
                        accuracy += -5;
                        return true;
                    }
                    else
                    {
                        if(disparity>= -100)
                        {
                            player.shotsTaken += 1;
                            accuracy += -10;
                            return true;
                        }
                        else
                        {
                            otherPlayer.blocks += 1;
                            player.turnovers += 1;
                            return false;
                        }
                    }
                }
            }
        }
    }

    public void PositionCheck()
    {
        int playerStatSum =100+ player.consistency.value +  zoneBonus ;
        int otherPlayerStatSum = otherPlayer.consistency.value + otherPlayer.positioning.value + otherZoneBonus;

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

   

    public void SetOtherPlayer(Player player)
    {
        otherPlayer = player;
        otherPlayerObject = otherPlayer.gameObject;
    }
    public Player GetOtherPlayer()
    {
        return otherPlayer;
    }
    public void SetZoneBonus(int zoneI)
    {
        zoneIndex = zoneI;
       switch (zoneIndex){
            case 0 :
                zoneBonus = player.inside.value;
                otherZoneBonus = otherPlayer.inside.value;
                break;
            case 1:
                zoneBonus = player.mid.value;
                otherZoneBonus = otherPlayer.mid.value;
                break;
            case 2:
                  zoneBonus = player.Outside.value;
                otherZoneBonus = otherPlayer.Outside.value;
                break;
        }
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
        if(hasJuked)
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
        Debug.Log("Done Picking");
        isPicking = false;
    }







}
