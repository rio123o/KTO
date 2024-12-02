using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPuddle : MonoBehaviour
{
    [SerializeField] private GameObject puddleObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
          return; 

        if(puddleObject.activeSelf)
        {
            puddleObject.SetActive(false);
        }
    }
}
