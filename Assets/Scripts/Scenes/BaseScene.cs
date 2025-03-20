using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    protected bool _init = false; 

    private void Awake()
    {
        Init(); 
    }

    protected virtual bool Init()
    {
        //�ʱ�ȭ�� �� �� ���� �ؾ� �Ѵ�
        if (_init)
            return false;

        _init = true;

        //EventSystem�� ������ �����ϴ� ����
        if (GameObject.FindObjectOfType<EventSystem>() == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";

        return true;

    }

    public abstract void Clear();
}
