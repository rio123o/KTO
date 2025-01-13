//  氷状態のキャラにアタッチする
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice_above_water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //  接触した場合
    private void OnCollisionEnter(Collision collision)
    {
        //  乗ったのが水上状態のキャラの場合
        if (collision.gameObject.CompareTag("Water"))
        {
            //  氷と一緒に動く
            collision.transform.parent = transform;
        }
    }

    //  接触が解除された場合
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            //  解除される
            collision.transform.parent = null;
        }
    }

}
