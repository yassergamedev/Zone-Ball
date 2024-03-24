using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coaching : MonoBehaviour, IDataPersistence
{
    private GameData gamedata;
    public void LoadData(GameData data)
    {
        gamedata = data;
    }
    public void SaveData(ref GameData data) { }
    // Update is called once per frame
   public void setCoaching(string selectedTeam, string oc, string dc, string hc)
    {
        FileDataHandler<TeamPersistent> teamHandler = new(Application.persistentDataPath + "/" + gamedata.id + "/Teams/", selectedTeam);
        TeamPersistent team = teamHandler.Load();

        team.HC = hc;
        team.DC = dc;
        team.OC = oc;
        teamHandler.Save(team);
        Debug.Log("Save done, presumabley");
    }
}
