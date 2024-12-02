using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class WaterLevelChangeButton : BaseButton
{
    [Header("水位を上昇または下降させたいオブジェクト")]
    [SerializeField] private Transform puddleObject;
    [Header("水位を上昇または下降させる距離")]
    [SerializeField] private float movePuddleDistance = -5f;
    [Header("水位を上昇または下降させるまでの時間")]
    [SerializeField] private float movePuddleDuration = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;  //  オブジェクトの移動先の位置を保持する変数
    private float moveSpeed;

    //  キャンセルトークンの導入
    private CancellationTokenSource cancellationTokenSource;

    //  移動中かどうかのフラグ
    private bool isMoving = false;

    void Start()
    {
        if (puddleObject != null)
        {
            initialPosition = puddleObject.position;
            targetPosition = initialPosition + new Vector3(0, movePuddleDistance, 0);
            moveSpeed = Mathf.Abs(movePuddleDistance) / movePuddleDuration;

            //  キャンセレーションソースの生成
            cancellationTokenSource = new CancellationTokenSource();
        }
        else
        {
            Debug.LogError($"{gameObject.name}: 水位を上昇または下降させたいオブジェクトが設定されていません。");
            //  ターゲットが設定されていない場合、以降の処理を行わない
            enabled = false;
        }
    }

    //  ボタンが押された時に実行するアクション
    public override async void Execute()
    {
        if (isMoving)
        {
            Debug.LogWarning($"{gameObject.name}: 既に移動中になっている");
            return;
        }

        if (cancellationTokenSource == null)
        {
            return;
        }

        try
        {
            isMoving = true;
            await MoveAsync(cancellationTokenSource.Token);
            Debug.Log($"{gameObject.name}: {puddleObject.name} を移動させた。");
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"{gameObject.name}: MoveAsyncがキャンセルされた。");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{gameObject.name}: MoveAsync中にエラーが発生した。{ex.Message}");
        }
        finally
        {
            isMoving = false;
        }
    }

    private async UniTask MoveAsync(CancellationToken cancellationToken)
    {
        Vector3 endPosition = targetPosition;

        while (Vector3.Distance(puddleObject.position, endPosition) > 0.01f)
        {
            //  キャンセルが要求されていたら例外を投げて処理を中断
            cancellationToken.ThrowIfCancellationRequested();

            puddleObject.position = Vector3.MoveTowards(puddleObject.position, endPosition, moveSpeed * Time.deltaTime); await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }

        puddleObject.position = endPosition;
    }

    //  オブジェクトが破棄される際にキャンセルを実行
    private void OnDestroy()
    {
        CancelMovement();
    }

    //  オブジェクトが無効化される際にもキャンセルを実行
    private void OnDisable()
    {
        CancelMovement();
    }

    //  オブジェクトが再び有効化された際にキャンセレーションソースをリセット
    private void OnEnable()
    {
        if (puddleObject != null)
        {
            //  新しいキャンセレーションソースを生成
            cancellationTokenSource = new CancellationTokenSource();
        }
    }

    //  キャンセレーションの実行とソースの破棄
    private void CancelMovement()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
