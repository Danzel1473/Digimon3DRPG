using System.Collections;
using UnityEngine;
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

    [SerializeField] Image fadePanel;

    public void FadeIn()
    {
        StartCoroutine(FadeInAction());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAction());
    }

    public IEnumerator FadeInAction()
    {
        float elapsedTime = 0f;
        float fadedTime = 1f;

        fadePanel.gameObject.SetActive(true);
        while(elapsedTime <= fadedTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime/fadedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

        public IEnumerator FadeOutAction()
    {
        float elapsedTime = 0f;
        float fadedTime = 1f;

        while(elapsedTime <= fadedTime)
        {
            fadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime/fadedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadePanel.gameObject.SetActive(false);
        yield break;
    }
}