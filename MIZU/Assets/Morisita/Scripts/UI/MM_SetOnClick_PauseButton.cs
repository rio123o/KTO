using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MM_SetOnClick_PauseButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(MM_TimeManager.instance.MoveTime);
        button.onClick.AddListener(()=>Destroy(this.gameObject));

    }


}
