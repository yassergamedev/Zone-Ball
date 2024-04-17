using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playfs : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Round16;
    public GameObject[] Quarters;
    public GameObject[] Semis;
  public GameObject[] Final;
   

    
    // Update is called once per frame
   public void setGames(string[] r16, string[] quarters, string[] semis, string[]final)
    {
        for(int i = 0; i < r16.Length;i++)
        {
            Round16[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = r16[i];
        }
        for(int i = 0; i< quarters.Length; i++)
        {
            Quarters[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = quarters[i]; 
        }
        for(int i = 0; i < semis.Length; i++)
        {
            Semis[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent <Text>().text = semis[i];
        }
        for(int i = 0; i < Final.Length; i++)
        {
            Final[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = final[i];
        }
    }
}
