using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RenameFile : MonoBehaviour
{
    public string gameName;
    public GameObject persistenceManager;

    private void Start()
    {
        gameName = this.gameObject.transform.parent.parent.name;
        persistenceManager = GameObject.FindGameObjectWithTag("Player");
    }

    public void RenameFolderAndFile()
    {
        string newFolderName = this.gameObject.transform.parent.GetChild(10).GetComponent<InputField>().text;
        string currentFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, gameName);
        string newFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, newFolderName);

        // Check if the new folder name already exists and adjust if necessary
        int counter = 1;
        while (Directory.Exists(newFolderPath))
        {
            newFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, newFolderName + counter.ToString());
            counter++;
        }
        newFolderName = Path.GetFileName(newFolderPath); // Update newFolderName to reflect the unique name

        
            FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
            Games games = gamesHandler.Load();
        FileDataHandler<CurrentGame> currentgameHandler= new(Application.persistentDataPath, "Current Game");
        CurrentGame cg= currentgameHandler.Load();
        for (int i = 0; i < games.games.Count; i++)
            {
                if (games.games[i].currentGame == gameName)
                {
                    games.games[i].currentGame = newFolderName;

                    if (cg.currentGame == gameName)
                    {
                    cg.currentGame = newFolderName;
                 }
                    break;
                }
            }
        currentgameHandler.Save(cg);
            gamesHandler.Save(games);

            FileDataHandler<GameData> gameDatHandler = new(Application.persistentDataPath + "/" + gameName, gameName);
            GameData data = gameDatHandler.Load();

            data.id = newFolderName;

            gameDatHandler.Save(data);

            if (Directory.Exists(currentFolderPath) && currentFolderPath != newFolderPath)
            {
                // Rename file inside the folder
                string currentFilePath = Path.Combine(currentFolderPath, gameName);
                string newFilePath = Path.Combine(currentFolderPath, newFolderName);

                if (File.Exists(currentFilePath))
                {
                    File.Move(currentFilePath, newFilePath);
                    Debug.Log("File renamed successfully from " + gameName + " to " + newFolderName);
                }
                else
                {
                    Debug.LogError("The file " + gameName + " does not exist.");
                }

                // Rename folder
                Directory.Move(currentFolderPath, newFolderPath);
                Debug.Log("Folder renamed successfully from " + gameName + " to " + newFolderName);
            }
            else
            {
                Debug.LogError("The folder " + gameName + " does not exist.");
            }
            this.gameObject.transform.parent.GetChild(8).GetComponent<Text>().text = newFolderName;
            gameName = newFolderName;
            this.gameObject.transform.parent.parent.name = newFolderName;

            persistenceManager.GetComponent<LoadGame>().LoadData(new GameData("", DateTime.Now, ""));
        
        
    }

    public void CloneFolder()
    {
        string sourceFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, gameName);
        string newFolderName = gameName + " -Copy";
        string newFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, newFolderName);

        // Check if the new folder name already exists and adjust if necessary
        int counter = 1;
        while (Directory.Exists(newFolderPath))
        {
            newFolderName = gameName + " -Copy" + " " +counter.ToString();
            newFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, newFolderName);
            counter++;
        }

        try
        {
            if (Directory.Exists(sourceFolderPath))
            {
                // Create the destination folder
                Directory.CreateDirectory(newFolderPath);

                // Copy all files
                string[] files = Directory.GetFiles(sourceFolderPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFilePath = Path.Combine(newFolderPath, fileName.Replace(gameName, newFolderName));
                    File.Copy(file, destinationFilePath, true);
                }

                // Copy all subfolders
                string[] subFolders = Directory.GetDirectories(sourceFolderPath);
                foreach (string subFolder in subFolders)
                {
                    string subFolderName = Path.GetFileName(subFolder);
                    string destinationSubFolderPath = Path.Combine(newFolderPath, subFolderName);
                    CloneFolderRecursive(subFolder, destinationSubFolderPath, newFolderName);
                }

                // Update Games file with the new cloned game
                FileDataHandler<Games> gamesHandler = new(Application.persistentDataPath, "Games");
                Games games = gamesHandler.Load();

                for (int i = 0; i < games.games.Count; i++)
                {
                    if (games.games[i].currentGame == gameName)
                    {
                        // Create a new instance of the Game object
                        CurrentGame newGame = new CurrentGame("","");

                        // Copy the values from the original game to the new game
                        newGame.currentGame = games.games[i].currentGame;
                        newGame.currentSeason = games.games[i].currentSeason; // repeat for all other fields
                        newGame.week = games.games[i].week;
                        newGame.day = games.games[i].day;
                        newGame.month = games.games[i].month;
                        newGame.year = games.games[i].year;
                        newGame.hour = games.games[i].hour; 
                        newGame.min = games.games[i].min;
                        newGame.sec = games.games[i].sec;
                        newGame.prevSalaryCap = games.games[i].prevSalaryCap;
                        newGame.SalaryCap = games.games[i].SalaryCap;
                        newGame.game = games.games[i].game;
                        newGame.timePlayed  = games.games[i].timePlayed;

                        // Add the new instance to the list
                        games.games.Add(newGame);

                        // Update the new instance's name
                        games.games[i + 1].currentGame = gameName + " -Copy";

                        for (int ki = 0; ki < games.games.Count; ki++)
                        {
                            Debug.Log("hey " + games.games[ki].currentGame);
                        }

                        break;
                    }
                }

                gamesHandler.Save(games);

                Debug.Log("Folder cloned successfully from " + sourceFolderPath + " to " + newFolderPath);
            }
            else
            {
                Debug.LogError("The source folder " + sourceFolderPath + " does not exist.");
            }

            persistenceManager.GetComponent<LoadGame>().LoadData(new GameData("", DateTime.Now, ""));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("An error occurred while cloning the folder: " + ex.Message);
        }
    }

    private void CloneFolderRecursive(string sourceFolderPath, string destinationFolderPath, string newFolderName)
    {
        // Create the destination folder if it doesn't exist
        if (!Directory.Exists(destinationFolderPath))
        {
            Directory.CreateDirectory(destinationFolderPath);
        }

        // Copy all files
        string[] files = Directory.GetFiles(sourceFolderPath);
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destinationFilePath = Path.Combine(destinationFolderPath, fileName.Replace(gameName, newFolderName));
            File.Copy(file, destinationFilePath, true);
        }

        // Copy all subfolders
        string[] subFolders = Directory.GetDirectories(sourceFolderPath);
        foreach (string subFolder in subFolders)
        {
            string subFolderName = Path.GetFileName(subFolder);
            string destinationSubFolderPath = Path.Combine(destinationFolderPath, subFolderName);
            CloneFolderRecursive(subFolder, destinationSubFolderPath, newFolderName);
        }
    }
}
