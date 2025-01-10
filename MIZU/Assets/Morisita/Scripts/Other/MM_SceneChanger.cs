using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string changeSceneName;
    [SerializeField]
    private int changeSceneNum=0;

    public void ReloadScene()
    {
        var scene=SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    public void SceneChange(int sceneNum)
    {
        changeSceneNum = sceneNum;
        SceneManager.LoadScene(changeSceneNum);

    }

    public void  SceneChange(string sceneName)
    {
        changeSceneName = sceneName;
        SceneManager.LoadScene(changeSceneName);
    }
}
