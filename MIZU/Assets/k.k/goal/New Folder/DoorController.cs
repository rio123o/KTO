using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;

    // 初期化
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // ドアを開ける
    public void OpenDoor()
    {
        animator.SetBool("goal", true);
    }

    // ドアを閉める
    public void CloseDoor()
    {
        animator.SetBool("goal", false);
    }
}