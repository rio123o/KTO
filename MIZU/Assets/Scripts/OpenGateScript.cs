using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Unity.VisualScripting;

public class OpenGateScript : MonoBehaviour
{
    [SerializeField]private float openSpeed = 2f;  //  上昇する速度
    [SerializeField] private float openHight = 5f;  //  上昇する高さ
    [SerializeField] private float defaltHight = 0f;  //  元の高さ

    private Vector3 originalPosition;
    private CancellationTokenSource moveCTS = null;  //  移動の時のキャンセルトークンソース

    void Start()
    {
        originalPosition = transform.position;
    }

    //  扉を上昇させる
    public void OpenGate()
    {
        //  すでに移動中の場合、キャンセルする
        if (moveCTS != null)
        {
            moveCTS.Cancel();
            moveCTS.Dispose();
        }


    }

    //  扉を閉める
    public void CloseGate() 
    {
        
    }

    //  指定する位置に移動する非同期メソッド
    private async UniTaskVoid MoveToPositionAsync(Vector3 targetPosition, CancellationToken cancellationToken)
    {
        try
        {
            while(Vector3.Distance(originalPosition, targetPosition) > 0.01f)
            {
                //  キャンセルが要求された場合、例外を投げて処理を中断する 
                cancellationToken.ThrowIfCancellationRequested();

                //  現在の位置からターゲットの位置に向けて移動する
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);

                //  次のフレームまで待機する
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
            //  最終的な位置に行けるように設定する
            transform.position = targetPosition;
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"{gameObject.name}の移動がキャンセルされた。");
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"移動中にエラーが発生した: {ex.Message}");
        }
        finally 
        {
            //  キャンセルトークンを破棄して、リセットする
            moveCTS.Dispose();
            moveCTS = null;
        }
    }

    private void OnDestroy()
    {
        moveCTS?.Cancel();
        moveCTS?.Dispose();
    }
}
