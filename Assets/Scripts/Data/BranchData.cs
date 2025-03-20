using System;
using System.Collections.Generic;

#region 데이터목록
[Serializable]
public class BranchData
{
    public int ID;
    public int standardStat;
    public int standard;
    public int trueID;
    public int falseID;
    public int statP;
    public int numP;
    public int statN;
    public int numN;
}
#endregion

[System.Serializable]
public class BranchDataLoader : ILoader<int, BranchData>
{
    public List<BranchData> data;

    public Dictionary<int, BranchData> MakeDict()
    {
        Dictionary<int, BranchData> dict = new Dictionary<int, BranchData>();
        foreach (BranchData _text in data)
            dict.Add(_text.ID, _text);

        return dict;
    }
}
