using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class goal : MonoBehaviour
{
    public DoorController doorController;

    public Transform moveDirectionReference;
    public float moveSpeed = 1.0f; // z方向への移動速度


    private void Awake()
    {
        // 子オブジェクトを取得
        moveDirectionReference = transform.Find("MoveDirection");
        if (moveDirectionReference == null)
        {
            Debug.LogError("MoveDirection child object is missing  Please add a child object named 'MoveDirection'.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerInputコンポーネントを取得して無効化
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.enabled = false;
            }

            doorController.OpenDoor();
            StartCoroutine(MoveObject(other.gameObject));
        }
    }

    private IEnumerator MoveObject(GameObject player)
    {
        if (moveDirectionReference == null)
        {
            Debug.LogError("Move direction reference is not set");
            yield break;
        }

        Vector3 moveDirection = (moveDirectionReference.position - transform.position).normalized;

        while (true)
        {
            player.transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
