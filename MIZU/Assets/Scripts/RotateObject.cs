using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("1�b�ŉ��x��]���邩")]
    [SerializeField] private float rotationSpeed = 90f;
    [Header("�Q�[���J�n���Ɏ����ŉ�]���n�߂�悤�ɂ��邩")]
    [SerializeField] private bool autoRotating = false;

    //  ��]�����ɂ��� 1���ʏ�A-1���t��]
    private int rotateDirection = 1;

    private bool isRotating = false;  //  ��]�̃t���O

    //  ��]��Ԃ��ω������ۂɒʒm���邽�߂̃C�x���g�̐錾
    public event Action<RotateObject, bool> RotatingChange;

    //  ���݂̉�]�������O����擾�\�ɂ���
    public int RotateDirection => rotateDirection;

    //  �I�u�W�F�N�g�����݉�]�����ǂ������O����擾�\�ɂ���
    public bool IsRotating => isRotating;

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
        RotatingChange?.Invoke(this, isRotating);
    }


    void Update()
    {
        if (isRotating)
        {
           transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        }
    }

    //  ��]������ݒ肷��
    public void SetRotationDirection(int direction)
    {
        if (direction != 1 && direction != -1)
        {
            Debug.LogError("��]������(�ʏ��])��-1(�t��]�̂ݐݒ�\�B");
            return;
        }

        if (rotateDirection != direction)
        {
            rotateDirection = direction;
            RotatingChange?.Invoke(this, isRotating);
        }
    }

}