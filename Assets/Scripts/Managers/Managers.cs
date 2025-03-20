using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region 0. Instance
    static Managers s_instance; //유일성 보장
    public static Managers Instance { get { Init(); return s_instance; } }

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    DataManager _data = new DataManager();
    GameManagerEx _game = new GameManagerEx();
    TimeManager _time = new TimeManager();
    StoryManager _story = new StoryManager();

    //static UI_Warning _warning = new UI_Warning();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input { get { return Instance._input; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static DataManager Data { get { return Instance._data; } }
    public static GameManagerEx Game { get { return Instance._game; } }
    public static TimeManager Time { get {  return Instance._time; } }
    public static StoryManager Story { get { return Instance._story; } }
    #endregion

    #region 디버깅 모드 관리
    [Header("스토리 시스템 디버킹 모드")]
    [SerializeField] public bool doesStorySkip = false;
    [SerializeField] public bool doesDialogueSkip = false;
    [SerializeField] public bool isTypeEffectOn = false;


    #endregion

    public static string savePath;
    public static bool IsPreloaded { get; private set; } = false;

    public static string GetText(int id)
    {
        // 1?. 주어진 ID에 해당하는 텍스트 데이터를 가져옴
        if (Managers.Data.UITexts.TryGetValue(id, out UITextData value) == false)
        {
            Debug.Log($"{id}의 텍스트 값 없어서 못넣음");
            return ""; // ID가 존재하지 않으면 빈 문자열 반환

        }

        return value.kor;
    }

    public static IEnumerator StartCoroutineStatic(IEnumerator coroutine)
    {
        if (Instance != null)
            yield return s_instance.StartCoroutine(coroutine);
    }


    void Awake()
    {
        savePath = Application.persistentDataPath + "/SaveData.json";
        Init();
    }


    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {

        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null) 
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            GameObject scene = GameObject.Find("@Scene");
            if (scene == null)
            {
                scene = new GameObject { name = "@Scene" };
            }


            DontDestroyOnLoad(go); //삭제 방지
            DontDestroyOnLoad(scene); //삭제 방지
            s_instance = go.GetComponent<Managers>();

            //프리로딩
            Managers.UI.ShowSceneUI<UI_LoadingScene>();
            if (!IsPreloaded)
            {
                s_instance.StartCoroutine(s_instance.PreloadData());
            }
            
        }
    
       
    }

    public static void Clear()
    {

    }

    private IEnumerator PreloadData()
    {
        if (IsPreloaded) yield break;

        yield return _data.Init(); // 데이터 매니저 초기화 (비동기 로딩)

        IsPreloaded = true; // 프리로딩 완료 플래그 설정
        Managers.UI.CloseSceneUI();

        GameObject.Find("@Scene").AddComponent<GameScene>();

    }

}

