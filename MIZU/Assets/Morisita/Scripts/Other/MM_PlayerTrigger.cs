using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider))]
public class MM_PlayerTrigger : MonoBehaviour
{
    string PLAYER = "Player";
    private bool isTrigger = false;
    private Collider hitCollider;

    private void OnEnable()
    {
        ResetTrigger();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(PLAYER))
        {
            hitCollider = other;
            isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER))
        {
            isTrigger = false;
        }

    }

    public bool GetIsTrigger() { return isTrigger; }
    public Collider GethitCollider() { return hitCollider; }

    public void ResetTrigger()
    {
        isTrigger = false;
    }

}
