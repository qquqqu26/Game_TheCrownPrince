using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;


public class UI_Button : UI_Popup
{
    enum Buttons
    {

    }
    enum Texts
    {

    }

    enum Images
    {
        Image
    }

    enum GameObjects
    {
        
    }


    public override bool Init()
    {
        if (!base.Init()) 
            return false;

        #region 디버깅: 프리팹 내 게임오브젝트 전체 출력
        Debug.Log($"[Start] mainMenu_PopUp(Clone): {gameObject.name}, Child count: {gameObject.transform.childCount}");

        foreach (Transform child in gameObject.transform)
        {
            Debug.Log($"[Start] Found child: {child.name}");
        }
        #endregion

        #region 1.자동 바인딩
        Bind<UnityEngine.UI.Button>(typeof(Buttons)); //typeof(시리즈)는 enum을 반환한다
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        #endregion

/*
        #region 2-2. 이벤트 추가: 이미지 드래그
        GameObject go = GetImage((int)Images.Image).gameObject;
        AddUIEvent(go,
        (PointerEventData data) => { go.transform.position = data.position; },
        Define.UIEvent.Drag);

        // `Image` 오브젝트에 `UI_EventHandler` 추가
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();
        if (evt == null)
            evt = go.AddComponent<UI_EventHandler>(); // 없으면 추가
        #endregion*/

        return true;
    }


   
}
