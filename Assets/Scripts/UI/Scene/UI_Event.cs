using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UI_HomePopup;

public class UI_Event : UI_Scene
{
    #region 1. ¿­°ÅÀÚ
    enum Texts
    { 
    
    }

    enum Objects
    {
        CoverPanel,
    }
    #endregion

    public override bool Init()
    {
        Bind();
        Gets();

        return true;
    }

    public override void Bind()
    {
        Bind<GameObject>(typeof(Objects));

    }

    public override void Gets()
    {
        GetObject((int)Objects.CoverPanel).AddUIEvent((PointerEventData data) => { Managers.Time.NextState(); });

    }
}
