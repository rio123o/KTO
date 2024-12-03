using UnityEngine;
using System.Collections;

public class KK_PlayerModelSwitcher : MonoBehaviour
{
    public GameObject liquidModel;   // 液体状態のモデル
    public GameObject gasModel;      // 気体状態のモデル
    public GameObject solidModel;    // 固体状態のモデル
    public GameObject slimeModel;    // スライム状態のモデル

    public GameObject transformationEffect;  // 変身エフェクト用のオブジェクト
    public float effectDuration = 2f;        // エフェクトの表示時間（秒）

    [HideInInspector] public GameObject currentModel; // 現在表示しているモデル

    void Start()
    {
        // 初期状態を液体モデルに設定
        SwitchToModel(liquidModel);

        // 変身エフェクトをゲーム開始時に非アクティブに設定
        if (transformationEffect != null)
        {
            transformationEffect.SetActive(false);  // ゲーム開始時にエフェクトを無効化
            //Debug.Log("Transformation effect set to inactive at game start.");
        }
    }

    public void SwitchToModel(GameObject newModel)
    {
        ///Debug.Log("Switching model to: " + newModel.name);

        // 現在のモデルがあれば非アクティブにする
        if (currentModel != null)
        {
            currentModel.SetActive(false);
            Debug.Log("Previous model deactivated: " + currentModel.name);
        }

        // 新しいモデルを設定してアクティブにする
        currentModel = newModel;
        currentModel.SetActive(true);
        //Debug.Log("New model activated: " + currentModel.name);

        // 変身エフェクトを表示する処理を開始
        if (transformationEffect != null)
        {
            StartCoroutine(PlayTransformationEffect());
        }
    }

    // 変身エフェクトを数秒間表示するコルーチン
    private IEnumerator PlayTransformationEffect()
    {
        // エフェクトをアクティブにする
        transformationEffect.SetActive(true);
       // Debug.Log("Transformation effect activated.");

        // 設定した秒数だけ待つ
        yield return new WaitForSeconds(effectDuration);

        // エフェクトを非アクティブにする
        transformationEffect.SetActive(false);
       // Debug.Log("Transformation effect deactivated.");
    }
}
