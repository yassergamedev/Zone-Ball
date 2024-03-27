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

    public int plays = 0;
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
       return new List<(string, Func<int>)> { ("Pressures", () => pressures), ("Blocks", () => blocks), ("Steals", () => steals), ("Fouls", () => fouls), ("Points Allowed", () => pointsAllowed),
                  ("Jukes", () => jukes), ("Shots", () => shots), ("Shots Taken", () => shotsTaken), ("Points Scored", () => pointsScored), ("Foul Shots", () => foulShots), ("Foul Shots Made", () => foulShotsMade),
                  ("Foul Points Scored", () => foulPointsScored), ("Turnovers", () => turnovers), ("Plays", () => plays), ("Inside Shots", () => insideShots), ("Inside Shots Made", () => insideShotsMade),
                  ("Mid Shots", () => midShots), ("Mid Shots Made", () => midShotsMade), ("Outside Shots", () => outsideShots), ("Outside Shots Made", () => outsideShotsMade) };


    }
    // Constructor
    public PlayerStatsPersistent(string id)
    {
        this.id = id;
     
    }
   
}






