using System.Collections.Generic;
using UnityEngine;

public class EggPool
{
    private List<Egg> poolEggs;

    public EggPool()
    {
        poolEggs = new List<Egg>();
    }

    public Egg GetObject()
    {
        if(poolEggs.Count == 0)
        {
            Debug.Log("Pool rỗng rồi, lấy gì rơi");
            return null;
        }
        return poolEggs[0];
    }

    public void AddObject(Egg egg)
    {
        poolEggs.Add(egg);
    }

    public void RemoveObject()
    {
        if (poolEggs.Count > 0)
        {
            poolEggs.RemoveAt(0);
        }
    }

    public int amoutPool()
    {
        return poolEggs.Count;
    }

}
