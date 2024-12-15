using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions inputActions;  //  自動生成された入力アクションクラスのインスタンス
    [Header("ImageSwitcherクラスの参照")]
    [SerializeField] private ImageSwitcher imageSwitcher;

    void Awake()
    {
        inputActions = new PlayerInputActions();  //  PlayerInputActionsインスタンスの初期化
    }

    void OnEnable()
    {
        inputActions.GamePlay.NextImage.performed += OnNextImage;  //  イベントリスナーを登録する
        inputActions.GamePlay.NextImage.Enable();  //  アクションを有効化する
    }

    void OnDisable()
    {
        inputActions.GamePlay.NextImage.performed -= OnNextImage;  //  イベントリスナーを解除する
        inputActions.GamePlay.NextImage.Disable();  //  アクションを無効化する
    }

    //  NextImageアクションが実行された際に呼び出されるメソッド
    private async void OnNextImage(InputAction.CallbackContext context)
    {
        if (imageSwitcher != null)
        {
            try
            {
                await imageSwitcher.HandleNextImage();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($" NextImageアクションのエラー: {ex.Message}\n{ex.StackTrace}");
            }
        }
        else
        {
            Debug.LogError("ImageSwitcherがアサインされていません。");
        }
    }
}
