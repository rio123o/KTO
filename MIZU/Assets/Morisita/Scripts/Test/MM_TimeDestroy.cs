using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_TimeDestroy : MonoBehaviour
{
    public void TimeDestroy(float limitTime)
    {
        Destroy(gameObject,limitTime);
    }
}
