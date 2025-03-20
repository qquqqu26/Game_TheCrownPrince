using System;
using System.Collections.Generic;

#region 데이터목록
[Serializable]
public class UITextData
{
    public int ID;
    public string kor;
    public string eng;
    public string chn;
    public string jpn;
}
#endregion

[System.Serializable]
public class UITextDataLoader : ILoader<int, UITextData>
{
    public List<UITextData> data;

    public Dictionary<int, UITextData> MakeDict()
    {
        Dictionary<int, UITextData> dict = new Dictionary<int, UITextData>();
        foreach (UITextData _text in data)
            dict.Add(_text.ID, _text);

        return dict;
    }
}
