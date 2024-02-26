using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{

    public NumberManager numberManager;
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
    public int zoneStyle; // Range: 1 to 12

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
        learning = Mathf.Clamp(learn, 1, 5);
        longevity = Mathf.Clamp(longev, 1, 5);
        personality = Mathf.Clamp(perso, 1, 5);
        zoneStyle = Mathf.Clamp(zone, 1, 12);
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
