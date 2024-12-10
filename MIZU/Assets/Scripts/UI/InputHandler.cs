using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions inputActions;  //  �����������ꂽ���̓A�N�V�����N���X�̃C���X�^���X
    [Header("ImageSwitcher�N���X�̎Q��")]
    [SerializeField] private ImageSwitcher imageSwitcher;

    void Awake()
    {
        inputActions = new PlayerInputActions();  //  PlayerInputActions�C���X�^���X�̏�����
    }

    void OnEnable()
    {
        inputActions.Gameplay.NextImage.performed += OnNextImage;  //  �C�x���g���X�i�[��o�^����
        inputActions.Gameplay.NextImage.Enable();  //  �A�N�V������L��������
    }

    void OnDisable()
    {
        inputActions.Gameplay.NextImage.performed -= OnNextImage;  //  �C�x���g���X�i�[����������
        inputActions.Gameplay.NextImage.Disable();  //  �A�N�V�����𖳌�������
    }

    //  NextImage�A�N�V���������s���ꂽ�ۂɌĂяo����郁�\�b�h
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
                Debug.LogError($" NextImage�A�N�V�����̃G���[: {ex.Message}\n{ex.StackTrace}");
            }
        }
        else
        {
            Debug.LogError("ImageSwitcher���A�T�C������Ă��܂���B");
        }
    }
}
