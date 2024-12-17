using UnityEngine;

public class WaterControl : MonoBehaviour
{
    public GameObject water; // 上下する水オブジェクト
    public float ascendAmount = 1.0f; // 上昇時の移動量
    public float descendAmount = 1.0f; // 下降時の移動量
    public float moveSpeed = 2.0f; // 移動速度
    public bool isAscending = true; // 上昇するか下降するか(Trueなら上がる)

    private bool isMoving = false; // 現在移動中かどうか
    private Vector3 targetPosition; // 水の目標位置
    private Vector3 initialPosition;//初期位置
    [SerializeField]
    private MM_PlayerSpownTest _spowntest;

    private void Start()
    {
        initialPosition = water.transform.position;
    }
    void Update()
    {
        if (isMoving)
        {
            // 現在の位置を目標位置に向けて移動
            water.transform.position = Vector3.MoveTowards(
                water.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // 目標位置に到達したら移動を停止
            if (Vector3.Distance(water.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }

        if (_spowntest.IsRespown)
        {
            ResetWater();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving) // 移動中でない場合のみ動作
        {
            // 移動目標位置を設定
            float direction = isAscending ? ascendAmount : -descendAmount;
            targetPosition = water.transform.position + new Vector3(0, direction, 0);

            // 移動を開始
            isMoving = true;

            // コライダーを一時的に無効化
            this.GetComponent<Collider>().enabled = false;
        }
    }

    public void ResetWater()
    {
        water.transform.position = initialPosition;
        isMoving = false;
        this.GetComponent<Collider>().enabled = true; // コライダーを再度有効化
    }
}
