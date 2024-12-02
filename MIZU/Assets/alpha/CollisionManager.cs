using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public GameObject player1Model;   // �v���C���[1���f��
    public GameObject player2Model;   // �v���C���[2���f��

    private Collider player1Collider;
    private Collider player2Collider;

    // �v���C���[1�ƃv���C���[2���ꂼ��̏Փ˂����I�u�W�F�N�g��ێ����郊�X�g
    private List<Collider> player1HitColliders = new List<Collider>();
    private List<Collider> player2HitColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        if (player1Model != null)
        {
            player1Collider = player1Model.GetComponent<Collider>();
        }
        if (player2Model != null)
        {
            player2Collider = player2Model.GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[1�̏Փ˂��`�F�b�N
        if (player1Collider != null)
        {
            CheckCollisions(player1Collider, player1HitColliders);
        }
        // �v���C���[2�̏Փ˂��`�F�b�N
        if (player2Collider != null)
        {
            CheckCollisions(player2Collider, player2HitColliders);
        }
    }

    // �Փ˂��m�F���郁�\�b�h
    private void CheckCollisions(Collider playerCollider, List<Collider> hitCollidersList)
    {
        // �O��̏Փ˃��X�g���N���A
        hitCollidersList.Clear();

        // �R���C�_�[�̎���ɂ���S�Ă̏Փˑ̂��擾
        Collider[] hitColliders = Physics.OverlapSphere(playerCollider.transform.position, playerCollider.bounds.extents.magnitude);

        // �e�R���C�_�[�ɂ��ď���
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider != playerCollider)
            {
                // �Փ˂����I�u�W�F�N�g�����X�g�ɒǉ�
                hitCollidersList.Add(hitCollider);

                // �Փ˂����I�u�W�F�N�g�̃^�O���m�F���ď���
                Debug.Log($"�Փ˂����I�u�W�F�N�g: {hitCollider.gameObject.name}, �^�O: {hitCollider.tag}");

            }
        }
    }

    // �v���C���[1���q�[���X�|�b�g�ɏՓ˂������ǂ������m�F
    public List<Collider> GetPlayer1HitColliders()
    {
        return player1HitColliders;
    }

    // �v���C���[2���q�[���X�|�b�g�ɏՓ˂������ǂ������m�F
    public List<Collider> GetPlayer2HitColliders()
    {
        return player2HitColliders;
    }
}
