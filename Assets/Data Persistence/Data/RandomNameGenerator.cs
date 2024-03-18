using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    private string firstNamesFile;
    private string lastNamesFile;
    public List<string> firstNames = null;
        public List<string> lastNames = null;
    private int firstIndex;
    private int lastIndex;
    public string fullName;
    void Start()
    {
       firstNamesFile = "first names";
       lastNamesFile = "last names";
        firstNames = new List<string>(Resources.Load<TextAsset>(firstNamesFile).text.Split('\n'));
        lastNames = new List<string>(Resources.Load<TextAsset>(lastNamesFile).text.Split('\n'));
     

    }

    public string  GenerateRandomPlayerName()
    {
        firstIndex = UnityEngine.Random.Range(0, firstNames.Count);
        //remove /r from the girst name
        firstNames[firstIndex] = firstNames[firstIndex].Replace("\r", "");

        lastIndex = UnityEngine.Random.Range(0, lastNames.Count);
        //remove /r from the last name
        lastNames[lastIndex] = lastNames[lastIndex].Replace("\r", "");
        fullName = firstNames[firstIndex] + " " + lastNames[lastIndex];
        return fullName;
    }

   
}

// Example usage:


