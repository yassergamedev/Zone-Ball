using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class PlayerPersistent 
{
    private int id;


    // Player basic info
    public string Name;
    public int Number;
    public int Age;
    public int YearsPro;

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

    

    // Hidden stats
    public int learning; // Range: 1 to 5
    public int longevity; // Range: 1 to 5
    public int personality; // Range: 1 to 5
    public int[] zoneStyle; // Range: 1 to 12
    public int ovrl;

    //setters for all stats
    public void setConsistency(int value)
    {
        consistency.value = value;
    }
    public void setAwareness(int value)
    {
        awareness.value = value;
    }
    public void setJuking(int value)
    {
        juking.value = value;
    }
    public void setControl(int value)
    {
        control.value = value;
    }
    public void setShooting(int value)
    {
        shooting.value = value;
    }
    public void setPositioning(int value)
    {
        positioning.value = value;
    }
    public void setSteal(int value)
    {
        steal.value = value;
    }
    public void setGuarding(int value)
    {
        guarding.value = value;
    }
    public void setPressure(int value)
    {
        pressure.value = value;
    }
    public void setInside(int value)
    {
        inside.value = value;
    }
    public void setMid(int value)
    {
        mid.value = value;
    }
    public void setOutside(int value)
    {
        Outside.value = value;
    }


    //arrays
    public List<(string, Func<StatPersistent>, Action<int>)> getStats() {

        return new List<(string, Func<StatPersistent>, Action<int>)> { ("Consistency",() => consistency, setConsistency), ("Awareness",() => awareness, setAwareness), ("Juking",() => juking, setJuking),
                  ("Control",() => control, setControl), ("Shooting",() => shooting, setShooting), ("Positioning",() => positioning, setPositioning),
                  ("Steal", () =>steal, setSteal), ("Guarding", () =>guarding, setGuarding), ("Pressure",() => pressure, setPressure), ("Inside",() => inside, setInside),
                  ("Mid", () =>mid, setMid), ("Outside",() => Outside, setOutside) };


    }
  
  
 
    // Constructor
    public PlayerPersistent(int id,
        string name, int num, int yers, int age,
        int ovrl, ContractPersistent c)
    {

        this.id = id;
        name = Name;
        num = Number;
        yers = YearsPro;
        age = Age;
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
      
        //we calculate a new overall and adjust the stat values based on it
        int newovrl = (consistency.value + awareness.value + juking.value + control.value +
                       shooting.value + positioning.value + steal.value + guarding.value + pressure.value +
                                  inside.value + mid.value + Outside.value) / 12;

        int dif = ovrl - newovrl;

        for(int i = 0; 0<dif*12; i++)
        {
            int randomStat = UnityEngine.Random.Range(0, 12);

            getStats()[randomStat].Item3(getStats()[randomStat].Item2().value + 1);
            
        }

       
        contract = c;

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

    // Constructor
    public StatPersistent(int value)
    {

        this.value = value;

        this.potential = UnityEngine.Random.Range(this.value, 100);


    }
}




