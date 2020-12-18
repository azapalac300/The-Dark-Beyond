using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameData{
    public static float PlayRadius
    {
        get
        {
            return 16000;
        }
    }

    public static bool Chance(float probability)
    {
        //Probability should be a number between 0 and 1
        return UnityEngine.Random.Range(0f, 1f) < probability;
    }

    public static float calculateRange(int i)
    {
        return UnityEngine.Random.Range(planetRanges[i][0], planetRanges[i][1]);
    }

    public static float [][] planetRanges { get
        {
            return new float[][]{
                new float[]{300, 550},
                new float[]{850, 1050},
                new float[]{1350, 1550},
                new float[]{1950, 2350},
                new float[]{2700, 3000},
                new float[]{3300, 3700},
            };
        }

     }


    public static float[][] moonRanges
    {
        get
        {
            return new float[][]{
                new float[]{100, 150}
            };
        }

    }
    public static float percentToDecimal(float percent)
    {
        return (percent / 100) + 1;
    }

    public static float decimalToPercent(float dec)
    {
        return (dec - 1) * 100;
    }


}




public class Map<A, B>
{
    private List<B> range;
    private Dictionary<A, int> map;
    public int Count { get { return range.Count; } }

    public Map()
    {
        range = new List<B>();
        map = new Dictionary<A, int>();
    }

    public B this[A key]
    {
        get { return range[map[key]]; }
        set { range[map[key]] = value; }
    }

    public B this[int index]
    {
        get { return range[index]; }
        set { range[index] = value; }
    }

    public bool ContainsKey(A key)
    {
        return map.ContainsKey(key);
    }

    public void Add(B rangeItem, A key)
    {
        range.Add(rangeItem);
        map.Add(key, range.Count - 1);
    }

    public void Remove(A key)
    {
        range.Remove(this[key]);
        map.Remove(key);
    }
}

