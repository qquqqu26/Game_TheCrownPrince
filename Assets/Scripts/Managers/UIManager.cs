using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
 1. sort order����
 */
public class UIManager 
{
    int _order = 10; 

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;
    private GameObject UI_loading;


    public GameObject Root
    {
        //���� �������� ����
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; 
        //ĵ���� 2�� �̻� ��ø��, �θ�� ������� ������ sorting order����

        if(sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        //����
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); //���� ���� & �θ�����

        return sceneUI;

    }
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        /*���� ����
         *T: �⺻ �̸�(name�� null�̸�)
         *name: ������ �̸�, �ɼ�
         */

        //����ó��
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        //����
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);

        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        Root.transform.SetParent(Util.FindFirstCanvasInHierarchy().transform, false);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        //��������� Ȯ���ؼ� �����ϴ� ����
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)
            Debug.Log($"Close Pop Failed, top is [{popup}]");

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        //����ó��: ����
        if (_popupStack.Count == 0) return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup.AsUnityNull();

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            _popupStack.Pop();
        
    }

    public void CloseSceneUI()
    {
        if (_sceneUI == null)
            return;

        Managers.Resource.Destroy(_sceneUI.gameObject);
        _sceneUI = null;
    }

    public void ShowLoadingUI()
    {
        if (UI_loading == null)
        {
            Debug.Log("UI_loading�������� ������!!!!!!!!!!!!!!!!");
            UI_loading = Managers.Resource.Instantiate("UI/Scene/UI_loading");
            //UI_loading.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }

        UI_loading.SetActive(true);
    }

    public void HideLoadingUI()
    {
        if (UI_loading != null)
            UI_loading.SetActive(false);
    }



}
