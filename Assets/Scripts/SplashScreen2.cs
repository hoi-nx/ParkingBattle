using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    AsyncOperation operation;


    public void LoadLevel()
    {
        // AsyncOperation operation=SceneManager.LoadSceneAsync(sceneIndex);
        StartCoroutine(LoadAsynchronously(1));
    }
    IEnumerator LoadAsynchronously(int senceIndex)
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            new WaitForSeconds(2.5f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            operation.allowSceneActivation = true;
            yield return null;
        }
    }
    IEnumerator Start()
    {


        yield return new WaitForSeconds(2.5f);
       // LoadLevel();
       

    }


}
