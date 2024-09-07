using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class SandboxManager : MonoBehaviour
{
    private static SandboxManager instance;
    public static SandboxManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SandboxManager>();
            }
            return instance;
        }
    }
    [SerializeField] private TextPanel textPanel;

    public IEnumerator GameText(string text)
    {
        textPanel.SetText(text);

        textPanel.gameObject.SetActive(true);
        yield return GameManager.Instance.WaitForKeyPress(KeyCode.Z);
        textPanel.gameObject.SetActive(false);
    }
    public IEnumerator GameText(string talkerName, string text)
    {
        textPanel.SetText(talkerName, text);

        textPanel.gameObject.SetActive(true);
        yield return GameManager.Instance.WaitForKeyPress(KeyCode.Z);
        textPanel.gameObject.SetActive(false);
    }

        public IEnumerator GameText(string text, float time)
    {
        textPanel.SetText(text);

        textPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        textPanel.gameObject.SetActive(false);
    }
    public IEnumerator GameText(string talkerName, string text, float time)
    {
        textPanel.SetText(talkerName, text);

        textPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        textPanel.gameObject.SetActive(false);
    }
}