using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    [Header("順番にアクティブ化する画像をリストに追加する")]
    [SerializeField] private List<Image> sequenceImages = new List<Image>();

    [Header("画像を表示する時に使うボタン")]
    [SerializeField] private Button controlButton;

    [Header("連射対策として、ボタン入力を無視する時間(秒)")]
    [SerializeField] private float debounceTime = 0.1f;

    private int currentStep = 0;  //  現在の画像表示の段階
    private bool isSequenceActive = false;  //  画像シークエンスがアクティブになっているか
    private bool canReceiveInput = true;  //  入力を受け取れるかどうかのフラグ

    void Start()
    {
        //  全ての画像を非アクティブに設定する
        foreach (var image in sequenceImages)
        {
            if (image != null)
                image.gameObject.SetActive(false);
            else
                Debug.LogError("Imageリストに未設定のオブジェクトがある");
        }

        //  ボタンが正しく割り当てられているかの確認
        if (controlButton != null)
            controlButton.onClick.AddListener(OnControlButtonClicked);
        else
            Debug.LogError("ボタンが割り当てられていない");
    }

    //  ゲームパッドのAボタンやキーボードのEnterキー押下を処理するメソッド
    public async UniTask HandleNextImage()
    {
        if (!canReceiveInput)
        {
            //  連射対策として、無視する処理
            return;
        }

        //  入力を受け付けないようにする
        canReceiveInput = false;

        if (isSequenceActive)
        {
            //  シーケンスがアクティブな場合、次の画像を表示またはシーケンスを終了する
            await AdvanceImageSequence();
        }
        else
        {
            //  シーケンスがアクティブでない場合、ボタンが選択されているか確認
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == controlButton.gameObject)
            {
                await StartImageSequence();
            }
        }

        //  debounceTime秒後に入力を再度受け付ける
        await UniTask.Delay(System.TimeSpan.FromSeconds(debounceTime));
        canReceiveInput = true;
    }

    //  ボタンが押された際に呼び出されるメソッド
    private void OnControlButtonClicked()
    {
        HandleNextImage().Forget();
    }

    //  画像シークエンスを開始するメソッド
    private UniTask StartImageSequence()
    {
        if (sequenceImages.Count == 0)
        {
            Debug.LogError("Image Sequenceが空になっている");
            return UniTask.CompletedTask;
        }

        isSequenceActive = true;
        currentStep = 0;

        //  ボタンを無効化
        controlButton.interactable = false;

        //  最初の画像を表示する
        var image = sequenceImages[currentStep];
        if (image != null)
        {
            image.gameObject.SetActive(true);
            currentStep++;
        }
        else
        {
            Debug.LogError($"Imageリストのインデックス {currentStep} に未設定のオブジェクトがある");
        }

        return UniTask.CompletedTask;
    }

    //  画像シークエンスを進行させるメソッド
    private UniTask AdvanceImageSequence()
    {
        if (currentStep < sequenceImages.Count)
        {
            //  前の画像を非アクティブ化する
            if (currentStep > 0)
            {
                var previousImage = sequenceImages[currentStep - 1];
                if (previousImage != null)
                {
                    previousImage.gameObject.SetActive(false);
                }
            }

            //  次の画像を表示する
            var image = sequenceImages[currentStep];
            if (image != null)
            {
                image.gameObject.SetActive(true);
                currentStep++;
            }
        }
        else
        {
            //  最後の画像が表示されている場合、すべての画像を非アクティブ化する
            foreach (var image in sequenceImages)
            {
                if (image != null)
                    image.gameObject.SetActive(false);
            }
            currentStep = 0;
            isSequenceActive = false;


            //  ボタンを再度有効化する
            controlButton.interactable = true;

            //  ボタンを選択状態に戻す
            EventSystem.current.SetSelectedGameObject(controlButton.gameObject);
        }

        return UniTask.CompletedTask;
    }
}