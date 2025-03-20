using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StoryManager
{


    public int startID
    {
        get { return (((Managers.Game.year - 12) * 4 + Managers.Game.month / 4) +1) * 1000 + 1; }
    }


}
