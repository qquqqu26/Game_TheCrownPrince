/*using DG.Tweening;
using DG.Tweening.Core.Easing;*/
using System;
using System.Collections;
using System.Collections.Generic;
/*using System.ComponentModel.Design.Serialization;*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_HomePopup : UI_Popup
{
    #region 열거자 목록
    enum Buttons
    {
        #region 0.기본
        ScheduleBtn,
        StatsBtn,
        TraitsBtn,
        FactionsBtn,
        LineageBtn,
        HomeBtn,
        #endregion

        #region 1.스케쥴
        MorningBtn,
        AfternoonBtn,
        ExecuteBtn,

        yehakBtn,
        akhakBtn,
        sahakBtn,
        uhakBtn,
        seohakBtn,
        suhakBtn,
        ihakBtn,
        sulhakBtn,
        umyanghakBtn,
        uihakBtn,
        yulhakBtn,
        nongjeongBtn,
        yeoksahakBtn,
        #endregion
    }

    enum Texts
    {
        #region 0. 기본
        YearText,
        MonthText,

        ScheduleText,
        StatsText,
        TraitsText,
        FactionsText,
        LineageText,
        HomeText,
        #endregion

        #region 1. 스케쥴
        yehakText,
        akhakText,
        sahakText,
        uhakText,
        seohakText,
        suhakText,
        ihakText,
        sulhakText,
        umyanghakText,
        uihakText,
        yulhakText,
        nongjeongText,
        yeoksahakText,

        yukYeText,
        japHakText,

        HeaderText,
        HeaderExplainText,
        MorningText,
        AfternoonText,
        ExecuteText,
        subjectNameText,
        subjectExplainText, //얘는 그냥 빈칸이라서 ID없음
        #endregion

        #region 2. 스탯
        StatHeaderText,
        stat1Text,
        stat2Text,
        stat3Text,
        stat4Text,
        stat5Text,
        stat6Text,

/*        stat7Text,
        stat8Text,
        stat9Text,
        stat10Text,
        stat11Text,
        stat12Text,
        stat13Text,*/

        stat1NumText,
        stat2NumText,
        stat3NumText,
        stat4NumText,
        stat5NumText,
        stat6NumText,

/*        stat7NumText,
        stat8NumText,
        stat9NumText,
        stat10NumText,
        stat11NumText,
        stat12NumText,
        stat13NumText,*/

        #endregion

    }

    public enum Images
    {
        HomeImage,
        PrinceImage,
    }

    public enum Popups
    {
        Prince,
        Schedules,
        Stats,
        Traits,
        Factions,
        Lineage,

    }

    public enum Sliders
    {
        stat1Slider,
        stat2Slider,
        stat3Slider,
        stat4Slider,
        stat5Slider,
        stat6Slider,

/*        stat7Slider,
        stat8Slider,
        stat9Slider,
        stat10Slider,
        stat11Slider,
        stat12Slider,
        stat13Slider,
        */
    }

    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind();
        Gets();

        RefreshUI();

        return true;

    }

    public override void Bind()
    {
        //모든 패널의 요소 바인딩
        Bind<Button>(typeof(Buttons), true);
        Bind<TextMeshProUGUI>(typeof(Texts), true);
        Bind<Image>(typeof(Images), true);
        Bind<Slider>(typeof(Sliders), true);
        Bind<GameObject>(typeof(Popups), true);
    }

    public override void Gets()
    {
        GetImage((int)Images.HomeImage).sprite = Managers.Resource.LoadSprite("HomeImage");

        GetImage((int)Images.PrinceImage).sprite = GetCharacter("Prince");

        #region 0.  Base 팝업
        GetButton((int)Buttons.ScheduleBtn).gameObject.AddUIEvent(ScheduleButton);
        GetButton((int)Buttons.StatsBtn).gameObject.AddUIEvent(StatsButton);
        GetButton((int)Buttons.TraitsBtn).gameObject.AddUIEvent(TraitsButton);
        GetButton((int)Buttons.FactionsBtn).gameObject.AddUIEvent(FactionsButton);
        GetButton((int)Buttons.LineageBtn).gameObject.AddUIEvent(LineageButton);
        GetButton((int)Buttons.HomeBtn).gameObject.AddUIEvent(HomeButton);

        GetTexts((int)Texts.ScheduleText, (int)Texts.HomeText, Define.HomeBaseID);
        GetText((int)Texts.YearText).text = Managers.Game.year+Managers.GetText(Define.YearText);
        GetText((int)Texts.MonthText).text = Managers.Game.month+Managers.GetText(Define.MonthText);

        #endregion

        #region 1. 일정 팝업
        GetButton((int)Buttons.MorningBtn).gameObject.AddUIEvent(morningBtnClick);
        GetButton((int)Buttons.AfternoonBtn).gameObject.AddUIEvent(afternoonBtnClick);
        GetButton((int)Buttons.ExecuteBtn).gameObject.AddUIEvent(executeBtnClick);
        for (int textID = (int)Buttons.yehakBtn; textID <= (int)Buttons.yeoksahakBtn; textID++)
        {
            int subjectID = (Define.subjectID) + (textID - (int)Buttons.yehakBtn);
            GetButton((int)textID).gameObject.AddUIEvent((PointerEventData data) => clickSubject(data, subjectID));
            GetButton((int)textID).gameObject.AddUIEvent((PointerEventData data) => enterSubject(data, subjectID), Define.UIEvent.Enter);
        }

        GetTexts((int)Texts.yehakText, (int)Texts.japHakText, Define.subjectID);

        RefreshSchedulesPopup();
        #endregion

        #region 2. 스탯 팝업
        for (int curSlider = 0; curSlider <= (int)Sliders.stat6Slider; ++curSlider)
        {
            Get<Slider>((int)curSlider).value = Managers.Game.stats[curSlider].value/Managers.Game.statMaxValue;
            GetText((int)Texts.stat1NumText + curSlider).text = Managers.Game.stats[curSlider].value.ToString();
            GetText((int)Texts.stat1Text + curSlider).text = Managers.GetText(Define.subjectID + curSlider);
        }
        #endregion
    }

    //*****************************************************************************************************

    string warningMessage = null;

    public void ScheduleButton(PointerEventData data)
    {
        ShowHomePopup(Popups.Schedules);
    }

    #region 0. 베이스 팝업 버튼
    public void StatsButton(PointerEventData data)
    {
        ShowHomePopup(Popups.Stats);
    }

    public void TraitsButton(PointerEventData data)
    {
        Debug.Log("TraitsButton");
        ShowHomePopup(Popups.Traits);

    }

    public void FactionsButton(PointerEventData data)
    {
        Debug.Log("FactionsButton");
        ShowHomePopup(Popups.Factions);


    }

    public void LineageButton(PointerEventData data)
    {
        Debug.Log("LineageBtn");
        ShowHomePopup(Popups.Lineage);


    }

    public void HomeButton(PointerEventData data)
    {
        ShowHomePopup(Popups.Prince);
    }

    #endregion

    #region 1. 스케쥴 팝업 버튼  

    private int[] studySet = new int[2];
    public int[] GetStudySet() { return studySet; }

    int studyIndex;
    bool isFull
    {
        get { return studyIndex == 2; }
    }

    public void clickSubject(PointerEventData data, int subjectID)
    {
        try {
            if (studyIndex == 0)
            {
                GetText((int)Texts.MorningText).text = Managers.GetText(subjectID);
            }
            else if (studyIndex == 1)
            {
                GetText((int)Texts.AfternoonText).text = Managers.GetText(subjectID);
            }

            studySet[studyIndex] = subjectID;
            ++studyIndex;
        }
        catch 
        {
            //범위 초과인데 내가 이걸 뭐 어떻게 해야 함?
        }


    }

    public void enterSubject(PointerEventData data, int subjectID)
    {
        GetText((int)Texts.subjectNameText).text = Managers.GetText(subjectID);
        GetText((int)Texts.subjectExplainText).text = Managers.GetText((subjectID+100));
    }

    public void morningBtnClick(PointerEventData data)
    {
        if (studyIndex != 0)
        {
            Debug.Log("오후 수업 => 오전 수업");

        }
        else
        {
            Debug.LogWarning("오후 수업 비었다는 팝업");

        }

    }

    public void afternoonBtnClick(PointerEventData data)
    {
        Debug.Log("afternoonBtnClick");
        if (studyIndex != 1)
        {
            Debug.Log("오후 수업 취소");
        }
        else
        {
            Debug.LogWarning("오후 수업 비었다는 팝업");

        }

    }


    public void executeBtnClick(PointerEventData data)
    {

        if(isFull == true)
        {
            Managers.Time.NextState();
        }
        else 
        {
            warningMessage = "오전 오후를 둘 다 채우세요";
            Managers.UI.ShowPopupUI<UI_Warning>().SetInfo(warningMessage);
        }

    }

    #endregion

    Popups curPanel;
    void ShowHomePopup(Popups popupType)
    {
        //기존 패널 닫기
        if (curPanel == Popups.Schedules)
            RefreshSchedulesPopup();
        GetObject((int)curPanel).SetActive(false);

        // 새 패널 열기
        curPanel = popupType;
        GetObject((int)curPanel).SetActive(true); 
    }

    void RefreshUI()
    {

        //모든 팝업패널 비활성화
        foreach (Popups popup in System.Enum.GetValues(typeof(Popups)))
            GetObject((int)popup).gameObject.SetActive(false);

        //홈 팝업
        ShowHomePopup(Popups.Prince);
    }

    void RefreshSchedulesPopup()
    {
        studyIndex = 0;
        GetTexts((int)Texts.HeaderText, (int)Texts.subjectNameText, Define.SchedulesPopupID);
        GetText((int)Texts.subjectNameText).text = Managers.GetText(Define.Subject);
        GetText((int)Texts.subjectExplainText).text = "";
    }

}