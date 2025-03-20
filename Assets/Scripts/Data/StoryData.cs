using System;
using System.Collections.Generic;

#region 데이터목록
[System.Serializable]
public class StoryData 
{
    public int ID; 
    public string charac;
    public string background;
    public int branchID;
    public string kor;
    public string eng;
    public string chn;
    public string jpn;

}
#endregion

[System.Serializable]
public class StoryDataLoader : ILoader<int, StoryData>
{

    public List<StoryData> data;

    public Dictionary<int, StoryData> MakeDict()
    {
        Dictionary<int, StoryData> dict = new Dictionary<int, StoryData>();
        foreach (StoryData story in data)
            dict.Add(story.ID, story);

        return dict;
    }

}
