using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

//��ɼ� �Լ����� �� ����
public class Util 
{
    //������Ʈ ���� �Լ� (�ֻ����θ� go, ã�� obj�̸�, ����� Ž�� ����)
    //*���⼭ ����� Ž���� �θ��� �ڽĸ� ã�� �� �ƴϰ�, �ڽ��� �ڽĵ� ã�� �� �ǹ�=>(Ʈ��� �ϴ� �� ����)
    public static T FindChild<T> (GameObject go, string name= null, bool recursive = false, bool includeInactive = false) where T : UnityEngine.Object
    {
        if (go == null) 
            return null;

        //1. ���Ӹ� Ž���ϴ� ���
        if (recursive == false)
        {
            //Transform������Ʈ�� �ڽ� ������Ʈ�� ����
            for (int i = 0; i < go.transform.childCount; i++)
            {
                UnityEngine.Transform transform = go.transform.GetChild(i);
                Debug.Log($"[����FindChild] Checking child: {transform.gameObject.name}");

                // �̸��� �� �־����� �׳� TŸ�� ��ȯ & �̸��� �´°� ã���� ���
                if (string.IsNullOrEmpty(name) || transform.gameObject.name == name)
                {
                    T component = transform.GetComponent<T>();

                    if (component != null)
                    {
                        Debug.Log($"[FindChild] Found matching component in: {transform.gameObject.name}");
                        return component;
                    }
                }
            }
        }
        else //2. ��������� �ڼ� Ž��
        {
            foreach (T component in go.GetComponentsInChildren<T>(includeInactive))
            {
                //Debug.Log($"[���FindChild] Checking child: {component.name}");
                // �̸��� �� �־����� �׳� TŸ�� ��ȯ & �̸��� �´°� ã���� ���
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;



    }

    //1-2. ������Ʈ ���� �Լ�
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false, bool isActivated = false)
    {
        UnityEngine.Transform transform = FindChild<UnityEngine.Transform>(go, name, recursive, isActivated);

        if (transform == null)
            return null;

        return transform.gameObject;

    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static Canvas FindFirstCanvasInHierarchy()
    {
        // �� ���� ��� Canvas ��������
        Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();

        if (allCanvases.Length > 0)
        {
            //Debug.Log($"[CanvasFinder] ù ��° Canvas({allCanvases[0].gameObject.name})�� ã��.");
            return allCanvases[0]; // ù ��° Canvas ��ȯ
        }

        Debug.LogError("[CanvasFinder] ������ Canvas�� ã�� �� ����!");
        return null;
    }

    public static void BindEvent(GameObject panel, Action<PointerEventData> clickAction)
    {
        UI_EventHandler eventHandler = panel.GetComponent<UI_EventHandler>();
        if (eventHandler != null)
        {
            eventHandler.OnClickHandler = clickAction;
        }
    }
}
