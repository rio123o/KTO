using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalDecisionScript : MonoBehaviour
{
    public bool isGoal = false;
    private int areaStay = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;  //  tag��Player�ł͖���������
        //  tag��Player��������
        areaStay++;
        areaChack();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        areaStay--;
    }

    private void areaChack()
    {
        if (areaStay != 2) return;

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");  //  tag��Player�̃I�u�W�F�N�g���������Ĕz��ɂ���

        Debug.Log("goal");
        isGoal = true;

        SceneManager.LoadScene("goalScene");
    }
}
