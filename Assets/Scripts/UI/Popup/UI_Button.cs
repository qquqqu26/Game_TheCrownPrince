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

        #region �����: ������ �� ���ӿ�����Ʈ ��ü ���
        Debug.Log($"[Start] mainMenu_PopUp(Clone): {gameObject.name}, Child count: {gameObject.transform.childCount}");

        foreach (Transform child in gameObject.transform)
        {
            Debug.Log($"[Start] Found child: {child.name}");
        }
        #endregion

        #region 1.�ڵ� ���ε�
        Bind<UnityEngine.UI.Button>(typeof(Buttons)); //typeof(�ø���)�� enum�� ��ȯ�Ѵ�
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        #endregion

/*
        #region 2-2. �̺�Ʈ �߰�: �̹��� �巡��
        GameObject go = GetImage((int)Images.Image).gameObject;
        AddUIEvent(go,
        (PointerEventData data) => { go.transform.position = data.position; },
        Define.UIEvent.Drag);

        // `Image` ������Ʈ�� `UI_EventHandler` �߰�
        UI_EventHandler evt = go.GetComponent<UI_EventHandler>();
        if (evt == null)
            evt = go.AddComponent<UI_EventHandler>(); // ������ �߰�
        #endregion*/

        return true;
    }


   
}
