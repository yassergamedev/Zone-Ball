using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{

    public NumberManager numberManager;

    //gameflow variables
    public bool isGuarded = false;

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
    public Contract contract;

    // Player stats with potential
    public Stat consistency;
    public Stat awareness;
    public Stat zoning;
    public Stat guarding;
    public Stat steal;
    public Stat shooting;
    public Stat positioning;
    public Stat juking;
    public Stat pressure;

    // Hidden stats
    public int learning; // Range: 1 to 5
    public int longevity; // Range: 1 to 5
    public int personality; // Range: 1 to 5
    public int[] zoneStyle; // Range: 1 to 12

    // Constructor
    public Player(string name, int number, int age, Contract con,
                      Stat cons, Stat awa, Stat zon,
                      Stat guard, Stat stl, Stat shoot,
                      Stat pos, Stat juk, Stat press,
                      int learn, int longev, int perso, int zone)
    {
        Name = name;
        Number = number;
        numberManager.SetPlayerNumber(number);
        Age = age;
        contract = con;
        consistency = cons;
        awareness = awa;
        zoning = zon;
        guarding = guard;
        steal = stl;
        shooting = shoot;
        positioning = pos;
        juking = juk;
        pressure = press;
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
    public Stat(int value, int potential)
    {
        this.value = Mathf.Clamp(value, 10, potential);
        this.potential = potential;
    }
}

