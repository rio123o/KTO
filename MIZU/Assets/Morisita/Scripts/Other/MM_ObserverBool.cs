using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MM_ObserverBool
{

    private bool oldBool;
    private bool newBool;

    bool onBoolChange;
    bool onBoolTrueChange;
    bool onBoolFalseChange;

    private bool _Bool
    {
        set
        {
            oldBool = newBool;
            newBool = value;

            if (oldBool == newBool)
            {
                onBoolChange = false;
                onBoolTrueChange = false;
                onBoolFalseChange = false;
            }
            else 
            {
                onBoolChange = true;
                if (newBool)
                {
                    onBoolTrueChange = true;
                    onBoolFalseChange = false;
                }
                else
                {
                    onBoolTrueChange = false;
                    onBoolFalseChange = true;
                }
            }

        }
    }
    public MM_ObserverBool() { }
    public MM_ObserverBool(bool inBool)
    {
              SetBool(inBool);
    }
    public void SetBool(bool inBool)
    {
        _Bool=inBool;
    }

    public bool OnBoolChange(bool inBool)
    {
        SetBool(inBool);
        return onBoolChange;
    }
    public bool OnBoolTrueChange(bool inBool)
    {
        SetBool(inBool);

        return onBoolTrueChange;
    }
    public bool OnBoolFalseChange(bool inBool)
    {
        SetBool(inBool);
        return onBoolFalseChange;
    }
}

