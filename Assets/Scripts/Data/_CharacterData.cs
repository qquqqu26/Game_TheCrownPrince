using System.Collections.Generic;

#region 데이터목록
[System.Serializable]
public class _CharacterData
{
    public string ID;
    public string spriteName;
    public int characTextID;

}
#endregion

[System.Serializable]
public class _CharacterDataLoader : ILoader<string, _CharacterData>
{

    public List<_CharacterData> data;

    public Dictionary<string, _CharacterData> MakeDict()
    {
        Dictionary<string, _CharacterData> dict = new Dictionary<string, _CharacterData>();
        foreach (_CharacterData charac in data)
            dict.Add(charac.ID, charac);

        return dict;
    }

}
