using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenWorldUI : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    GameObject currentMenu;
    List<GameObject> menuHistory = new List<GameObject>();
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(GameManager.Instance.state != GameManager.SituState.InUI)
            {
                GameManager.Instance.state = GameManager.SituState.InUI;
                menuHistory.Clear();

                foreach(GameObject menu in menus)
                {
                    menu.SetActive(false);
                }

                menus[0].SetActive(true);
                currentMenu= menus[0];
                menuHistory.Add(currentMenu);
            }
            else if(GameManager.Instance.state == GameManager.SituState.InUI && menuHistory.Count -1 < 1)
            {
                foreach(GameObject menu in menus)
                {
                    menu.SetActive(false);
                    GameManager.Instance.state = GameManager.SituState.OpenWorld;
                }
            }
        }
    }
}