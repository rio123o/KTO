using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class WaterLevelChangeButton : BaseButton
{
    [Header("���ʂ��㏸�܂��͉��~���������I�u�W�F�N�g")]
    [SerializeField] private Transform puddleObject;
    [Header("���ʂ��㏸�܂��͉��~�����鋗��")]
    [SerializeField] private float movePuddleDistance = -5f;
    [Header("���ʂ��㏸�܂��͉��~������܂ł̎���")]
    [SerializeField] private float movePuddleDuration = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;  //  �I�u�W�F�N�g�̈ړ���̈ʒu��ێ�����ϐ�
    private float moveSpeed;

    //  �L�����Z���g�[�N���̓���
    private CancellationTokenSource cancellationTokenSource;

    //  �ړ������ǂ����̃t���O
    private bool isMoving = false;

    void Start()
    {
        if (puddleObject != null)
        {
            initialPosition = puddleObject.position;
            targetPosition = initialPosition + new Vector3(0, movePuddleDistance, 0);
            moveSpeed = Mathf.Abs(movePuddleDistance) / movePuddleDuration;

            //  �L�����Z���[�V�����\�[�X�̐���
            cancellationTokenSource = new CancellationTokenSource();
        }
        else
        {
            Debug.LogError($"{gameObject.name}: ���ʂ��㏸�܂��͉��~���������I�u�W�F�N�g���ݒ肳��Ă��܂���B");
            //  �^�[�Q�b�g���ݒ肳��Ă��Ȃ��ꍇ�A�ȍ~�̏������s��Ȃ�
            enabled = false;
        }
    }

    //  �{�^���������ꂽ���Ɏ��s����A�N�V����
    public override async void Execute()
    {
        if (isMoving)
        {
            Debug.LogWarning($"{gameObject.name}: ���Ɉړ����ɂȂ��Ă���");
            return;
        }

        if (cancellationTokenSource == null)
        {
            return;
        }

        try
        {
            isMoving = true;
            await MoveAsync(cancellationTokenSource.Token);
            Debug.Log($"{gameObject.name}: {puddleObject.name} ���ړ��������B");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"{gameObject.name}: MoveAsync���L�����Z�����ꂽ�B");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{gameObject.name}: MoveAsync���ɃG���[�����������B{ex.Message}");
        }
        finally
        {
            isMoving = false;
        }
    }

    private async UniTask MoveAsync(CancellationToken cancellationToken)
    {
        Vector3 endPosition = targetPosition;

        while (Vector3.Distance(puddleObject.position, endPosition) > 0.01f)
        {
            //  �L�����Z�����v������Ă������O�𓊂��ď����𒆒f
            cancellationToken.ThrowIfCancellationRequested();

            puddleObject.position = Vector3.MoveTowards(puddleObject.position, endPosition, moveSpeed * Time.deltaTime); await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }

        puddleObject.position = endPosition;
    }

    //  �I�u�W�F�N�g���j�������ۂɃL�����Z�������s
    private void OnDestroy()
    {
        CancelMovement();
    }

    //  �I�u�W�F�N�g�������������ۂɂ��L�����Z�������s
    private void OnDisable()
    {
        CancelMovement();
    }

    //  �I�u�W�F�N�g���ĂїL�������ꂽ�ۂɃL�����Z���[�V�����\�[�X�����Z�b�g
    private void OnEnable()
    {
        if (puddleObject != null)
        {
            //  �V�����L�����Z���[�V�����\�[�X�𐶐�
            cancellationTokenSource = new CancellationTokenSource();
        }
    }

    //  �L�����Z���[�V�����̎��s�ƃ\�[�X�̔j��
    private void CancelMovement()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
