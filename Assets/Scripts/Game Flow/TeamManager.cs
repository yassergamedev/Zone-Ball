using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour,IDataPersistence
{
    private GameData gameData;
    private TeamPersistent Home, Guest;
    private CurrentGame cg;
    public Text HomeText, GuestText;
    public GameObject HomeObject, GuestObject;
    public GameObject teamPlayer, oppPlayer;
    public Transform teamDepth, oppDepth;
    public GameObject playerDepth;
    public GameObject ball;
    public SoundManager soundManager;
    bool isTimeFinished = true;
    public void LoadData(GameData data)
    {
       gameData = data;
        FileDataHandler<CurrentGame> currentGame = new(Application.persistentDataPath , "Current Game");   
         cg = currentGame.Load();
        Debug.Log(cg.currentGame);
        Debug.Log(cg.game.opponent);
       FileDataHandler<TeamPersistent> firstTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", cg.game.opponent);
        TeamPersistent firstTeam = firstTeamHandler.Load();
        string opp = firstTeam.matchesPlayed[cg.week].opponent;
        Debug.Log(opp);
        FileDataHandler<TeamPersistent> secondTeamHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", opp);
        TeamPersistent secondTeam = secondTeamHandler.Load();

        Home = firstTeam.matchesPlayed[cg.week].isHome ? firstTeam : secondTeam;
        Guest = firstTeam.matchesPlayed[cg.week].isHome ? secondTeam : firstTeam;

        HomeText.text = Home.name;
        GuestText.text = Guest.name;

        List<GameObject> playersToPlay = new(), otherPlayersToPlay = new();
        List<PlayerActions> playerActions = new(), otherPlayerActions = new();
        for (int i = 0, k = 0; i<Home.players.Length; i++)
        {
            if (Home.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", Home.players[i]);
                PlayerPersistent player = playerHandler.Load();
                Debug.Log(player.Name);
                if (player.plays > 0)
                {
                   
                    GameObject playerObject = Instantiate(teamPlayer, HomeObject.transform);
                    playerObject.transform.name = player.Name;
                    playerObject.GetComponent<PlayerActions>().playerPersistent = player;
                    playerObject.GetComponent<PlayerActions>().index = k;
                    playerObject.GetComponent<PlayerMovement>().playerPersistent = player;
                    playersToPlay.Add(playerObject);
                    playerActions.Add(playerObject.GetComponent<PlayerActions>());
                    k++;
                }
            }
        }
        for (int i = 0, k=0; i < Guest.players.Length; i++)
        {
            if (Guest.players[i] != "")
            {
                FileDataHandler<PlayerPersistent> playerHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Players/", Guest.players[i]);
                PlayerPersistent player = playerHandler.Load();
                if (player.plays > 0)
                {
                 
                    GameObject playerObject = Instantiate(oppPlayer, GuestObject.transform);
                    playerObject.transform.name = player.Name;
                    playerObject.GetComponent<PlayerActions>().playerPersistent = player;
                    playerObject.GetComponent<PlayerActions>().index = k;
                    playerObject.GetComponent<PlayerMovement>().playerPersistent = player;

                    otherPlayersToPlay.Add(playerObject);
                    otherPlayerActions.Add(playerObject.GetComponent<PlayerActions>());
                    k++;
                }
            }
        }

        foreach(GameObject pl in playersToPlay)
        {
            pl.GetComponent<PossessionManager>().playersToPlay = playerActions;
            pl.GetComponent<PossessionManager>().otherPlayersToPlay = otherPlayerActions;
        }
        foreach (GameObject pl in otherPlayersToPlay)
        {
            pl.GetComponent<PossessionManager>().playersToPlay = playerActions;
            pl.GetComponent<PossessionManager>().otherPlayersToPlay = otherPlayerActions;
        }

        int randomPlayer = Random.Range(0, GuestObject.transform.childCount);
        GameObject ballobj = Instantiate(ball, GuestObject.transform.GetChild(randomPlayer));
        if(GuestObject.transform.GetChild(randomPlayer).CompareTag("Player"))
        {
            ballobj.transform.position = GuestObject.transform.GetChild(randomPlayer).transform.position + new Vector3(-0.3f, 0, 0);
        }
        else
        {
            ballobj.transform.position = GuestObject.transform.GetChild(randomPlayer).transform.position + new Vector3(0.3f, 0, 0);
        }

        setDepth(HomeObject, teamDepth);
        setDepth(GuestObject, oppDepth);
    }


    public void setDepth(GameObject team, Transform table)
    {
        Vector2 sizeDelta = table.transform.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y = team.transform.childCount * 50;
        table.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        for (int i = 0; i < team.transform.childCount; i++)
        {
            GameObject depth = Instantiate(playerDepth, table.transform);
            depth.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = team.transform.GetChild(i).gameObject.name;
            depth.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = team.transform.GetChild(i).gameObject.GetComponent<PlayerActions>().playerPersistent.plays.ToString();
            depth.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = team.transform.GetChild(i).gameObject.GetComponent<PlayerActions>().playerPersistent.defPlays.ToString();
        }
    }

    public void SaveData(ref GameData data)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GuestObject.GetComponent<Team>().Plays == 0 &&
            HomeObject.GetComponent<Team>().Plays == 0 &&
            isTimeFinished)
        {
            if(GuestObject.GetComponent<Team>().teamScore == HomeObject.GetComponent<Team>().teamScore)
            {
                GuestObject.GetComponent<Team>().Plays += 6;
                HomeObject.GetComponent<Team>().Plays += 6;
            }
            else
            {
                StartCoroutine(finishGame());
                isTimeFinished = false;
            }
           
        }
    }

    IEnumerator finishGame()
    {
        soundManager.PlayBuzz();


        Home.matchesPlayed[cg.week].isPlayed = true;
        Home.matchesPlayed[cg.week].isReady = false;
        Home.matchesPlayed[cg.week].score = HomeObject.GetComponent<Team>().teamScore;
        Home.matchesPlayed[cg.week].oppScore = GuestObject.GetComponent<Team>().teamScore;


        Guest.matchesPlayed[cg.week].isPlayed = true;
        Guest.matchesPlayed[cg.week].isReady = false;
        Guest.matchesPlayed[cg.week].score = GuestObject.GetComponent<Team>().teamScore;
        Guest.matchesPlayed[cg.week].oppScore = HomeObject.GetComponent<Team>().teamScore;


        if (GuestObject.GetComponent<Team>().teamScore > HomeObject.GetComponent<Team>().teamScore)
        {
            Home.matchesPlayed[cg.week].result = false;
            Guest.matchesPlayed[cg.week].result = true;
        }
        else
        {
            Home.matchesPlayed[cg.week].result = true;
            Guest.matchesPlayed[cg.week].result = false;

        }

        List<PlayerStatsPersistent> HomeStats = new List<PlayerStatsPersistent>{ };
        List<PlayerStatsPersistent> GuestStats = new List<PlayerStatsPersistent> { };

        for(int i = 0; i < GuestObject.transform.childCount; i++)
        {
            PlayerStatsPersistent stats = GuestObject.transform.GetChild(i).gameObject.GetComponent<PlayerActions>().playerStatsPersistent;
            FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/" + Guest.name + "/"
                + cg.week.ToString(), GuestObject.transform.GetChild(i).gameObject.name);
            statsHandler.Save(stats);
        }
        for (int i = 0; i < HomeObject.transform.childCount; i++)
        {
            PlayerStatsPersistent stats = HomeObject.transform.GetChild(i).gameObject.GetComponent<PlayerActions>().playerStatsPersistent;
            FileDataHandler<PlayerStatsPersistent> statsHandler = new(Application.persistentDataPath + "/" + gameData.id + "/" + gameData.currentSeason + "/" + Home.name + "/"
                + cg.week.ToString(), HomeObject.transform.GetChild(i).gameObject.name);
            statsHandler.Save(stats);
        }

        FileDataHandler<TeamPersistent> HomeHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", Home.name);
        HomeHandler.Save(Home);
        FileDataHandler<TeamPersistent> GuestHandler = new(Application.persistentDataPath + "/" + gameData.id + "/Teams/", Guest.name);
        GuestHandler.Save(Guest);
        yield return new WaitForSeconds(2f);
    }
}
