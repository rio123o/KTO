using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Test_SoundTest : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.GameOver);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            MM_SoundManager.Instance.PlayBGM(MM_SoundManager.SoundType.BGM,true,5f);
        }
    }
}
