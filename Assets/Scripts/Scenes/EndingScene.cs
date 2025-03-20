using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//사실 불필요함..

public class EndingScene : BaseScene
{
    protected override bool Init()
    {
        if(!base.Init())
            return false;

        //SceneType = Define.Scene.Ending;
        return true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //씬매니져는 우리가 만든게 아님.
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
    public override void Clear()
    {
        Debug.Log("EndingScene Clear!");
    }
}
