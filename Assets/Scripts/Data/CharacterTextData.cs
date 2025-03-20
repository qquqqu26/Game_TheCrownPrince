using System;
using System.Collections.Generic;

#region 데이터목록
[Serializable]
public class CharacterTextData
{
    public int ID;
    public string kor;
    public string eng;
    public string chn;
    public string jpn;
}
#endregion

[System.Serializable]
public class CharacterTextDataLoader : ILoader<int, CharacterTextData>
{
    public List<CharacterTextData> data;

    public Dictionary<int, CharacterTextData> MakeDict()
    {
        Dictionary<int, CharacterTextData> dict = new Dictionary<int, CharacterTextData>();
        foreach (CharacterTextData _text in data)
            dict.Add(_text.ID, _text);

        return dict;
    }
}
