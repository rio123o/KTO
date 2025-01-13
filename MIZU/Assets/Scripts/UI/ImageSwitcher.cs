using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    [Header("���ԂɃA�N�e�B�u������摜�����X�g�ɒǉ�����")]
    [SerializeField] private List<Image> sequenceImages = new List<Image>();

    [Header("�摜��\�����鎞�Ɏg���{�^��")]
    [SerializeField] private Button controlButton;

    [Header("�A�ˑ΍�Ƃ��āA�{�^�����͂𖳎����鎞��(�b)")]
    [SerializeField] private float debounceTime = 0.1f;

    private int currentStep = 0;  //  ���݂̉摜�\���̒i�K
    private bool isSequenceActive = false;  //  �摜�V�[�N�G���X���A�N�e�B�u�ɂȂ��Ă��邩
    private bool canReceiveInput = true;  //  ���͂��󂯎��邩�ǂ����̃t���O

    void Start()
    {
        //  �S�Ẳ摜���A�N�e�B�u�ɐݒ肷��
        foreach (var image in sequenceImages)
        {
            if (image != null)
                image.gameObject.SetActive(false);
            else
                Debug.LogError("Image���X�g�ɖ��ݒ�̃I�u�W�F�N�g������");
        }

        //  �{�^�������������蓖�Ă��Ă��邩�̊m�F
        if (controlButton != null)
            controlButton.onClick.AddListener(OnControlButtonClicked);
        else
            Debug.LogError("�{�^�������蓖�Ă��Ă��Ȃ�");
    }

    //  �Q�[���p�b�h��A�{�^����L�[�{�[�h��Enter�L�[�������������郁�\�b�h
    public async UniTask HandleNextImage()
    {
        if (!canReceiveInput)
        {
            //  �A�ˑ΍�Ƃ��āA�������鏈��
            return;
        }

        //  ���͂��󂯕t���Ȃ��悤�ɂ���
        canReceiveInput = false;

        if (isSequenceActive)
        {
            //  �V�[�P���X���A�N�e�B�u�ȏꍇ�A���̉摜��\���܂��̓V�[�P���X���I������
            await AdvanceImageSequence();
        }
        else
        {
            //  �V�[�P���X���A�N�e�B�u�łȂ��ꍇ�A�{�^�����I������Ă��邩�m�F
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == controlButton.gameObject)
            {
                await StartImageSequence();
            }
        }

        //  debounceTime�b��ɓ��͂��ēx�󂯕t����
        await UniTask.Delay(System.TimeSpan.FromSeconds(debounceTime));
        canReceiveInput = true;
    }

    //  �{�^���������ꂽ�ۂɌĂяo����郁�\�b�h
    private void OnControlButtonClicked()
    {
        HandleNextImage().Forget();
    }

    //  �摜�V�[�N�G���X���J�n���郁�\�b�h
    private UniTask StartImageSequence()
    {
        if (sequenceImages.Count == 0)
        {
            Debug.LogError("Image Sequence����ɂȂ��Ă���");
            return UniTask.CompletedTask;
        }

        isSequenceActive = true;
        currentStep = 0;

        //  �{�^���𖳌���
        controlButton.interactable = false;

        //  �ŏ��̉摜��\������
        var image = sequenceImages[currentStep];
        if (image != null)
        {
            image.gameObject.SetActive(true);
            currentStep++;
        }
        else
        {
            Debug.LogError($"Image���X�g�̃C���f�b�N�X {currentStep} �ɖ��ݒ�̃I�u�W�F�N�g������");
        }

        return UniTask.CompletedTask;
    }

    //  �摜�V�[�N�G���X��i�s�����郁�\�b�h
    private UniTask AdvanceImageSequence()
    {
        if (currentStep < sequenceImages.Count)
        {
            //  �O�̉摜���A�N�e�B�u������
            if (currentStep > 0)
            {
                var previousImage = sequenceImages[currentStep - 1];
                if (previousImage != null)
                {
                    previousImage.gameObject.SetActive(false);
                }
            }

            //  ���̉摜��\������
            var image = sequenceImages[currentStep];
            if (image != null)
            {
                image.gameObject.SetActive(true);
                currentStep++;
            }
        }
        else
        {
            //  �Ō�̉摜���\������Ă���ꍇ�A���ׂẲ摜���A�N�e�B�u������
            foreach (var image in sequenceImages)
            {
                if (image != null)
                    image.gameObject.SetActive(false);
            }
            currentStep = 0;
            isSequenceActive = false;


            //  �{�^�����ēx�L��������
            controlButton.interactable = true;

            //  �{�^����I����Ԃɖ߂�
            EventSystem.current.SetSelectedGameObject(controlButton.gameObject);
        }

        return UniTask.CompletedTask;
    }

    public void HandleBackImage()
    {
        if (!isSequenceActive)
            return;

        //  �\������Ă���摜��2���ڈȍ~�̎�
        if (currentStep > 1)
        {
            //  ���ݕ\������Ă���摜
            var currentImage = sequenceImages[currentStep - 1];
            if (currentImage != null)
            {
                currentImage.gameObject.SetActive(false);
            }

            currentStep--;

            //  �O�̉摜
            var previousImage = sequenceImages[currentStep - 1];
            if (previousImage != null)
            {
                previousImage.gameObject.SetActive(true);
            }
            return;
        }

        //  �\������Ă���摜��1���ڂ̎�
        //  �S�Ẳ摜���\���ɂ���
        foreach (var image in sequenceImages)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
            }
        }
        currentStep = 0;
        isSequenceActive = false;
        //  �{�^�����ēx�L��������
        controlButton.interactable = true;
        // �{�^����I����Ԃɖ߂�
        EventSystem.current.SetSelectedGameObject(controlButton.gameObject);

    }
}