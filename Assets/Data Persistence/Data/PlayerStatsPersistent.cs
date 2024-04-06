using JetBrains.Annotations;
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
    public int turnOn = 0;

   // public int plays = 0;
    public int plays = 0;
    public int defPlays = 0;

    //---------- zone ------------
    public int insideShots = 0;
    public int insideShotsMade = 0;
    public int midShots = 0;
    public int midShotsMade = 0;
    public int outsideShots = 0;
    public int outsideShotsMade = 0;
    public List<(string, Func<int>, Action<int>)> getStats()
    {

        //return the list of stats
        return new List<(string, Func<int>, Action<int>)> {
    ("plays", () => plays, (value) => plays = value),
    ("defPlays", () => defPlays, (value) => defPlays = value),
    ("pressures", () => pressures, (value) => pressures = value),
    ("blocks", () => blocks, (value) => blocks = value),
    ("steals", () => steals, (value) => steals = value),
    ("fouls", () => fouls, (value) => fouls = value),
    ("pointsAllowed", () => pointsAllowed, (value) => pointsAllowed = value),
    ("jukes", () => jukes, (value) => jukes = value),
    ("shots", () => shots, (value) => shots = value),
    ("shotsTaken", () => shotsTaken, (value) => shotsTaken = value),
    ("pointsScored", () => pointsScored, (value) => pointsScored = value),
    ("foulShots", () => foulShots, (value) => foulShots = value),
    ("foulShotsMade", () => foulShotsMade, (value) => foulShotsMade = value),
    ("foulPointsScored", () => foulPointsScored, (value) => foulPointsScored = value),
    ("turnovers", () => turnovers, (value) => turnovers = value),
    ("insideShots", () => insideShots, (value) => insideShots = value),
    ("insideShotsMade", () => insideShotsMade, (value) => insideShotsMade = value),
    ("midShots", () => midShots, (value) => midShots = value),
    ("midShotsMade", () => midShotsMade, (value) => midShotsMade = value),
    ("outsideShots", () => outsideShots, (value) => outsideShots = value),
    ("outsideShotsMade", () => outsideShotsMade, (value) => outsideShotsMade = value)
};


    }
    // Constructor
    public PlayerStatsPersistent(string id)
    {
        this.id = id;
     
    }
   
}






