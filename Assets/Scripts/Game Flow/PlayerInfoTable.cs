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
                // Check if the collider belongs to a player
                Player player = hit.transform.gameObject.GetComponent<Player>();
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
                                    DisplayGameFlowInfo(player);
                                    // Call the function to display game flow info
                                    break;
                                }
                        }
                        
                    }
                        // Call the function to display player info
                        
                }
            }
        }

    }

    public void DisplayPlayerInfo(Player player)
    {
        tableAnimator.Play("Table Anim",0);
        // Iterate through each row
        foreach (Transform row in rows)
        {
            Debug.Log("Row: " + row.gameObject.name);
            if (row.gameObject.name != "Table Header" || row.gameObject.name != "Buttons Header")
            {
               
                if(row.gameObject.name == "Player Name")
                {
                    Transform name = row.GetChild(0);

                    name.gameObject.GetComponent<Text>().text = (player.Name ==""?  hit.transform.gameObject.name: player.name);
                }
                else
                {
                    foreach((string statname, Stat stat) in player.stats)
                    {
                     
                        if(row.gameObject.name == statname)
                        {
                           
                            Transform valueCell = row.GetChild(1);
                            Transform textObjV = valueCell.GetChild(0);
                            textObjV.gameObject.GetComponent<Text>().text = stat.value.ToString();

                            Transform potentialCell = row.GetChild(2);
                            Transform textObjP = potentialCell.GetChild(0);
                            textObjP.gameObject.GetComponent<Text>().text = stat.potential.ToString();

                        }
                    }
                   
                }
            }


         
        }
    }
    public void DisplayGameFlowInfo(Player player)
    {
        tableAnimator.Play("Table Anim", 0);
        // Iterate through each row
        foreach (Transform row in rows)
        {
            Debug.Log("Row: " + row.gameObject.name);
            if (row.gameObject.name != "Table Header" || row.gameObject.name != "Buttons Header")
            {

                if (row.gameObject.name == "Player Name")
                {
                    Transform name = row.GetChild(0);

                    name.gameObject.GetComponent<Text>().text = (player.Name == "" ? hit.transform.gameObject.name : player.name);
                }
                else
                {
                    foreach ((string statname, System.Func<int> stat) in player.gameFlowStats)
                    {

                        if (row.gameObject.name == statname)
                        {

                            Transform valueCell = row.GetChild(1);
                            Transform textObjV = valueCell.GetChild(0);
                            int statN = stat();
                            textObjV.gameObject.GetComponent<Text>().text = statN.ToString();

                     

                        }
                    }

                }
            }



        }
    }
}
