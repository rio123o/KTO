using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("切り替え後のカメラ")]
    private CinemachineVirtualCamera virtualCamera;

    // 切り替え後のカメラの元々のPriorityを保持しておく
    private int defaultPriority;

    // Start is called before the first frame update
    void Start()
    {
        defaultPriority = virtualCamera.Priority;
    }

    void Update()
    {
        // プレイヤーがコライダー内にいるか確認
        if (!IsPlayerInCollider())
        {
            virtualCamera.Priority = defaultPriority; // カメラを元に戻す
        }
    }

    private bool IsPlayerInCollider()
    {
        // Collider範囲内にいるプレイヤーオブジェクトをチェック
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<Collider>().bounds.extents.magnitude);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Colliderの範囲に入り続けている間実行され続ける
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 他のvirtualCameraよりも高い優先度にすることで切り替わる
            virtualCamera.Priority = 100;
        }
    }

    /// <summary>
    /// Colliderから出たときに実行される
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 元のpriorityに戻す
            virtualCamera.Priority = defaultPriority;
        }
    }
}
