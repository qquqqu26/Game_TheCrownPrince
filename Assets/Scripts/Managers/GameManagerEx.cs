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
    //커스터마이징 필요
    None,
    Uncheck,
    Done
}

[Serializable]
public class GameData{


    #region 1. 능력치
    public Dictionary<int, PlayerStatData> stat;
    public float stress;
    public float stamina;
    public float loyalty;
    public float majesty;

/*    public float yehak;        // 예학 (禮學)
    public float akhak;        // 악학 (樂學)
    public float sahak;        // 사학 (射學)
    public float uhak;         // 어학 (御學)
    public float seohak;       // 서학 (書學)
    public float suhak;        // 수학 (數學)

    public float ihak;         // 이학 (理學)
    public float sulhak;      // (기)술학 
    public float umyanghak;    // 음양학 (陰陽學)
    public float uihak;        // 의학 (醫學)
    public float yulhak;       // 율학 (律學)
    public float nongjeong;    // 농정 (農政)
    public float yeoksahak;    // 역사학 (史學)*/
    #endregion

    #region 2. 국력
    public float money;
    #endregion

    public int year;
    public int month;
    public IMonthState state;

    public int MaxMonth;
    //클리어한 엔딩 
    public Record[] endings = new Record[MAX_ENDING_COUNT];
}

public class GameManagerEx 
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value;  } }

    #region 1. 능력치
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

    #region 3. 시간
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

    #region 4. 기타
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
            Debug.Log("[GameManagerEx] StartData가 로드되지 않았습니다!");
            return;
        }


        #region 1. 능력치 초기화
        //DataManager.PlayerStats를 GameManagerEx의 Stats로 깊은 복사
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
            Debug.Log($"경로: {Managers.savePath}");
            Debug.LogWarning("[WARNING] 파일이 존재하지 않음. 기본 데이터 생성 중...");
            string defaultJson = "{}"; // 기본 JSON 값
            File.WriteAllText(Managers.savePath, defaultJson);
        }

        File.WriteAllText(Managers.savePath, jsonStr);
        Debug.Log($"Save Game Completed : {Managers.savePath}");

    }

    public bool LoadGame()
    {
        //로드했던 파일 유무가 있는지 반환함
        if (File.Exists(Managers.savePath) == false)
            return false;

        //저장했던 파일이 있으면
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
