using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    public enum MoveMode
    {
        Vertical,    // 直線追従
        Horizontal   // ジグザグ追従
    }

    public MoveMode moveMode = MoveMode.Vertical; // 現在の追従モード
    public float speed = 3f;                      // 移動速度

    public Vector3 relativeEndPoint = new Vector3(0, 5, 0); // 開始位置からの相対座標
    private Vector3 startPoint;   // 床の開始位置
    private Vector3 endPoint;     // 床の終了位置

    private bool movingToEnd = true;

    public void Start()
    {
        // 現在地を開始位置として設定
        startPoint = transform.position;

        // 終了位置を相対的に計算
        endPoint = startPoint + relativeEndPoint;
    }

    public void Update()
    {
        // モードに応じて処理を切り替え
        switch (moveMode)
        {
            case MoveMode.Vertical:
                VerticalFollow();
                break;

            case MoveMode.Horizontal:
                HorizontalFollow();
                break;
        }
    }

    // 直線追従の処理
    private void VerticalFollow()
    {
        // 床を移動
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPoint) < 0.1f)
            {
                movingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPoint) < 0.1f)
            {
                movingToEnd = true;
            }
        }
    }

    private void HorizontalFollow()
    {
        // 水平方向の追従処理をここに実装
        // 必要に応じてパターンを変更
    }
}
