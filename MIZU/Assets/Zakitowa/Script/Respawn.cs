using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        // プレイヤーの初期位置を保存
        startPosition = transform.position;
    }

    void Update()
    {
        // プレイヤーの高さが特定の値より低くなった場合
        if (transform.position.y < -20)
        {
            Respawn();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 障害物に衝突したとき
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
    }

        // スタート位置に戻す処理
        void Respawn()
    {
        transform.position = startPosition;
    }
}
