using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MM_PlayerPhaseState))]

public class puddle_Script : MonoBehaviour
{

    private float contactTime = 0f;  //  �ڐG��������
    private bool isColliding = false;  //  �I�u�W�F�N�g���ڐG���Ă��邩�̃t���O
    public float destroyTime = 2f;  //  �I�u�W�F�N�g���j�󂳂�鎞��

    MM_PlayerPhaseState _pState;



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("puddle"))
        {
            isColliding = true;
            Debug.Log("OK");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("puddle"))
        {
            isColliding = false;
            contactTime = 0f;  //  ���ꂽ��ڐG���Ԃ����Z�b�g����
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _pState = GetComponent<MM_PlayerPhaseState>();
    }

    // Update is called once per frame
    void Update()
    {
        //  puddle�ƐڐG������
        if(isColliding)
        {
            //  �v���C���[�̏�Ԃ�Liquid�̎�
            if(_pState.GetState() == MM_PlayerPhaseState.State.Liquid)
            {
                contactTime += Time.deltaTime;
                //Debug.Log("Count");

                if (contactTime >= destroyTime)
                {
                    //Destroy(gameObject);
                }
            }
            else  //  �v���C���[�̏�Ԃ�Liquid�ȊO�̎�
            {
                contactTime = 0f;
            }

        }
    }
}
