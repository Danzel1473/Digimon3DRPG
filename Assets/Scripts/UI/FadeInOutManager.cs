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

    public void SceneLoadWithFade(string sceneName, bool dataLoad = false)
    {
        StartCoroutine(SceneLoadAction(sceneName, dataLoad));
    }

    private IEnumerator SceneLoadAction(string sceneName, bool dataLoad = false)
    {
        yield return FadeOutAction();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
            yield return null;

        if(dataLoad)
        {
            SaveSystem.LoadGame();
        }

        yield return FadeInAction();
    }
}