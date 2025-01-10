using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MM_PlayerStateManager : MM_SingletonMonoBehaviour<MM_PlayerStateManager>
{
    public enum PlayerState
    {
        None,
        Playing,
        Death,
        Respown,
        Pause,
    }

    PlayerState playerState;

    Stack<PlayerState> playerStates=new Stack<PlayerState>();
    private void Update()
    {
        PopOverStack();
    }

    void PopOverStack()
    {
        if (playerStates.Count > 10)
        {
            for (; playerStates.Count > 10;)
            {
                playerStates.Pop();
            }
        }
    }
    public PlayerState GetPlayerState()
    {
        return playerState;
    }
    public Stack<PlayerState> GetPlayerStateStack()
    {
        return playerStates;
    }
    public void SetPlayerState(PlayerState state)
    {
        if(state==PlayerState.Death)
        {
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.Death);
        }
        playerState = state;
        playerStates.Push(state);
    }
}
