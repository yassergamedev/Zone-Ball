using System.Collections;
using System.Collections.Generic;
using UnityEngine;



  public class FlavourTexts : MonoBehaviour
{
     string[] insideShots;
     string[] midShots;
     string[] outsideTexts;
     string[] insideMiss;
     string[] midMiss;
     string[] outsideMiss;
     string[] jukeSuccess;
     string[] stealSuccess;
     string[] minorFouls;
     string[] majorFouls;
    // Start is called before the first frame update
    void Start()
    {

        insideShots = new string[] {
            "Drops a 4 like nothing!",
        "Lobs that 4 in with ease!",
        "Powerful Inside Shot! 4 points!",
        "It's up and in for 4!" };
        midShots = new string[]
        {
           "Nails that Mid for 5!",
        "Completes the 5 point play!",
        "Succeeds for 5 points!",
        "A great mid shot for 5!"
        };
        outsideTexts = new string[]
        {
              "Makes the 6 Point Bomb!",
        "Wow! What a shot for 6!",
        "Sinks the Deep 6!",
        "Launched for 6.. and it's good!"
        };
        insideMiss = new string[]
        {
              "And he misses inside!",
        "Inside opportunity gone for 4!",
        "Oh! No luck on that 4 ball!",
        "Aw how'd he miss that 4!"
        };
        midMiss = new string[]
        {
              "No dice on that 5!",
        "And a missed mid range!",
        "Rushed mid shot, that's a miss!",
        "No points on that mid shot!"
        };
        outsideMiss = new string[]
        {
                 "Little off-target with that one!",
        "Deep chance, wasted effort!",
        "Out of his range, that's a miss!",
        "Risk and no rewards on that 6!"
        };
        jukeSuccess = new string[]
        {
              "Oh! That was a killer fake-out!",
        "Psyche! That's a juke folks!",
        "The double-move got em!",
        "Studder-step success!"
        };
        stealSuccess = new string[]
        {
             "Stolen! Going the other way",
        "The double-move got em!",
        "A well-timed steal!",
        "Turnover, that's a steal!"
        };
        minorFouls = new string[]
        {
          "A bit too physical, minor foul!",
        "The ref called it, a minor foul!",
        "That's a foul and two chances!",
        "Lack of awareness, foul!"
        };
        majorFouls = new string[]
        {
          "Way too rough on that play!",
        "Ref's all over that, major foul!",
        "Emotions rising, a major foul!",
        "C'mon, that's not allowed! Big foul!"
        };


    }

    public string chooseRandom(string action)
    {
        switch(action)
        {
            case "insideMade":
                return insideShots[Random.Range(0, insideShots.Length)];
        case "midMade":
                return midShots[Random.Range(0, midShots.Length)];
                case "outsideMade":
                return outsideTexts[Random.Range(0, outsideTexts.Length)];
                case "insideMiss":
                return insideMiss[Random.Range(0, insideMiss.Length)];
                case "midMiss":
                return midMiss[Random.Range(0, midMiss.Length)];
                    case "outsideMiss":
                return outsideMiss[Random.Range(0, outsideMiss.Length)];
                case "jukeSuccess":
                return jukeSuccess[Random.Range(0, jukeSuccess.Length)];
                case "stealSuccess":
                return stealSuccess[Random.Range(0, stealSuccess.Length)];
                case "minorFoul":
                return minorFouls[Random.Range(0, minorFouls.Length)];
                case "majorFoul":
                return majorFouls[Random.Range(0, majorFouls.Length)];
                default:
                return "No action found";

        }
    }
}
