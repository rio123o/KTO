using UnityEngine;

public class FloatingEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 横方向の移動速度
    public float floatAmplitude = 0.5f; // 上下の振幅
    public float floatFrequency = 2.0f; // 上下の周波数

    private Vector3 startPosition;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;
    }

    void Update()
    {
        // 横方向に移動
        Vector3 horizontalMovement = new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        // 上下に浮く動き
        Vector3 floatOffset = new Vector3(0, Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, 0);

        // 新しい位置を設定
        transform.position = startPosition + floatOffset + horizontalMovement;

        // X座標を更新してスクロールを続ける
        startPosition.x += moveSpeed * Time.deltaTime;
    }
}
