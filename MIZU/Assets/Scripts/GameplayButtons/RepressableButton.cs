using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepressableButton : MonoBehaviour, IButtonAction
{
    //  ボタンが押されている間のフラグ
    protected bool isPressed = false;

    //  ボタンが押された時に実行されるアクションを定義している抽象メソッド
    public abstract void Execute();

    //  プレイヤーがオブジェクトに接触した時に呼び出される
    private void OnCollisionEnter(Collision collision)
    {
        //  プレイヤー以外の衝突の場合は何もしない
        if (!collision.gameObject.CompareTag("Player")) return;

        if (isPressed) return;  //  既に押されていた場合は何もしない


        //  ボタンが押された状態にする
        isPressed = true;
        Execute();
    }

    //  プレイヤーがオブジェクトから離れた時に呼び出される
    private void OnCollisionExit(Collision collision)
    {
        //  プレイヤー以外の離脱の場合は何もしない
        if (!collision.gameObject.CompareTag("Player")) return;

        //  ボタンが押された状態をリセット
        isPressed = false;
    }
}
