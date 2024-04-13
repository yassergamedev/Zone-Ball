using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Configuration")]
    public bool newGame = false;
    private GameData gameData;
    
    
    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler<GameData> GameDataHandler;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance !=null)
        {
            Debug.LogError("More than one instance of DataPersistenceManager at scene");
        }
    }
    private void Start()
    {
        //fileDataHandler = new FileDataHandler<GameData>(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        if(newGame)
        {
            NewGame();
        }else
        {
            FileDataHandler<CurrentGame> gameDataHandler = new(Application.persistentDataPath, "Current Game");

           
            LoadGame(gameDataHandler.Load().currentGame);
        }
      
    }
    
    public void NewGame()
    {
        DateTime dateTime = DateTime.Now;
        int year = dateTime.Year;
        string[] west = 
        {
            "Arizona Jaguars",
            "California Lightning",
            "Kansas Coyotes",
            "Minnesota Wolves",
            "Nevada Magic",
            "New Mexico Dragons",
            "Oklahoma Stoppers",
            "Oregon Trail Makers",
            "Texas Rattlesnakes",
            "Washington Hornets"
        };
        string[] east =
        {
            "Alabama Alligators",
            "Florida Dolphins",
            "Georgia Bears",
            "Maryland Sharks",
            "Michigan Warriors",
            "New York Owls",
            "Ohio True Frogs",
            "Pennsylvania Rush",
            "Virginia Bobcats",
            "Wisconsin Crows"
        };
        //without the hour

        string date = dateTime.ToString().Replace("/","");
        date = date.Replace(":","");
        string gameDataId = "Game " + date;

        FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
        CurrentGame current = currHandler.Load();

        if (current != null)
        {
           current.currentGame = gameDataId;
            current.week = 1;

        }
        else
        {
            current = new(gameDataId, "Season " + year);
           
        }
        currHandler.Save(current);
        FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
        Games games = gamesHandler.Load();
        if(games != null)
        {
            games.games.Add(current);
            
        }
        else
        {
            games = new();
            games.games.Add(current);
        }
        gamesHandler.Save(games);
        for (int j = 0; j < west.Length; j++)
        {
            FileDataHandler<TeamPersistent> teamhandler = new(Application.persistentDataPath + "/" + gameDataId + "/Teams/", west[j]);
            TeamPersistent team = new(west[j], west[j], "West", new string[14]);

            teamhandler.Save(team);
        }
        for (int j = 0; j < east.Length; j++)
        {
            FileDataHandler<TeamPersistent> teamhandler = new(Application.persistentDataPath + "/" + gameDataId + "/Teams/", east[j]);
            TeamPersistent team = new(east[j], east[j], "East", new string[14]);

            teamhandler.Save(team);
        }
        FileDataHandler<LeaguePersistent> leagueHandler = new(Application.persistentDataPath + "/" + gameDataId + "/Season "+ year +"/", "League "+ year);
        LeaguePersistent league = new("League "+year, year.ToString());

        leagueHandler.Save(league);
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameDataId + "/Season " + year + "/", "Season " + year);
        Season season = new("Season " + year, year.ToString(), "League "+year);
        seasonHandler.Save(season);

        GameDataHandler = new FileDataHandler<GameData>(Application.persistentDataPath + "/" + gameDataId, gameDataId);
        this.gameData = new GameData(gameDataId,dateTime, "Season "+ year);
        GameDataHandler.Save(gameData);


        LoadGame(gameDataId);
       
    }
    public void SaveGame()
    {
        foreach(var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        GameDataHandler.Save(gameData);
    }
    public void LoadGame(string gameInfo)
    {
        GameDataHandler = new(Application.persistentDataPath + "/" + gameInfo, gameInfo);
        gameData = GameDataHandler.Load();
        if (gameData == null)
        {
            Debug.Log("no data initialized. Initalizing default new game..");
            //NewGame();
        }
        else
        {
            foreach (var dataPersistenceObject in dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(gameData);
            }
        }

        
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>( dataPersistenceObjects);
    }
}
[System.Serializable]
public class CurrentGame
{
    public string currentGame;
    public string currentSeason;
    public int week;
    public string day, month, year, hour, min, sec;
    public MatchPlayed game;
    public CurrentGame(string currentGame, string currentSeason)
    {
        this.currentGame = currentGame;
        week = 1;
        game = null;
        day = DateTime.Now.Day.ToString();
        month = DateTime.Now.Month.ToString();
        year = DateTime.Now.Year.ToString();
        hour = DateTime.Now.Hour.ToString();
        min = DateTime.Now.Minute.ToString();
        sec = DateTime.Now.Second.ToString();
        this.currentSeason = currentSeason;
    }
}
[System.Serializable]
public class Games
{
    public List<CurrentGame> games;
    public Games( )
    {
        games = new List<CurrentGame>();
    }
}