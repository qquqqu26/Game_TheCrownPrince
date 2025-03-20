using System;
using System.Collections.Generic;

[System.Serializable]
public class StartData 
{
    public int month;
    public int year;

    public float startValue = 0f;

}

/*[System.Serializable]
public class StartDataLoader : ILoader<int, StartData>
{
    public List<StartData> data = new List<StartData>();

    public Dictionary<int, StartData> MakeDict()
    {
        Dictionary<int, StartData> dic = new Dictionary<int, StartData>();

        foreach (StartData _data in data)
            dic.Add(_data.ID, _data);

        return dic;
    }
}*/