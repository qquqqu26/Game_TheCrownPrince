using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : UI_Scene
{
    enum Images
    {
        Image,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<Image>(typeof(Images));
        GetImage((int)Images.Image).sprite = Managers.Resource.LoadSprite("mainMenu_Img");

        return true;
    }
}
