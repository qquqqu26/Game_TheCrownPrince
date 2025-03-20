using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Warning : UI_Popup
{
    #region UI요소 열거
    enum Texts
    {
        WarningTitleText,
        WarningBodyText,
        ButtonText,
    }

    enum Buttons
    {
        Button,
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
        Bind<TextMeshProUGUI>(typeof(Texts));     
        Bind<Button>(typeof(Buttons));     
    }

    public override void Gets()
    {

        GetText((int)Texts.WarningTitleText).text = Managers.GetText(Define.WarningTitleText);
        GetText((int)Texts.ButtonText).text = Managers.GetText(Define.WarningButtonText);
        GetText((int)Texts.WarningBodyText).text = warningMessage;

        GetButton((int)Buttons.Button).gameObject.AddUIEvent(
            (PointerEventData data) => { ClosePopupUI(); }
            );
    }

    private static string _warningMessage;
    public static string warningMessage
    {
        get { return _warningMessage;  }
        set { _warningMessage = value; }
    }
    public void SetInfo(string m)
    {
        warningMessage = m;
    }
}
