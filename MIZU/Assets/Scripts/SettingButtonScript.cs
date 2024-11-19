using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject settingImage;

    public void SettingButton()
    {
        settingImage.SetActive(true);
    }
}
