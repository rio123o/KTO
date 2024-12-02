using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_Cloud_Animation_Manager : MM_Player_Animation_Manager
{
    public override void PlayAnim()
    {
        float pSpeed_y = playerTest.GetAbsSpeed().y;

        _animator.SetFloat("Speed_y", pSpeed_y);
    }
}
