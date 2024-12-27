//using Cysharp.Threading.Tasks.Triggers;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class MM_PlayerControllerManager : MM_SingletonMonoBehaviour<MM_PlayerControllerManager>
//{
//    enum MAPS
//    {
//       None,
//       Player,
//       UI,
//    }
//    [SerializeField]
//    private InputActionAsset inputSettings;

//    private InputActionMap playerInputmap;

//    private InputActionMap UIInputmap;

//    [SerializeField]
//    PlayerInput[] playerInput;

//    MM_PauseState pauseState;
//    string PLAYERMAP = "Player";
//    string UIMAP = "UI";

//    void Start()
//    {
//        playerInputmap = inputSettings.FindActionMap(MAPS.Player.ToString());
//        UIInputmap = inputSettings.FindActionMap(MAPS.UI.ToString());
//        SetControlPlayerMap();
//        pauseState=new MM_PauseState();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (MM_PlayerStateManager.Instance.GetPlayerState() == MM_PlayerStateManager.PlayerState.Pause)
//        {
//            pauseState.OnPause();
//        }
//        else
//        {
//            pauseState.OffPause();
//        }
//    }


//    public void SetControlUIMap()
//    {
//        playerInputmap.Disable();
//        UIInputmap.Enable();
//        foreach (var map in playerInput)
//        {
//            map.SwitchCurrentActionMap(UIMAP);
//        }

//    }

//    public void SetControlPlayerMap()
//    {

//        playerInputmap.Enable();
//        UIInputmap.Disable();
//        foreach (var map in playerInput)
//        {
//            map.SwitchCurrentActionMap(PLAYERMAP);
//        }
//    }
//}
