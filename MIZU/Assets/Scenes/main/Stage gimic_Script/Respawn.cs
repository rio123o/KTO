using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // 落下ゾーンに触れた際にスタート位置に戻す
        if (other.gameObject.CompareTag("Dead Zone"))
        {
            Debug.Log("Death");
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        transform.position = startPosition;
    }
}
