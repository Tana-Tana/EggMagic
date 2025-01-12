using System.Collections.Generic;

public class EggPool
{
    private List<Egg> poolEggs = new List<Egg>();

    public Egg GetObject()
    {
        if(poolEggs.Count == 0)
        {
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


}
