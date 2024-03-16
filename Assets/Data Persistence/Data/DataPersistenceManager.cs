using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header ("File Configuration")]
    [SerializeField] private string fileName;
    private GameData gameData;

    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler fileDataHandler;
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
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    
    public void NewGame()
    {
        this.gameData = new GameData();
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

        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
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
