using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 상태 패턴 설계
public interface IMonthState
{
    void Enter();
}

public class HomeState : IMonthState
{
    public void Enter()
    {
        Managers.UI.ShowPopupUI<UI_HomePopup>();
    }

}

public class StudyState : IMonthState
{
    public void Enter()
    {
        Managers.UI.ShowSceneUI<UI_Study>();
    }

}

public class ActivityState : IMonthState
{
    public void Enter()
    {
        Managers.UI.ShowSceneUI<UI_Activity>();

    }

}

public class StoryState : IMonthState
{
    public void Enter()
    {
        Managers.UI.ShowSceneUI<UI_Story>();
    }
}

public class EventState : IMonthState
{
    public void Enter()
    {
        Managers.UI.ShowSceneUI<UI_Event>();
    }
}
#endregion

public class TimeManager
{

    private IMonthState _curState;
    private bool isQuaterStart {
        get { return (Managers.Game.month % 3 == 1); }
    }


    public void SetState(IMonthState newState)
    {
        Managers.UI.CloseSceneUI();
        Managers.UI.ClosePopupUI();

        _curState = newState;
        _curState.Enter();
    }


    public void NextState()
    {
        if (_curState is HomeState) 
            SetState(new StudyState());
        else if (_curState is StudyState)
            SetState(new ActivityState());
        else if (_curState is ActivityState) {

            if (isQuaterStart)
                SetState(new StoryState());
            else
                SetState(new EventState());
        }
        else {

            ++Managers.Game.month;
            if (Managers.Game.month > 12)
            {
                Managers.Game.month = 1;
                ++Managers.Game.year;
            }

            SetState(new HomeState());
            
        }

    }

    public void Init()
    {
        Managers.Time.SetState(new HomeState());
    }

}
