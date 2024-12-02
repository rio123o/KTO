using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class MM_PlayerSpownTest : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private List<GameObject> player;
    [SerializeField]
    private float spownTime = 5f;

    [SerializeField, Header("初期リスポーン地点")]
    private Transform firstPlayerSpownPoint;
    [SerializeField]
    private Transform playerSpownPoint;

    bool isRespown = false;

    void Update()
    {
        if (player != null)
            CheckPlayerDeath();
        else
            print($"PlayerSpownTestError");

        if (isRespown)
        {
            RespownAllPlayer();
        }
    }

    async private void Spown(GameObject p)
    {
        var token = this.GetCancellationTokenOnDestroy();

        p.transform.position = playerSpownPoint.position;

        await UniTask.Delay(TimeSpan.FromSeconds(spownTime), cancellationToken: token);

        p.SetActive(true);
    }

    public void GetJoinPlayer(PlayerInput playerInput)
    {
        player.Add(playerInput.gameObject);
        playerInput.transform.position = firstPlayerSpownPoint.position;
    }
    //public void LeftJoinPlayer(PlayerInput playerInput)
    //{
    //    //player.Remove(playerInput.gameObject);
    //}

    public void SpownPointUpdate(Transform transform)
    {
        playerSpownPoint = transform;
    }

    private void CheckPlayerDeath()
    {
        foreach (var p in player)
        {
            if (!p.activeSelf)
            {
                isRespown = true;
                return;
            }
        }
    }
    private void RespownAllPlayer()
    {
        foreach (var p in player)
        {
            p.GetComponent<MM_Test_Player>().Death();
            Spown(p);
        }
        isRespown = false;
    }
}
