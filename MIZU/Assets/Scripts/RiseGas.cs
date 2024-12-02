using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

// 必要なコンポーネントを強制的にアタッチ
[RequireComponent(typeof(Collider))]

public class RiseGas : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 5f;   //  上昇速度
    [SerializeField] private float moveRightSpeed = 5f;  //  右方向への移動速度

    [SerializeField] private bool isRising = false;
    [SerializeField] private bool isMovingRight = false;

    //  複数のトリガーが同時に入る場合のカウント
    private int riseGasCount = 0;
    private int moveRightGasCount = 0;

    //  キャンセルトークンの導入
    private CancellationTokenSource riseCancel;
    private CancellationTokenSource moveRightCancel;

    [SerializeField] private MM_PlayerPhaseState _pState;

    void Start()
    {
        //  必要なコンポーネントの取得
        _pState = GetComponent<MM_PlayerPhaseState>();

        //  エラーのチェック
        if (_pState == null)
        {
            Debug.LogError("MM_PlayerPhaseState コンポーネントが見つかりません");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //  riseGasタグのオブジェクトがトリガーに入った場合
        if(other.CompareTag("riseGas"))
        {
            riseGasCount++;

            if (!isRising) 
            {
                riseCancel = new CancellationTokenSource();
                StartRise(riseCancel.Token).Forget();
            }

            Debug.Log($"playerはRiseGasトリガーによって、上昇し始めた。 Count: {riseGasCount}");
        }

        // moveRightGasタグのオブジェクトがトリガーに入った場合
        if (other.CompareTag("moveRightGas"))
        {
            moveRightGasCount++;

            if (!isMovingRight)
            {
                moveRightCancel = new CancellationTokenSource();
                StartMoveRight(moveRightCancel.Token).Forget();
            }

            Debug.Log($"playerはmoveRightGasトリガーによって、右方向への移動を始めた。 Count: {moveRightGasCount}");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //  riseGasタグのオブジェクトがトリガーから出た場合
        if (other.CompareTag("riseGas"))
        {
            riseGasCount--;

            Debug.Log($"playerはriseGasトリガーから出た。 Count: {riseGasCount}");
            if (riseGasCount <= 0)
            {
                riseGasCount = 0;
                isRising = false;

                //  タスクのキャンセル
                riseCancel?.Cancel();
                riseCancel = null;

                Debug.Log("playerは全てのriseGasトリガーから出た。");
            }
        }

        //  moveRightGasタグのオブジェクトがトリガーから出た場合
        if (other.CompareTag("moveRightGas"))
        {
            moveRightGasCount--;
            Debug.Log($"playerはmoveRightGasトリガーから出た。 Count: {moveRightGasCount}");
            if (moveRightGasCount <= 0)
            {
                moveRightGasCount = 0;
                isMovingRight = false;

                //  タスクのキャンセル
                moveRightCancel?.Cancel();
                moveRightCancel = null;

                Debug.Log("playerは全てのmoveRightGasトリガーから出た。");
            }
        }
    }

    //  上昇処理をUniTaskで実装する
    private async UniTaskVoid StartRise(CancellationToken cancellationToken)
    {
        try
        {
            isRising = true;

            while (isRising && !cancellationToken.IsCancellationRequested)
            {
                if (_pState.GetState() == MM_PlayerPhaseState.State.Gas)
                {
                    float step = riseSpeed * Time.deltaTime;
                    transform.position += Vector3.up * step;

                    
                }

                //  無限に上昇させるため、距離のチェックを削除
                //  移動停止はトリガー退出によって行われる
                await UniTask.Yield();
            }

        }
        catch(OperationCanceledException)
        {
            Debug.Log("StartRiseがキャンセルされました。");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"StartRise エラー: {ex.Message}");
        }
    }

    // 右方向移動処理をUniTaskで実装
    private async UniTaskVoid StartMoveRight(CancellationToken cancellationToken)
    {
        try
        {
            isMovingRight = true;

            while (isMovingRight && !cancellationToken.IsCancellationRequested)
            {
                if (_pState.GetState() == MM_PlayerPhaseState.State.Gas)
                {
                    float step = moveRightSpeed * Time.deltaTime;
                    transform.position += Vector3.right * step;
                }

                // 無限に右方向に移動させるため、距離のチェックを削除
                // 移動停止はトリガー退出によって行われる

                await UniTask.Yield();
            }

        }
        catch(OperationCanceledException)
        {
            Debug.Log("StartMoveRightがキャンセルされました。");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"StartMoveRight エラー: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        //  全てのキャンセルトークンをキャンセル
        riseCancel?.Cancel();
        moveRightCancel?.Cancel();

        //  フラグとカウントのリセット
        isRising = false;
        isMovingRight = false;
        riseGasCount = 0;
        moveRightGasCount = 0;

        Debug.Log("RiseGasスクリプトを破棄された。フラグとカウントをリセットし、タスクをキャンセルした。");
    }

    private void OnDisable()
    {
        //  全てのキャンセルトークンをキャンセル
        riseCancel?.Cancel();
        moveRightCancel?.Cancel();

        //  フラグとカウントのリセット
        isRising = false;
        isMovingRight = false;
        riseGasCount = 0;
        moveRightGasCount = 0;

        Debug.Log("RiseGasスクリプトが無効化された。フラグとカウントをリセットし、タスクをキャンセルした。");
    }
}
