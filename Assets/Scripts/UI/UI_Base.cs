using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected bool _init = false;

    public void Start()
    {
        Init();
    }

    public virtual bool Init()
    {
        if (_init)
            return false;

        return _init = true;
    }

    //원래 함수
    //목록을 넘겨주는 함수, 매개템플릿는 진짜 type, 매개변수는 목록의 type 즉 enum이 된다.
/*    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); //사실상 type은 enum이다
        //names = { startTxt, quitTxt, titleTxt}
        //이렇게 있을까?

            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); //딕셔너리에 추가, 근데 objects는 아직 없을 지도?

        //탐색하며 오브젝트 맵핑: 최상단 obj밑으로 쭉 순회하며 같은 이름 있는 지
        for (int i = 0; i < names.Length; i++)
        {
            //컴포넌트가 아닌 게임 오브젝트를 매핍할 경우 예외
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }

            //컴포넌트 맵핑할 경우
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
                Debug.LogError($"[Bind] 실패 to find object: {names[i]}");
            else
                Debug.Log($"[Bind] 성공적 bound object: {names[i]}");

        }

    }*/

    protected void Bind<T>(Type type, bool includeInactive = false) where T : UnityEngine.Object
    {
            string[] names = Enum.GetNames(type); //사실상 type은 enum이다

            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects); //딕셔너리에 추가, 근데 objects는 아직 없을 지도?

            //탐색하며 오브젝트 맵핑 : 최상단 obj밑으로 쭉 순회하며 같은 이름 있는 지
            for (int i = 0; i < names.Length; i++)
            {
                //컴포넌트가 아닌 게임 오브젝트를 매핍할 경우 예외
                if (typeof(T) == typeof(GameObject))
                {
                    objects[i] = Util.FindChild(gameObject, names[i], true, includeInactive);
                }

                //컴포넌트 맵핑할 경우
                else
                {
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true, includeInactive);
                }

/*            if (objects[i] == null)
                Debug.LogError($"[Bind] 실패 to find object: {names[i]}");
            else
                Debug.Log($"[Bind 성공]: {names[i]}");*/


            }

    }

    protected void GetTexts(int startEnum, int endEnum, int startDefine)
    {
        for (int textID = startEnum; textID <= endEnum; textID++)
            GetText(textID).text = Managers.GetText(startDefine + (textID - startEnum));
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        //딕셔너리에 _objects의 타입이 있으면 꺼내기, 없으면 걍 null반환&종료
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        //있으면 UnityEngine.Object 타입에서 T로 캐스팅해서 반환
        return objects[idx] as T;
    }

    //자주 사용하는 요소에 대한 겟함수 별도로 3개
    protected TextMeshProUGUI GetText(int idx)
    {
/*        if (!Managers.Data.IsDataLoaded)
        {
            Debug.LogWarning($"[Managers] 데이터가 아직 로드되지 않았습니다. ID({idx})의 텍스트를 가져올 수 없습니다.");
        }*/
        return Get<TextMeshProUGUI>(idx);
    }
    
    protected Button GetButton(int idx) { return Get<Button>(idx); }

    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx);  }
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        /*로직:
        go 게임 오브젝트에 붙여놓았던 스크립트(UI_EventHandler)를 GetComponent(추출) => evt
        그 이벤트에 메소드(action) 구독시키기.
        이벤트 중 핸들러 고르기 (Define에서 정의)
         */
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        if (evt == null)
        {
            Debug.LogError($"[AddUIEvent] {go.name}에 UI_EventHandler를 추가할 수 없음.");
            return;
        }
        

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;

            case Define.UIEvent.Enter:
                evt.OnEnterHandler -= action;
                evt.OnEnterHandler += action;
                break;
        }

    }


    public virtual void Bind()
    {
    }

    public virtual void Gets()
    {
    }
    
    public Sprite GetCharacter(string name)
    {
        return Managers.Resource.LoadSprite($"Characters/{name}");

    }
}
