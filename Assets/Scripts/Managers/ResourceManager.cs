using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager 
{
    public T Load<T> (string path) where T : Object
    {
/*        if (Resources.Load<T>(path) == null)
            Debug.Log($"[Load] 비었다, 접근 주소:{path} ");*/

        return Resources.Load<T>(path);
    }


    public Sprite LoadSprite (string path)
    {
/*        Sprite sprite = Resources.Load<Sprite>($"3Sprites/{path}");

        if (sprite == null)
            Debug.Log($"3Sprites/{path}가 널이다");

        return sprite;
*/

        return Resources.Load<Sprite>($"3Sprites/{path}");
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"1Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"[리소스 매니저]실패 to load prefab");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go) {

        if (go == null)
            return;

        Object.Destroy(go);
    }

}
