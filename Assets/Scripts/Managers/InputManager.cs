using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{

    public Action KeyAction = null;
    
    public void OnUpdate()
    {
        //리스너 패턴
        //입력 잇는지 체크하고, 있었다면 이벤트로 전파

        //마우스든, 키보드든 모든 입력(anyKey) 확인
        if (Input.anyKey == false)
            return;
        if(KeyAction != null)
        {
            KeyAction.Invoke();
        }
    }
}
