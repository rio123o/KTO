using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_OneWayTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private MM_PlayerTrigger playerTrigger;

    Collider myCollider;
    Collider hitCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        hitCollider = playerTrigger.GethitCollider();

        if (hitCollider == null || myCollider == null)
            return;

        if (playerTrigger.GetIsTrigger())
        {
            Physics.IgnoreCollision(hitCollider,myCollider, true);
        }
        else
            Physics.IgnoreCollision(hitCollider, myCollider, false);
    }
}
