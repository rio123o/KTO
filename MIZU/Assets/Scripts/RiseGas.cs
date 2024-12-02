using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

// �K�v�ȃR���|�[�l���g�������I�ɃA�^�b�`
[RequireComponent(typeof(Collider))]

public class RiseGas : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 5f;   //  �㏸���x
    [SerializeField] private float moveRightSpeed = 5f;  //  �E�����ւ̈ړ����x

    [SerializeField] private bool isRising = false;
    [SerializeField] private bool isMovingRight = false;

    //  �����̃g���K�[�������ɓ���ꍇ�̃J�E���g
    private int riseGasCount = 0;
    private int moveRightGasCount = 0;

    //  �L�����Z���g�[�N���̓���
    private CancellationTokenSource riseCancel;
    private CancellationTokenSource moveRightCancel;

    [SerializeField] private MM_PlayerPhaseState _pState;

    void Start()
    {
        //  �K�v�ȃR���|�[�l���g�̎擾
        _pState = GetComponent<MM_PlayerPhaseState>();

        //  �G���[�̃`�F�b�N
        if (_pState == null)
        {
            Debug.LogError("MM_PlayerPhaseState �R���|�[�l���g��������܂���");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //  riseGas�^�O�̃I�u�W�F�N�g���g���K�[�ɓ������ꍇ
        if(other.CompareTag("riseGas"))
        {
            riseGasCount++;

            if (!isRising) 
            {
                riseCancel = new CancellationTokenSource();
                StartRise(riseCancel.Token).Forget();
            }

            Debug.Log($"player��RiseGas�g���K�[�ɂ���āA�㏸���n�߂��B Count: {riseGasCount}");
        }

        // moveRightGas�^�O�̃I�u�W�F�N�g���g���K�[�ɓ������ꍇ
        if (other.CompareTag("moveRightGas"))
        {
            moveRightGasCount++;

            if (!isMovingRight)
            {
                moveRightCancel = new CancellationTokenSource();
                StartMoveRight(moveRightCancel.Token).Forget();
            }

            Debug.Log($"player��moveRightGas�g���K�[�ɂ���āA�E�����ւ̈ړ����n�߂��B Count: {moveRightGasCount}");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //  riseGas�^�O�̃I�u�W�F�N�g���g���K�[����o���ꍇ
        if (other.CompareTag("riseGas"))
        {
            riseGasCount--;

            Debug.Log($"player��riseGas�g���K�[����o���B Count: {riseGasCount}");
            if (riseGasCount <= 0)
            {
                riseGasCount = 0;
                isRising = false;

                //  �^�X�N�̃L�����Z��
                riseCancel?.Cancel();
                riseCancel = null;

                Debug.Log("player�͑S�Ă�riseGas�g���K�[����o���B");
            }
        }

        //  moveRightGas�^�O�̃I�u�W�F�N�g���g���K�[����o���ꍇ
        if (other.CompareTag("moveRightGas"))
        {
            moveRightGasCount--;
            Debug.Log($"player��moveRightGas�g���K�[����o���B Count: {moveRightGasCount}");
            if (moveRightGasCount <= 0)
            {
                moveRightGasCount = 0;
                isMovingRight = false;

                //  �^�X�N�̃L�����Z��
                moveRightCancel?.Cancel();
                moveRightCancel = null;

                Debug.Log("player�͑S�Ă�moveRightGas�g���K�[����o���B");
            }
        }
    }

    //  �㏸������UniTask�Ŏ�������
    private async UniTaskVoid StartRise(CancellationToken cancellationToken)
    {
        try
        {
            isRising = true;

            while (isRising && !cancellationToken.IsCancellationRequested)
            {
                if (_pState.GetState() == MM_PlayerPhaseState.State.Gas)
                {
                    float step = riseSpeed * Time.deltaTime;
                    transform.position += Vector3.up * step;

                    
                }

                //  �����ɏ㏸�����邽�߁A�����̃`�F�b�N���폜
                //  �ړ���~�̓g���K�[�ޏo�ɂ���čs����
                await UniTask.Yield();
            }

        }
        catch(OperationCanceledException)
        {
            Debug.Log("StartRise���L�����Z������܂����B");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"StartRise �G���[: {ex.Message}");
        }
    }

    // �E�����ړ�������UniTask�Ŏ���
    private async UniTaskVoid StartMoveRight(CancellationToken cancellationToken)
    {
        try
        {
            isMovingRight = true;

            while (isMovingRight && !cancellationToken.IsCancellationRequested)
            {
                if (_pState.GetState() == MM_PlayerPhaseState.State.Gas)
                {
                    float step = moveRightSpeed * Time.deltaTime;
                    transform.position += Vector3.right * step;
                }

                // �����ɉE�����Ɉړ������邽�߁A�����̃`�F�b�N���폜
                // �ړ���~�̓g���K�[�ޏo�ɂ���čs����

                await UniTask.Yield();
            }

        }
        catch(OperationCanceledException)
        {
            Debug.Log("StartMoveRight���L�����Z������܂����B");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"StartMoveRight �G���[: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        //  �S�ẴL�����Z���g�[�N�����L�����Z��
        riseCancel?.Cancel();
        moveRightCancel?.Cancel();

        //  �t���O�ƃJ�E���g�̃��Z�b�g
        isRising = false;
        isMovingRight = false;
        riseGasCount = 0;
        moveRightGasCount = 0;

        Debug.Log("RiseGas�X�N���v�g��j�����ꂽ�B�t���O�ƃJ�E���g�����Z�b�g���A�^�X�N���L�����Z�������B");
    }

    private void OnDisable()
    {
        //  �S�ẴL�����Z���g�[�N�����L�����Z��
        riseCancel?.Cancel();
        moveRightCancel?.Cancel();

        //  �t���O�ƃJ�E���g�̃��Z�b�g
        isRising = false;
        isMovingRight = false;
        riseGasCount = 0;
        moveRightGasCount = 0;

        Debug.Log("RiseGas�X�N���v�g�����������ꂽ�B�t���O�ƃJ�E���g�����Z�b�g���A�^�X�N���L�����Z�������B");
    }
}
