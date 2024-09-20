using TMPro;
using UnityEngine;

public class InterectionUI : MonoBehaviour
{
    private static InterectionUI instance;
    public static InterectionUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InterectionUI>();
            }
            return instance;
        }
    }
    [SerializeField] private TMP_Text interectionText;

    public void UpdateTextPosition(Transform target)
    {
        if (target == null || GameManager.Instance.state != GameManager.SituState.OpenWorld)
        {
            DisableText();
            return;
        }

        interectionText.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        interectionText.gameObject.SetActive(false);
    }
}