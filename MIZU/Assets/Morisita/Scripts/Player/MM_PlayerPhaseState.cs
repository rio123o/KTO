using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_PlayerPhaseState : MonoBehaviour
{
    public enum State
    {
        Init,
        Liquid, // …
        Gas,    // ‹C‘Ì
        Solid,  // ŒÅ‘Ì
    }

    private State m_state;

    public MM_PlayerPhaseState()
    {
        m_state = State.Init;
    }

    public void ChangeState(MM_PlayerPhaseState.State state)
    {
        MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.Transform,0.3f);
        m_state = state;
    }

    public State GetState()
    {
        return m_state;
    }
}
