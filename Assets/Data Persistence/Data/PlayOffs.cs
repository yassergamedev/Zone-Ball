using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayOffs 
{
    public string id;
    //we use teampersistent id's
    public string[] roundOf16;
    public string[] quarterFinals;
    public string[] semiFinals;
    public string[] finals;
    public string winner;
   public PlayOffs(string id)
    {
        this.id = id;
        this.roundOf16 = new string[]{ };
        this.quarterFinals = new string[] { };
        this.semiFinals = new string[] { };
        this.finals = new string[] { };
        this.winner = "";
    }
}
