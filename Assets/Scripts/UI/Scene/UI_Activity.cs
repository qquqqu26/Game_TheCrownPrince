using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Activity : UI_Scene
{ 
    enum Places
    {
        bookstore,
        Hongmungwan,
    }

    enum Texts
    {

    }

    enum Images
    {

    }

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
        Bind<GameObject>(typeof(Places));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public override void Gets()
    {
        GetObject((int)Places.bookstore).AddUIEvent(go);
        GetObject((int)Places.Hongmungwan).AddUIEvent(go);
    }

    public void go(PointerEventData data)
    {
        Managers.Time.NextState();
    }
}
