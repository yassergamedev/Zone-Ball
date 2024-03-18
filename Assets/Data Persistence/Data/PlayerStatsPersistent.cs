using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerStatsPersistent : MonoBehaviour
{

    
    //game flow stats

    //---------- defensive ------------

    public int pressures = 0;
    public int blocks = 0;
    public int steals = 0;
    public int fouls = 0;
    public int pointsAllowed = 0;

    //---------- offensive ------------

    public int jukes = 0;
    //public int shot percentage in db
    public int shots = 0;
    public int shotsTaken = 0;
    public int pointsScored = 0;
    public int foulShots = 0;
    public int foulShotsMade = 0;
    public int foulPointsScored = 0;
    public int turnovers = 0;

    public int plays = 0;
    public int maxPlays;

    //---------- zone ------------
    public int insideShots = 0;
    public int insideShotsMade = 0;
    public int midShots = 0;
    public int midShotsMade = 0;
    public int outsideShots = 0;
    public int outsideShotsMade = 0;

    // Constructor
    public PlayerStatsPersistent(int pressures, int blocks, int steals, int fouls, int pointsAllowed, int jukes, int shots, int shotsTaken, int pointsScored, int foulShots, int foulShotsMade, int foulPointsScored, int turnovers, int plays, int maxPlays, int insideShots, int insideShotsMade, int midShots, int midShotsMade, int outsideShots, int outsideShotsMade)
    {
        this.pressures = pressures;
        this.blocks = blocks;
        this.steals = steals;
        this.fouls = fouls;
        this.pointsAllowed = pointsAllowed;
        this.jukes = jukes;
        this.shots = shots;
        this.shotsTaken = shotsTaken;
        this.pointsScored = pointsScored;
        this.foulShots = foulShots;
        this.foulShotsMade = foulShotsMade;
        this.foulPointsScored = foulPointsScored;
        this.turnovers = turnovers;
        this.plays = plays;
        this.maxPlays = maxPlays;
        this.insideShots = insideShots;
        this.insideShotsMade = insideShotsMade;
        this.midShots = midShots;
        this.midShotsMade = midShotsMade;
        this.outsideShots = outsideShots;
        this.outsideShotsMade = outsideShotsMade;
    }
   
}






