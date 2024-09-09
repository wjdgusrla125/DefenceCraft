using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName; 
    public string loadingSceneName = "Loading";
    
    private bool isPaused = false;

    private void Update()
    {
        
    }

    public void GoToNextScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public void DirectNextScene()
    {
        SceneManager.LoadScene("Lobby");
    }

    private IEnumerator LoadSceneAsync()
    {
        // 로딩 씬을 비동기적으로 로드
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        // 로딩 씬에서 LoadingProgress 스크립트를 찾습니다
        LoadingProgress loadingProgress = FindObjectOfType<LoadingProgress>();

        // 다음 씬을 비동기적으로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (loadingProgress != null)
            {
                loadingProgress.UpdateProgress(progress);
            }

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // 로딩 씬 언로드
        yield return SceneManager.UnloadSceneAsync(loadingSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}