using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationMask : MonoBehaviour
{
    [Header("MM_PlayerTriggerコンポーネントへの参照")]
    [SerializeField] private MM_PlayerTrigger playerTrigger;

    [Header("アニメーションを再生するアニメーター")]
    [SerializeField] private Animator animator;

    [Header("アニメーターのトリガー名")]
    [SerializeField] private string triggerName = "Move Trigger";

    private bool animationTrigger = false;  //  アニメーションを再生するためのフラグ

    void Update()
    {
        if(playerTrigger.GetIsTrigger() && !animationTrigger)
        {
            PlayAnimation();
            animationTrigger = true;
        }
    }

    //  アニメーションを再生するメソッド
    private void PlayAnimation()
    {
        if(animator != null)
        { 
            animator.SetTrigger(triggerName);
            Debug.Log("アニメーションを再生した");
        }
        else
        {
            Debug.LogWarning("アニメーターがアサインされていない");
        }
    }
}
