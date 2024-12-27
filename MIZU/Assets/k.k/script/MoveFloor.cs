using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KK_MoveFloor : MonoBehaviour
{
    public enum MoveMode
    {
        Vertical, // 直線追従
        Horizontal    // ジグザグ追従
    }

    public MoveMode moveMode = MoveMode.Vertical; // 現在の追従モード
    public float speed = 3f;                           // 移動速度

    public Vector3 startPoint; // 床の開始位置
    public Vector3 endPoint;   // 床の終了位置

    private bool movingToEnd = true;

    public void Start()
    {
        startPoint = transform.position;
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

    // ジグザグ追従の処理
    private void HorizontalFollow()
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
}