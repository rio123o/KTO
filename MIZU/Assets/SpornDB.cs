using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; // ダンボールのPrefab
    public Transform spawnPoint; // スポーン位置
    public float spawnInterval = 2f; // スポーン間隔

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // 一定間隔でダンボールを生成
        if (timer >= spawnInterval)
        {
            SpawnBox();
            timer = 0f;
        }
    }

    void SpawnBox()
    {
        if (boxPrefab != null && spawnPoint != null)
        {
            // スポーン位置にPrefabを生成
            Instantiate(boxPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}