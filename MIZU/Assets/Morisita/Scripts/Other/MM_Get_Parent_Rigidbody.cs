using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Get_Parent_Rigidbody : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponentInParent<Rigidbody>();
    }

}
