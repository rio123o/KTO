using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasDirection : MonoBehaviour
{
    [Header("どの方向にGasが飛ぶようにするか")]
    [SerializeField, Range(-1f, 1f)] private float directionX = 0f;  //  X軸の方向
    [SerializeField, Range(-1f, 1f)] private float directionY = 0f;  //  Y軸の方向
    private float directionZ = 0f;  //  Zは０で固定する

    [Header("RotateObjectへの参照")]
    [SerializeField] private RotateObject rotateObject;

    //  動作するためのフラグ
    public bool isDirectionActive;

    //  isDirectionActiveがtrueの時、gasがこの空間で飛ぶようにし、
    //  RotateObject回転方向が変わった時に反対の方向に飛ぶようにしている
    public Vector3 Dir => isDirectionActive ? new Vector3(directionX, directionY, directionZ).normalized * rotateObject.RotateDirection : Vector3.zero;

    void Awake()
    {
        if(rotateObject == null)
        {
            Debug.LogError($"RotateObjectが{gameObject.name}に設定されていない。");
            enabled = false;  //  スクリプトの実行を停止させる
            return;
        }

        //  RotateObjectのRotatingChangeイベントにリスナーを追加する
        rotateObject.RotatingChange += OnRotatingChange;
    }

    void OnDestroy()
    {
        if (rotateObject != null)
        {
            rotateObject.RotatingChange -= OnRotatingChange;
        }
    }

    //  RotateObjectの回転状況が変わった時に呼び出されるもの
    private void OnRotatingChange(RotateObject rotate, bool isRotating)
    {
        UpdateDirection(isRotating);
    }

    //  isDirectionActiveフラグを更新する
    private void UpdateDirection(bool isRotating)
    {
        isDirectionActive = isRotating;
    }

}
