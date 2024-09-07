using System;
using TMPro;
using UnityEngine;

public class TextPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text talkerName;
    [SerializeField] private TMP_Text text;

    public TMP_Text TalkerName => talkerName;
    public TMP_Text Text => text;

    public void SetText(string text)
    {
        this.text.text = text;
    }

        public void SetText(string talkerName, string text)
    {
        this.talkerName.text = talkerName;
        this.text.text = text;
    }
}