using System.Collections;
using UnityEngine;

public class BreakFloor : MonoBehaviour
{
    public int requiredPlayers = 2; // ����̂ɕK�v�ȃv���C���[��
    public float timeWindow = 1f; // �����ƌ��Ȃ����߂̎��ԁi�b�j
    private int currentPlayerCount = 0; // ���ݏ��ɏ���Ă���v���C���[�̐�
    private float lastPlayerEnterTime = 0f; // �Ō�̃v���C���[�����������
    private bool isBroken = false; // ������ꂽ���ǂ���

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount++;

            // �ŏ��̃v���C���[��������Ƃ��A�^�C���X�^���v���L�^
            if (currentPlayerCount == 1)
            {
                lastPlayerEnterTime = Time.time;
            }

            // �����������`�F�b�N
            CheckIfShouldBreak();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount--;
        }
    }

    private void CheckIfShouldBreak()
    {
        // �K�v�Ȑl���ɒB���Ă��āA�����ԓ��ɏ�����ꍇ�ɉ�
        if (currentPlayerCount >= requiredPlayers && !isBroken)
        {
            if (Time.time - lastPlayerEnterTime <= timeWindow)
            {
                DestroyFloor();
            }
        }
    }

    private void DestroyFloor()
    {
        isBroken = true;
        // �����A�N�e�B�u��
        gameObject.SetActive(false);

        // �G�t�F�N�g��A�j���[�V������ǉ�����ꍇ�͂����ɏ������L�q
        Debug.Log("Floor has broken!");
    }
}
