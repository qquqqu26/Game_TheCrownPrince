using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//��� ���ʿ���..

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
            //���Ŵ����� �츮�� ����� �ƴ�.
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
    public override void Clear()
    {
        Debug.Log("EndingScene Clear!");
    }
}
