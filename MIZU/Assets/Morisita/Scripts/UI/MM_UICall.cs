using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MM_UICall : MonoBehaviour
{
    [SerializeField]
    private InputAction playerPauseInputAction;
    [SerializeField]
    MM_UI_Instantiate instantiateUI;

    private GameObject createdUI;
    private void Awake()
    {
        playerPauseInputAction.performed += CreateUI;
        playerPauseInputAction.Enable();
    }

    public void CreateUI(InputAction.CallbackContext context)
    {
        if (createdUI==null)
        {
            SetUI();
            MM_TimeManager.Instance.StopTime();
        }
        else
        {
            Destroy(createdUI);
            MM_TimeManager.Instance.MoveTime();
        }
    }

    void SetUI()
    {
       createdUI = instantiateUI.CreateUI();
    }
}
