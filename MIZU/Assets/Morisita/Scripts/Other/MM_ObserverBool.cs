using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MM_ObserverBool
{

    private bool oldBool;
    private bool newBool;

    private bool _Bool
    {
        set
        {
            oldBool = newBool;
            newBool = value;

            if (oldBool == newBool)
            {
                OnBoolChange = false;
                OnBoolTrueChange = false;
                OnBoolTrueChange = false;
            }
            else 
            {
                OnBoolChange = true;
                if (newBool)
                {
                    OnBoolTrueChange = true;
                    onBoolFalseChange = false;
                }
                else
                {
                    OnBoolTrueChange = false;
                    OnBoolFalseChange = true;
                }
            }

        }
    }
    public MM_ObserverBool() { }
    public MM_ObserverBool(bool inBool)
    {
        _Bool = inBool;
    }
    public void SetBool(bool inBool)
    {
        _Bool=inBool;
    }
    bool onBoolChange;
    bool onBoolTrueChange;
    bool onBoolFalseChange;
    public bool OnBoolChange
    {
        get => onBoolChange;
        private set { onBoolChange = value; }
    }
    public bool OnBoolTrueChange
    {
        get => onBoolTrueChange;
        private set { onBoolTrueChange = value; }
    }
    public bool OnBoolFalseChange
    {
        get => onBoolFalseChange;
        private set { onBoolFalseChange = value; }
    }
}

