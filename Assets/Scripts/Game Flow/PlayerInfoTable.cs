using System;
using UI.Tables;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoTable : MonoBehaviour
{
    public Transform table;
    public string type;
    private Transform[] rows;
    public Animator tableAnimator;
    private Camera mainCamera;
    RaycastHit2D hit;
    private void Start()
    {
        
        mainCamera = Camera.main;
        int rowsCount = table.childCount;
        rows = new Transform[table.childCount];
        Transform row;
        for (int i = 0; i<rowsCount; i++)
        {
            row = table.GetChild(i);
        
            rows[i] = row;
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            
           
            // Cast a ray from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Perform a 2D raycast
            hit = Physics2D.Raycast(ray.origin, ray.direction);
           
            // Check if the ray hits a collider
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    hit.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                // Check if the collider belongs to a player
                PlayerPersistent player = hit.transform.gameObject.GetComponent<PlayerActions>().playerPersistent;
                PlayerStatsPersistent playerStats = hit.transform.gameObject.GetComponent<PlayerActions>().playerStatsPersistent;
                if (player != null)
                {
                   if((hit.transform.gameObject.CompareTag( "Player" ) && name == "Player Info") ||
                       (hit.transform.gameObject.CompareTag("OppPlayer") && name == "Opp Info")  )
                       {
                        switch (type)
                            {
                            case "Stats":
                                {
                                    DisplayPlayerInfo(player);
                                    break;
                                }
                            case "gameFlow":
                                {
                                    DisplayGameFlowInfo(player,playerStats);
                                    // Call the function to display game flow info
                                    break;
                                }
                        }
                        
                    }
                        // Call the function to display player info
                        
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Cast a ray from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Perform a 2D raycast
            hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit.transform.CompareTag("Player"))
            {
                hit.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }else
            {
                hit.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
           
        }
    }

    public void DisplayPlayerInfo(PlayerPersistent player)
    {
        tableAnimator.Play("Table Anim",0);
        // Iterate through each row
        foreach (Transform row in rows)
        {
          
            if (row.gameObject.name != "Table Header" || row.gameObject.name != "Buttons Header")
            {
              
                if(row.gameObject.name == "Name")
                {
                    Transform name = row.GetChild(0);

                    name.gameObject.GetComponent<Text>().text = player.Name;
                }
                else
                {
                    foreach((string statname, System.Func<StatPersistent> stat, Action<int>s, Action<int>a, Action<int> v) in player.getStats())
                    {
                     
                        if(row.gameObject.name == statname)
                        {
                           
                            Transform valueCell = row.GetChild(1);
                            Transform textObjV = valueCell.GetChild(0);
                            textObjV.gameObject.GetComponent<Text>().text = stat().value.ToString();


                        }
                    }
                   
                }
            }


         
        }
    }
    public void DisplayGameFlowInfo(PlayerPersistent player, PlayerStatsPersistent playerStats)
    {
        tableAnimator.Play("Table Anim", 0);
        // Iterate through each row
        foreach (Transform row in rows)
        {
        
            if (row.gameObject.name != "Table Header" || row.gameObject.name != "Buttons Header")
            {
                if(row.gameObject.name == "plays")
                {
                    Debug.Log(player.plays);
                   row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = player.plays.ToString();
                 
                }
                if(row.gameObject.name == "defPlays")
                {
                    row.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = player.defPlays.ToString();
                }
                if (row.gameObject.name == "Name")
                {
                   

                    row.GetChild(0).gameObject.GetComponent<Text>().text = player.Name;
                }

                else
                {
                    foreach ((string statname, System.Func<int> stat, System.Action<int> b) in playerStats.getStats())
                    {

                        if (row.gameObject.name == statname)
                        {

                            Transform valueCell = row.GetChild(1);
                            Transform textObjV = valueCell.GetChild(0);
                            int statN = stat();
                            Debug.Log(statname + " " + statN);  
                            textObjV.gameObject.GetComponent<Text>().text = statN.ToString();

                     

                        }
                    }

                }
            }



        }
    }
}
