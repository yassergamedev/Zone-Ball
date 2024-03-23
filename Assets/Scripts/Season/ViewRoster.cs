using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewRoster : MonoBehaviour, IDataPersistence
{
    public  Transform Table;
    private string selectedTeam;
    public GameObject PlayerInfo;
    public GameObject Contract;
    
    private GameData gameData;

    public void LoadData(GameData data)
    {
        gameData = data;
    }
    public void SaveData(ref GameData data) { }
    // Start is called before the first frame update
    public void GenerateList(Transform selected)
    {
        for(int i = 0; i < Table.childCount; i++)
        {
            if(Table.GetChild(i).gameObject.name != "Header")
            Destroy(Table.GetChild(i).gameObject);
        }   
        selectedTeam = selected.name;
        FileDataHandler<TeamPersistent> _teamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", selectedTeam);
        TeamPersistent team = _teamHandler.Load();
       

        Vector2 sizeDelta = Table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.players.Length * 100;
        Table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;



      
        Debug.Log(Table.transform.GetComponent<RectTransform>().rect.height);
        for (int i = 0; i < team.players.Length; i++)
        {
            if (team.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> _playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", team.players[i]);
                PlayerPersistent player = _playerHandler.Load();

                GameObject playerInfo = Instantiate(PlayerInfo, Table);

                playerInfo.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Name;
                playerInfo.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Number.ToString();
                playerInfo.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Age.ToString();

                playerInfo.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.consistency.value.ToString();
                playerInfo.transform.GetChild(4).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.awareness.value.ToString();
                playerInfo.transform.GetChild(5).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.juking.value.ToString();
                playerInfo.transform.GetChild(6).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.control.value.ToString();
                playerInfo.transform.GetChild(7).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.shooting.value.ToString();
                playerInfo.transform.GetChild(8).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.positioning.value.ToString();
                playerInfo.transform.GetChild(9).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.steal.value.ToString();
                playerInfo.transform.GetChild(10).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.guarding.value.ToString();
                playerInfo.transform.GetChild(11).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.pressure.value.ToString();
                playerInfo.transform.GetChild(12).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.inside.value.ToString();
                playerInfo.transform.GetChild(13).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.mid.value.ToString();
                playerInfo.transform.GetChild(14).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.Outside.value.ToString();
                playerInfo.transform.GetChild(15).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.ovrl.ToString();

                GameObject playerContract = Instantiate(Contract, Table);
                playerContract.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.salary.ToString();
                playerContract.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = player.contract.years.ToString() ;
                
          
            }
        }
    }
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }
}
