using System.Collections.Generic;
using UnityEngine;

public class OpenWorldUI : MonoBehaviour
{
    private static OpenWorldUI instance;
    public static OpenWorldUI Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] GameObject[] menus;
    [SerializeField] List<GameObject> menuHistory = new List<GameObject>();

    public void Awake()
    {
        instance = this;
    }
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            //오픈월드 메뉴창 오픈
            if(GameManager.Instance.state != GameManager.SituState.InUI)
            {
                GameManager.Instance.state = GameManager.SituState.InUI;
                menuHistory.Clear();
                
                foreach(GameObject menu in menus)
                {
                    menu.SetActive(false);
                }
                GameObject currentMenu = menus[(int)OWUIMenuBtnType.RootMenu];
                currentMenu.SetActive(true);
                menuHistory.Add(currentMenu);
            }
            else if(GameManager.Instance.state == GameManager.SituState.InUI && menuHistory.Count <= 1)
            {
                foreach(GameObject menu in menus)
                {
                    menu.SetActive(false);
                    GameManager.Instance.state = GameManager.SituState.OpenWorld;
                }
            }
            else
            {
                SwitchPrevMenu();
            }
        }
    }

    private void SwitchPrevMenu()
    {
        menuHistory[menuHistory.Count - 1].SetActive(false);
        menuHistory.RemoveAt(menuHistory.Count - 1);
    }

    public void SwitchMenu(OWUIMenuBtnType type)
    {
        GameObject currentMenu = menus[(int)type];

        currentMenu.SetActive(true);
        menuHistory.Add(currentMenu);
    }
}

public enum OWUIMenuBtnType
{
    RootMenu,
    DigimonMenu,
    BagMenu,
    SaveBtn,
    profileMenu
}