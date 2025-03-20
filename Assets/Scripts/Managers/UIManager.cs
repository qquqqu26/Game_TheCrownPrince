using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
 1. sort order관리
 */
public class UIManager 
{
    int _order = 10; 

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;
    private GameObject UI_loading;


    public GameObject Root
    {
        //폴더 형식으로 정리
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
        //캔버스 2개 이상 중첩시, 부모와 관계없이 고유한 sorting order가짐

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

        //로직
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); //폴더 정리 & 부모지정

        return sceneUI;

    }
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        /*인자 설명
         *T: 기본 이름(name이 null이면)
         *name: 프리팹 이름, 옵션
         */

        //예외처리
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        //로직
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);

        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        Root.transform.SetParent(Util.FindFirstCanvasInHierarchy().transform, false);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        //명시적으로 확인해서 삭제하는 버전
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)
            Debug.Log($"Close Pop Failed, top is [{popup}]");

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        //예외처리: 종료
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
            Debug.Log("UI_loading프리팹이 없어잉!!!!!!!!!!!!!!!!");
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
