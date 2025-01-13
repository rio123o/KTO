using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DirectionSwap : MonoBehaviour
{
    [Header("�ǂ̈ʁA��]�����邩�̐ݒ�")]
    [SerializeField] private float rotationAngle = 120f;
    [SerializeField] private float rotationSpeed = 0.1f;

    //  InputAction�A�Z�b�g���Q�Ƃł���ϐ�
    [Header("InputAction��move�A�N�V����������")]
    [SerializeField] InputActionReference inputActions;

    private bool seeRight = true;  //  �E�������Ă��邩�̃t���O

    private Quaternion _initialRotation; //  �����̉�]�p
    private Quaternion _rotatedRotation;  //  ��]��̉�]�p

    private CancellationTokenSource rotationCTS = null;  //  �L�����Z���g�[�N��

    void Start()
    {
        _initialRotation = gameObject.transform.rotation;
        _rotatedRotation = Quaternion.Euler(0, rotationAngle, 0) * _initialRotation;  //  �����̉�]�p�̈ʒu���猩�Ẳ�]�p
    }

    void Awake()
    {
        if (inputActions == null)
            Debug.LogError("�A�N�V������������Ȃ��B");

        inputActions.action.performed += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        //  �������ւ̓��͂����o����
        if (input.x < 0 && seeRight)
        {
            //  ���݁A��]���Ă���ꍇ
            if (rotationCTS != null)
            {
                rotationCTS.Cancel();
                rotationCTS.Dispose();
            }

            //  �V�����L�����Z���g�[�N�����쐬����
            rotationCTS = new CancellationTokenSource();

            //  �������ւ̉�]���n�߂�
            RotatePlayerAsync(_rotatedRotation, false, rotationCTS.Token).Forget();
        }
        //  �E�����ւ̓��͂����o����
        else if (input.x > 0 && !seeRight)
        {
            //  ���݁A��]���Ă���ꍇ
            if (rotationCTS != null)
            {
                rotationCTS.Cancel();
                rotationCTS.Dispose();
            }

            //  �V�����L�����Z���g�[�N�����쐬����
            rotationCTS = new CancellationTokenSource();

            //  �E�����ւ̉�]���n�߂�
            RotatePlayerAsync(_initialRotation, true, rotationCTS.Token).Forget(); ;
        }
    }

    private async UniTaskVoid RotatePlayerAsync(Quaternion targetRotation, bool seeRight, CancellationToken cancellationToken)
    {
        try
        {
            //  ���݂̉�]���擾
            Quaternion initialRotation = transform.rotation;

            float elapsed = 0f;  //  �o�߂�������

            while (elapsed < rotationSpeed)
            {
                //  �L�����Z�����v�����ꂽ�ꍇ�A�����𒆒f����
                cancellationToken.ThrowIfCancellationRequested();

                //  ��]���X���[�Y�ɕ�Ԃ���
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / rotationSpeed);

                elapsed += Time.deltaTime;

                //  ���̃t���[���܂őҋ@
                await UniTask.Yield();
            }

            //  �ŏI�I�ȉ�]��ݒ�
            transform.rotation = targetRotation;

            //  �����̃t���O���X�V
            this.seeRight = seeRight;

            Debug.Log($"{gameObject.name}��{(seeRight ? "�E" : "��")}�������������B");
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"{gameObject.name}�̉�]���L�����Z�����ꂽ�B");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"��]���ɃG���[����������: {ex.Message}");
        }
        finally
        {
            //  �L�����Z���g�[�N����j�����A��]CTS�����Z�b�g����
            rotationCTS?.Dispose();
            rotationCTS = null;
        }
    }

    private void OnEnable()
    {
        inputActions.action.Enable();  //  �A�N�V������L��������
    }

    private void OnDisable()
    {
        inputActions.action.Disable();  //  �A�N�V�����𖳌�������
    }

    private void OnDestroy()
    {
        if (inputActions != null)
            inputActions.action.performed -= OnMovePerformed;

        //  �X�N���v�g��j���������ɉ�]���L�����Z������
        rotationCTS?.Cancel();
        rotationCTS?.Dispose();
    }
}
