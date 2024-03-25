using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LeaguePersistent 
{
    string id;
    string date;
    List<TeamRecord> teams;
    List<MatchPersistent> matches;

    public LeaguePersistent(string id, string date)
    {
        this.id = id;
        this.date = date;
        teams = new();
        matches = new();
    }
}

[System.Serializable]
public class TeamRecord
{
    public string teamId;
    public int wins;
    public int losses;
    public int pointsScored;
    public int pointsAllowed;
    public int turnovers;
}