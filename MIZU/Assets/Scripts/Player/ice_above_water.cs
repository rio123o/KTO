//  �X��Ԃ̃L�����ɃA�^�b�`����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice_above_water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //  �ڐG�����ꍇ
    private void OnCollisionEnter(Collision collision)
    {
        //  ������̂������Ԃ̃L�����̏ꍇ
        if (collision.gameObject.CompareTag("Water"))
        {
            //  �X�ƈꏏ�ɓ���
            collision.transform.parent = transform;
        }
    }

    //  �ڐG���������ꂽ�ꍇ
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            //  ���������
            collision.transform.parent = null;
        }
    }

}
