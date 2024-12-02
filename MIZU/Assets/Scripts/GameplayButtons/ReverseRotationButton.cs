using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseRotationButton : RepressableButton
{
    [Header("�Q�Ƃ���RotateObject")]
    [SerializeField] private RotateObject rotateObject;

    public override void Execute()
    {
        if (rotateObject == null)
        {
            Debug.LogError($"{gameObject.name}: RotateObject ���ݒ肳��Ă��܂���B");
            return;
        }

        //  ���݂̉�]�����𔽓]����
        int newDirection = rotateObject.RotateDirection == 1 ? -1 : 1;
        rotateObject.SetRotationDirection(newDirection);

        Debug.Log($"{gameObject.name}: RotateObject�̉�]������{(newDirection == 1 ? "���]" : "�t��]")}�ɕύX�����B");
    }
}
