using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeagueMenu : MonoBehaviour, IDataPersistence
{
    private GameData gameData;
    private CurrentGame game;
    public Text League;
    public Button Load;
    public void SaveData(ref GameData gamdata)
    {

    }
    public void LoadData(GameData gamdata)
    {
    
        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame currGame = currHandler.Load();
        Load.GetComponent<SceneStuff>().game = currGame;

        League.text = "Game " + currGame.year.ToString() + "-" + currGame.month.ToString() + "-" +
            currGame.day.ToString() +" "+ currGame.hour.ToString()+":"+ currGame.min.ToString() + ":" + currGame.sec.ToString()+"\n "+
            currGame.currentSeason.ToString();


    }
   
}
