using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GameObject _gauge;
    [SerializeField] private int _HP;
    private float _HP1;

    public GameObject managerObject;  // ModeManager�����Ă���I�u�W�F�N�g
    private ModeManager _modeManager;
    private CollisionManager _collisionManager;

    public enum Player { Player1, Player2 }
    public Player player;

    public float water = 1;
    public float ice = 1;
    public float cloud = 1;
    public float slime = 1;

    //private bool isDead = false;

    private List<string> allowedTags = new List<string> { "HealSpot", "Ground" };

    void Start()
    {
        _modeManager = managerObject.GetComponent<ModeManager>();
        _collisionManager = managerObject.GetComponent<CollisionManager>();
        _HP1 = _gauge.GetComponent<RectTransform>().sizeDelta.x / _HP;
    }

    void Update()
    {
        //if (!isDead)
        //{
            string currentModelTag = (player == Player.Player1) ? _modeManager.player1ModelTag : _modeManager.player2ModelTag;

            // �_���[�W����
            float attackPower = 0.1f;
            BeInjured(attackPower, currentModelTag);

            // �v���C���[1�ƃv���C���[2�̃q�[���X�|�b�g�Փ˃`�F�b�N
            if (player == Player.Player1)
            {
                // �v���C���[1���q�[���X�|�b�g�ɏՓ˂������`�F�b�N
                foreach (Collider col in _collisionManager.GetPlayer1HitColliders())
                {
                    if (allowedTags.Contains(col.gameObject.tag))
                    {
                        Heal(100f);  // �v���C���[1�̉񕜗�
                        break;
                    }
                }
            }
            else if (player == Player.Player2)
            {
                // �v���C���[2���q�[���X�|�b�g�ɏՓ˂������`�F�b�N
                foreach (Collider col in _collisionManager.GetPlayer2HitColliders())
                {
                    if (allowedTags.Contains(col.gameObject.tag))
                    {
                        Heal(100f);  // �v���C���[2�̉񕜗�
                        break;
                    }
                }
            }
        }
    //}

    public void BeInjured(float attack, string modelTag)
    {
        float damage = 0;

        switch (modelTag)
        {
            case "Water":
                damage = _HP1 * attack * water;
                break;
            case "Ice":
                damage = _HP1 * attack * ice;
                break;
            case "Cloud":
                damage = _HP1 * attack * cloud;
                break;
            case "Slime":
                damage = _HP1 * attack * slime;
                break;
        }

        StartCoroutine(DamageCoroutine(damage));
    }

    IEnumerator DamageCoroutine(float damage)
    {
        Vector2 currentSize = _gauge.GetComponent<RectTransform>().sizeDelta;
        currentSize.x -= damage;

        if (currentSize.x <= 0)
        {
            currentSize.x = 0;
            Debug.Log(player + " is dead!");

            // �v���C���[1�܂��̓v���C���[2�ɑΉ�����MM_Test_Player�X�N���v�g���擾
            MM_Test_Player playerScript = null;

            if (player == Player.Player1 && player1Object != null)
            {
                playerScript = player1Object.GetComponent<MM_Test_Player>();
            }
            else if (player == Player.Player2 && player2Object != null)
            {
                playerScript = player2Object.GetComponent<MM_Test_Player>();
            }

            // playerScript���擾�ł����ꍇ�AOnStateChangeLiquid���Ăяo��
            if (playerScript != null)
            {
                playerScript.OnStateChangeLiquid(new InputAction.CallbackContext());
            }
        }

        _gauge.GetComponent<RectTransform>().sizeDelta = currentSize;
        yield return null;
    }


    // �񕜏���
    public void Heal(float healAmount)
    {
        float heal = _HP1 * healAmount;
        StartCoroutine(HealCoroutine(heal));
    }

    IEnumerator HealCoroutine(float heal)
    {
        Vector2 currentSize = _gauge.GetComponent<RectTransform>().sizeDelta;
        currentSize.x += heal;

        // �Q�[�W���ő啝�𒴂��Ȃ��悤�ɂ���
        float maxWidth = _HP1 * _HP;
        if (currentSize.x > maxWidth)
        {
            currentSize.x = maxWidth;
        }

        _gauge.GetComponent<RectTransform>().sizeDelta = currentSize;
        yield return null;
    }
}
