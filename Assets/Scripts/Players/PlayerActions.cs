using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerActions : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject FloatingTextPrefab;
    private GameObject newInstanceOfText;
    public Player player;
    private Player otherPlayer;
    private GameObject otherPlayerObject;
    private GameObject foulManager;
   public GameObject ball;


    private Team team;
    public PossessionManager possessionManager;

    string playerTag;
    private GameObject Net;
    private int zoneBonus;
    private int otherZoneBonus;
    private int positionalBonus;
    private int accuracy;
    private int zoneIndex;
    public bool hasPicked = false;
    public bool isPicking = false;
    private float timeToPick = 2.0f;
    private float timeMoving = 0f;
    private bool showedText = false;
    private float waitingTime = 0.75f;
    void Update()
    {
        
      
    }
    private void Start()
    {
        
        newInstanceOfText = GameObject.FindGameObjectWithTag("Commentary");
        foulManager = GameObject.FindGameObjectWithTag("FoulManager");
        newInstanceOfText.GetComponent<TextMesh>().text = "";

        playerTag = gameObject.tag;
        if (playerTag == "Player")
        {
            Net = GameObject.FindGameObjectWithTag("Left Net");
            team = GameObject.FindGameObjectWithTag("Team").GetComponent<Team>();
        }
        else
        {
            Net = GameObject.FindGameObjectWithTag("Right Net");
            team = GameObject.FindGameObjectWithTag("Opp").GetComponent<Team>();
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
    //action 1 == the other player tries to steal the ball
    public IEnumerator Steal()
    {
       // newInstanceOfText.GetComponent<TextMesh>().text = "Calculating Foul";

        switch (FoulCheck())
        {
            case "Major Foul":   
                
                ShowFloatingTextPrefab("Major Foul");
                newInstanceOfText.transform.Translate(new Vector3(0,0.5f,0));
                yield return new WaitForSeconds(waitingTime);

                otherPlayer.fouls += 1;
                player.foulShots += 2;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                StartCoroutine(FoulShot(3));
                
                break;
            case "Minor Foul":
                
                   
                ShowFloatingTextPrefab("Minor Foul");
                yield return new WaitForSeconds(waitingTime);
                otherPlayer.fouls += 1;
                player.foulShots += 3;
                foulManager.GetComponent<FoulManager>().isFouled = true;
                StartCoroutine(FoulShot(2));
                break;
            case "No Foul":
               
                   
                ShowFloatingTextPrefab("Ball Stolen");
                yield return new WaitForSeconds(waitingTime);
                
                otherPlayer.steals += 1;
                possessionManager.ChangePossession(otherPlayerObject);
                break;

        }
        yield return new WaitForSeconds(waitingTime);
        newInstanceOfText.GetComponent<TextMesh>().text = "";

    }
    //action 2 == the player tries to shoot the ball
    public IEnumerator Shoot()
    {
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
                        
                           
                        ShowFloatingTextPrefab("Inside Shot Made!");

                        player.pointsScored += 4;
                        player.insideShotsMade += 1;
                        player.insideShots += 1;
                        otherPlayer.pointsAllowed += 4;
                        team.teamScore += 4;
                        yield return new WaitForSeconds(waitingTime);
                        newInstanceOfText.GetComponent<TextMesh>().text = "";
                        break;
                    case 1:
                        
                           
                        ShowFloatingTextPrefab("Mid Shot Made!");
                        player.pointsScored += 5;
                        player.midShotsMade += 1;
                        player.midShots += 1;
                        otherPlayer.pointsAllowed += 5;
                        team.teamScore += 5;
                        yield return new WaitForSeconds(waitingTime);
                        newInstanceOfText.GetComponent<TextMesh>().text = "";
                        break;
                    case 2:
                       
                         
                        ShowFloatingTextPrefab("Outside Shot Made!");
                        player.pointsScored += 6;
                        player.outsideShotsMade += 1;
                        player.outsideShots += 1;
                        otherPlayer.pointsAllowed += 6;
                        team.teamScore += 6;
                        yield return new WaitForSeconds(waitingTime);
                        newInstanceOfText.GetComponent<TextMesh>().text = "";
                        break;
                }
            }
            else
            {
                
                    
                ShowFloatingTextPrefab("Missed Shot");
                yield return new WaitForSeconds(waitingTime);
                newInstanceOfText.GetComponent<TextMesh>().text = "";
                possessionManager.ChangePossession(otherPlayerObject);
                
               
            }


        }
        else {

            
              
            ShowFloatingTextPrefab("Blocked!");
            yield return new WaitForSeconds(waitingTime);
            newInstanceOfText.GetComponent<TextMesh>().text = "";
            possessionManager.ChangePossession(otherPlayerObject);
        }
        hasPicked = true;
       
    }
    //action 3 == the player tries to juke the other player
    public IEnumerator Juke()
    {

        if (JukeCheck() == "Juke")
        {
        
            ShowFloatingTextPrefab("Juke Successful");
            otherPlayer.isJuked = true;
            yield return new WaitForSeconds(waitingTime);
            newInstanceOfText.GetComponent<TextMesh>().text = "";
            StartCoroutine(Shoot());
        }
        else
        {
            if (JukeCheck() == "Steal")
            {
                ShowFloatingTextPrefab("Ball Stolen");
                
                yield return new WaitForSeconds(waitingTime);
                newInstanceOfText.GetComponent<TextMesh>().text = "";
                otherPlayer.steals += 1;
                possessionManager.ChangePossession(otherPlayerObject);
            }
            else
            {
                ShowFloatingTextPrefab("Juke Failed");
         
                yield return new WaitForSeconds(waitingTime);
                StartCoroutine(PickAnAction());
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
       
        FoulShotCheck();
        for(int i = 0; i<times; i++)
        {
            yield return new WaitForSeconds(waitingTime);
            int RandShot = UnityEngine.Random.Range(0, 100);
            if(RandShot <= accuracy)
            {
                player.foulShotsMade += 2;
                player.pointsScored += 2;
                team.teamScore += 2;
                ShowFloatingTextPrefab("Shot Made");
        
            }
            else
            {
                ShowFloatingTextPrefab("Shot Missed");
            
            }
        }
        foulManager.GetComponent<FoulManager>().isFouled = false;
        yield return new WaitForSeconds(waitingTime);
        newInstanceOfText.GetComponent<TextMesh>().text = "";
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
        int disparity = 0;
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
       
        newInstanceOfText.GetComponent<TextMesh>().text =  text;
        
    }

  

    public IEnumerator PickAnAction()
    {
        isPicking = true;
        int action = UnityEngine.Random.Range(1, 4);
        Debug.Log(action);
        newInstanceOfText.GetComponent<TextMesh>().text = "";
        switch (action)
        {
            case 1:
               
                
              
               ShowFloatingTextPrefab("Guarding Player going To Steal");
                yield return new WaitForSeconds(waitingTime);
                StartCoroutine(Steal());
                
                break;
            case 2:


                ShowFloatingTextPrefab("Going To Shoot");
                
                yield return new WaitForSeconds(waitingTime);
                StartCoroutine(Shoot());
             
                break;
            case 3:
                

           
                
              
                ShowFloatingTextPrefab("Going To Juke");
                yield return new WaitForSeconds(waitingTime);
                StartCoroutine(Juke());
                break;
        }
        isPicking = false;

    }






}
