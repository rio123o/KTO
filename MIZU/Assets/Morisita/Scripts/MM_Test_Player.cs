using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MM_PlayerPhaseState))]

public class MM_Test_Player: MonoBehaviour
{
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _JumpPower;
    [SerializeField]
    private float _MoveSpeed;
    [SerializeField]
    private Material[] _playerMaterials = new Material[2];

    bool isOnGround = false;
    bool isOnWater = false;

    Rigidbody _rb;
    PlayerInput _playerInput;
    MeshRenderer _meshRenderer;
    [SerializeField]
    MM_PlayerPhaseState pState;

    [SerializeField]
    TextMeshProUGUI Debug_Phasetext;

    private Vector3 _velocity;

    public GameObject[] characters;  // キャラクターオブジェクトの配列
    private int currentIndex = 0;    // 現在のキャラクターインデックス


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _meshRenderer = GetComponent<MeshRenderer>();
        pState = GetComponent<MM_PlayerPhaseState>();

        //if (_playerInput.user.index == 0)
        //    _meshRenderer.material = _playerMaterials[0];
        //else
        //    _meshRenderer.material = _playerMaterials[1];

        pState.ChangeState(MM_PlayerPhaseState.State.Liquid);

        // 初期のキャラクターの設定
        SetActiveCharacter(0);
    }

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        Debug_Phasetext.text = "Player:" + pState.GetState();
        //print("Player:" + pState.GetState());
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        _rb.AddForce(new Vector3(0, -_gravity, 0), ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        isOnGround = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = false;
    }


    // メソッド名は何でもOK
    // publicにする必要がある
    public void OnMove(InputAction.CallbackContext context)
    {
        // 固体の時水に触れてなかったら動けない
        if(pState.GetState()==MM_PlayerPhaseState.State.Solid)
            if (!isOnWater) return;
        // MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        // 2Dなので横移動だけ
        _velocity = new Vector3(axis.x*_MoveSpeed, 0, 0);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        // 押した瞬間だけ反応する
        if (!context.performed) return;
        // 地面にいないなら跳べない
        if (!isOnGround) return;
        // 水に触れていたら跳べない
        if (isOnWater) return;
        // 気体なら跳べない
        if (pState.GetState() == MM_PlayerPhaseState.State.Gas) return;


        _rb.AddForce(new Vector3(0, _JumpPower, 0), ForceMode.VelocityChange);

        print("Jumpが押されました");
    }

    /// <summary>
    /// 気体へ変化
    /// </summary>
    public void OnStateChangeGas(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 水じゃなかったら受け付けない
        if (pState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        pState.ChangeState(MM_PlayerPhaseState.State.Gas);

        // モデルを気体のやつに変える処理
        // 現在のキャラクターを非アクティブに設定
        SetActiveCharacter(1);

        print("GAS(気体)になりました");
    }

    /// <summary>
    /// 固体へ変化
    /// </summary>
    public void OnStateChangeSolid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 水じゃなかったら受け付けない
        if (pState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        pState.ChangeState(MM_PlayerPhaseState.State.Solid);

        // モデルを固体のやつに変える処理
        // 現在のキャラクターを非アクティブに設定
        SetActiveCharacter(2);

        print("SOLID(固体)になりました");
    }
    /// <summary>
    /// 液体へ変化
    /// </summary>
    public void OnStateChangeLiquid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 固体・気体・スライムじゃなかったら受け付けない
        if (pState.GetState() == MM_PlayerPhaseState.State.Liquid) return;

        pState.ChangeState(MM_PlayerPhaseState.State.Liquid);

        // モデルを水のやつに変える処理
        // 現在のキャラクターを非アクティブに設定
        SetActiveCharacter(0);

        print("LIQUID(水)になりました");
    }

    /// <summary>
    /// スライムへ変化
    /// </summary>
    public void OnStateChangeSlime(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 水じゃなかったら受け付けない
        if (pState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        pState.ChangeState(MM_PlayerPhaseState.State.Slime);

        // モデルをスライムのやつに変える処理
        //
        //

        print("SLIME(スライム)になりました");

    }

    private void SetActiveCharacter(int index)
    {
        if(index < 0 || index >= characters.Length)
            return;

        characters[currentIndex].SetActive(false);  // 今のキャラクターの状態を非アクティブにする
        characters[index].SetActive(true);  // indexで指定した状態をアクティブにする
        currentIndex = index;  // 今のインデックスを更新する

    }
}
