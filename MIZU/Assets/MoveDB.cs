using UnityEngine;

public class BoxMover : MonoBehaviour
{
    public float speed = 2f;      // 移動速度
    public float distance = 5f;  // 移動距離
    private Vector3 startPosition;
    private float spawnTime;     // 生成された瞬間の時間

    void Start()
    {
        // 初期位置と生成時間を記録
        startPosition = transform.position;
        spawnTime = Time.time; // このダンボールが生成された瞬間の時間
    }

    void Update()
    {
        // 生成された瞬間の時間を基準に移動距離を計算
        float elapsedTime = Time.time - spawnTime; // このダンボールの経過時間
        float offset = elapsedTime * speed;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        // 距離を超えたらオブジェクトを削除
        if (offset >= distance)
        {
            Destroy(gameObject);
        }
    }
}