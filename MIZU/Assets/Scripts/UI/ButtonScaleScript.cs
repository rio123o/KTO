using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Vector3 originalScale;
    [Header("‘I‘ð‚³‚ê‚Ä‚¢‚éŽž‚Ì‘å‚«‚³")]
    [SerializeField]private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = selectedScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
