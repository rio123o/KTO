using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepressableButton : MonoBehaviour, IButtonAction
{
    //  �{�^����������Ă���Ԃ̃t���O
    protected bool isPressed = false;

    //  �{�^���������ꂽ���Ɏ��s�����A�N�V�������`���Ă��钊�ۃ��\�b�h
    public abstract void Execute();

    //  �v���C���[���I�u�W�F�N�g�ɐڐG�������ɌĂяo�����
    private void OnCollisionEnter(Collision collision)
    {
        //  �v���C���[�ȊO�̏Փ˂̏ꍇ�͉������Ȃ�
        if (!collision.gameObject.CompareTag("Player")) return;

        if (isPressed) return;  //  ���ɉ�����Ă����ꍇ�͉������Ȃ�

        //  �v���C���[�̏�Ԃ��擾����
        var playerPhaseState = collision.gameObject.GetComponent<MM_PlayerPhaseState>();

        //  �v���C���[��Solid�̏�Ԃł͂Ȃ��ꍇ�ɕԂ�
        if (playerPhaseState == null || playerPhaseState.GetState() != MM_PlayerPhaseState.State.Solid)
        {
            Debug.Log($"{gameObject.name}: �v���C���[��Solid�̏�Ԃł͂Ȃ�");
            return;
        }

        //  �{�^���������ꂽ��Ԃɂ���
        isPressed = true;
        Execute();
    }

    //  �v���C���[���I�u�W�F�N�g���痣�ꂽ���ɌĂяo�����
    private void OnCollisionExit(Collision collision)
    {
        //  �v���C���[�ȊO�̗��E�̏ꍇ�͉������Ȃ�
        if (!collision.gameObject.CompareTag("Player")) return;

        //  �{�^���������ꂽ��Ԃ����Z�b�g
        isPressed = false;
    }
}
