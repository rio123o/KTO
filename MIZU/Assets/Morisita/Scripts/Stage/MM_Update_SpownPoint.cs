using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Update_SpownPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private MM_PlayerSpownTest spowntest;
    [SerializeField]
    private Transform spownPoint;
    [SerializeField]
    private MM_PlayerTrigger trigger;

    // Update is called once per frame
    void Update()
    {
        if(trigger.GetIsTrigger())
        {
            spowntest.SetSpownPoint(spownPoint);
        }
    }
}
