using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseRotationButton : RepressableButton
{
    [Header("QÆ‚·‚éRotateObject")]
    [SerializeField] private RotateObject rotateObject;

    public override void Execute()
    {
        if (rotateObject == null)
        {
            Debug.LogError($"{gameObject.name}: RotateObject ‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñB");
            return;
        }

        //  Œ»İ‚Ì‰ñ“]•ûŒü‚ğ”½“]‚·‚é
        int newDirection = -rotateObject.RotateDirection;
        rotateObject.SetRotationDirection(newDirection);

        Debug.Log($"{gameObject.name}: RotateObject‚Ì‰ñ“]•ûŒü‚ğ{(newDirection == 1 ? "³“]" : "‹t‰ñ“]")}‚É•ÏX‚µ‚½B");
    }
}
