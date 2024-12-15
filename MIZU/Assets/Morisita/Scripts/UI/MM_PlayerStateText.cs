using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MM_PlayerStateText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        //if (MM_PlayerStateManager.Instance.GetPlayerState() == MM_PlayerStateManager.PlayerState.Respown)
        //    MM_TimeManager.Instance.StopTime();
        string text = $"PlayerState:{MM_PlayerStateManager.Instance.GetPlayerState()}";
        textMeshProUGUI.text = text;
    }

}
