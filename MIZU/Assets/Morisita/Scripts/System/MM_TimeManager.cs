using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MM_TimeManager instance=null;
    [SerializeField]
    private float defaultTimeScale = 1.0f;
    [SerializeField]
    private float nowTimeScale=1.0f;
    [SerializeField]
    private bool isStopTime;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitTimeScale();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeScale();
    }
    
    void InitTimeScale()
    {
        Time.timeScale = defaultTimeScale;
    }

    void UpdateTimeScale()
    {
        if (!isStopTime)
            Time.timeScale = GetTimeScale();
        else
            Time.timeScale = 0;
    }

    public void ResetTimeScale()
    {
        Time.timeScale=defaultTimeScale;
    }
    public void StopTime()
    {
        isStopTime = true;
    }

    public void MoveTime()
    {
        isStopTime = false;
    }

    public void ChangeTimeScale(float timescale)
    {
        nowTimeScale = timescale;
    }
    public float GetTimeScale()
    {
        return nowTimeScale;
    }
}
