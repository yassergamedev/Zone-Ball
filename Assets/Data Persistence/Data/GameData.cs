using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData 
{

    public int[] newGameDraftPool;




    public GameData(int[] ids)
    {

        newGameDraftPool = ids;

    }

    public string GenerateRandomPlayerName(List<string> firstNames, List<string> lastNames,int firstCount, int lastCount)
    {
        // Read all lines from first names file into a list


        // Select a random first name and remove it from the list
        int firstNameIndex = UnityEngine.Random.Range(0, firstCount);
        string firstName = firstNames[firstNameIndex];
        firstNames.RemoveAt(firstNameIndex);

        // Select a random last name and remove it from the list
        int lastNameIndex = UnityEngine.Random.Range(0, lastCount);
        string lastName = lastNames[lastNameIndex];
        lastNames.RemoveAt(lastNameIndex);

        // Combine the first and last names to create the player's name
        string playerName = firstName + " " + lastName;

        // Write back remaining names to files
        File.WriteAllLines("first names.txt", firstNames.ToArray());
        File.WriteAllLines("last names.txt", lastNames.ToArray());

        return playerName;
    }


}
