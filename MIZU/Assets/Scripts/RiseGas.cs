using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

// 必要なコンポーネントを強制的にアタッチ
[RequireComponent(typeof(Collider), typeof(Rigidbody))]

public class RiseGas : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 5f;   //  上昇速度
    [SerializeField] private float moveRightSpeed = 5f;  //  右方向への移動速度

    [SerializeField] private MM_PlayerPhaseState _pState;

    //  Gasに影響を与えるアクティブなDirectionのリスト
    private List<GasDirection> activeDirections = new List<GasDirection>();

    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //  必要なコンポーネントの取得
        
        //  エラーのチェック
        if (!TryGetComponent<MM_PlayerPhaseState>(out _pState))
        {
            Debug.LogError("MM_PlayerPhaseState コンポーネントが見つかりません");
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_pState.GetState() != MM_PlayerPhaseState.State.Gas) return;

        Vector3 finalMove = Vector3.zero;

        foreach (var direction in activeDirections)
        {
            // 各方向に応じた速度を乗じて移動ベクトルを累積
            float speed = Mathf.Abs(direction.Dir.x) > 0 ? moveRightSpeed : riseSpeed;
            finalMove += direction.Dir.normalized * speed;
        }

        if (finalMove == Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        rigidbody.velocity = finalMove;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GasDirection dir))
        {
            //  ガスエリアに入ったDirectionをリストに追加
            if (!activeDirections.Contains(dir))
            {
                activeDirections.Add(dir);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GasDirection directionSwap))
        {
            //  ガスエリアから出たDirectionをリストから削除
            if (activeDirections.Contains(directionSwap))
            {
                activeDirections.Remove(directionSwap);
            }
        }
    }
}
