using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TitlePopup : UI_Popup
{
    #region UI열거자
    enum Buttons
    {
        startBtn,
        loadBtn,
        recordBtn,
        settingsBtn,
        exitBtn,

    }
    enum Texts
    {
        titleTxt,
        startTxt,
        loadTxt,
        recordTxt,
        settingsTxt,
        exitTxt,
    }

    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        Bind();
        Gets();

        return true;

    }

    public override void Bind()
    {
        //typeof(시리즈)는 enum을 반환한다
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public override void Gets()
    {
        GetButton((int)Buttons.startBtn).gameObject.AddUIEvent(OnStartBtnClicked);
        GetButton((int)Buttons.loadBtn).gameObject.AddUIEvent(OnLoadBtnClicked);
        GetButton((int)Buttons.recordBtn).gameObject.AddUIEvent(OnRecordBtnClicked);
        GetButton((int)Buttons.settingsBtn).gameObject.AddUIEvent(OnSettingBtnClicked);
        GetButton((int)Buttons.exitBtn).gameObject.AddUIEvent(OnExitBtnClicked);

        GetTexts((int)Texts.titleTxt, (int)Texts.exitTxt, Define.TitlePopupID);

    }

    public void OnStartBtnClicked(PointerEventData data)
    {
        //이전 데이터가 있는 경우
        if (Managers.Game.LoadGame())
        {
            Managers.UI.ShowPopupUI<UI_StartOrLoad>()
                .SetInfo(startNewGame);

        }
        else
        {
            startNewGame();

        }
    }

    public void startNewGame()
    {
        Managers.Game.Init();
        Managers.Game.SaveGame();
        Managers.Time.Init();
        //Managers.Data.Init();



    }

    public void OnLoadBtnClicked(PointerEventData data)
    {
        Debug.Log("OnLoadBtnClicked clicked");

        if (Managers.Game.LoadGame())
        {

        }
        else
        {

        }
    }

    public void OnRecordBtnClicked(PointerEventData data)
    {
        Debug.Log("OnRecordBtnClicked clicked");

    }

    public void OnSettingBtnClicked(PointerEventData data)
    {
        Debug.Log("OnSettingBtnClicked clicked");
    }
    
    public void OnExitBtnClicked(PointerEventData data)
    {
        Debug.Log("OnExitBtnClicked clicked");
    }

   

}
