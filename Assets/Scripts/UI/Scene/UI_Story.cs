using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_Story: UI_Scene
{
    #region 열거자
    public enum Images
    {
        BackgroundImage,

        charac1,
/*        charac2,
        charac3,*/

    }

    enum Texts
    {
        nameText,
        positionText,
        dialogueText,
    }

    public enum Objects
    {
        //이미지들임
        NextPanel,
        NameBox,

    }

    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind();
        dialogueID = Managers.Story.storyStartID;
        Gets();

        return true;

    }

    public override void Bind()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(Objects));
    }

    public override void Gets()
    {
        NextScene();
        GetObject((int)Objects.NextPanel).AddUIEvent((PointerEventData data) => { NextScene(); });

    }

    //***************************
    StoryManager _story = Managers.Story;
    StoryData story = null;
    _CharacterData speaker
    {
        get { return Managers.Data.Characters[story.charac]; }
    }

    int _dialogueID;
    int dialogueID
    {
        get { return _dialogueID; }
        set { _dialogueID = value; }
    }

    int _lastID;
    int lastID
    {
        get { return (_lastID); }
        set { _lastID = value; }
    }
    
    bool shouldChange(string text)
    {
         return (text != ""); 
    }

    int nameId
    {
        get { return 1000 + speaker.characTextID; }
    }
    int positionId
    {
        get { return 2000 + speaker.characTextID; }
    }
    
    bool doesBranchMerge(int branchID)
    {
        return (branchID != 0);
    }
    bool isBranch;

    public void NextScene()
    {
        //이름, 직위 박스, 인물 등등 다 refresh로 비활성화
        RefreshUI();


        #region 디버깅: 스킵
        if (Managers.Instance.doesDialogueSkip == true)
        {
            while (true)
            {
                story = Managers.Data.Stories[dialogueID];

                if (doesBranchMerge(story.branchID) == true)
                {
                    Debug.Log($"브랜치 진입[{dialogueID}]:{story.kor}");
                    lastID = dialogueID + 1;
                    dialogueID = split(story.branchID);
                    isBranch = true; ;
                }
                else
                {
                    if (Managers.Data.Stories.ContainsKey(++dialogueID) == false)
                    {
                        if (isBranch == true)
                        {
                            Debug.Log($"브랜치 탈출[{dialogueID}]:{story.kor}");
                            dialogueID = lastID;
                            isBranch = false;
                        }
                        else
                        {
                            Debug.Log($"스토리 종료[{dialogueID}]: {Managers.Data.Stories[dialogueID-1].kor}.");
                            Managers.Time.NextState();
                            return;
                        }



                    }
                }

            }

        }
        else
        {
            #endregion

            story = Managers.Data.Stories[dialogueID];
            _story.didTypeCompleted = true;

            SetBackground();
            SetDialogue();

            #region 시작 대사키 처리

            if (doesBranchMerge(story.branchID) == true)
            {
                //브랜치 진입
                lastID = dialogueID + 1;
                dialogueID = split(story.branchID);
                isBranch = true; ;
            }
            else
            {

                if (Managers.Data.Stories.ContainsKey(++dialogueID) == false)
                {
                    if (isBranch == true)
                    {
                        dialogueID = lastID;
                        isBranch = false;
                        //브랜치 탈출
                    }
                    else
                    {
                        Debug.Log($"스토리 대사키 없음: {dialogueID}/종료?");
                        Managers.Time.NextState();
                        return;
                    }

                }
            }
            #endregion

        }



    }



    public void RefreshUI()
    {
        //인물과 NameBox는 비활성화

        GetImage((int)Images.charac1).gameObject.SetActive(false);
/*        GetImage((int)Images.charac2).gameObject.SetActive(false);
        GetImage((int)Images.charac3).gameObject.SetActive(false);*/

        GetObject((int)Objects.NameBox).SetActive(false);

    }

    private IEnumerator TypeText(string text)
    {
        GetText((int)Texts.dialogueText).text = "";
        _story.didTypeCompleted = false;
        int wordIndex = 0;

        while(_story.didTypeCompleted == false)
        {
            GetObject((int)Objects.NextPanel).gameObject.SetActive(false);
            GetText((int)Texts.dialogueText).text += text[wordIndex];
            yield return new WaitForSeconds(_story.typeSpeed);

            if(++wordIndex == text.Length)
            {
                GetObject((int)Objects.NextPanel).gameObject.SetActive(true);
                _story.didTypeCompleted = true;
                yield break;
            }
        }

    }

    private int split(int branchID)
    {
        int statID = Managers.Data.Branches[branchID].standardStat;
        float statValue = Managers.Game.stats[statID].value;

        if (statValue >= Managers.Data.Branches[branchID].standard)
        {
            Debug.Log($"{Managers.Game.stats[statID].type} 성공: {statValue} >= {Managers.Data.Branches[branchID].standard}");
            return Managers.Data.Branches[branchID].trueID;
        }
        else
        {
            Debug.Log($"{Managers.Game.stats[statID].type} 실패: {statValue} < {Managers.Data.Branches[branchID].standard}");
            return Managers.Data.Branches[branchID].falseID;

        }
    }

    private void SetBackground()
    {
        //GetImage((int)Images.BackgroundImage).gameObject.SetActive(true);
        if (shouldChange(story.background) == true)
        {
            GetImage((int)Images.BackgroundImage).sprite = Managers.Resource.LoadSprite($"Backgrounds/{story.background}");

        }
    }

    private void SetDialogue()
    {
        if (shouldChange(story.charac) == true)
        {
            #region 인물: 활성화&이미지 덮기
            GetImage((int)Images.charac1).gameObject.SetActive(true);
            GetImage((int)Images.charac1).sprite = GetCharacter($"{speaker.spriteName}");
            #endregion

            #region 이름상자: 활성화&정보 덮어 씌우기
            GetObject((int)Objects.NameBox).SetActive(true);
            GetText((int)Texts.nameText).text = Managers.Data.CharacterTexts[nameId].kor;
            GetText((int)Texts.positionText).text = Managers.Data.CharacterTexts[positionId].kor;
            #endregion

        }

        #region text처리
        if (Managers.Instance.isTypeEffectOn == true)
        {
            StartCoroutine(TypeText(story.kor));

        }
        else
        {
            GetText((int)Texts.dialogueText).text = story.kor;
        }
        #endregion
    }

}
