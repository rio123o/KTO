using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MM_Fall_Platform : MonoBehaviour
{
    [SerializeField]
    private MM_PlayerTrigger pTrigger;
    [SerializeField]
    private GameObject platformObcjet;
    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private float waitTime;

    [SerializeField]
    Vector3 originalPosition;
    Rigidbody rb;
    [SerializeField]
    bool isFall = false;
    [SerializeField]
    bool isRespown = false;
    [SerializeField]
    bool isRunFall = false;

    Vector3 _fallSpeed;

    MM_ObserverBool observerBool_isRespown;
    MM_ObserverBool observerBool_isTrigger;
    CancellationTokenSource cts;
    CancellationToken token;

    // Start is called before the first frame update
    void Start()
    {
        observerBool_isRespown = new MM_ObserverBool();
        observerBool_isTrigger = new MM_ObserverBool();
        cts = new CancellationTokenSource();
        token = cts.Token;
        rb = GetComponent<Rigidbody>();
        originalPosition = gameObject.transform.position;
        isRespown = false;
        rb.isKinematic = true;
        rb.freezeRotation = true;

        _fallSpeed = new(0, -fallSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerRideOnFloor();
        CheckIsRespown();
    }
    void CheckPlayerRideOnFloor()
    {
        if (observerBool_isTrigger.OnBoolTrueChange(pTrigger.GetIsTrigger()) && !isRunFall)
        {
            print("do");
            Fall(token);
        }
    }

    void CheckIsRespown()
    {
        if (MM_PlayerStateManager.Instance.GetPlayerState() == MM_PlayerStateManager.PlayerState.Respown)
            isRespown = true;
        else
            isRespown = false;

        if (observerBool_isRespown.OnBoolTrueChange(isRespown))
            Reset();

    }

    async void Fall(CancellationToken ctoken)
    {
        isRunFall = true;
        var destroyToken = this.GetCancellationTokenOnDestroy();
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(ctoken, destroyToken).Token;
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: linkedToken);
            isFall = true;
            while (isFall)
            {
                rb.MovePosition(rb.position + _fallSpeed * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: linkedToken);
            }
        }
        catch (OperationCanceledException)
        {
            print("FallèàóùÇ™ÉLÉÉÉìÉZÉãÇ≥ÇÍÇ‹ÇµÇΩ");
            Reset();
        }
        isRunFall = false;
    }

    private void Reset()
    {
        cts.Cancel();
        cts.Dispose();

        transform.position = originalPosition;
        SetActiveTrueFallPlatform();
        ResetBool();
        SetCancelToken();
        pTrigger.ResetTrigger();

    }
    void SetCancelToken()
    {
        cts = new CancellationTokenSource();
        token = cts.Token;
    }

    void SetActiveFalseFallPlatform()
    {
        platformObcjet.SetActive(false);
        pTrigger.gameObject.SetActive(false);
    }

    void SetActiveTrueFallPlatform()
    {
        platformObcjet.SetActive(true);
        pTrigger.gameObject.SetActive(true);
    }

    void ResetBool()
    {
        isFall = false;
        isRespown = false;
        isRunFall = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("puddle"))
        {
            print("hureta");

            //SetActiveFalseFallPlatform();

            isFall = false;
        }
    }
}
