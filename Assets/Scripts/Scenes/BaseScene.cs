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
        //초기화는 단 한 번만 해야 한다
        if (_init)
            return false;

        _init = true;

        //EventSystem이 없으면 생성하는 로직
        if (GameObject.FindObjectOfType<EventSystem>() == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";

        return true;

    }

    public abstract void Clear();
}
