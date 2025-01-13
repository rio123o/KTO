using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DirectionSwap : MonoBehaviour
{
    [Header("どの位、回転させるかの設定")]
    [SerializeField] private float rotationAngle = 120f;
    [SerializeField] private float rotationSpeed = 0.1f;

    //  InputActionアセットを参照できる変数
    [Header("InputActionのmoveアクションを入れる")]
    [SerializeField] InputActionReference inputActions;

    private bool seeRight = true;  //  右を向いているかのフラグ

    private Quaternion _initialRotation; //  初期の回転角
    private Quaternion _rotatedRotation;  //  回転後の回転角

    private CancellationTokenSource rotationCTS = null;  //  キャンセルトークン

    void Start()
    {
        _initialRotation = gameObject.transform.rotation;
        _rotatedRotation = Quaternion.Euler(0, rotationAngle, 0) * _initialRotation;  //  初期の回転角の位置から見ての回転角
    }

    void Awake()
    {
        if (inputActions == null)
            Debug.LogError("アクションが見つからない。");

        inputActions.action.performed += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        //  左方向への入力を検出する
        if (input.x < 0 && seeRight)
        {
            //  現在、回転している場合
            if (rotationCTS != null)
            {
                rotationCTS.Cancel();
                rotationCTS.Dispose();
            }

            //  新しいキャンセルトークンを作成する
            rotationCTS = new CancellationTokenSource();

            //  左方向への回転を始める
            RotatePlayerAsync(_rotatedRotation, false, rotationCTS.Token).Forget();
        }
        //  右方向への入力を検出する
        else if (input.x > 0 && !seeRight)
        {
            //  現在、回転している場合
            if (rotationCTS != null)
            {
                rotationCTS.Cancel();
                rotationCTS.Dispose();
            }

            //  新しいキャンセルトークンを作成する
            rotationCTS = new CancellationTokenSource();

            //  右方向への回転を始める
            RotatePlayerAsync(_initialRotation, true, rotationCTS.Token).Forget(); ;
        }
    }

    private async UniTaskVoid RotatePlayerAsync(Quaternion targetRotation, bool seeRight, CancellationToken cancellationToken)
    {
        try
        {
            //  現在の回転を取得
            Quaternion initialRotation = transform.rotation;

            float elapsed = 0f;  //  経過した時間

            while (elapsed < rotationSpeed)
            {
                //  キャンセルが要求された場合、処理を中断する
                cancellationToken.ThrowIfCancellationRequested();

                //  回転をスムーズに補間する
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / rotationSpeed);

                elapsed += Time.deltaTime;

                //  次のフレームまで待機
                await UniTask.Yield();
            }

            //  最終的な回転を設定
            transform.rotation = targetRotation;

            //  向きのフラグを更新
            this.seeRight = seeRight;

            Debug.Log($"{gameObject.name}が{(seeRight ? "右" : "左")}方向を向いた。");
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"{gameObject.name}の回転がキャンセルされた。");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"回転中にエラーが発生した: {ex.Message}");
        }
        finally
        {
            //  キャンセルトークンを破棄し、回転CTSをリセットする
            rotationCTS?.Dispose();
            rotationCTS = null;
        }
    }

    private void OnEnable()
    {
        inputActions.action.Enable();  //  アクションを有効化した
    }

    private void OnDisable()
    {
        inputActions.action.Disable();  //  アクションを無効化した
    }

    private void OnDestroy()
    {
        if (inputActions != null)
            inputActions.action.performed -= OnMovePerformed;

        //  スクリプトを破棄した時に回転をキャンセルする
        rotationCTS?.Cancel();
        rotationCTS?.Dispose();
    }
}
