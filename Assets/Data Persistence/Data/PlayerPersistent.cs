using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class PlayerPersistent 
{

    public string id;


    // Player basic info
    public string Name;
    public int Number;
    public int Age;
    public int YearsPro;
    public string type;

    public ContractPersistent contract;

    // Player stats with potential
    public StatPersistent consistency;
    public StatPersistent awareness;

    public StatPersistent juking;
    public StatPersistent control;
    public StatPersistent shooting;

    public StatPersistent positioning;
    public StatPersistent steal;
    public StatPersistent guarding;
    public StatPersistent pressure;

    public StatPersistent inside;
    public StatPersistent mid;
    public StatPersistent Outside;

    public string[] gameFlowStats;
    

    // Hidden stats
    public int learning; // Range: 1 to 5
    public int longevity; // Range: 1 to 5
    public int personality; // Range: 1 to 5
    public int[] zoneStyle; // Range: 1 to 12
    public int ovrl;

    //setters for all stats
 

    //arrays
    public List<(string, Func<StatPersistent>)> getStats() {

        return new List<(string, Func<StatPersistent>)> { ("Consistency",() => consistency), ("Awareness",() => awareness), ("Juking",() => juking),
                  ("Control",() => control), ("Shooting",() => shooting), ("Positioning",() => positioning),
                  ("Steal", () =>steal), ("Guarding", () =>guarding), ("Pressure",() => pressure), ("Inside",() => inside),
                  ("Mid", () =>mid), ("Outside",() => Outside) };


    }
    //a copy constructor
    public PlayerPersistent(PlayerPersistent p)
    {
        id = p.id;
        Name = p.Name;
        Number = p.Number;
        YearsPro = p.YearsPro;
        Age = p.Age;
        ovrl = p.ovrl;
        consistency = p.consistency;
        awareness = p.awareness;
        juking = p.juking;
        control = p.control;
        shooting = p.shooting;
        positioning = p.positioning;
        steal = p.steal;
        guarding = p.guarding;
        pressure = p.pressure;
        inside = p.inside;
        mid = p.mid;
        Outside = p.Outside;
        learning = p.learning;
        longevity = p.longevity;
        personality = p.personality;
        zoneStyle = p.zoneStyle;
        contract = p.contract;
    }




    // Constructor
    public PlayerPersistent(string id, int num, string name, string type,
        int yers, int age,
        int overall)
    {

        this.id = id;
        Name = name;
        this.type = type;
        Number = num;
        YearsPro = yers;
        Age = age;
        ovrl = overall;
        Debug.Log(ovrl);
        // based on the ovrl we distribute the stats 

        consistency = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        awareness = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        juking = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        control = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        shooting = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        positioning = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        steal = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        guarding = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        pressure = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        inside = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        mid = new StatPersistent(UnityEngine.Random.Range(10, ovrl));
        Outside = new StatPersistent(UnityEngine.Random.Range(10, ovrl));

        learning = UnityEngine.Random.Range(1, 5);
        longevity = UnityEngine.Random.Range(1, 5);
        personality = UnityEngine.Random.Range(1, 5);
        int z = UnityEngine.Random.Range(1, 12);
        int pay = 0;
        if (ovrl >= 55)
        {
            pay = 40000;
        }
        else
        {
            if (ovrl >= 50)
            {
                pay = 30000;
            }
            else if (ovrl >= 45)
            {
                pay = 20000;
            }
            else
            {
                if (ovrl >= 40)
                {
                    pay = 15000;
                } else
                {
                    if (ovrl >= 31)
                    {
                        pay = 10000;
                    }

                }

            }
        }

        contract = new ContractPersistent(UnityEngine.Random.Range(1, 5), pay);

        List<int[]> zoneStyles = new List<int[]>()
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
        zoneStyle = zoneStyles[z];

        //declaring the stats list
        List<(string, Func<StatPersistent>)> list = getStats();
        //we calculate a new overall and adjust the stat values based on it
        int newovrl = (consistency.value + awareness.value + juking.value + control.value +
                     shooting.value + positioning.value + steal.value + guarding.value + pressure.value +
                                inside.value + mid.value + Outside.value) / 12;

        int dif = ovrl - newovrl;

        dif *= 12;
        for (int i = 0; i < dif; i++)
        {
            int randomStat;
            if (type == "def")
            {
                randomStat = UnityEngine.Random.Range(6, 12);
            }
            else
            {
                int randomNum = UnityEngine.Random.Range(0, 2);
                if (randomNum == 0)
                    randomStat = UnityEngine.Random.Range(3, 6);
                else
                    randomStat = UnityEngine.Random.Range(10, 12);
            }


            list[randomStat].Item2().setValue(list[randomStat].Item2().value + 1);
            if (list[randomStat].Item2().value > list[randomStat].Item2().potential)
            {
                list[randomStat].Item2().setPotential(list[randomStat].Item2().potential + 1);
            }


        }

        gameFlowStats = new string[] { };
        

    }
    public string GenerateRandomPlayerName(List<string> firstNames, List<string> lastNames, int firstCount, int lastCount)
    {
        // Read all lines from first names file into a list


        // Select a random first name and remove it from the list
        int firstNameIndex = UnityEngine.Random.Range(0, firstCount);
        string firstName = firstNames[firstNameIndex];
        firstNames.RemoveAt(firstNameIndex);

        // Select a random last name and remove it from the list
        int lastNameIndex = UnityEngine.Random.Range(0, lastCount);
        string lastName = lastNames[lastNameIndex];
        lastNames.RemoveAt(lastNameIndex);

        // Combine the first and last names to create the player's name
        string playerName = firstName + " " + lastName;

        // Write back remaining names to files
        File.WriteAllLines("first names.txt", firstNames.ToArray());
        File.WriteAllLines("last names.txt", lastNames.ToArray());

        return playerName;
    }

}


[System.Serializable]
public struct ContractPersistent
{
    public int years;
    public float salary; // Min: 5000, Max: 90000

    // Constructor
    public ContractPersistent(int years, int salary)
    {
        this.years = years;
        this.salary = Mathf.Clamp(salary, 5000, 90000);
    }
}

[System.Serializable]
public struct StatPersistent
{
    public int value;
    public int potential; // Maximum value the stat can reach

    public void setValue(int val)
    {
        value = val;
    }
    public void setPotential(int val)
    {
        potential = val;
    }
    // Constructor
    public StatPersistent(int value)
    {

        this.value = value;

        this.potential = UnityEngine.Random.Range(value, 100);


    }
}




