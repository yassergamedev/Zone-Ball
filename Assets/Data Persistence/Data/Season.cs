using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Season
{
    //using id's
    public string id;
    public string date;
    public int week;
    public int PlayOffRound;
    public string league;
    public string playOffs;
    public string phase;
    public Progress[] progress;
    public List<TeamPersistent> finalTeamStandings;
    public List<string> Quarters, Semis,Final;

    public string[] firstRoundRookieDraft;
    public string[] secondRoundRookieDraft;
    public string[] bestEastPlayers;
    public string[] bestWestPlayers;
    public string[] bestOfThreeMatches;
    //Conference
    public string winner;

    public Season(string id, string date, string league)
    {
        this.id = id;
        this.date = date;
        week = 0;
        this.league = league;
        this.playOffs = "";
        this.bestEastPlayers = new string[] { };
        this.bestWestPlayers = new string[] { };
        this.bestOfThreeMatches = new string[] { };
        this.winner ="";
        Quarters = new();
        Semis = new();
        Final = new();
        phase = "Season";
        PlayOffRound = 0;
        finalTeamStandings = new List<TeamPersistent> { };
        progress = new Progress[]
        {
            new(1,false),
            new(5,false),
            new(9,false),
            new(13, false),
            new(17,false),
            new(21,false),
            new(25,false),
        };
    }

}

[System.Serializable]
public class Progress
{
    public int week;
    public bool hasProgressed;


    public void setWeek(int w)
    { week = w; }
    public void setHasProgressed(bool hasProgressed)
    {
        this.hasProgressed = hasProgressed;
    }
    public Progress(int w, bool h)
    {
        week = w;
        hasProgressed = h;
    }
}





