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
    public PlayerStatsPersistent stats;
    public string[] gameFlowStats;
    

    // Hidden stats
    public int learning; // Range: 1 to 5
    public int longevity; // Range: 1 to 5
    public int personality; // Range: 1 to 5
    public int[] zoneStyle; // Range: 1 to 12
    public int ovrl,prevOvrl;
    public int plays;
    public int defPlays;
    public bool isJuked;
    public string team, prevTeam,status;
    public bool hasExtended = false;
    
    public int OPOY, DPOY, AllStarSelections, championships, draftSelections, teamDrafted;

    //setters for all stats
 

    //arrays
    public List<(string, Func<StatPersistent>, Action<int>, Action<int>, Action<int>)> getStats() {

        return new List<(string, Func<StatPersistent>, Action<int>, Action<int>, Action<int>)> { 
            
            ("Consistency",() => consistency,(val)=>consistency.setPrevValue(val),(val)=>consistency.setValue(val),(val)=>consistency.setPotential(val))
            
            , ("Awareness",() => awareness,(val)=>awareness.setPrevValue(val),(val)=>awareness.setValue(val),(val)=>awareness.setPotential(val)),
            ("Juking",() => juking,(val) => juking.setPrevValue(val),(val) => juking.setValue(val),(val)=>juking.setPotential(val)),
                  ("Control",() => control,(val)=>control.setPrevValue(val),(val)=>control.setValue(val),(val)=>control.setPotential(val)),
            ("Shooting",() => shooting,(val)=>shooting.setPrevValue(val),(val)=>shooting.setValue(val),(val)=>shooting.setPotential(val)),
            ("Positioning",() => positioning,(val)=>positioning.setPrevValue(val),(val)=>positioning.setValue(val),(val)=>positioning.setPotential(val)),
                  ("Steal", () =>steal,(val)=>steal.setPrevValue(val),(val)=>steal.setValue(val),(val)=>steal.setPotential(val)),
            ("Guarding", () =>guarding,(val)=>guarding.setPrevValue(val),(val)=>guarding.setValue(val),(val)=>guarding.setPotential(val)),
            ("Pressure",() => pressure,(val)=>pressure.setPrevValue(val),(val)=>pressure.setValue(val),(val)=>pressure.setPotential(val)), 
            ("Inside",() => inside,(val)=>inside.setPrevValue(val),(val)=>inside.setValue(val),(val)=>inside.setPotential(val)),
                  ("Mid", () =>mid,(val)=>mid.setPrevValue(val),(val)=>mid.setValue(val),(val)=>mid.setPotential(val)),
            ("Outside",() => Outside,(val)=>Outside.setPrevValue(val),(val)=>Outside.setValue(val),(val)=>Outside.setPotential(val)) };


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
        int overall, string status)
    {

        this.id = id;
        Name = name;
        this.type = type;
        Number = num;
        YearsPro = yers;
        Age = age;
        ovrl = overall;
        this.status = status;
        prevOvrl = ovrl;
        Debug.Log(ovrl);
        // based on the ovrl we distribute the stats 
        hasExtended = false;
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
        stats = new(name);
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
        List<(string, Func<StatPersistent>, Action<int>, Action<int>, Action<int>)> list = getStats();
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
                    randomStat = UnityEngine.Random.Range(2, 6);
                else
                    randomStat = UnityEngine.Random.Range(10, 12);
            }


            list[randomStat].Item4(list[randomStat].Item2().value + 1);
            list[randomStat].Item3(list[randomStat].Item2().value );
            if (list[randomStat].Item2().value > list[randomStat].Item2().potential)
            {
                list[randomStat].Item5(list[randomStat].Item2().potential + 1);
            }


        }

        gameFlowStats = new string[] { };
        plays = 4;
        defPlays = 4;

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
    public float salary;
   
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
    public int prevValue;
    public void setValue(int val)
    {
        value = val;
    }
    public void setPrevValue(int val)
    {
        prevValue = val;
    }
    public void setPotential(int val)
    {
        potential = val;
    }
    // Constructor
    public StatPersistent(int value)
    {

        this.value = value;
        this.prevValue = value;
        this.potential = UnityEngine.Random.Range(value, 100);


    }
}




