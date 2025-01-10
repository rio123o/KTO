using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MM_Wind_Effect_Switcher : RepressableButton
{
    [SerializeField]
    private GameObject UpWindEffect;
    [SerializeField]
    private GameObject DownWindEffect;

    [SerializeField]
    private bool IsStartEnableUpWindEffect=false;
    [SerializeField]
    private bool IsStartEnableDownWindEffect=false;

    void Start()
    {
        UpWindEffect.SetActive(IsStartEnableUpWindEffect);
        DownWindEffect.SetActive(IsStartEnableDownWindEffect);
    }
    public override void Execute()
    {
        print("Switch");
        if (UpWindEffect.activeSelf)
            UpWindEffect.SetActive(false);
        else
            UpWindEffect.SetActive(true);

        if (DownWindEffect.activeSelf)
            DownWindEffect.SetActive(false);
        else
            DownWindEffect.SetActive(true);

    }
}
