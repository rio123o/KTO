using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;  //  1秒で何度回転するか
    [Header("ゲーム開始時に自動で回転を始めるようにするか")]
    [SerializeField] private bool autoRotating = false;


    private bool isRotating = false;  //  回転のフラグ

    //  回転状態が変化した際に通知するためのイベントの宣言
    public event Action<RotateObject, bool> RotatingChange;

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
        RotatingChange?.Invoke(this, true);
        Debug.Log($"{gameObject.name}: Rotation started.");
    }

    //  回転しているかどうかを返す
    public bool IsRotating
    {
        get { return isRotating; }
    }

    void Update()
    {
        if (isRotating)
        {
            
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);

        }
    }
}