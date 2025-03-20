using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class UI_Study : UI_Scene
{
    #region UI열거자
    enum Texts
    {
        statText,
        statNumText,

    }

    enum Images
    {
        SiKangwonImage,
    }

    enum Panels
    {
        NextPanel,
    }

    enum Sliders
    {
        statSlider,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind();
        studySet = FindObjectOfType<UI_HomePopup>().GetStudySet();
        Gets();

        return true;
    }

    public override void Bind()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(Panels));

    }

    public override void Gets()
    {
        GetText((int)Texts.statText).text = Managers.GetText(studySet[studyIndex]);
        key = (studySet[studyIndex] - Define.subjectID);
        GetText((int)Texts.statNumText).text = Managers.Game.stats[key].value.ToString();
        GetObject((int)Panels.NextPanel).gameObject.AddUIEvent((PointerEventData data) => { nextState(); });

    }

    //**********************************************************************************************************************

    int[] studySet;
    int studyIndex = 0;
    int key = 0;
    float speed = 0.05f;

    private void nextState()
    {
        Debug.Log($"오전{studySet[0]}&오후{studySet[1]}");
        updateStat();

        if (studyIndex == 1)
        {
            Managers.Time.NextState();

            return;
        }


        ++studyIndex;
        Gets();

    }

    float increasingValue = 10f;
    private void updateStat()
    {
        Debug.Log($"전: {Managers.Game.stats[key].value}");
        Managers.Game.stats[key].value += increasingValue;
        Debug.Log($"후: {Managers.Game.stats[key].value}");
        GetText((int)Texts.statNumText).text = Managers.Game.stats[key].value.ToString();

    }

    private IEnumerator statIncreasing()
    {

        float rising = 0;
        while (true)
        {
            if(increasingValue >= rising)
            {
                Managers.Game.stats[key].value += increasingValue;
                Get<Slider>((int)Sliders.statSlider).value = Managers.Game.stats[key].value / Managers.Game.statMaxValue;
                yield return new WaitForSeconds(speed);
            }
            else
            {
                Debug.Log($"후: {Managers.Game.stats[key].value}");
                GetText((int)Texts.statNumText).text = Managers.Game.stats[key].value.ToString();
                yield break;
            }
        }
    }
}
