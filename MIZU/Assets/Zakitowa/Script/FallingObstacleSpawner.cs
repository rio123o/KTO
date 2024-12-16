using System.Collections;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;  // 障害物のプレハブ
    public float fallSpeed = 5f;  // 障害物の落下速度
    public float respawnTime = 3f;  // 障害物の復活時間
    public Vector3 spawnPosition;  // 障害物が生成される位置

    private GameObject currentObstacle;  // 現在アクティブな障害物

    private void Start()
    {
        // 最初の障害物を生成
        SpawnNewObstacle();
    }

    private void Update()
    {
        if (currentObstacle != null)
        {
            // 障害物が落ちる処理
            currentObstacle.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // 障害物が画面外に落ちた場合に新たに生成
            if (currentObstacle.transform.position.y < -10f)
            {
                Destroy(currentObstacle);  // 現在の障害物を削除
                SpawnNewObstacle();  // 新しい障害物を生成
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが障害物に触れると死亡
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが死亡する処理（例: ヘルス減少やリスタート）
            collision.gameObject.GetComponent<PlayerController>().Die();

            // 障害物を削除して新しい障害物を生成
            Destroy(currentObstacle);
            SpawnNewObstacle();
        }
    }

    private void SpawnNewObstacle()
    {
        // 新しい障害物を生成
        currentObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
