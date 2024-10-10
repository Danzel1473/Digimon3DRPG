using UnityEngine;

public class OWMenuButton : MonoBehaviour
{
    public OWUIMenuBtnType type;
    
    public void ActivateMenu()
    {
        OpenWorldUI.Instance.SwitchMenu(type);
    }
}