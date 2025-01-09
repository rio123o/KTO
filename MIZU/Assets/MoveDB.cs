using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour
{
    public float speed = 2f;      // 移動速度
    public float distance = 5f;  // 移動距離
    private Vector3 startPosition;
    private bool isMoving = true;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // 時間に基づいてスムーズに移動
            float offset = Time.time * speed;
            transform.position = startPosition + new Vector3(offset, 0, 0);

            // 移動距離が指定値を超えたら終了
            if (Vector3.Distance(startPosition, transform.position) >= distance)
            {
                isMoving = false;
                Destroy(gameObject); // オブジェクトを削除
            }
        }
    }
}

//これでダンボール動くと思う

//移動方向を変更したい場合は、new Vector3の値をnew Vector3(0, offset, 0)（縦移動）やnew Vector3(0, 0, offset)（奥行き移動）に変更して
//ダンボールプレハブ化してこのスクリプトアタッチして