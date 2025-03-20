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

    //���� �Լ�
    //����� �Ѱ��ִ� �Լ�, �Ű����ø��� ��¥ type, �Ű������� ����� type �� enum�� �ȴ�.
/*    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); //��ǻ� type�� enum�̴�
        //names = { startTxt, quitTxt, titleTxt}
        //�̷��� ������?

            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); //��ųʸ��� �߰�, �ٵ� objects�� ���� ���� ����?

        //Ž���ϸ� ������Ʈ ����: �ֻ�� obj������ �� ��ȸ�ϸ� ���� �̸� �ִ� ��
        for (int i = 0; i < names.Length; i++)
        {
            //������Ʈ�� �ƴ� ���� ������Ʈ�� ������ ��� ����
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }

            //������Ʈ ������ ���
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
                Debug.LogError($"[Bind] ���� to find object: {names[i]}");
            else
                Debug.Log($"[Bind] ������ bound object: {names[i]}");

        }

    }*/

    protected void Bind<T>(Type type, bool includeInactive = false) where T : UnityEngine.Object
    {
            string[] names = Enum.GetNames(type); //��ǻ� type�� enum�̴�

            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects); //��ųʸ��� �߰�, �ٵ� objects�� ���� ���� ����?

            //Ž���ϸ� ������Ʈ ���� : �ֻ�� obj������ �� ��ȸ�ϸ� ���� �̸� �ִ� ��
            for (int i = 0; i < names.Length; i++)
            {
                //������Ʈ�� �ƴ� ���� ������Ʈ�� ������ ��� ����
                if (typeof(T) == typeof(GameObject))
                {
                    objects[i] = Util.FindChild(gameObject, names[i], true, includeInactive);
                }

                //������Ʈ ������ ���
                else
                {
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true, includeInactive);
                }

/*            if (objects[i] == null)
                Debug.LogError($"[Bind] ���� to find object: {names[i]}");
            else
                Debug.Log($"[Bind ����]: {names[i]}");*/


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
        //��ųʸ��� _objects�� Ÿ���� ������ ������, ������ �� null��ȯ&����
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        //������ UnityEngine.Object Ÿ�Կ��� T�� ĳ�����ؼ� ��ȯ
        return objects[idx] as T;
    }

    //���� ����ϴ� ��ҿ� ���� ���Լ� ������ 3��
    protected TextMeshProUGUI GetText(int idx)
    {
/*        if (!Managers.Data.IsDataLoaded)
        {
            Debug.LogWarning($"[Managers] �����Ͱ� ���� �ε���� �ʾҽ��ϴ�. ID({idx})�� �ؽ�Ʈ�� ������ �� �����ϴ�.");
        }*/
        return Get<TextMeshProUGUI>(idx);
    }
    
    protected Button GetButton(int idx) { return Get<Button>(idx); }

    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx);  }
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        /*����:
        go ���� ������Ʈ�� �ٿ����Ҵ� ��ũ��Ʈ(UI_EventHandler)�� GetComponent(����) => evt
        �� �̺�Ʈ�� �޼ҵ�(action) ������Ű��.
        �̺�Ʈ �� �ڵ鷯 ���� (Define���� ����)
         */
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        if (evt == null)
        {
            Debug.LogError($"[AddUIEvent] {go.name}�� UI_EventHandler�� �߰��� �� ����.");
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
