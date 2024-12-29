using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlendShapeController : MonoBehaviour
{
    [Header("TriggerAnimationMaskへの参照")]
    [SerializeField] private TriggerAnimationMask mask;

    [Header("目を開ける対象の子オブジェクト")]
    [SerializeField] private GameObject player1EyesObject;
    [SerializeField] private GameObject player2EyesObject;

    [Header("目を開くためのブレンドシェイプのインデックス")]
    [SerializeField] private int player1EyesOpenBlendShapeIndex = 0;
    [SerializeField] private int player2EyesOpenBlendShapeIndex = 0;

    [Header("目を開ける際のブレンドシェイプの重量")]
    [SerializeField]private float openWeight = 10f;

    private Animator animator;
    private bool hasAnimationPlayed = false;
    private bool isMonitoring = true;  //  ループの制御フラグ

    void Start()
    {
        //  必要な参照が設定されているか確認
        if (mask == null)
        {
            Debug.LogError("maskがアタッチされていない");
            return;
        }

        if (!mask.TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("maskにAnimatorコンポーネントが存在していない");
        }

        if (player1EyesObject == null || player2EyesObject == null)
        {
            Debug.LogError("player1EyesObjectかplayer2EyesObjectがアタッチされていない");
        }

        //  アニメーションの完了を監視する
        seeAnimation().Forget();
    }

    private async UniTaskVoid seeAnimation()
    {
        try
        {
            while (isMonitoring)
            {
                if (animator == null)
                {
                    await UniTask.Yield();
                    continue;
                }

                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                //  アニメーションがTake 001ステートにあり、再生が終了した場合
                if (stateInfo.IsName("Take 001") && stateInfo.normalizedTime >= 1.0f && !hasAnimationPlayed)
                {
                    OpenEyes();
                    hasAnimationPlayed = true;

                    isMonitoring = false;

                    break;
                }

                // 次のフレームまで待機
                await UniTask.Yield();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"seeAnimationのエラー: {ex.Message}");
        }
    }
    //  目を開けるブレンドシェイプを決定するメソッド
    private void OpenEyes()
    {
        //  player1の仮面と同じ形のもののSkinnedMeshRendererを取得する
        if (player1EyesObject.TryGetComponent<SkinnedMeshRenderer>(out var player1EyeRenderer))
        {
            player1EyeRenderer.SetBlendShapeWeight(player1EyesOpenBlendShapeIndex, openWeight);
            Debug.Log("左側の仮面の目が開いた");
        }

        //  player2の仮面と同じ形のもののSkinnedMeshRendererを取得する
        if (player2EyesObject.TryGetComponent<SkinnedMeshRenderer>(out var player2EyeRenderer))
        {
            player2EyeRenderer.SetBlendShapeWeight(player2EyesOpenBlendShapeIndex, openWeight);
            Debug.Log("右側の仮面の目が開いた");
        }

    }

    private void OnDestroy()
    {
        //  ゲームオブジェクトが破棄される際に監視を停止
        isMonitoring = false;
    }
}
