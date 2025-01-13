using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(goalDecisionScript))]

public class canvasAppear : MonoBehaviour
{
    goalDecisionScript goalDecision;

    public GameObject goalCanvas;

    void Start()
    {
        goalDecision = GetComponent<goalDecisionScript>();
    }

    private void goalCanvasAppear()
    {
        goalCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (goalDecision.isGoal == true)
        {
            Debug.Log("appear");
            goalCanvasAppear();
        }
    }
}
