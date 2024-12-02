using UnityEngine;
using System.Collections;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GameObject _gauge;
    [SerializeField] private int _HP;
    private float _HP1;

    public GameObject managerObject;  // ModeManagerがついているオブジェクト
    private ModeManager _modeManager;
    private CollisionManager _collisionManager;

    public enum Player { Player1, Player2 }
    public Player player;

    public float water = 1;
    public float ice = 1;
    public float cloud = 1;
    public float slime = 1;

    private bool isDead = false;

    void Start()
    {
        _modeManager = managerObject.GetComponent<ModeManager>();
        _collisionManager = managerObject.GetComponent<CollisionManager>();
        _HP1 = _gauge.GetComponent<RectTransform>().sizeDelta.x / _HP;
    }

    void Update()
    {
        if (!isDead)
        {
            string currentModelTag = (player == Player.Player1) ? _modeManager.player1ModelTag : _modeManager.player2ModelTag;

            // ダメージ処理
            float attackPower = 0.1f;
            BeInjured(attackPower, currentModelTag);

            // プレイヤー1とプレイヤー2のヒールスポット衝突チェック
            if (player == Player.Player1)
            {
                // プレイヤー1がヒールスポットに衝突したかチェック
                foreach (Collider col in _collisionManager.GetPlayer1HitColliders())
                {
                    if (col.gameObject.CompareTag("HealSpot"))
                    {
                        Heal(100f);  // プレイヤー1の回復量
                        break;
                    }
                }
            }
            else if (player == Player.Player2)
            {
                // プレイヤー2がヒールスポットに衝突したかチェック
                foreach (Collider col in _collisionManager.GetPlayer2HitColliders())
                {
                    if (col.gameObject.CompareTag("HealSpot"))
                    {
                        Heal(100f);  // プレイヤー2の回復量
                        break;
                    }
                }
            }
        }
    }

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

        if (currentSize.x <= 0 && !isDead)
        {
            currentSize.x = 0;
            isDead = true;
            Debug.Log(player + " is dead!");
        }

        _gauge.GetComponent<RectTransform>().sizeDelta = currentSize;
        yield return null;
    }

    // 回復処理
    public void Heal(float healAmount)
    {
        float heal = _HP1 * healAmount;
        StartCoroutine(HealCoroutine(heal));
    }

    IEnumerator HealCoroutine(float heal)
    {
        Vector2 currentSize = _gauge.GetComponent<RectTransform>().sizeDelta;
        currentSize.x += heal;

        // ゲージが最大幅を超えないようにする
        float maxWidth = _HP1 * _HP;
        if (currentSize.x > maxWidth)
        {
            currentSize.x = maxWidth;
        }

        _gauge.GetComponent<RectTransform>().sizeDelta = currentSize;
        yield return null;
    }
}
