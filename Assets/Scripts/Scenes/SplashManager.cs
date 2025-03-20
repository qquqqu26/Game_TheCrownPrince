using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashManager : MonoBehaviour
{
    public float delayTime = 3f; // µÙ∑π¿Ã Ω√∞£
  
    public void Start()
    {
        Invoke("LoadMainMenu", delayTime);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
