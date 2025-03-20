using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region �����͸��
[System.Serializable]
public class PlayerStatData
{
        public int ID;
        public string type;
        public float value;
        public const int maxValue = 100;
        public string IconPath;
}
#endregion

[System.Serializable]
public class PlayerStatDataLoader : ILoader<int, PlayerStatData>
{

    public List<PlayerStatData> data = new List<PlayerStatData>();
    
    public Dictionary<int, PlayerStatData> MakeDict()
    {
        Dictionary<int, PlayerStatData> dic = new Dictionary<int, PlayerStatData>();

        foreach (PlayerStatData _stat in data)
            dic.Add(_stat.ID, _stat);

        return dic;
    }

}