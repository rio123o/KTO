using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
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
    MM_ObserverBool _observer;

    public GameObject buttonTop;  // 押される部分のオブジェクト
    private Material buttonMaterial; // ボタン上部のマテリアル
    public Material pressedMaterial;  // 押されたときの色
    public float pressOffset = 0.01f;   // ボタンが下がる距離

    private Vector3 initialbuttonPosition;  // ボタン上部の初期位置
   
    private void Start()
    {
        _observer = new MM_ObserverBool();
        initialPosition = water.transform.position;
        
        initialbuttonPosition = buttonTop.transform.localPosition;
        buttonMaterial = buttonTop.GetComponent<Renderer>().material;
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

        if (_observer.OnBoolTrueChange(_spowntest.GetIsRespown()))
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
     
        // ボタン上部を下げる
        buttonTop.transform.localPosition = new Vector3(
            initialbuttonPosition.x,
            initialbuttonPosition.y - pressOffset,
            initialbuttonPosition.z
        );

        // 色を変更
        buttonTop.GetComponent<Renderer>().material = pressedMaterial;

        MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.ButttonPush);
        MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.WaterUpDown);
    }

    public void ResetWater()
    {
        water.transform.position = initialPosition;
        isMoving = false;
        this.GetComponent<Collider>().enabled = true; // コライダーを再度有効化

       
        buttonTop.transform.localPosition = initialbuttonPosition;                                    
        // 色を初期状態に戻す
        buttonTop.GetComponent<Renderer>().material = buttonMaterial;

    }
}