using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasDirection : MonoBehaviour
{
    [Header("�ǂ̕�����Gas����Ԃ悤�ɂ��邩")]
    [SerializeField, Range(-1f, 1f)] private float directionX = 0f;  //  X���̕���
    [SerializeField, Range(-1f, 1f)] private float directionY = 0f;  //  Y���̕���
    private float directionZ = 0f;  //  Z�͂O�ŌŒ肷��

    [Header("RotateObject�ւ̎Q��")]
    [SerializeField] private RotateObject rotateObject;

    //  ���삷�邽�߂̃t���O
    public bool isDirectionActive;

    //  isDirectionActive��true�̎��Agas�����̋�ԂŔ�Ԃ悤�ɂ��A
    //  RotateObject��]�������ς�������ɔ��΂̕����ɔ�Ԃ悤�ɂ��Ă���
    public Vector3 Dir => isDirectionActive ? new Vector3(directionX, directionY, directionZ).normalized * rotateObject.RotateDirection : Vector3.zero;

    void Awake()
    {
        if(rotateObject == null)
        {
            Debug.LogError($"RotateObject��{gameObject.name}�ɐݒ肳��Ă��Ȃ��B");
            enabled = false;  //  �X�N���v�g�̎��s���~������
            return;
        }

        //  RotateObject��RotatingChange�C�x���g�Ƀ��X�i�[��ǉ�����
        rotateObject.RotatingChange += OnRotatingChange;
    }

    void OnDestroy()
    {
        if (rotateObject != null)
        {
            rotateObject.RotatingChange -= OnRotatingChange;
        }
    }

    //  RotateObject�̉�]�󋵂��ς�������ɌĂяo��������
    private void OnRotatingChange(RotateObject rotate, bool isRotating)
    {
        UpdateDirection(isRotating);
    }

    //  isDirectionActive�t���O���X�V����
    private void UpdateDirection(bool isRotating)
    {
        isDirectionActive = isRotating;
    }

}
