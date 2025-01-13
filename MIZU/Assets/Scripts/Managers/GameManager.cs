using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("main Scene");
    }


    public void RetrunTitle()
    {
        SceneManager.LoadScene("titleScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //  ŠJ”­ŠÂ‹«‚Å‚ÌI—¹‚Ìê‡
#else
       Application.Quit();
#endif
    }
}