using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Player_Animation_Manager : MonoBehaviour
{
    public MM_Test_Player playerTest;
    [HideInInspector]
    public Animator _animator;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    virtual protected void Update()
    {
        PlayAnim();
    }

    public virtual void PlayAnim()
    {

    }
}
