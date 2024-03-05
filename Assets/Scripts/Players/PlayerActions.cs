using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    // Start is called before the first frame update
    public ActionTextManager actionTextManager;
    public Player player;
    private Player otherPlayer;
    private GameObject otherPlayerObject;
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
    private bool freeBall = true;
    void Update()
    {
       
    }
    private void Start()
    {
        
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

    public IEnumerable MoveBall()
    {
        while(ball.transform.position != Net.transform.position )
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, Net.transform.position, 0.1f);
            freeBall = false;
            yield return null;


        }  
           freeBall = true;   
    }
    //action 1 == the other player tries to steal the ball
    public void Steal()
    {
        switch (FoulCheck())
        {
            case "Major Foul":
                actionTextManager.ShowActionText("Major Foul");
                otherPlayer.fouls += 1;
                player.foulShots += 2;
                FoulShot(3);
                
                break;
            case "Minor Foul":
                actionTextManager.ShowActionText("Minor Foul");
                otherPlayer.fouls += 1;
                player.foulShots += 3;
                FoulShot(2);
                break;
            case "No Foul":
                actionTextManager.ShowActionText("Ball Stolen");
                otherPlayer.steals += 1;
                possessionManager.ChangePossession(otherPlayerObject);
                break;

        }
    }
    //action 2 == the player tries to shoot the ball
    public void Shoot()
    {
        if(ShotCheck())
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
                        actionTextManager.ShowActionText("Inside Shot Made");
                        player.pointsScored += 4;
                        player.insideShotsMade += 1;
                        player.insideShots += 1;
                        otherPlayer.pointsAllowed += 4;
                        team.teamScore += 4;
                        break;
                    case 1:
                        Debug.Log("mid Shot Made");
                        player.pointsScored += 5;
                        player.midShotsMade += 1;
                        player.midShots += 1;
                        otherPlayer.pointsAllowed += 5;
                        team.teamScore += 5;
                        break;
                    case 2:
                        Debug.Log("Outside Shot Made");
                        player.pointsScored += 6;
                        player.outsideShotsMade += 1;
                        player.outsideShots += 1;
                        otherPlayer.pointsAllowed += 6;
                        team.teamScore += 6;
                        break;
                }
            }
            else
            {
                Debug.Log("Missed");
               
                    possessionManager.ChangePossession(otherPlayerObject);
                
               
            }


        }
        else { 

            Debug.Log("Blocked");
            possessionManager.ChangePossession(otherPlayerObject);
        }
        
    }
    //action 3 == the player tries to juke the other player
    public void Juke()
    {
        
        if(JukeCheck() == "Juke")
        {
            otherPlayer.isJuked = true;
            Shoot();
        }else if(JukeCheck() == "Steal")
        {
            Debug.Log("Ball Stolen while tring to juke");
            otherPlayer.steals += 1;
            possessionManager.ChangePossession(otherPlayerObject);
        }
        else
        {
            Debug.Log("Nothing");
            PickAnAction();
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
                possessionManager.ChangePossession(otherPlayerObject);
                Debug.Log("Steal");
                return "No Foul";
            }
        }

    }
   
    public void FoulShot(int times)
    {
        FoulShotCheck();
        for(int i = 0; i<times; i++)
        {
            int RandShot = UnityEngine.Random.Range(0, 100);
            if(RandShot <= accuracy)
            {
                player.foulShotsMade += 2;
                player.pointsScored += 2;
                team.teamScore += 2;
                
            }
            else
            {
                Debug.Log("Missed");
            }
        }
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

    public void PickAnAction()
    {
        actionTextManager.ShowActionText("Minor Foul");
        int action = UnityEngine.Random.Range(1, 4);
        Debug.Log(action);
        switch (action)
        {
            case 1:
                Debug.Log("Going to steal");
                Steal();
                break;
            case 2:
                Debug.Log("Going to shoot");
                Shoot();
                break;
            case 3:
                Debug.Log("Going to juke");
                Juke();
                break;
        }
    }

    
    
}
