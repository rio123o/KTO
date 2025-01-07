using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MM_SoundManager : MM_SingletonMonoBehaviour<MM_SoundManager>
{
    public float masterVolume = 1f;
    public float seVolume = 1f;
    public float bgmVolume = 1f;

    private AudioSource bgmSource;
    private Dictionary<SoundType, AudioClip> audioClips = new();

    public enum SoundType
    {
        None,
        BGM,
        GameOver,
        TitleBGM,
        StageBGM,
        Transform,
        ButttonPush,
        // ここに再生するSE,BGMの種類を追加する

    }

    [System.Serializable]
    public class SoundItem
    {
        public SoundType type;
        public AudioClip clip;
    }

    public SoundItem[] preloadedSounds;

    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;

        LoadPreloadedSounds();
    }

    private void LoadPreloadedSounds()
    {
        foreach (var soundItem in preloadedSounds)
        {
            if (soundItem.clip != null)
            {
                audioClips[soundItem.type] = soundItem.clip;
            }
        }
    }

    public void LoadSound(SoundType type, AudioClip clip)
    {
        audioClips[type] = clip;
    }

    /// <summary>
    /// SEを再生します
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    public void PlaySE(SoundType type, float volume = 1f)
    {
        if (audioClips.TryGetValue(type, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume * seVolume * masterVolume);
        }
        else
        {
            Debug.LogWarning($"サウンド {type} が見つかりません。");
        }
    }
    /// <summary>
    /// BGMを再生します
    /// BGMをフェードイン・フェードアウトさせることもできます
    /// fadeDuration=1f,フェードイン・フェードアウトに1秒
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fade"></param>
    /// <param name="fadeDuration"></param>
    public void PlayBGM(SoundType type, bool fade = false, float fadeDuration = 1f)
    {
        if (audioClips.TryGetValue(type, out AudioClip clip))
        {
            if (fade)
            {
                StartCoroutine(FadeBGM(clip, fadeDuration));
            }
            else
            {
                bgmSource.clip = clip;
                bgmSource.volume = bgmVolume * masterVolume;
                bgmSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"BGM {type} が見つかりません。");
        }
    }

    private System.Collections.IEnumerator FadeBGM(AudioClip newClip, float fadeDuration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, bgmVolume * masterVolume, timer / fadeDuration);
            yield return null;
        }
    }
    /// <summary>
    /// 全ての音の音量を設定します。
    /// 値は0~1
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume * masterVolume;
    }
    /// <summary>
    /// SEの音量を設定します。
    /// 値は0~1
    /// </summary>
    /// <param name="volume"></param>
    public void SetSEVolume(float volume)
    {
        seVolume = Mathf.Clamp01(volume);
    }
    /// <summary>
    /// BGMの音量を設定します。
    /// 値は0~1
    /// </summary>
    /// <param name="volume"></param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume * masterVolume;
    }
}