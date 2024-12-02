using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPuddle : BaseButton
{
    [Header("消滅させたいオブジェクト")]
    [SerializeField] private GameObject puddleObject;

    //  ボタンが押された時に実行するアクション
    public override void Execute()
    {
        if(puddleObject.activeSelf && puddleObject != null)
        {
            puddleObject.SetActive(false);
        }
    }
}
