using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class OWMenuButton : MonoBehaviour
{
    public OWUIMenuBtnType type;
    
    public void ActivateMenu()
    {
        OpenWorldUI.Instance.SwitchMenu(type);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }
}