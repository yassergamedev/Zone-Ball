using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Configuration")]
    public bool newGame = false;
    private GameData gameData;

    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler<GameData> GameDataHandler;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of DataPersistenceManager at scene");
        }
        instance = this;
    }

    private void Start()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        if (newGame)
        {
            NewGame();
        }
        else
        {
            Debug.Log(UnityEngine.Application.persistentDataPath);
            FileDataHandler<CurrentGame> gameDataHandler = new(UnityEngine.Application.persistentDataPath, "Current Game");
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
        string date = dateTime.ToString().Replace("/", "");
        date = date.Replace(":", "-");
        string gameDataId = "Game " + date;

        FileDataHandler<CurrentGame> currHandler = new(UnityEngine.Application.persistentDataPath, "Current Game");
        CurrentGame current = currHandler.Load();

        if (current != null)
        {
            current.currentGame = gameDataId;
            current.week = 0;
            current.game = null;
            current.day = DateTime.Now.Day.ToString();
            current.month = DateTime.Now.Month.ToString();
            current.year = DateTime.Now.Year.ToString();
            current.hour = DateTime.Now.Hour.ToString();
            current.min = DateTime.Now.Minute.ToString();
            current.sec = DateTime.Now.Second.ToString();
            current.currentSeason = "Season " + year;
            current.timePlayed = 0f;
        }
        else
        {
            current = new(gameDataId, "Season " + year);
        }
        currHandler.Save(current);

        FileDataHandler<Games> gamesHandler = new(UnityEngine.Application.persistentDataPath, "Games");
        Games games = gamesHandler.Load();
        if (games != null)
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
            FileDataHandler<TeamPersistent> teamhandler = new(UnityEngine.Application.persistentDataPath + "/" + gameDataId + "/Teams/", west[j]);
            TeamPersistent team = new(west[j], west[j], "West", new string[14]);
            teamhandler.Save(team);
        }
        for (int j = 0; j < east.Length; j++)
        {
            FileDataHandler<TeamPersistent> teamhandler = new(UnityEngine.Application.persistentDataPath + "/" + gameDataId + "/Teams/", east[j]);
            TeamPersistent team = new(east[j], east[j], "East", new string[14]);
            teamhandler.Save(team);
        }

        FileDataHandler<LeaguePersistent> leagueHandler = new(UnityEngine.Application.persistentDataPath + "/" + gameDataId + "/Season " + year + "/", "League " + year);
        LeaguePersistent league = new("League " + year, year.ToString());
        leagueHandler.Save(league);

        FileDataHandler<Season> seasonHandler = new(UnityEngine.Application.persistentDataPath + "/" + gameDataId + "/Season " + year + "/", "Season " + year);
        Season season = new("Season " + year, year.ToString(), "League " + year);
        seasonHandler.Save(season);

        GameDataHandler = new FileDataHandler<GameData>(UnityEngine.Application.persistentDataPath + "/" + gameDataId, gameDataId);
        this.gameData = new GameData(gameDataId, dateTime, "Season " + year);
        GameDataHandler.Save(gameData);

        LoadGame(gameDataId);
    }

    public void SaveGame()
    {
        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
        GameDataHandler.Save(gameData);
    }

    public void LoadGame(string gameInfo)
    {
        GameDataHandler = new(UnityEngine.Application.persistentDataPath + "/" + gameInfo, gameInfo);
        gameData = GameDataHandler.Load();
        if (gameData == null)
        {
            Debug.Log("no data initialized. Initializing default new game...");
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
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // New function to open save file dialog and save game
    public void OpenSaveFileDialog()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.InitialDirectory = UnityEngine.Application.persistentDataPath;
        saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        saveFileDialog.Title = "Save Game Data";

        IntPtr windowHandle = GetActiveWindow();
        if (saveFileDialog.ShowDialog(new WindowWrapper(windowHandle)) == DialogResult.OK)
        {
            string saveFilePath = saveFileDialog.FileName;
            SaveGameToFile(saveFilePath);
        }
    }

    private void SaveGameToFile(string saveFilePath)
    {
        // Your game saving logic here
        FileDataHandler<GameData> gameDataHandler = new FileDataHandler<GameData>(saveFilePath,"");
        GameData gameData = new GameData("YourGameDataId", DateTime.Now, "Season");
        gameDataHandler.Save(gameData);
        Debug.Log("Game saved successfully at " + saveFilePath);
    }

   

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    // Helper class to handle window wrapper
    public class WindowWrapper : IWin32Window
    {
        private IntPtr hwnd;

        public WindowWrapper(IntPtr handle)
        {
            hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return hwnd; }
        }
    }
}

[System.Serializable]
public class CurrentGame
{
    public string currentGame;
    public string currentSeason;
    public int week;
    public string day, month, year, hour, min, sec;
    public int prevSalaryCap = 360000, SalaryCap = 360000, gamePlays = 40, minPlays = 4, maxPlays = 8;
    public MatchPlayed game;
    public float timePlayed; // New field for tracking total time played
    public CurrentGame(string currentGame, string currentSeason)
    {
        this.currentGame = currentGame;
        week =0;
        game = null;
        day = DateTime.Now.Day.ToString();
        month = DateTime.Now.Month.ToString();
        year = DateTime.Now.Year.ToString();
        hour = DateTime.Now.Hour.ToString();
        min = DateTime.Now.Minute.ToString();
        sec = DateTime.Now.Second.ToString();
        this.currentSeason = currentSeason;
        timePlayed = 0f; // Initialize timePlayed
    }
}

[System.Serializable]
public class Games
{
    public List<CurrentGame> games;
    public bool isFirstBoot = true;
    public Games()
    {
        games = new List<CurrentGame>();
    }
}
