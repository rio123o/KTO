using UnityEngine;

public class MultiPlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform[] players; // 全プレイヤーのTransform
    [SerializeField] private Transform initialRespawnPoint; // 初期リスポーン地点
    [SerializeField] private Transform respawnPoint;       // 通常リスポーン地点
    [SerializeField] private float fallThreshold = -10f;   // 落下とみなす高さ

    private void Start()
    {
        // ゲーム開始時に全プレイヤーを初期リスポーン
        InitialRespawnAll();
    }

    private void Update()
    {
        // 全プレイヤーの位置を監視し、落下していたら全員リスポーン
        foreach (var player in players)
        {
            if (player.position.y < fallThreshold)
            {
                RespawnAll(); // いずれかのプレイヤーが落下したら全員リスポーン
                break;
            }
        }
    }

    // 初期リスポーン
    private void InitialRespawnAll()
    {
        foreach (var player in players)
        {
            player.position = initialRespawnPoint.position;
            Debug.Log($"{player.name} has been moved to the initial respawn point.");
        }
    }

    // 通常リスポーン
    private void RespawnAll()
    {
        foreach (var player in players)
        {
            player.position = respawnPoint.position;
            Debug.Log($"{player.name} has been respawned.");
        }
    }
}
