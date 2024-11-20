using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Unity.VisualScripting;

public class OpenGateScript : MonoBehaviour
{
    [SerializeField]private float openSpeed = 2f;  //  �㏸���鑬�x
    [SerializeField] private float openHight = 5f;  //  �㏸���鍂��
    [SerializeField] private float defaltHight = 0f;  //  ���̍���

    private Vector3 originalPosition;
    private CancellationTokenSource moveCTS = null;  //  �ړ��̎��̃L�����Z���g�[�N���\�[�X

    void Start()
    {
        originalPosition = transform.position;
    }

    //  �����㏸������
    public void OpenGate()
    {
        //  ���łɈړ����̏ꍇ�A�L�����Z������
        if (moveCTS != null)
        {
            moveCTS.Cancel();
            moveCTS.Dispose();
        }


    }

    //  ����߂�
    public void CloseGate() 
    {
        
    }

    //  �w�肷��ʒu�Ɉړ�����񓯊����\�b�h
    private async UniTaskVoid MoveToPositionAsync(Vector3 targetPosition, CancellationToken cancellationToken)
    {
        try
        {
            while(Vector3.Distance(originalPosition, targetPosition) > 0.01f)
            {
                //  �L�����Z�����v�����ꂽ�ꍇ�A��O�𓊂��ď����𒆒f���� 
                cancellationToken.ThrowIfCancellationRequested();

                //  ���݂̈ʒu����^�[�Q�b�g�̈ʒu�Ɍ����Ĉړ�����
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);

                //  ���̃t���[���܂őҋ@����
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
            //  �ŏI�I�Ȉʒu�ɍs����悤�ɐݒ肷��
            transform.position = targetPosition;
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"{gameObject.name}�̈ړ����L�����Z�����ꂽ�B");
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"�ړ����ɃG���[����������: {ex.Message}");
        }
        finally 
        {
            //  �L�����Z���g�[�N����j�����āA���Z�b�g����
            moveCTS.Dispose();
            moveCTS = null;
        }
    }

    private void OnDestroy()
    {
        moveCTS?.Cancel();
        moveCTS?.Dispose();
    }
}
