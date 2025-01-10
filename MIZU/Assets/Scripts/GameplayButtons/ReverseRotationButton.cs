using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseRotationButton : RepressableButton
{
    [Header("参照するRotateObject")]
    [SerializeField] private RotateObject rotateObject;

    /// 
    [Header("対象のオブジェクト")]
    [SerializeField] private GameObject targetObject;
    /// 

    public override void Execute()
    {
        if (rotateObject == null)
        {
            Debug.LogError($"{gameObject.name}: RotateObject が設定されていません。");
            return;
        }

        //  現在の回転方向を反転する
        int newDirection = -rotateObject.RotateDirection;
        rotateObject.SetRotationDirection(newDirection);

        Debug.Log($"{gameObject.name}: RotateObjectの回転方向を{(newDirection == 1 ? "正転" : "逆回転")}に変更した。");


        ToggleRotation();
    }



    /// 
    public void ToggleRotation()
    {
        Debug.Log("hhhhhhhhhhhhhhhhhhhhhh");
        if (targetObject == null)
        {
            Debug.LogWarning("ターゲットオブジェクトが設定されていない");
            return;
        }

        // 現在の回転角を取得
        Vector3 currentRotation = targetObject.transform.eulerAngles;

        // x軸の回転を180度反転
        currentRotation.x = (currentRotation.x + 180) % 360;

        // 新しい回転角を適用
        targetObject.transform.eulerAngles = currentRotation;
    }
    ///
}
