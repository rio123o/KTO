using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{

    // プレイヤーがゲームにJoinするためのInputAction
    [SerializeField] private InputAction playerJoinInputAction;

    [SerializeField] private InputActionAsset inputasset;
    // PlayerInputがアタッチされているプレイヤーオブジェクト
    [SerializeField] private PlayerInput playerPrefab = default;

    // 最大参加人数
    [SerializeField] private int maxPlayerCount = default;


    // Join済みのデバイス情報
    private InputDevice[] joinedDevices = default;
    // 現在のプレイヤー数
    private int currentPlayerCount = 0;


    private void Awake()
    {
        playerJoinInputAction = inputasset.actionMaps[0].FindAction("Move");
        // 最大参加可能数で配列を初期化
        joinedDevices = new InputDevice[maxPlayerCount];
        // InputActionを有効化し、コールバックを設定
        playerJoinInputAction.Enable();
        playerJoinInputAction.performed += OnJoin;
    }

    private void OnDestroy()
    {
        playerJoinInputAction.Dispose();
    }

    /// <summary>
    /// デバイスによってJoin要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            return;
        }

        // Join要求元のデバイスが既に参加済みのとき、処理を終了
        foreach (var device in joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // PlayerInputを所持した仮想のプレイヤーをインスタンス化
        // ※Join要求元のデバイス情報を紐づけてインスタンスを生成する
        PlayerInput.Instantiate(
        prefab: playerPrefab.gameObject,
        playerIndex: currentPlayerCount,
        pairWithDevice: context.control.device
        );

        // Joinしたデバイス情報を保存
        joinedDevices[currentPlayerCount] = context.control.device;

        currentPlayerCount++;
    }
}
