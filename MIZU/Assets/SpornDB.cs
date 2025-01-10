using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject[] boxPrefabs; // ダンボールのPrefabを格納する配列
    public Transform spawnPoint;   // スポーン位置
    public float spawnInterval = 2f; // スポーン間隔

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBox();
            timer = 0f;
        }
    }

    void SpawnBox()
    {
        if (boxPrefabs.Length > 0 && spawnPoint != null)
        {
            // 配列からランダムにPrefabを選択して生成
            int randomIndex = Random.Range(0, boxPrefabs.Length);
            Instantiate(boxPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation);
        }
    }
}

