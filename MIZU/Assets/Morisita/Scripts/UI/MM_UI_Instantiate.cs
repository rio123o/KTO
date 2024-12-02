using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MM_UI_Instantiate : MonoBehaviour
{
    private Transform UIRoot;
    [SerializeField]
    private GameObject displayUI;

    private void Start()
    {
        UIRoot = transform;
    }
    public GameObject CreateUI()
    {
        return Instantiate(displayUI, UIRoot);
    }

}
