using UnityEngine;

public class MoveMenuButtonManager : MonoBehaviour
{
    [SerializeField] GameObject DisableMenu;
    [SerializeField] GameObject activeMenu;


    public void PerformAction()
    {
        DisableMenu.SetActive(false);
        activeMenu.SetActive(true);
    }
}