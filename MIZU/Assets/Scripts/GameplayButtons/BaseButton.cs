using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseButton : MonoBehaviour, IButtonAction
{
    protected bool isPressed = false;  //  �{�^���������ꂽ���ǂ����̃t���O

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
        if (playerPhaseState ==  null || playerPhaseState.GetState() != MM_PlayerPhaseState.State.Solid)
        {
            Debug.Log($"{gameObject.name}: �v���C���[��Solid�̏�Ԃł͂Ȃ�");
            return;
        }

        //  �{�^���������ꂽ��Ԃɂ���
        isPressed = true;
        Execute();
    }
}
