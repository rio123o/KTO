using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_TransformEffect : MM_EffectPlayer
{
    private MM_PlayerPhaseState.State oldState;
    private MM_PlayerPhaseState pState;
    void Start()
    {
        pState=GetComponent<MM_PlayerPhaseState>();
        SetParticleTransform(this.gameObject.transform);
        oldState = pState.GetState();
    }
    private void Update()
    {
        if (oldState != pState.GetState())
        {
            Play();
        }
        oldState = pState.GetState();
    }
}
