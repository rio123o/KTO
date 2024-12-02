using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("1秒で何度回転するか")]
    [SerializeField] private float rotationSpeed = 90f;
    [Header("ゲーム開始時に自動で回転を始めるようにするか")]
    [SerializeField] private bool autoRotating = false;

    //  回転方向について 1が通常、-1が逆回転
    private int rotateDirection = 1;

    private bool isRotating = false;  //  回転のフラグ

    //  回転状態が変化した際に通知するためのイベントの宣言
    public event Action<RotateObject, bool> RotatingChange;

    //  現在の回転方向を外から取得可能にする
    public int RotateDirection => rotateDirection;

    //  オブジェクトが現在回転中かどうかを外から取得可能にする
    public bool IsRotating => isRotating;

    void Start()
    {
        if(autoRotating)
        {
            StartRotation();
        }
    }

    //  回転を開始するメソッド
    public void StartRotation()
    {
        if (isRotating) return;

        isRotating = true;
        RotatingChange?.Invoke(this, isRotating);
    }


    void Update()
    {
        if (isRotating)
        {
           transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        }
    }

    //  回転方向を設定する
    public void SetRotationDirection(int direction)
    {
        if (direction != 1 && direction != -1)
        {
            Debug.LogError("回転方向は(通常回転)か-1(逆回転のみ設定可能。");
            return;
        }

        if (rotateDirection != direction)
        {
            rotateDirection = direction;
            RotatingChange?.Invoke(this, isRotating);
        }
    }

}