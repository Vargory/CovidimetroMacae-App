using Lean.Transition.Method;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public float duration;
    float minimum = 0;
    float maximum = 1;
    public float number;

    float t = 0;

    public LeanGraphicColor animation;

    public int sceneIndex;
    public void Start()
    {
        
    }

    public void Update()
    {
        t += Time.deltaTime / duration;
        number = Mathf.Lerp(minimum, maximum, t);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
       
        if(number == 1)
        {

            Debug.Log("Foi 1");
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                yield return null;
            }
        }
        
    }
}
