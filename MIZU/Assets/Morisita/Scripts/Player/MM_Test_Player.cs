using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MM_PlayerPhaseState))]
public class MM_Test_Player : MonoBehaviour
{
    [Header("運動ステータス")]
    [SerializeField]
    private float _defaultGravity;
    [SerializeField]
    private float nowGravity;
    [SerializeField]
    private float _JumpPower;
    [SerializeField]
    private float _MovePower;
    [SerializeField]
    private float _LimitXSpeed;
    [SerializeField]
    private float _LimitYSpeed;
    [SerializeField, Header("慣性力,-1~10")]
    private float _InertiaPower;

    [SerializeField]
    private float _NowXSpeed;
    [SerializeField]
    private float _NowYSpeed;
    [SerializeField]
    private int _pRotation = 1;
    [SerializeField]
    private MM_GroundCheck _groundCheck;

    bool isOnGround = false;
    [SerializeField]
    bool isOnWater = false;
    [SerializeField]
    bool isDead = false;

    Rigidbody _rb;
    PlayerInput _playerInput;
    MeshRenderer _meshRenderer;
    MM_PlayerPhaseState _playerPhaseState;

    [SerializeField]
    private Vector3 _velocity;
    [SerializeField]
    private Vector3 _rbvelocity;

    private KK_PlayerModelSwitcher _modelSwitcher;
    private MM_Player_State_GameObject_Switcher _gameObjectSwitcher;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _playerPhaseState = GetComponent<MM_PlayerPhaseState>();
        _modelSwitcher = GetComponent<KK_PlayerModelSwitcher>(); // PlayerModelSwitcher コンポーネントを取得
        _gameObjectSwitcher = GetComponent<MM_Player_State_GameObject_Switcher>();

        if (_groundCheck == null)
            Debug.LogWarning($"{nameof(_groundCheck)}がアタッチされていません");

        _gameObjectSwitcher.InitSwitch();
        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Liquid);


        nowGravity = _defaultGravity;

        _InertiaPower = Mathf.Clamp(_InertiaPower, -1, 10);

    }

    private void Update()
    {
        PlayerStateUpdateFunc();
        LimitedSpeed();
        _rbvelocity = _rb.velocity;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        Gravity();
        GroundCheck();
        Move();

    }

    void Gravity()
    {
        _rb.AddForce(new Vector3(0, -nowGravity, 0), ForceMode.Acceleration);
    }

    void Move()
    {
        // 横移動
        MoveHorizontal();
        // ガスの時の縦移動
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Gas)
            MoveVertical();
    }

    void MoveHorizontal()
    {
        if (_velocity.x != 0)
            _rb.AddForce(_velocity, ForceMode.Acceleration);
        else
            _rb.AddForce(new Vector3(-_rb.velocity.x * _InertiaPower, _rb.velocity.y, _rb.velocity.z), ForceMode.Acceleration);

    }

    void MoveVertical()
    {
        if (_velocity.y != 0)
            _rb.AddForce(_velocity, ForceMode.Acceleration);
        else
            _rb.AddForce(new Vector3(_rb.velocity.x, -_rb.velocity.y * _InertiaPower, _rb.velocity.z), ForceMode.Acceleration);
    }
    void LimitedSpeed()
    {
        // 速度制限、上限を超えたら上限まで下げる
        if (GetAbsSpeed().x > _LimitXSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x / (GetAbsSpeed().x / _LimitXSpeed), _rb.velocity.y, _rb.velocity.z);
        }
        if (GetAbsSpeed().y > _LimitYSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y / (GetAbsSpeed().y / _LimitYSpeed), _rb.velocity.z);
        }

        // 計算打ち切り、一定以下なら0にする
        if (GetAbsSpeed().x < 1)
            _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
    }

    private void GroundCheck()
    {
        isOnGround = _groundCheck.IsGround();
        isOnWater = _groundCheck.IsPuddle();
    }
    private void GroundFlagReset()
    {
        _groundCheck.ResetFlag();
        GroundCheck();
    }

    private void PlayerStateUpdateFunc()
    {
        // 今のプレイヤーの速度を確認できるようにする
        _NowXSpeed = GetAbsSpeed().x;
        _NowYSpeed = GetAbsSpeed().y;

        switch (_playerPhaseState.GetState())
        {
            case MM_PlayerPhaseState.State.Gas: PlayerGasStateUpdateFunc(); break;
            case MM_PlayerPhaseState.State.Solid: PlayerSolidStateUpdateFunc(); break;
            case MM_PlayerPhaseState.State.Liquid: PlayerLiquidStateUpdateFunc(); break;
            default: Debug.LogError($"エラー、プレイヤーのステートが{_playerPhaseState.GetState()}になっています"); break;
        }
    }

    private void PlayerGasStateUpdateFunc() { }
    private void PlayerSolidStateUpdateFunc() { }
    private void PlayerLiquidStateUpdateFunc()
    {
        IsPuddleCollisionDeadCount();
    }
    public void Death()
    {
        isDead = true;
        GroundFlagReset();
        this.gameObject.SetActive(false);
    }

    public void Rivive()
    {
        isDead = false;
        GroundFlagReset();
        this.gameObject.SetActive(true);
    }
    // メソッド名は何でもOK
    // publicにする必要がある
    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        // 固体の時水に触れてなかったら動けない
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Solid)
            if (!isOnWater)
            {
                // Velocityをリセットする
                _velocity = Vector3.zero;
                return;
            }
        // MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        //print($"{nameof(axis.x)}:{axis.x}");
        // プレイヤーが右向きなら1、左なら－1
        if (axis.x != 0)
            _pRotation = axis.x > 0f ? 1 : -1;

        // 2Dなので横移動だけ
        _velocity = new Vector3(axis.x * _MovePower, _velocity.y, 0);

    }
    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        // 気体でなければ縦移動はできない
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Gas)
            return;

        // MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        _velocity = new Vector3(_velocity.x, axis.y * _MovePower, 0);
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
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Gas) return;

        _rb.AddForce(new Vector3(0, _JumpPower, 0), ForceMode.VelocityChange);

        //print("Jumpが押されました");
    }


    // 水に触れたら死亡までのカウントを開始
    async private void IsPuddleCollisionDeadCount()
    {
        var token = this.GetCancellationTokenOnDestroy();
        float contactTime = 0f;
        float destroyTime = 0.00001f;
        //float destroyTime = 3f;

        while(isOnWater)
        {
            contactTime += Time.deltaTime;
            try
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }
            catch(OperationCanceledException)
            {
                print("プレイヤーがデストロイされました。");
                return;
            }
            if (contactTime >= destroyTime)
                Death();
        }
    }

    /// <summary>
    /// 気体へ変化
    /// </summary>
    public void OnStateChangeGas(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        OnStateChangeGas();
    }

    public void OnStateChangeGas()
    {
        // 水じゃなかったら受け付けない
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Gas);

        // 重力を0にする
        nowGravity = 0;
        // 空気抵抗を発生させる
        _rb.drag = 10;

        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());
        // モデルを気体のやつに変える処理
        _modelSwitcher.SwitchToModel(_modelSwitcher.gasModel);
        //

        print("GAS(気体)になりました");
    }
    /// <summary>
    /// 固体へ変化
    /// </summary>
    public void OnStateChangeSolid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        OnStateChangeSolid();
    }
    public void OnStateChangeSolid()
    {
        // 水じゃなかったら受け付けない
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Solid);


        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());

        // モデルを固体のやつに変える処理
        _modelSwitcher.SwitchToModel(_modelSwitcher.solidModel);

        print("SOLID(固体)になりました");
    }
    /// <summary>
    /// 液体（人型）へ変化
    /// </summary>
    public void OnStateChangeLiquid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        OnStateChangeLiquid();
    }
    public void OnStateChangeLiquid()
    {
        // 固体・気体じゃなかったら受け付けない
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Liquid);

        // 重力を通常に戻す
        nowGravity = _defaultGravity;
        // 空気抵抗をなくす
        _rb.drag = 0;

        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());

        // モデルを水のやつに変える処理
        _modelSwitcher.SwitchToModel(_modelSwitcher.liquidModel);
        print("LIQUID(水)になりました");
    }

    public int GetPlayerOrientation()
    {
        return _pRotation;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
    public void AddVelocity(Vector3 addvelocity)
    {
        _velocity += addvelocity;
    }

    public void SetVelocity(Vector3 setvelocity)
    {
        _velocity = setvelocity;
    }
    public Vector3 GetSpeed()
    {
        return _rb.velocity;
    }

    public Vector2 GetAbsSpeed()
    {
        var velo = _rb.velocity;

        velo.x = Mathf.Sqrt(Mathf.Pow(_rb.velocity.x, 2));
        velo.y = Mathf.Sqrt(Mathf.Pow(_rb.velocity.y, 2));

        return velo;
    }


    public bool GetIsDead()
    {
        return isDead;
    }
}
