using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerStatsPersistent
{


    //game flow stats
    string id;
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

   // public int plays = 0;
    public int maxPlays;

    //---------- zone ------------
    public int insideShots = 0;
    public int insideShotsMade = 0;
    public int midShots = 0;
    public int midShotsMade = 0;
    public int outsideShots = 0;
    public int outsideShotsMade = 0;
    public List<(string, Func<int>)> getStats()
    {

       //return the list of stats
       return new List<(string, Func<int>)> { ("pressures", () => pressures), ("blocks", () => blocks), ("steals", () => steals), ("fouls", () => fouls), ("pointsAllowed", () => pointsAllowed),
                  ("jukes", () => jukes), ("shots", () => shots), ("shotsTaken", () => shotsTaken), ("pointsScored", () => pointsScored), ("foulShots", () => foulShots), ("foulShotsMade", () => foulShotsMade),
                  ("foulPointsScored", () => foulPointsScored), ("turnovers", () => turnovers), ("insideShots", () => insideShots), ("insideShotsMade", () => insideShotsMade),
                  ("midShots", () => midShots), ("midShotsMade", () => midShotsMade), ("outsideShots", () => outsideShots), ("outsideShotsMade", () => outsideShotsMade) };


    }
    // Constructor
    public PlayerStatsPersistent(string id)
    {
        this.id = id;
     
    }
   
}






