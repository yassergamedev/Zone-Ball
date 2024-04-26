using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TeamsGeneration : MonoBehaviour,IDataPersistence
{
    private GameData gameData;
    public Transform Teamstable;
    public Transform Playerstable;
    public RandomNameGenerator rng;
    private Transform[] TeamsRows;
    private Transform[] PlayersRows;
    private TeamPersistent selectedTeam;
    List<(string, List<MatchPlayed>)> matches;

    // Start is called before the first frame update
    void Start()
    {
        string[] west =
        {
            "Arizona Jaguars",
            "California Lightning",
            "Kansas Coyotes",
            "Minnesota Wolves",
            "Nevada Magic",
            "New Mexico Dragons",
            "Oklahoma Stoppers",
            "Oregon Trail Makers",
            "Texas Rattlesnakes",
            "Washington Hornets"
        };
        string[] east =
        {
            "Alabama Alligators",
            "Florida Dolphins",
            "Georgia Bears",
            "Maryland Sharks",
            "Michigan Warriors",
            "New York Owls",
            "Ohio True Frogs",
            "Pennsylvania Rush",
            "Virginia Bobcats",
            "Wisconsin Crows"
        };
        matches = new List<(string, List<MatchPlayed>)>(){
            ("Alabama Alligators", new List<MatchPlayed>()
            {
                new("Florida Dolphins",true),
                new("Oregon Trail Makers",false),
                new("Wisconsin Crows", false),
                new("Maryland Sharks", true),
                new("Arizona Jaguars", true),
                new("Michigan Warriors",false),
                new("New York Owls", true),
                new("Washington Hornets", false),
                new("Ohio True Frogs",false),
                new("Minnesota Wolves", true),
                new("Pennsylvania Rush",true),
                new("Texas Rattlesnakes", false),
                new("Virginia Bobcats", true),
                new("Wisconsin Crows", true),
                new("California Lightning", true),
                new("Florida Dolphins",false),
                new("Georgia Bears",false),
                new("Nevada Magic",false),
                new("Maryland Sharks",false),
                new("Michigan Warriors", true),
                new("Kansas Coyotes",true),
                new("New York Owls", false),
                new("Ohio True Frogs", true),
                new( "Oklahoma Stoppers", false),
                new("Pennsylvania Rush",false),
                new("Virginia Bobcats",false),
                new("Nevada Magic", true),
                new("Georgia Bears", true)
            }
            ), ("Florida Dolphins", new List<MatchPlayed>()
            {
                new("Alabama Alligators",false),
                new("Texas Rattlesnakes",false),
                new("Georgia Bears", false),
                new("Michigan Warriors", true),
                new("California Lightning", true),
                new("Maryland Sharks",false),
                new("Pennsylvania Rush", true),
                new("Arizona Jaguars", false),
                 new("New York Owls", false),
                new("Nevada Magic",true),
                new("Wisconsin Crows", true),
                new("Washington Hornets",false),

                new("Ohio True Frogs", true),
                new("Georgia Bears", true),
                new("Kansas Coyotes", true),
                new("Alabama Alligators", true),

                new("Virginia Bobcats",false),
                new("Oklahoma Stoppers",false),
                new("Michigan Warriors",false),
                new("Maryland Sharks",false),
                new("Minnesota Wolves", true),
                new("Pennsylvania Rush",false),
                new("New York Owls", true),
                new("Oregon Trail Makers", false),
                new( "Wisconsin Crows", false),
                new("Ohio True Frogs",false),
                new("New Mexico Dragons",true),
                new("Virginia Bobcats", true),

            }
            ),
             ("Georgia Bears", new List<MatchPlayed>()
            {
                new("Maryland Sharks",true),
                new("Washington Hornets",false),
                new("Florida Dolphins",false),
                new("Pennsylvania Rush", true),
                new("Kansas Coyotes", true),
                new("Ohio True Frogs", false),
                new("Virginia Dolphins",true),
                new("California Lightning", false),
                new("Wisconsin Crows", false),
                 new("New Mexico Dragons", true),
                new("Ney York Owls",true),
                new("Arizona Jaguars", false),
                new("Michigan Warriors",true),

                new("Florida Dolphins", false),
                new("Minnesota Wolves", true),
                new("Maryland Sharks", false),
                new("Alabama Alligators", true),

                new("Oregon Trail Makers",false),
                new("Pennsylvania Rush",false),
                new("Ohio True Frogs",true),
                new("Nevada Magic",true),
                new("Virginia Bobcats",false),
                new("Wisconsin Crows",true),
                new("Texas Rattlesnakes", false),
                new("New York Owls", false),
                new( "Michigan Warriors", false),
                new("Oklahoma Stoppers",true),
                new("Alabama Alligators",false),


            }
            ), ("Maryland Sharks", new List<MatchPlayed>()
            {
               new("Georgia Bears",false),
new("Arizona Jaguars",false),
new("New York Owls",false),
new("Alabama Alligators",false),
new("Minnesota Wolves",true),
new("Florida Dolphins",false),
new("Ohio True Frogs",false),
new("Kansas Coyotes",true),
new("Pennsylvania Rush",false),
new("Oklahoma Stoppers",false),
new("Virginia Bobcats",false),
new("California Lightning",true),
new("Wisconsin Crows",true),
new("New York Owls",true),
new("Nevada Magic",true),
new("Georgia Bears",true),
new("Michigan Warriors",true),
new("Texas Rattlesnakes",true),
new("Alabama Alligators",false),
new("Florida Dolphins",false),
new("New Mexico Dragons",true),
new("Ohio True Frogs",true),
new("Pennsylvania Rush",false),
new("Washington Hornets",false),
new("Virginia Bobcats",false),
new("Wisconsin Crows",true),
new("Oregon Trail Makers",true),
new("Michigan Warriors",true),

            }
            ),("Michigan Warriors", new List<MatchPlayed>()

            {
                new("New York Owls", true),
new("California Lightning", false),
new("Pennsylvania Rush", false),
new("Florida Dolphins", false),
new("Nevada Magic", true),
new("Alabama Alligators", true),
new("Wisconsin Crows", true),
new("Minnesota Wolves", false),
new("Virginia Bobcats", false),
new("Oregon Trail Makers", true),
new("Ohio True Frogs", true),
new("Kansas Coyotes", false),
new("Georgia Bears", false),
new("Pennsylvania Rush", true),
new("New Mexico Dragons", true),
new("New York Owls", false),
new("Maryland Sharks", true),
new("Washington Hornets", false),
new("Florida Dolphins", true),
new("Alabama Alligators", false),
new("Oklahoma Stoppers", true),
new("Wisconsin Crows", false),
new("Virginia Bobcats", true),
new("Arizona Jaguars", false),
new("Ohio True Frogs", false),
new("Georgia Bears", true),
new("Texas Rattlesnakes", true),
new("Maryland Sharks", false)

            }),
             ("New York Owls", new List<MatchPlayed>
             {
                 new("New York Owls", true),
new("Kansas Coyotes", false),
new("Maryland Sharks", false),
new("Virginia Bobcats", false),
new("New Mexico Dragons", false),
new("Wisconsin Crows", true),
new("Alabama Alligators", true),
new("Nevada Magic", false),
new("Florida Dolphins", false),
new("Texas Rattlesnakes", false),
new("Georgia Bears", false),
new("Minnesota Wolves", false),
new("Pennsylvania Rush", false),
new("Maryland Sharks", true),
new("Oklahoma Stoppers", false),
new("Michigan Warriors", false),
new("Ohio True Frogs", false),
new("Arizona Jaguars", false),
new("Virginia Bobcats", false),
new("Wisconsin Crows", false),
new("Oregon Trail Makers", false),
new("Alabama Alligators", false),
new("Florida Dolphins", true),
new("California Lightning", false),
new("Georgia Bears", false),
new("Washington Hornets", true)

             }),
             ("New York Owls", new List<MatchPlayed>()
             {
                 new("Michigan Warriors", false),
new("Kansas Coyotes", false),
new("Maryland Sharks", true),
new("Virginia Bobcats", true),
new("New Mexico Dragons", true),
new("Wisconsin Crows", false),
new("Alabama Alligators", false),
new("Nevada Magic", false),
new("Florida Dolphins", true),
new("Texas Rattlesnakes", true),
new("Georgia Bears", false),
new("Minnesota Wolves", false),
new("Pennsylvania Rush", true),
new("Maryland Sharks", false),
new("Oklahoma Stoppers", true),
new("Michigan Warriors", true),
new("Ohio True Frogs", false),
new("Arizona Jaguars", false),
new("Virginia Bobcats", false),
new("Wisconsin Crows", true),
new("Oregon Trail Makers", true),
new("Alabama Alligators", true),
new("Florida Dolphins", false),
new("California Lightning", false),
new("Georgia Bears", true),
new("Pennsylvania Rush", false),
new("Washington Hornets", true),
new("Ohio True Frogs", true)

             }),("Ohio True Frogs", new(){
             new("Pennsylvania Rush", true),
new("Minnesota Wolves", false),
new("Virginia Bobcats", false),
new("Wisconsin Crows", true),
new("Oklahoma Stoppers", true),
new("Georgia Bears", true),
new("Maryland Sharks", false),
new("New Mexico Dragons", false),
new("Alabama Alligators", true),
new("Washington Hornets", true),
new("Michigan Warriors", false),
new("Nevada Magic", false),
new("Florida Dolphins", false),
new("Virginia Bobcats", true),
new("Oregon Trail Makers", true),
new("Pennsylvania Rush", false),
new("New York Owls", true),
new("California Lightning", false),
new("Wisconsin Crows", false),
new("Georgia Bears", false),
new("Texas Rattlesnakes", true),
new("Maryland Sharks", true),
new("Alabama Alligators", false),
new("Kansas Coyotes", false),
new("Michigan Warriors", true),
new("Florida Dolphins", true),
new("Arizona Jaguars", true),
new("New York Owls", false)
}),("Pennsylvania Rush", new(){
new("Ohio True Frogs", false),
new("Nevada Magic", false),
new("Michigan Warriors", true),
new("Georgia Bears", false),
new("Oregon Trail Makers", true),
new("Virginia Bobcats", false),
new("Florida Dolphins", false),
new("Oklahoma Stoppers", false),
new("Maryland Sharks", true),
new("Arizona Jaguars", true),
new("Alabama Alligators", false),
new("New Mexico Dragons", false),
new("New York Owls", false),
new("Michigan Warriors", false),
new("Texas Rattlesnakes", true),
new("Ohio True Frogs", true),
new("Wisconsin Crows", false),
new("Kansas Coyotes", false),
new("Georgia Bears", true),
new("Virginia Bobcats", true),
new("Washington Hornets", true),
new("Florida Dolphins", true),
new("Maryland Sharks", false),
new("Minnesota Wolves", false),
new("Alabama Alligators", true),
new("New York Owls", true),
new("California Lightning", true),
new("Wisconsin Crows", true)
}),("Virginia Bobcats", new()
{
    new("Wisconsin Crows", true),
new("New Mexico Dragons", false),
new("Ohio True Frogs", true),
new("New York Owls", false),
new("Texas Rattlesnakes", true),
new("Pennsylvania Rush", true),
new("Georgia Bears", false),
new("Oregon Trail Makers", false),
new("Michigan Warriors", true),
new("California Lightning", true),
new("Maryland Sharks", false),
new("Oklahoma Stoppers", false),
new("Alabama Alligators", false),
new("Ohio True Frogs", false),
new("Washington Hornets", true),
new("Wisconsin Crows", false),
new("Florida Dolphins", true),
new("Minnesota Wolves", false),
new("New York Owls", true),
new("Pennsylvania Rush", false),
new("Arizona Jaguars", true),
new("Georgia Bears", true),
new("Michigan Warriors", false),
new("Nevada Magic", false),
new("Maryland Sharks", true),
new("Alabama Alligators", true),
new("Kansas Coyotes", true),
new("Florida Dolphins", false)

}),("Wisconsin Crows", new() {
new("Virginia Bobcats", false),
new("Oklahoma Stoppers", false),
new("Alabama Alligators", true),
new("Ohio True Frogs", false),
new("Washington Hornets", true),
new("New York Owls", true),
new("Michigan Warriors", false),
new("Texas Rattlesnakes", false),
new("Georgia Bears", true),
new("Kansas Coyotes", true),
new("Florida Dolphins", false),
new("Oregon Trail Makers", false),
new("Maryland Sharks", false),
new("Alabama Alligators", false),
new("Arizona Jaguars", true),
new("Virginia Bobcats", true),
new("Pennsylvania Rush", true),
new("Nevada Magic", false),
new("Ohio True Frogs", true),
new("New York Owls", false),
new("California Lightning", true),
new("Michigan Warriors", true),
new("Georgia Bears", false),
new("New Mexico Dragons", false),
new("Florida Dolphins", true),
new("Maryland Sharks", true),
new("Minnesota Wolves", true),
new("Pennsylvania Rush", false)
}), new("Arizona Jaguars", new()
{
    new("California Lightning", true),
new("Maryland Sharks", true),
new("Washington Hornets", false),
new("Minnesota Wolves", true),
new("Alabama Alligators", false),
new("Nevada Magic", false),
new("New Mexico Dragons", true),
new("Florida Dolphins", true),
new("Oklahoma Stoppers", false),
new("Pennsylvania Rush", false),
new("Oregon Trail Makers", true),
new("Georgia Bears", true),
new("Texas Rattlesnakes", true),
new("Washington Hornets", true),
new("Wisconsin Crows", false),
new("California Lightning", false),
new("Kansas Coyotes", false),
new("New York Owls", true),
new("Minnesota Wolves", false),
new("Nevada Magic", true),
new("Virginia Bobcats", false),
new("New Mexico Dragons", false),
new("Oklahoma Stoppers", true),
new("Michigan Warriors", true),
new("Oregon Trail Makers", false),
new("Texas Rattlesnakes", false),
new("Ohio True Frogs", false),
new("Kansas Coyotes", true)

}),("California Lightning", new()
{
    new("Arizona Jaguars", false),
new("Michigan Warriors", true),
new("Kansas Coyotes", false),
new("Nevada Magic", true),
new("Florida Dolphins", false),
new("Minnesota Wolves", false),
new("Oregon Trail Makers", true),
new("Georgia Bears", true),
new("New Mexico Dragons", false),
new("Virginia Bobcats", false),
new("Washington Hornets", true),
new("Maryland Sharks", true),
new("Oklahoma Stoppers", true),
new("Kansas Coyotes", true),
new("Alabama Alligators", false),
new("Arizona Jaguars", true),
new("Texas Rattlesnakes", false),
new("Ohio True Frogs", true),
new("Nevada Magic", false),
new("Minnesota Wolves", true),
new("Wisconsin Crows", false),
new("Oregon Trail Makers", false),
new("New Mexico Dragons", true),
new("New York Owls", true),
new("Washington Hornets", false),
new("Oklahoma Stoppers", false),
new("Pennsylvania Rush", false),
new("Texas Rattlesnakes", true)

}),("Kansas Coyotes", new()
{
    new("Minnesota Wolves", true),
new("New York Owls", true),
new("California Lightning", true),
new("Oregon Trail Makers", true),
new("Georgia Bears", false),
new("Oklahoma Stoppers", false),
new("Texas Rattlesnakes", true),
new("Maryland Sharks", true),
new("Washington Hornets", false),
new("Wisconsin Crows", false),
new("New Mexico Dragons", true),
new("Michigan Warriors", true),
new("Nevada Magic", true),
new("California Lightning", false),
new("Florida Dolphins", false),
new("Minnesota Wolves", false),
new("Arizona Jaguars", true),
new("Pennsylvania Rush", true),
new("Oregon Trail Makers", false),
new("Oklahoma Stoppers", true),
new("Alabama Alligators", false),
new("Texas Rattlesnakes", false),
new("Washington Hornets", true),
new("Ohio True Frogs", true),
new("New Mexico Dragons", false),
new("Nevada Magic", false),
new("Virginia Bobcats", false),
new("Arizona Jaguars", false)

}),("Minnesota Wolves", new()
{
    new("Kansas Coyotes", false),
new("Ohio True Frogs", true),
new("New Mexico Dragons", false),
new("Arizona Jaguars", false),
new("Maryland Sharks", false),
new("California Lightning", true),
new("Oklahoma Stoppers", true),
new("Michigan Warriors", true),
new("Oregon Trail Makers", false),
new("Alabama Alligators", false),
new("Texas Rattlesnakes", true),
new("New York Owls", true),
new("Washington Hornets", true),
new("New Mexico Dragons", true),
new("Georgia Bears", false),
new("Kansas Coyotes", true),
new("Nevada Magic", false),
new("Virginia Bobcats", true),
new("Arizona Jaguars", true),
new("California Lightning", false),
new("Florida Dolphins", false),
new("Oklahoma Stoppers", false),
new("Oregon Trail Makers", true),
new("Pennsylvania Rush", true),
new("Texas Rattlesnakes", false),
new("Washington Hornets", false),
new("Wisconsin Crows", false),
new("Nevada Magic", true)

}), ("Nevada Magic", new()
{
    new("New Mexico Dragons", true),
new("Pennsylvania Rush", true),
new("Oregon Trail Makers", false),
new("California Lightning", false),
new("Michigan Warriors", false),
new("Arizona Jaguars", true),
new("Washington Hornets", true),
new("New York Owls", true),
new("Texas Rattlesnakes", false),
new("Florida Dolphins", false),
new("Oklahoma Stoppers", true),
new("Ohio True Frogs", true),
new("Kansas Coyotes", false),
new("Oregon Trail Makers", true),
new("Maryland Sharks", false),
new("New Mexico Dragons", false),
new("Minnesota Wolves", true),
new("Wisconsin Crows", true),
new("California Lightning", true),
new("Arizona Jaguars", false),
new("Georgia Bears", false),
new("Washington Hornets", false),
new("Texas Rattlesnakes", true),
new("Virginia Bobcats", true),
new("Oklahoma Stoppers", false),
new("Kansas Coyotes", true),
new("Alabama Alligators", false),
new("Minnesota Wolves", false)

}),("New Mexico Dragons", new()
{
    new("Nevada Magic", false),
new("Virginia Bobcats", true),
new("Minnesota Wolves", true),
new("Texas Rattlesnakes", true),
new("New York Owls", false),
new("Washington Hornets", false),
new("Arizona Jaguars", false),
new("Ohio True Frogs", true),
new("California Lightning", true),
new("Georgia Bears", false),
new("Kansas Coyotes", false),
new("Pennsylvania Rush", true),
new("Oregon Trail Makers", true),
new("Minnesota Wolves", false),
new("Michigan Warriors", false),
new("Nevada Magic", true),
new("Oklahoma Stoppers", false),
new("Alabama Alligators", true),
new("Texas Rattlesnakes", false),
new("Washington Hornets", true),
new("Maryland Sharks", false),
new("Arizona Jaguars", true),
new("California Lightning", false),
new("Wisconsin Crows", true),
new("Kansas Coyotes", true),
new("Oregon Trail Makers", false),
new("Florida Dolphins", false),
new("Oklahoma Stoppers", true)

}),("Oklahoma Stoppers", new()
{
    new("Oregon Trail Makers", true),
new("Wisconsin Crows", true),
new("Texas Rattlesnakes", false),
new("Washington Hornets", true),
new("Ohio True Frogs", false),
new("Kansas Coyotes", true),
new("Minnesota Wolves", false),
new("Pennsylvania Rush", true),
new("Arizona Jaguars", true),
new("Maryland Sharks", false),
new("Nevada Magic", false),
new("Virginia Bobcats", true),
new("California Lightning", false),
new("Texas Rattlesnakes", true),
new("New York Owls", false),
new("Oregon Trail Makers", false),
new("New Mexico Dragons", true),
new("Florida Dolphins", true),
new("Washington Hornets", false),
new("Kansas Coyotes", false),
new("Michigan Warriors", false),
new("Minnesota Wolves", true),
new("Arizona Jaguars", false),
new("Alabama Alligators", true),
new("Nevada Magic", true),
new("California Lightning", true),
new("Georgia Bears", false),
new("New Mexico Dragons", false)

}),("Oregon Trail Makers", new()
{
    new("Oklahoma Stoppers", false),
new("Alabama Alligators", true),
new("Nevada Magic", true),
new("Kansas Coyotes", false),
new("Pennsylvania Rush", false),
new("Texas Rattlesnakes", false),
new("California Lightning", false),
new("Virginia Bobcats", true),
new("Minnesota Wolves", true),
new("Michigan Warriors", false),
new("Arizona Jaguars", false),
new("Wisconsin Crows", true),
new("New Mexico Dragons", false),
new("Nevada Magic", false),
new("Ohio True Frogs", false),
new("Oklahoma Stoppers", true),
new("Washington Hornets", false),
new("Georgia Bears", true),
new("Kansas Coyotes", true),
new("Texas Rattlesnakes", true),
new("New York Owls", false),
new("California Lightning", true),
new("Minnesota Wolves", false),
new("Florida Dolphins", true),
new("Arizona Jaguars", true),
new("New Mexico Dragons", true),
new("Maryland Sharks", false),
new("Washington Hornets", true)

}),("Texas Rattlesnakes", new()
{
    new("Washington Hornets", true),
new("Florida Dolphins", true),
new("Oklahoma Stoppers", true),
new("New Mexico Dragons", false),
new("Virginia Bobcats", false),
new("Oregon Trail Makers", true),
new("Kansas Coyotes", false),
new("Wisconsin Crows", true),
new("Nevada Magic", true),
new("New York Owls", false),
new("Minnesota Wolves", false),
new("Alabama Alligators", true),
new("Arizona Jaguars", false),
new("Oklahoma Stoppers", false),
new("Pennsylvania Rush", false),
new("Washington Hornets", false),
new("California Lightning", true),
new("Maryland Sharks", true),
new("New Mexico Dragons", true),
new("Oregon Trail Makers", false),
new("Ohio True Frogs", false),
new("Kansas Coyotes", true),
new("Nevada Magic", false),
new("Georgia Bears", true),
new("Minnesota Wolves", true),
new("Arizona Jaguars", true),
new("Michigan Warriors", false),
new("California Lightning", false)

}), ("Washington Hornets", new()
{
    new("Texas Rattlesnakes", false),
new("Georgia Bears", true),
new("Arizona Jaguars", true),
new("Oklahoma Stoppers", false),
new("Wisconsin Crows", false),
new("New Mexico Dragons", true),
new("Nevada Magic", false),
new("Alabama Alligators", true),
new("Kansas Coyotes", true),
new("Ohio True Frogs", false),
new("California Lightning", false),
new("Florida Dolphins", true),
new("Minnesota Wolves", false),
new("Arizona Jaguars", false),
new("Virginia Bobcats", false),
new("Texas Rattlesnakes", true),
new("Oregon Trail Makers", true),
new("Michigan Warriors", true),
new("Oklahoma Stoppers", true),
new("New Mexico Dragons", false),
new("Pennsylvania Rush", false),
new("Nevada Magic", true),
new("Kansas Coyotes", false),
new("Maryland Sharks", true),
new("California Lightning", true),
new("Minnesota Wolves", true),
new("New York Owls", false),
new("Oregon Trail Makers", false)

})

        };

        
        int TeamsRowsCount = Teamstable.childCount;
        TeamsRows = new Transform[Teamstable.childCount];
        Transform row;
        for (int i = 0; i < TeamsRowsCount; i++)
        {
            row = Teamstable.GetChild(i);
            TeamsRows[i] = row;
            
        }
        int PlayersRowsCount = Playerstable.childCount;
        PlayersRows = new Transform[Playerstable.childCount];
        
        for (int i = 0; i < PlayersRowsCount; i++)
        {
            row = Playerstable.GetChild(i);
            PlayersRows[i] = row;

        }


    }
    public void LoadData(GameData data)
    {
        gameData = data;
        Debug.Log(gameData.id);
    }
    public void SaveData(ref GameData data)
    {

    }
    public void GenerateTeams()
    {
        List<(string, int, string)> teams = new List<(string, int, string)>();

        foreach (Transform row in TeamsRows)
        {
            Debug.Log(row.gameObject.name);

            if (row.gameObject.name != "Header" && row.gameObject.name != "West" && row.gameObject.name != "East")
            {
                string name = row.gameObject.name;
                Debug.Log(name);    
                FileDataHandler<TeamPersistent> teamHandler = new FileDataHandler<TeamPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Teams/", name);
                
                TeamPersistent team = teamHandler.Load();
                (string, List<MatchPlayed>) matchesOfTeam = findElement(name);

                team.matchesPlayed = matchesOfTeam.Item2;

                Transform teamN = row.gameObject.transform;
                Transform playstyle = teamN.GetChild(1);
                Transform playstyleSlider = playstyle.GetChild(0);
                int playstyleValue = (int)playstyleSlider.gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                Debug.Log("playstyle value:"+playstyleValue);
                Transform experience = teamN.GetChild(2);
                Transform experienceOption = experience.GetChild(0);
                Transform experienceOptionText = experienceOption.GetChild(0);
                string experienceOptionChoice = experienceOptionText.GetComponent<TextMeshProUGUI>().text;

                Debug.Log(experienceOptionChoice);
                team.playstyle = playstyleValue;
                team.start = experienceOptionChoice;

                for (int i = 0; i < 9; i++)
                {
                    string type;
                    int ovrl = 0;
                    int years = 0, age = 0;
                    string playerName = rng.GenerateRandomPlayerName();
                    FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Players/", playerName);

                    int randomNum = UnityEngine.Random.Range(0, playstyleValue);
                    if (randomNum < playstyleValue)
                    {
                        type = "off";
                    }
                    else
                    {
                        type = "def";
                    }
                    Debug.Log(playerName + " is a " + type + " player");
                    switch (experienceOptionChoice)
                    {
                        case "Young":
                            ovrl = Random.Range(31, 40);
                            years = Random.Range(1, 3);
                            age = Random.Range(18, 21);
                            break;
                        case "Average":
                            ovrl = Random.Range(41, 50);
                            years = Random.Range(3, 6);
                            age = Random.Range(22, 25);
                            break;
                        case "Experienced":
                            ovrl = Random.Range(51, 60);
                            years = Random.Range(6, 9);
                            age = Random.Range(26, 29);
                            break;
                    }
                    PlayerPersistent player = new PlayerPersistent(playerName,i, playerName, type, years, age, ovrl,"Signed");
                    player.team = team.name;
                    playerHandler.Save(player);
                    team.players[i] = playerName;
                    team.salaryCap -= (int)player.contract.salary;
                }
                teamHandler.Save(team);


            }
        }
    }

    public (string,List<MatchPlayed>) findElement(string element)
    {
        (string, List<MatchPlayed>) e = new("", new() { });
        foreach ((string, List<MatchPlayed>) ele in matches)
        {
            if (ele.Item1 == element)
                e = ele;
           
        }
        return e;
    }
    public void setSelectedTeam(GameObject selected)
    {
        FileDataHandler<TeamPersistent> teamHandler = new FileDataHandler<TeamPersistent>(Application.persistentDataPath +"/"+ gameData.id + "/Teams/", selected.name);
        selectedTeam = teamHandler.Load();
        Debug.Log(selectedTeam.name);
        ShowPlayers();
    }

    public void ShowPlayers()
    {
        string[] teamplayers = selectedTeam.players;

        int i = 0;
            foreach(Transform row in PlayersRows)
            {

                FileDataHandler<PlayerPersistent> playerHandler = new FileDataHandler<PlayerPersistent>(Application.persistentDataPath + "/" + gameData.id + "/Players/", teamplayers[i]);
                PlayerPersistent player = playerHandler.Load();
                Debug.Log(player.id);
                if (row.gameObject.name != "Header")
                {
                    
                    int rowChildren = row.childCount;

                    for(int j = 0, s = 0; j<rowChildren; j++)
                    {
                        if (row.GetChild(j).gameObject.name == "Name")
                        {

                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.Name;
                            

                        }else if (row.GetChild(j).gameObject.name == "Age")
                        {
                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.Age.ToString();
                        }
                        else 
                        {
                           
                            row.GetChild(j).GetChild(0).GetComponent<Text>().text = player.getStats()[s].Item2().value.ToString();
                        s++;
                        }

                    }
                }
                i++;
            }
        
    }

}
