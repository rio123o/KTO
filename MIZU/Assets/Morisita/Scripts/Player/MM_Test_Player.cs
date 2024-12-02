using System.Collections;
using System.Collections.Generic;
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
    [Header("�^���X�e�[�^�X")]
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
    [SerializeField, Header("������,-1~10")]
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
        _modelSwitcher = GetComponent<KK_PlayerModelSwitcher>(); // PlayerModelSwitcher �R���|�[�l���g���擾
        _gameObjectSwitcher = GetComponent<MM_Player_State_GameObject_Switcher>();

        if (_groundCheck == null)
            Debug.LogWarning($"{nameof(_groundCheck)}���A�^�b�`����Ă��܂���");

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
    }

    private void FixedUpdate()
    {
        Gravity();
        GroundCheck();
        Move();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Death();
        }
    }

    void Gravity()
    {
        _rb.AddForce(new Vector3(0, -nowGravity, 0), ForceMode.Acceleration);
    }

    void Move()
    {
        // ���ړ�
        MoveHorizontal();
        // �K�X�̎��̏c�ړ�
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
        // ���x�����A����𒴂��������܂ŉ�����
        if (GetAbsSpeed().x > _LimitXSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x / (GetAbsSpeed().x / _LimitXSpeed), _rb.velocity.y, _rb.velocity.z);
        }
        if (GetAbsSpeed().y > _LimitYSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y / (GetAbsSpeed().y / _LimitYSpeed), _rb.velocity.z);
        }

        // �v�Z�ł��؂�A���ȉ��Ȃ�0�ɂ���
        if (GetAbsSpeed().x < 1)
            _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
    }

    private void GroundCheck()
    {
        isOnGround = _groundCheck.IsGround();
        isOnWater = _groundCheck.IsPuddle();
    }

    private void PlayerStateUpdateFunc()
    {
        // ���̃v���C���[�̑��x���m�F�ł���悤�ɂ���
        _NowXSpeed = GetAbsSpeed().x;
        _NowYSpeed = GetAbsSpeed().y;

        switch (_playerPhaseState.GetState())
        {
            case MM_PlayerPhaseState.State.Gas: PlayerGasStateUpdateFunc(); break;
            case MM_PlayerPhaseState.State.Solid: PlayerSolidStateUpdateFunc(); break;
            case MM_PlayerPhaseState.State.Liquid: PlayerLiquidStateUpdateFunc(); break;
            case MM_PlayerPhaseState.State.Slime: PlayerSlimeStateUpdateFunc(); break;
            default: Debug.LogError($"�G���[�A�v���C���[�̃X�e�[�g��{_playerPhaseState.GetState()}�ɂȂ��Ă��܂�"); break;
        }
    }

    private void PlayerGasStateUpdateFunc() { }
    private void PlayerSolidStateUpdateFunc() { }
    private void PlayerLiquidStateUpdateFunc()
    {
        StartCoroutine(IsPuddleCollisionDeadCount());
    }
    private void PlayerSlimeStateUpdateFunc() { }
    private void Death()
    {
        _groundCheck.ResetFlag();
        this.gameObject.SetActive(false);
    }
    // ���\�b�h���͉��ł�OK
    // public�ɂ���K�v������
    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        // �ő̂̎����ɐG��ĂȂ������瓮���Ȃ�
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Solid)
            if (!isOnWater)
            {
                // Velocity�����Z�b�g����
                _velocity = Vector3.zero;
                return;
            }
        // MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        //print($"{nameof(axis.x)}:{axis.x}");
        // �v���C���[���E�����Ȃ�1�A���Ȃ�|1
        if (axis.x != 0)
            _pRotation = axis.x > 0f ? 1 : -1;

        // 2D�Ȃ̂ŉ��ړ�����
        _velocity = new Vector3(axis.x * _MovePower, _velocity.y, 0);

    }
    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        // �C�̂łȂ���Ώc�ړ��͂ł��Ȃ�
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Gas)
            return;

        // MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        _velocity = new Vector3(_velocity.x, axis.y * _MovePower, 0);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        // �������u�Ԃ�����������
        if (!context.performed) return;
        // �n�ʂɂ��Ȃ��Ȃ璵�ׂȂ�
        if (!isOnGround) return;
        // ���ɐG��Ă����璵�ׂȂ�
        if (isOnWater) return;
        // �C�̂Ȃ璵�ׂȂ�
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Gas) return;

        _rb.AddForce(new Vector3(0, _JumpPower, 0), ForceMode.VelocityChange);

        print("Jump��������܂���");
    }


    // ���ɐG�ꂽ�玀�S�܂ł̃J�E���g���J�n
    private IEnumerator IsPuddleCollisionDeadCount()
    {
        float contactTime = 0f;
        float destroyTime = 0.00001f;

        while (isOnWater)
        {
            contactTime += Time.deltaTime;
            yield return null;
            if (contactTime >= destroyTime)
            {
                Death();
            }
            // print($"{nameof(contactTime)}:{contactTime}");
        }
    }

    /// <summary>
    /// �C�̂֕ω�
    /// </summary>
    public void OnStateChangeGas(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // ������Ȃ�������󂯕t���Ȃ�
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Gas);

        // �d�͂�0�ɂ���
        nowGravity = 0;
        // ��C��R�𔭐�������
        _rb.drag = 10;

        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());
        // ���f�����C�̂̂�ɕς��鏈��
        _modelSwitcher.SwitchToModel(_modelSwitcher.gasModel);
        //

        print("GAS(�C��)�ɂȂ�܂���");
    }
    /// <summary>
    /// �ő̂֕ω�
    /// </summary>
    public void OnStateChangeSolid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // ������Ȃ�������󂯕t���Ȃ�
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Solid);


        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());

        // ���f�����ő̂̂�ɕς��鏈��
        _modelSwitcher.SwitchToModel(_modelSwitcher.solidModel);

        print("SOLID(�ő�)�ɂȂ�܂���");
    }
    /// <summary>
    /// �t�́i�l�^�j�֕ω�
    /// </summary>
    public void OnStateChangeLiquid(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // �ő́E�C�́E�X���C������Ȃ�������󂯕t���Ȃ�
        if (_playerPhaseState.GetState() == MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Liquid);

        // �d�͂�ʏ�ɖ߂�
        nowGravity = _defaultGravity;
        // ��C��R���Ȃ���
        _rb.drag = 0;

        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _gameObjectSwitcher.Switch(_playerPhaseState.GetState());

        // ���f���𐅂̂�ɕς��鏈��
        _modelSwitcher.SwitchToModel(_modelSwitcher.liquidModel);
        print("LIQUID(��)�ɂȂ�܂���");
    }

    /// <summary>
    /// �X���C���֕ω�
    /// </summary>
    public void OnStateChangeSlime(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // ������Ȃ�������󂯕t���Ȃ�
        if (_playerPhaseState.GetState() != MM_PlayerPhaseState.State.Liquid) return;

        _playerPhaseState.ChangeState(MM_PlayerPhaseState.State.Slime);

        _velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        // ���f�����X���C���̂�ɕς��鏈��
        _modelSwitcher.SwitchToModel(_modelSwitcher.slimeModel);

        print("SLIME(�X���C��)�ɂȂ�܂���");

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
}
