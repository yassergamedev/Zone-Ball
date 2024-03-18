using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header ("File Configuration")]
    [SerializeField] private string fileName;
    private GameData gameData;
    public RandomNameGenerator rng;

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
        NewGame();
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
            "New Mexico",
            "Oklahoma Stoppers",
            "Oregon Trail Makers",
            "Texas Battlesnakes",
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
        string date = dateTime.ToString().Replace("/","");
        date = date.Replace(":","");
        string gameDataId = "Game " + date;
        
        string[] newGameDraftPool = new string[180];
        string playerName;
        for (int i = 0; i < 180; i++)
        {
            playerName = rng.GenerateRandomPlayerName();

            Debug.Log("Generated player name: " + playerName);
            
            FileDataHandler<PlayerPersistent> fileDataHandler = new(Application.persistentDataPath+"/" +gameDataId+ "/Players/", playerName);
            PlayerPersistent player = new(playerName, playerName, i % 50, 0, UnityEngine.Random.Range(18, 22), UnityEngine.Random.Range(31, 50));

            fileDataHandler.Save(player);
            newGameDraftPool[i] = player.id;
        }

        for(int j = 0; j < west.Length; j++)
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
        this.gameData = new GameData(gameDataId,dateTime, newGameDraftPool);
        GameDataHandler.Save(gameData); 
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
            NewGame();
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
