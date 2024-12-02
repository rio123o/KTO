using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotateButton : BaseButton
{
    [Header("回転させたいオブジェクト")]
    [SerializeField] private RotateObject rotateObject;

    //  ボタンが押された時に実行するアクション
    public override void Execute()
    {
        if (rotateObject != null)
        {
            rotateObject.StartRotation();
        }
    }
}