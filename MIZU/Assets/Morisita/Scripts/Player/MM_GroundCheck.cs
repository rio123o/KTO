using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MM_GroundCheck : MonoBehaviour
{
    //[Header("エフェクトがついた床を判定するか")] public bool checkPlatformGroud = true;

    private string groundTag = "Ground";
    private string groundTag2 = "MoveGround";
    private string waterTag = "puddle";
    //private string platformTag = "GroundPlatform";
    //private string moveFloorTag = "MoveFloor";
    //private string fallFloorTag = "FallFloor";
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private bool isGroundEnter, isGroundStay, isGroundExit;
    [SerializeField]
    private bool isOnWater = false;
    [SerializeField]
    private bool isWaterEnter, isWaterStay, isWaterExit;


    public void ResetFlag()
    {
        isOnGround = false;
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        isOnWater = false;
        isWaterEnter = false;
        isWaterStay = false;
        isWaterExit = false;

        //print("GroudFlagReset");
    }

    //接地判定を返すメソッド
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isOnGround = true;
        }
        else if (isGroundExit)
        {
            isOnGround = false;
        }
        return isOnGround;
    }

    public bool IsPuddle()
    {
        if (isWaterEnter || isWaterStay)
        {
            isOnWater = true;
        }
        else if (isWaterExit)
        {
            isOnWater = false;
        }
        return isOnWater;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(groundTag)|| collision.CompareTag(groundTag2))
        {
            isGroundEnter = true;
            isGroundExit = false;
        }
        if (collision.CompareTag(waterTag))
        {
            isWaterEnter = true;
            isWaterExit = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(groundTag) || collision.CompareTag(groundTag2))
        {
            isGroundStay = true;
            isGroundExit = false;
        }
        if (collision.CompareTag(waterTag))
        {
            isWaterStay = true;
            isWaterExit = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(groundTag) || collision.CompareTag(groundTag2))
        {
            isGroundEnter = false;
            isGroundStay = false;
            isGroundExit = true;
        }
        if (collision.CompareTag(waterTag))
        {
            isWaterEnter = false;
            isWaterStay = false;
            isWaterExit = true;
        }
    }
}