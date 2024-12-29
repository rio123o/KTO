using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_PauseState : MonoBehaviour
{
    bool isPause;

    MM_ObserverBool observerBool;
    // Start is called before the first frame update

    public MM_PauseState()
    {
        observerBool = new MM_ObserverBool();
    }
    public void OnPause()
    {
        isPause = true;
        if (observerBool.OnBoolTrueChange(isPause))
            OnPause();
        MM_TimeManager.Instance.StopTime();
        //MM_PlayerControllerManager.Instance.SetControlUIMap();

    }

    public void OffPause()
    {
        isPause=false;
        if (observerBool.OnBoolFalseChange(isPause))
            OffPause();
        MM_TimeManager.Instance.MoveTime();
        //MM_PlayerControllerManager.Instance.SetControlPlayerMap();

    }
}
