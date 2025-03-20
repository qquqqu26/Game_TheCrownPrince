using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static Define;

[Serializable]
public enum Record
{
    //Ŀ���͸���¡ �ʿ�
    None,
    Uncheck,
    Done
}

[Serializable]
public class GameData{


    #region 1. �ɷ�ġ
    public Dictionary<int, PlayerStatData> stat;
    public float stress;
    public float stamina;
    public float loyalty;
    public float majesty;

/*    public float yehak;        // ���� (����)
    public float akhak;        // ���� (����)
    public float sahak;        // ���� (����)
    public float uhak;         // ���� (����)
    public float seohak;       // ���� (����)
    public float suhak;        // ���� (���)

    public float ihak;         // ���� (����)
    public float sulhak;      // (��)���� 
    public float umyanghak;    // ������ (������)
    public float uihak;        // ���� (���)
    public float yulhak;       // ���� (����)
    public float nongjeong;    // ���� (����)
    public float yeoksahak;    // ������ (����)*/
    #endregion

    #region 2. ����
    public float money;
    #endregion

    public int year;
    public int month;
    public IMonthState state;

    public int MaxMonth;
    //Ŭ������ ���� 
    public Record[] endings = new Record[MAX_ENDING_COUNT];
}

public class GameManagerEx 
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value;  } }

    #region 1. �ɷ�ġ
    public Dictionary<int, PlayerStatData> stats = new Dictionary<int, PlayerStatData>();

    public int statMaxValue = 100;
    public float stress
    {
        get { return _gameData.stress; }
        set { _gameData.stress = value;}
    }

    public float stamina
    {
        get { return _gameData.stamina; }
        set { _gameData.stamina = value; }
    }

    public float loyalty
    {
        get { return _gameData.loyalty; }
        set { _gameData.loyalty = value; }
    }

    public float majesty
    {
        get { return _gameData.majesty; }
        set { _gameData.majesty = value; }
    }


   /* public float yehak
    {
        get { return _gameData.yehak; }
        set { _gameData.yehak = value; }
    }

    public float akhak
    {
        get { return _gameData.akhak; }
        set { _gameData.akhak = value; }
    }

    public float sahak
    {
        get { return _gameData.sahak; }
        set { _gameData.sahak = value; }
    }

    public float uhak
    {
        get { return _gameData.uhak; }
        set { _gameData.uhak = value; }
    }

    public float seohak
    {
        get { return _gameData.seohak; }
        set { _gameData.seohak = value; }
    }

    public float suhak
    {
        get { return _gameData.suhak; }
        set { _gameData.suhak = value; }
    }

    public float ihak
    {
        get { return _gameData.ihak; }
        set { _gameData.ihak = value; }
    }

    public float sulhak
    {
        get { return _gameData.sulhak; }
        set { _gameData.sulhak = value; }
    }

    public float umyanghak
    {
        get { return _gameData.umyanghak; }
        set { _gameData.umyanghak = value; }
    }

    public float uihak
    {
        get { return _gameData.uihak; }
        set { _gameData.uihak = value; }
    }

    public float yulhak
    {
        get { return _gameData.yulhak; }
        set { _gameData.yulhak = value; }
    }

    public float nongjeong
    {
        get { return _gameData.nongjeong; }
        set { _gameData.nongjeong = value; }
    }

    public float yeoksahak
    {
        get { return _gameData.yeoksahak; }
        set { _gameData.yeoksahak = value; }
    }
*/
    #endregion

    #region 3. �ð�
    public int month
    {
        get { return _gameData.month; }
        set { _gameData.month = value; }
    }
    public int year
    {
        get { return _gameData.year; }
        set { _gameData.year = value; }
    }

/*    public IMonthState state
    {
        get { return _gameData.state; }
        set
        {
            //state?.Exit();
            state = value;
            state.Enter();
        }
    }*/
    
    public int MaxMonth
    {
        get { return _gameData.MaxMonth; }
        set { _gameData.MaxMonth = value; }
    }


    #endregion

    #region 4. ��Ÿ
    public static int maxStat
    {
        get { return maxStat; }
        set { maxStat = value; }
    }
    #endregion

    //public UI_Warning.
    public void Init()
    {
        StartData startData = Managers.Data.Start;

        
        if(startData == null )
        {
            Debug.Log("[GameManagerEx] StartData�� �ε���� �ʾҽ��ϴ�!");
            return;
        }


        #region 1. �ɷ�ġ �ʱ�ȭ
        //DataManager.PlayerStats�� GameManagerEx�� Stats�� ���� ����
        stats = new Dictionary<int, PlayerStatData>();

        foreach (var pair in Managers.Data.PlayerStats)
        {
            PlayerStatData original = pair.Value;

            PlayerStatData copy = new PlayerStatData()
            {
                ID = original.ID,
                type = original.type,
                value = original.value,
                IconPath = original.IconPath
            };
            stats.Add(pair.Key, copy);
        }

        stress = startData.startValue;
        stamina = startData.startValue;
        majesty = startData.startValue;
        loyalty = startData.startValue;
        #endregion

        year = startData.year;
        month = startData.month;
        MaxMonth = 12 * 6;
    }
    #region Save&Load

    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        if (!File.Exists(Managers.savePath))
        {
            Debug.Log($"���: {Managers.savePath}");
            Debug.LogWarning("[WARNING] ������ �������� ����. �⺻ ������ ���� ��...");
            string defaultJson = "{}"; // �⺻ JSON ��
            File.WriteAllText(Managers.savePath, defaultJson);
        }

        File.WriteAllText(Managers.savePath, jsonStr);
        Debug.Log($"Save Game Completed : {Managers.savePath}");

    }

    public bool LoadGame()
    {
        //�ε��ߴ� ���� ������ �ִ��� ��ȯ��
        if (File.Exists(Managers.savePath) == false)
            return false;

        //�����ߴ� ������ ������
        string fileStr = File.ReadAllText(Managers.savePath);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if(data != null )
        {
            Managers.Game.SaveData = data;
        }

        Debug.Log($"Save Game Loaded : {Managers.savePath}");
        return true;
    }
    #endregion
}
