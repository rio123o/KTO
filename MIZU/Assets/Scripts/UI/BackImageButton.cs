using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class BackImageButton : MonoBehaviour
{
    private PlayerInputActions backInputActions;  //  自動生成された入力アクションクラスのインスタンス

    [Header("ImageSwitcherクラスの参照")]
    [SerializeField] private ImageSwitcher imageSwitcher;

    void Awake()
    {
        backInputActions = new PlayerInputActions();  //  PlayerInputActionsインスタンスの初期化
    }

    void OnEnable()
    {
        backInputActions.GamePlay.Back.performed += OnBackButton;  //  イベントリスナーを登録する
        backInputActions.GamePlay.Back.Enable();  //  アクションを有効化する
    }

    void OnDisable()
    {
        backInputActions.GamePlay.Back.performed -= OnBackButton;  //  イベントリスナーを解除する
        backInputActions.GamePlay.Back.Disable();  //  アクションを無効化する
    }

    private void OnBackButton(InputAction.CallbackContext context)
    {
        //  ImageSwitcherが設定されていた場合、HandleBackImageを返す
        if (imageSwitcher != null)
        {
            imageSwitcher.HandleBackImage();
        }
    }
}
