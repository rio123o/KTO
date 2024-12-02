using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_Water_Animation_Manager : MM_Player_Animation_Manager
{
    [SerializeField]
    private MM_GroundCheck groundCheck;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isAir = false;

    Vector3 defaultPosition = new(0f, -0.3f, 0);
    Vector3 defaultRotation = new(0f, 0f, 0f);

    void ResetTransform()
    {
        this.transform.localPosition = defaultPosition;
        this.transform.localEulerAngles = defaultRotation;
    }

    public override void PlayAnim()
    {
        ResetTransform();

        float pSpeed_X = playerTest.GetAbsSpeed().x;
        float pSpeed_Y = playerTest.GetSpeed().y;
        bool isGround = groundCheck.IsGround();


        _animator.SetFloat("Speed", pSpeed_X);

        if (pSpeed_Y > 0.1 && !isJumping)
        {
            _animator.SetBool("OnGround", false);
            _animator.Play("jump", 0, 0f);
            isJumping = true;
        }
        else if (isGround)
        {
            _animator.SetBool("OnGround", true);
            isJumping = false;
            isAir = false;
        }
        else if (!isGround && !isJumping && !isAir)
        {
            _animator.SetBool("OnGround", false);
            _animator.Play("falling", 0, 0.0f);
            isAir = true;
        }
    }
}
