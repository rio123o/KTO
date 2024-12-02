using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPuddle : BaseButton
{
    [Header("���ł��������I�u�W�F�N�g")]
    [SerializeField] private GameObject puddleObject;

    //  �{�^���������ꂽ���Ɏ��s����A�N�V����
    public override void Execute()
    {
        if(puddleObject.activeSelf && puddleObject != null)
        {
            puddleObject.SetActive(false);
        }
    }
}
