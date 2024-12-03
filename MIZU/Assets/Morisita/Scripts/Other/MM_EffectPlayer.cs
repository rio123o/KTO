using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class MM_EffectPlayer : MonoBehaviour
{
    [Tooltip("発生させるエフェクト(パーティクル)")]
    public ParticleSystem _particle;
    // エフェクトを出す場所
    protected Transform _particleTransform;

    virtual public void Play(float time)
    {
        ParticleInstantiate(_particleTransform,time);
    }
    virtual public void Play()
    {
        ParticleInstantiate(_particleTransform);
    }

    virtual async public void ParticleInstantiate(Transform tf)
    {
        var token = this.GetCancellationTokenOnDestroy();
        ParticleSystem particle = Instantiate(_particle, tf);

        float lifetime = particle.main.startLifetimeMultiplier;
        particle.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(lifetime),cancellationToken :token);

        Destroy(particle.gameObject);

    }
    virtual async public void ParticleInstantiate(Transform tf, float time)
    {
        var token = this.GetCancellationTokenOnDestroy();
        ParticleSystem particle = Instantiate(_particle, tf);

        float lifetime = time;

        particle.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(lifetime), cancellationToken: token);

        Destroy(particle);

    }

    public void SetParticleTransform(Transform tf)
    {
        _particleTransform = tf;
    }
   
    public void SameParentObjectRotation()
    {
        _particleTransform.rotation = this.transform.rotation;
    }

    public void SameParentObjectScale()
    {
        _particleTransform.localScale = this.transform.localScale;
    }
}



