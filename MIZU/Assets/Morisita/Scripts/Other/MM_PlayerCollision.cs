using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MM_PlayerCollision : MonoBehaviour
{
    string PLAYER = "Player";
    private bool isCollision = false;
    private Collider hitCollider;
    // Start is called before the first frame update

    private void OnEnable()
    {
        ResetCollision();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER))
        {
            hitCollider = collision.collider;
            isCollision = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag(PLAYER))
        {
            isCollision = false;
        }
    }

    public bool GetIsCollision() {  return isCollision; }
    public Collider GetHitCollider() { return hitCollider; }

    public void ResetCollision()
    {
        isCollision=false;
    }
}
