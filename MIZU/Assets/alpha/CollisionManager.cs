using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public GameObject player1Model;   // プレイヤー1モデル
    public GameObject player2Model;   // プレイヤー2モデル

    private Collider player1Collider;
    private Collider player2Collider;

    // プレイヤー1とプレイヤー2それぞれの衝突したオブジェクトを保持するリスト
    private List<Collider> player1HitColliders = new List<Collider>();
    private List<Collider> player2HitColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        if (player1Model != null)
        {
            player1Collider = player1Model.GetComponent<Collider>();
        }
        if (player2Model != null)
        {
            player2Collider = player2Model.GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤー1の衝突をチェック
        if (player1Collider != null)
        {
            CheckCollisions(player1Collider, player1HitColliders);
        }
        // プレイヤー2の衝突をチェック
        if (player2Collider != null)
        {
            CheckCollisions(player2Collider, player2HitColliders);
        }
    }

    // 衝突を確認するメソッド
    private void CheckCollisions(Collider playerCollider, List<Collider> hitCollidersList)
    {
        // 前回の衝突リストをクリア
        hitCollidersList.Clear();

        // コライダーの周りにある全ての衝突体を取得
        Collider[] hitColliders = Physics.OverlapSphere(playerCollider.transform.position, playerCollider.bounds.extents.magnitude);

        // 各コライダーについて処理
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider != playerCollider)
            {
                // 衝突したオブジェクトをリストに追加
                hitCollidersList.Add(hitCollider);

                // 衝突したオブジェクトのタグを確認して処理
                Debug.Log($"衝突したオブジェクト: {hitCollider.gameObject.name}, タグ: {hitCollider.tag}");

            }
        }
    }

    // プレイヤー1がヒールスポットに衝突したかどうかを確認
    public List<Collider> GetPlayer1HitColliders()
    {
        return player1HitColliders;
    }

    // プレイヤー2がヒールスポットに衝突したかどうかを確認
    public List<Collider> GetPlayer2HitColliders()
    {
        return player2HitColliders;
    }
}
