using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override bool Init()
    {
        if(!base.Init())
            return false;

        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Title>();
        Managers.UI.ShowPopupUI<UI_TitlePopup>(); 
        return true;
    }


    public override void Clear()
    {
        //종료됐을 때 날라가는 부분
        Debug.Log("GameScene Clear!");
    }


}
