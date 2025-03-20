using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;


public class StoryManager
{
    public void Init()
    {
        Debug.Log("스토리 매니저 이닛");
    }

    public int storyStartID
    {
        get { return (((Managers.Game.year - 12) * 4 + Managers.Game.month / 4) +1) * 1000 + 1; }
    }


    //StoryData story = Managers.Data.Stories[storyStartID];


    public bool didTypeCompleted = false;
    public float typeSpeed = 0.04f;


    
}
