using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class changeRiseGasTag : MonoBehaviour
{
    [Header("回転状態に応じてタグを付与・除去する対象のオブジェクト")]
    [SerializeField] private GameObject riseGusObject;
    [Header("付与したいタグの名前を入力する")]
    [Tooltip("(上昇:riseGas)(右に移動:moveRightGas)")]
    [SerializeField] private string activeTag;

    private RotateObject rotateObject;


    void Awake()
    {
        rotateObject = GetComponent<RotateObject>();

        if (rotateObject == null)
        {
            Debug.LogError($"RotateObjectが{gameObject.name}に見つかりません。");
            return;
        }

        if (riseGusObject == null)
        {
            Debug.LogError($"RiseGasObjectが{gameObject.name}に設定されていません。");
            return;
        }
        if (string.IsNullOrEmpty(activeTag))
        {
            Debug.LogError($"ActiveTagが{gameObject.name}に設定されていません。");
            return;
        }

        //  RotateObjectのイベントを読む
        rotateObject.RotatingChange += RotateStateChange;

        //  初期状態でタグを設定する
        UpdateTag(rotateObject.IsRotating);
    }

    //  スクリプトが破棄される時に、Awake()のイベントを読む行為を破棄する
    void OnDestroy()
    {
        if (rotateObject != null)
        {
            rotateObject.RotatingChange -= RotateStateChange;
        }
    }

    //  RotateObjectの回転状態が変化した際に呼び出されるメソッド
    private void RotateStateChange(RotateObject rotate, bool isRotating)
    {
        UpdateTag(isRotating);
    }

    private void UpdateTag(bool isRotating)
    {
        if(isRotating) 
        {
            //  すでに"riseGas"のタグが付いている場合、何もしない
            if (riseGusObject.CompareTag(activeTag)) return;

            //  "riseGas"のタグの付与
            riseGusObject.tag = activeTag;
            Debug.Log($"{riseGusObject.name}に'{activeTag}'のタグを付与した。");
            return;
        }

        //  "riseGas"のタグが付いていない場合、何もしない
        if (!riseGusObject.CompareTag(activeTag)) return;

        //  "Untagged"のタグに変更する
        riseGusObject.tag = "Untagged";
        Debug.Log($"{riseGusObject.name}から'{activeTag}'のタグを消しました。");
    }
}
