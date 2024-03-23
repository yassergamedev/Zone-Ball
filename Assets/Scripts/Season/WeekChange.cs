using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeekChange : MonoBehaviour,IDataPersistence
{
    GameData gameData;
    TMP_Dropdown dropdown;
    Season currentSeason;
    public int currentWeek;
   public void SaveData(ref GameData gameData) { }
    public void LoadData(GameData data)
    {
        gameData = data;
        FileDataHandler<Season> seasonHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/", gameData.currentSeason);
        currentSeason = seasonHandler.Load();
        currentWeek = currentSeason.week;
    }


    
   
    public void ChangeWeek(TMP_Dropdown weekSelector )
    {
        Debug.Log(weekSelector.value +1);
    }
}
