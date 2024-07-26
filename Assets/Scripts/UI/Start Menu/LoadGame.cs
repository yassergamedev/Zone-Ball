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
    public  string FormatTime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600f);
        seconds -= hours * 3600;
        int minutes = Mathf.FloorToInt(seconds / 60f);
        seconds -= minutes * 60;
        int secs = Mathf.FloorToInt(seconds);

        return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", hours, minutes, secs);
    }
    public void LoadData(GameData gamdata)
    {

        FileDataHandler<Games> gamesHanlder = new(Application.persistentDataPath, "Games");
        games = gamesHanlder.Load();

        for (int i = 0; i < Content.transform.childCount; i++)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < games.games.Count; i++)
        {
            Debug.Log(games.games[i].currentGame);
            GameObject loadedGame = Instantiate(LoadedGame, Content.transform);
            loadedGame.name = games.games[i].currentGame;
            loadedGame.transform.GetChild(1).GetChild(4).GetComponent<Text>().text = games.games[i].year + "-" + games.games[i].month + "-" + games.games[i].day + " " + games.games[i].hour + ":" + games.games[i].min + ":" + games.games[i].sec;
            loadedGame.transform.GetChild(1).GetChild(5).GetComponent<Text>().text = games.games[i].currentSeason;
            loadedGame.transform.GetChild(1).GetChild(7).GetComponent<Text>().text = (games.games[i].week +1).ToString();
            loadedGame.transform.GetChild(1).GetChild(8).GetComponent<Text>().text = games.games[i].currentGame;
            loadedGame.transform.GetChild(1).GetChild(9).GetComponent<Text>().text = FormatTime(games.games[i].timePlayed);
            loadedGame.transform.GetChild(1).GetChild(10).GetComponent<InputField>().text = games.games[i].currentGame;
            loadedGame.name = games.games[i].currentGame;
            loadedGame.GetComponent<SceneStuff>().game = games.games[i];
        }
    }
}
