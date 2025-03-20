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
    static Managers s_instance; //���ϼ� ����
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

    #region ����� ��� ����
    [Header("���丮 �ý��� ���ŷ ���")]
    [SerializeField] public bool doesStorySkip = false;
    [SerializeField] public bool doesDialogueSkip = false;
    [SerializeField] public bool isTypeEffectOn = false;


    #endregion

    public static string savePath;
    public static bool IsPreloaded { get; private set; } = false;

    public static string GetText(int id)
    {
        // 1?. �־��� ID�� �ش��ϴ� �ؽ�Ʈ �����͸� ������
        if (Managers.Data.UITexts.TryGetValue(id, out UITextData value) == false)
        {
            Debug.Log($"{id}�� �ؽ�Ʈ �� ��� ������");
            return ""; // ID�� �������� ������ �� ���ڿ� ��ȯ

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


            DontDestroyOnLoad(go); //���� ����
            DontDestroyOnLoad(scene); //���� ����
            s_instance = go.GetComponent<Managers>();

            //�����ε�
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

        yield return _data.Init(); // ������ �Ŵ��� �ʱ�ȭ (�񵿱� �ε�)

        IsPreloaded = true; // �����ε� �Ϸ� �÷��� ����
        Managers.UI.CloseSceneUI();

        GameObject.Find("@Scene").AddComponent<GameScene>();

    }

}

