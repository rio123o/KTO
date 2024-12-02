using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_Ice_Animation_Manager : MM_Player_Animation_Manager
{
    public override void PlayAnim()
    {
        float pSpeed_x = playerTest.GetAbsSpeed().x;

        _animator.SetFloat("Speed", pSpeed_x);
    }
}
