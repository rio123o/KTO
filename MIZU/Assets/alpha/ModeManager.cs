using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public GameObject player1Model;   // player1のモデル
    public GameObject player2Model;   // player2のモデル

    [HideInInspector] public KK_PlayerModelSwitcher player1Mode;
    [HideInInspector] public KK_PlayerModelSwitcher player2Mode;

    [HideInInspector] public string player1ModelTag;
    [HideInInspector] public string player2ModelTag;

    [HideInInspector] public GameObject Player1 => player1Model;
    [HideInInspector] public GameObject Player2 => player2Model;

    void Start()
    {
        // nullチェックしてコンポーネント取得
        if (player1Model != null)
        {
            player1Mode = player1Model.GetComponent<KK_PlayerModelSwitcher>();
        }
        if (player2Model != null)
        {
            player2Mode = player2Model.GetComponent<KK_PlayerModelSwitcher>();
        }
    }

    void Update()
    {
        // モデルの存在とcurrentModelのnullチェックを追加
        if (player1Mode != null && player1Mode.currentModel != null)
        {
            player1ModelTag = player1Mode.currentModel.tag;
            Debug.Log("Player1 model tag: " + player1ModelTag);
        }
        else
        {
            Debug.LogWarning("Player1Model or its current model is missing!");
        }

        if (player2Mode != null && player2Mode.currentModel != null)
        {
            player2ModelTag = player2Mode.currentModel.tag;
            Debug.Log("Player2 model tag: " + player2ModelTag);
        }
        else
        {
            Debug.LogWarning("Player2Model or its current model is missing!");
        }
    }
}
