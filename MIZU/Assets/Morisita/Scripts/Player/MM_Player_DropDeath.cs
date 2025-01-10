using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_DropDeath : MonoBehaviour
{
    // Start is called before the first frame update
    MM_Test_Player player;
    MM_PlayerPhaseState phaseState;
    void Start()
    {
        player = GetComponent<MM_Test_Player>();  
        phaseState=GetComponent<MM_PlayerPhaseState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (phaseState.GetState() != MM_PlayerPhaseState.State.Solid&&other.CompareTag("puddle"))
            player.Death();
    }
}
