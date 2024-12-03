using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MM_PlayerSpownTest : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private List<GameObject> playerGameObjects;
    [SerializeField]
    private List<MM_Test_Player> testPlayerScripts;

    [SerializeField]
    private float spownTime = 5f;

    [SerializeField, Header("初期リスポーン地点")]
    private Transform firstPlayerSpownPoint;
    [SerializeField]
    private Transform playerSpownPoint;


    // 最大参加人数
    [SerializeField] private int maxPlayerCount = default;
    // 現在のプレイヤー数
    private int currentPlayerCount = 0;
    bool isRespown = false;

    MM_ObserverBool observerBool;

    private void Start()
    {
        observerBool = new MM_ObserverBool(isRespown);

    }
    void Update()
    {
        if (observerBool.OnBoolTrueChange)
        {
            DeathAllPlayer(testPlayerScripts);
            print("allspown");
            RespownAllPlayer();
        }

        if (playerGameObjects != null&&!isRespown)
        {
            CheckPlayerDeath();

        }

        observerBool.SetBool(isRespown);
    }

    private void Spown(List<GameObject> _player,List<MM_Test_Player> _tplayer)
    {
        foreach (var p in _player)
        {
            p.transform.position = playerSpownPoint.position;
        }
        foreach(var tp in _tplayer)
        {
            tp.Rivive();
        }
        isRespown = false;

        print("spown");
    }

    public void GetJoinPlayer(PlayerInput playerInput)
    {
        //プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            //print("playermax");
            return;
        }

        playerGameObjects.Add(playerInput.gameObject);
        testPlayerScripts.Add(playerInput.gameObject.GetComponent<MM_Test_Player>());
        playerInput.transform.position = firstPlayerSpownPoint.position;

        currentPlayerCount++;
    }
    //public void LeftJoinPlayer(PlayerInput playerInput)
    //{
    //    //player.Remove(playerInput.gameObject);
    //}

    public void SetSpownPoint(Transform transform)
    {
        playerSpownPoint = transform;
    }

    private void CheckPlayerDeath()
    {
        foreach (var tp in testPlayerScripts)
        {
            if (tp.GetIsDead())
                isRespown = true;
        }
    }
    private void DeathAllPlayer(List<MM_Test_Player> _tplayer)
    {
        foreach (var tp in _tplayer)
            tp.Death();
    }
    async private void RespownAllPlayer()
    {
        var token = this.GetCancellationTokenOnDestroy();

        await UniTask.Delay(TimeSpan.FromSeconds(spownTime), cancellationToken: token);

        Spown(playerGameObjects,testPlayerScripts);

    }
}
