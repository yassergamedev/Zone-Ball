using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{

    public NumberManager numberManager;

    //gameflow variables
    public bool isGuarded = false;
    public bool isJuked = false;
    public bool isShooting = false;
    public int foulOutThreshold = 0;
    public bool foulout = false;

    //game flow stats

    //---------- defensive ------------

    public int pressures = 0;
    public int blocks = 0;
    public int steals = 0;
    public int fouls = 0;
    public int pointsAllowed = 0;

    //---------- offensive ------------

    public int jukes = 0;
    //public int shot percentage in db
    public int shots = 0;
    public int shotsTaken = 0;
    public int pointsScored = 0;
    public int foulShots = 0;
    public int foulShotsMade = 0;
    public int foulPointsScored = 0;
    public int turnovers = 0;

    public int plays = 0;
    public int maxPlays;
   
    //---------- zone ------------
    public int insideShots = 0;
    public int insideShotsMade = 0;
    public int midShots = 0;
    public int midShotsMade = 0;
    public int outsideShots = 0;
    public int outsideShotsMade = 0;

    public List<int[]> zoneStyles = new List<int[]>()
    {
        new int[] {20, 30, 50},
        new int[] {20, 40, 40},
        new int[] {20, 50, 30},
        new int[] {30, 20, 50},
        new int[] {30, 30, 40},
        new int[] {30, 40, 30},
        new int[] {30, 50, 20},
        new int[] {40, 20, 40},
        new int[] {40, 30, 30},
        new int[] {40, 40, 20},
        new int[] {50, 20, 30},
        new int[] {50, 30, 20},
    };
    // Player basic info
    public string Name;
    public int Number;
    public int Age;
    public int YearsPro;
    public Contract contract;

    // Player stats with potential
    public Stat consistency;
    public Stat awareness;

    public Stat juking;
    public Stat control;
    public Stat shooting;

    public Stat positioning;
    public Stat steal;
    public Stat guarding;
    public Stat pressure;

    public Stat inside;
    public Stat mid;
    public Stat Outside;
 

    // Hidden stats
    public int learning; // Range: 1 to 5
    public int longevity; // Range: 1 to 5
    public int personality; // Range: 1 to 5
    public int[] zoneStyle; // Range: 1 to 12


    //arrays
    public List<(string, Stat)> stats;

    private void Start()
    {

        consistency = new Stat(Random.Range(10, 100));
        awareness = new Stat(Random.Range(10, 100));
        juking = new Stat(Random.Range(10, 100));
            control = new Stat(Random.Range(10, 100));
        shooting = new Stat(Random.Range(10, 100));
        positioning = new Stat(Random.Range(10, 100));
        steal = new Stat(Random.Range(10, 100));
        guarding = new Stat(Random.Range(10, 100));
        pressure = new Stat(Random.Range(10, 100));
        inside = new Stat(Random.Range(10, 100));
        mid = new Stat(Random.Range(10, 100));
        Outside = new Stat(Random.Range(10, 100));

        stats = new List<(string, Stat)>{ ("Consistency", consistency), ("Awareness", awareness), ("Juking", juking), ("Control", control), ("Shooting", shooting), ("Positioning", positioning), ("Steal", steal), ("Guarding", guarding), ("Pressure", pressure), ("Inside", inside), ("Mid", mid), ("Outside", Outside) };
    }
    private void Update()
    {
        if (foulOutThreshold >= 3)
        {
            foulout = true;
            foulOutThreshold = 0;
        }
    }
    // Constructor
    public Player(string name, int number, int age, Contract con,
                      Stat cons, Stat awa, 
                      Stat guard, Stat stl, Stat shoot,
                      Stat pos, Stat juk, Stat press,
                      Stat ins, Stat mids, Stat outs
                      )
    {
        Name = name;
        Number = number;
        numberManager.SetPlayerNumber(number);
        Age = age;
        contract = con;
        consistency = cons;
        awareness = awa;
 
        guarding = guard;
        steal = stl;
        shooting = shoot;
        positioning = pos;
        juking = juk;
        pressure = press;

        inside = ins;
        mid = mids;
        Outside = outs;

        learning = Random.Range( 1, 5);
        longevity = Random.Range(1, 5);
        personality = Random.Range(1, 5);
        int z = Random.Range(1, 12);
        zoneStyle = zoneStyles[z];
    }
}


[System.Serializable]
public struct Contract
{
    public int years;
    public float salary; // Min: 5000, Max: 90000

    // Constructor
    public Contract(int years, float salary)
    {
        this.years = years;
        this.salary = Mathf.Clamp(salary, 5000, 90000);
    }
}

[System.Serializable]
public struct Stat
{
    public int value;
    public int potential; // Maximum value the stat can reach

    // Constructor
    public Stat(int value)
    {
       
        this.value = value;
       
        this.potential = Random.Range(this.value, 100);
        

    }
}




