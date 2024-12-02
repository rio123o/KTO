using System.Collections;
using UnityEngine;

public class BreakFloor : MonoBehaviour
{
    public int requiredPlayers = 2; // 壊れるのに必要なプレイヤー数
    public float timeWindow = 1f; // 同時と見なすための時間（秒）
    private int currentPlayerCount = 0; // 現在床に乗っているプレイヤーの数
    private float lastPlayerEnterTime = 0f; // 最後のプレイヤーが乗った時間
    private bool isBroken = false; // 床が壊れたかどうか

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount++;

            // 最初のプレイヤーが乗ったとき、タイムスタンプを記録
            if (currentPlayerCount == 1)
            {
                lastPlayerEnterTime = Time.time;
            }

            // 同時条件をチェック
            CheckIfShouldBreak();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentPlayerCount--;
        }
    }

    private void CheckIfShouldBreak()
    {
        // 必要な人数に達していて、かつ時間内に乗った場合に壊す
        if (currentPlayerCount >= requiredPlayers && !isBroken)
        {
            if (Time.time - lastPlayerEnterTime <= timeWindow)
            {
                DestroyFloor();
            }
        }
    }

    private void DestroyFloor()
    {
        isBroken = true;
        // 床を非アクティブ化
        gameObject.SetActive(false);

        // エフェクトやアニメーションを追加する場合はここに処理を記述
        Debug.Log("Floor has broken!");
    }
}
