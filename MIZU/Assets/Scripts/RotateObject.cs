using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;  //  1�b�ŉ��x��]���邩
    [Header("�Q�[���J�n���Ɏ����ŉ�]���n�߂�悤�ɂ��邩")]
    [SerializeField] private bool autoRotating = false;


    private bool isRotating = false;  //  ��]�̃t���O

    //  ��]��Ԃ��ω������ۂɒʒm���邽�߂̃C�x���g�̐錾
    public event Action<RotateObject, bool> RotatingChange;

    void Start()
    {
        if(autoRotating)
        {
            StartRotation();
        }
    }

    //  ��]���J�n���郁�\�b�h
    public void StartRotation()
    {
        if (isRotating) return;

        isRotating = true;
        RotatingChange?.Invoke(this, true);
        Debug.Log($"{gameObject.name}: Rotation started.");
    }

    //  ��]���Ă��邩�ǂ�����Ԃ�
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