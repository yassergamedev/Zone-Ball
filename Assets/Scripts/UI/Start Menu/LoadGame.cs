using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour, IDataPersistence
{
    private GameData gameData;
    private Games games;
    private CurrentGame game;
    public GameObject Content, LoadedGame;
    public void SaveData(ref GameData gamdata)
    {

    }
    public void LoadData(GameData gamdata)
    {
        FileDataHandler<Games> gamesHanlder = new(Application.persistentDataPath, "Games");
        games = gamesHanlder.Load();
        for (int i = 0; i < games.games.Count; i++)
        {
            GameObject loadedGame = Instantiate(LoadedGame, Content.transform);
            loadedGame.transform.GetChild(0).GetComponent<Text>().text = games.games[i].year + "-" + games.games[i].month + "-" + games.games[i].day + " " + games.games[i].hour + ":" + games.games[i].min + ":" + games.games[i].sec;
            loadedGame.transform.GetChild(1).GetComponent<Text>().text = games.games[i].currentSeason;
            loadedGame.transform.GetChild(3).GetComponent<Text>().text = games.games[i].week.ToString();
            loadedGame.GetComponent<SceneStuff>().game = games.games[i];
        }
    }
}
