using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
//using static Define;

public class UI_StartOrLoad : UI_Popup
{
    #region UI열거자
    enum Buttons
    {
        YesBtn,
        NoBtn,

    }
    enum Texts
    {
        HeaderTxt,
        BodyTxt,
        YesTxt,
        NoTxt,
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
        Bind<Button>(typeof(Buttons)); //typeof(시리즈)는 enum을 반환한다
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public override void Gets()
    {
        GetButton((int)Buttons.YesBtn).gameObject.AddUIEvent(OnClickedYes);
        GetButton((int)Buttons.NoBtn).gameObject.AddUIEvent(OnClickedNo);

        GetText((int)Texts.YesTxt).text = Managers.GetText(Define.YesText);
        GetText((int)Texts.NoTxt).text = Managers.GetText(Define.NoText);
        GetText((int)Texts.HeaderTxt).text = Managers.GetText(Define.StartOrLoadHeaderText);
        GetText((int)Texts.BodyTxt).text = Managers.GetText(Define.StartOrLoadBodyText);
    }

    Action _onClickYesButton;

    public void SetInfo(Action onClickYesButton)
    {
        _onClickYesButton = onClickYesButton;
    }

    public void OnClickedYes(PointerEventData data)
    {
        Managers.UI.ClosePopupUI(this);

        if (_onClickYesButton != null)
            _onClickYesButton.Invoke();

    }

    public void OnClickedNo(PointerEventData data)
    {
        Managers.UI.ClosePopupUI(this);

    }
}
