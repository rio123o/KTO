using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{
    [SerializeField] private float healAmount = 100f; // 回復量
    [SerializeField] private float healInterval = 1f; // 回復間隔

    private List<GaugeController> playersInArea = new List<GaugeController>(); // エリア内のプレイヤーリスト

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("bbbbbbbbbbbbbbbbbbbbb");
            GaugeController playerGauge = other.GetComponent<GaugeController>();
            if (playerGauge != null && !playersInArea.Contains(playerGauge))
            {
                playersInArea.Add(playerGauge); // エリア内のプレイヤーを追加
                StartCoroutine(HealPlayer(playerGauge)); // 回復を開始
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GaugeController playerGauge = other.GetComponent<GaugeController>();
            if (playerGauge != null && playersInArea.Contains(playerGauge))
            {
                playersInArea.Remove(playerGauge); // エリア内のプレイヤーを削除
            }
        }
    }

    private IEnumerator HealPlayer(GaugeController playerGauge)
    {
        while (playersInArea.Contains(playerGauge))
        {
            playerGauge.Heal(healAmount); // プレイヤーを回復
            yield return new WaitForSeconds(healInterval); // 指定時間ごとに回復
        }
    }
}
