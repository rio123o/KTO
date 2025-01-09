using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("生成するプレハブ")]
    public GameObject prefab; // 生成するプレハブを指定

    [Header("生成間隔")]
    public float spawnInterval = 2f; // 生成間隔（秒）

    private void Start()
    {
        // 一定間隔でオブジェクトを生成
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    private void SpawnObject()
    {
        if (prefab != null)
        {
            // プレハブの初期座標と回転を参照して生成
            Vector3 prefabPosition = prefab.transform.position;
            Quaternion prefabRotation = prefab.transform.rotation;

            GameObject instance = Instantiate(prefab, prefabPosition, prefabRotation);

            Debug.Log($"オブジェクト生成: {instance.name} at {prefabPosition}");
        }
        else
        {
            Debug.LogError("プレハブが設定されていません！");
        }
    }
}
