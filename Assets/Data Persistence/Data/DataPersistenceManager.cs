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

    private FileDataHandler<GameData> fileDataHandler;
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
        fileDataHandler = new FileDataHandler<GameData>(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    
    public void NewGame()
    {
        int[] newGameDraftPool = new int[90];
        string playerName;
        for (int i = 0; i < 90; i++)
        {
            playerName = rng.GenerateRandomPlayerName();

            

            Debug.Log("Generated player name: " + playerName);
            
            FileDataHandler<PlayerPersistent> fileDataHandler = new(Application.persistentDataPath + "/Players/", playerName + ".json");
            PlayerPersistent player = new((int)Time.time + i, playerName, i % 14, 0, Random.Range(18, 22), Random.Range(31, 50), new ContractPersistent(1, 10000));

            fileDataHandler.Save(player);
            newGameDraftPool[i] = player.id;
        }
       


        this.gameData = new GameData(newGameDraftPool);
    }
    public void SaveGame()
    {
        foreach(var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }
    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
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
