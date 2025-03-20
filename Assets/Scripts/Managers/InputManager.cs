using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{

    public Action KeyAction = null;
    
    public void OnUpdate()
    {
        //������ ����
        //�Է� �մ��� üũ�ϰ�, �־��ٸ� �̺�Ʈ�� ����

        //���콺��, Ű����� ��� �Է�(anyKey) Ȯ��
        if (Input.anyKey == false)
            return;
        if(KeyAction != null)
        {
            KeyAction.Invoke();
        }
    }
}
