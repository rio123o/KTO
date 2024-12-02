using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotateButton : BaseButton
{
    [Header("��]���������I�u�W�F�N�g")]
    [SerializeField] private RotateObject rotateObject;

    //  �{�^���������ꂽ���Ɏ��s����A�N�V����
    public override void Execute()
    {
        if (rotateObject != null)
        {
            rotateObject.StartRotation();
        }
    }
}