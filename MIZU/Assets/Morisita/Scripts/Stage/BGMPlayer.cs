using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    MM_SoundManager.SoundType playSoundType=MM_SoundManager.SoundType.None;
    void Start()
    {
        Play(playSoundType);
    }

    void Play(MM_SoundManager.SoundType type)
    {
        MM_SoundManager.Instance.PlayBGM(type);
    }
}
