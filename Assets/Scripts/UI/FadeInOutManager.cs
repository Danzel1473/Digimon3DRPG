using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOutManager : MonoBehaviour
{
    private static FadeInOutManager instance;
    public static FadeInOutManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FadeInOutManager>();
            }
            return instance;
        }
    }

    [SerializeField] private Image fadePanel;

    public void FadeIn()
    {
        fadePanel.gameObject.SetActive(false);
        //StartCoroutine(FadeInAction());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAction());
    }

    public IEnumerator FadeInAction()
    {
        float elapsedTime = 0f;
        float fadeDuration = 1f;
        Color panelColor = fadePanel.color;
        panelColor.a = 1f;
        fadePanel.color = panelColor;

        fadePanel.gameObject.SetActive(true);
        while (elapsedTime <= fadeDuration)
        {
            panelColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelColor.a = 0f;
        fadePanel.color = panelColor;
        fadePanel.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutAction()
    {
        float elapsedTime = 0f;
        float fadeDuration = 1f;
        Color panelColor = fadePanel.color;
        panelColor.a = 0f;
        fadePanel.color = panelColor;

        fadePanel.gameObject.SetActive(true);
        while (elapsedTime <= fadeDuration)
        {
            panelColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelColor.a = 1f;
        fadePanel.color = panelColor;
    }

    public void SceneLoadWithFade(string sceneName)
    {
        StartCoroutine(SceneLoadAction(sceneName));
    }

    private IEnumerator SceneLoadAction(string sceneName)
    {
        yield return FadeOutAction();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
            yield return null;

        Debug.Log("씬 로드 성공");
        yield return FadeInAction();
    }
}