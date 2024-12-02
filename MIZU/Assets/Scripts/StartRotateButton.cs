using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotateButton : MonoBehaviour
{
    [SerializeField] private RotateObject rotateObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (rotateObject != null)
            {
                rotateObject.StartRotation();
            }
        }
    }


}
