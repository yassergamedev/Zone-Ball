using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class TeamPersistent 
{
    public int id;
    public string name;
    public int[] players;
    public TeamPersistent(int id, string name)
   {
        this.id = id;
        this.name = name;
       
        int salaryCap = 350000;
        int randomRange = 100000;
        int sal=0, salIncrement = 0;
        int ovrl = 0;
        for(int i = 0; i < 12; i++)
        {
           
            if (randomRange >10000)
            {
                sal = RoundToNearestThousand(UnityEngine.Random.Range(10000, randomRange));
                randomRange -= sal;
            }
            else
            {
                sal = RoundToNearestThousand(UnityEngine.Random.Range(10000, 20000));
            }

            for (int j = 10000, b = 31, u = 40; j < 100000; j += 5000, b+=5, u+=5)
            {
                if (b == 36) b = 40;
                if(j<sal)
                {
                    ovrl = UnityEngine.Random.Range(b, u);
                }
            }
           
         
            int yrs = UnityEngine.Random.Range(1, 5);
            int age = UnityEngine.Random.Range(18, 34);
            ContractPersistent con = new ContractPersistent(yrs, sal);
          //  players[i] = new PlayerPersistent((int)Time.time + i, "Player " + i,i, yrs, age, ovrl, con);
        }
    }

    private int RoundToNearestThousand(int value)
    {
        return ((value + 500) / 1000) * 1000; // Round to the nearest thousand
    }
}
