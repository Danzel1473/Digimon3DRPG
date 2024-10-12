using System.IO;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] GameObject continueBtn;

    public void Start()
    {
        string savePath = Application.persistentDataPath + "/save.json";
        continueBtn.SetActive(File.Exists(savePath));
    }
}
