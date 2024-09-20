using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour
{
    [SerializeField]
    private PopupButton buttonPrefab;
    private List<PopupButton> buttons = new List<PopupButton>();
    public List<PopupButton> Buttons => buttons;
    public int num;
    
    public void SetMenu(int num, List<PopupButtonType> types)
    {
        ClearMenu();
        this.num = num;

        foreach(PopupButtonType type in types)
        {
            buttons.Add(Instantiate(buttonPrefab, transform));
            buttons[buttons.Count-1].SetType(type);
        }
    }

        public void SetMenu(List<PopupButtonType> types)
    {
        ClearMenu();
        num = 0;

        foreach(PopupButtonType type in types)
        {
            buttons.Add(Instantiate(buttonPrefab, transform));
            buttons[buttons.Count-1].SetType(type);
        }
    }

    public void ClearMenu()
    {
        for(int i = transform.childCount -1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        buttons.Clear();
    }

    public void ShowMenuAtPosition(Vector3 position)
    {
        transform.position = position;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 adjustment = Vector3.zero;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (corners[2].x > screenWidth)
            adjustment.x = screenWidth - corners[2].x;

        if (corners[0].x < 0)
            adjustment.x = -corners[0].x;

        if (corners[2].y > screenHeight)
            adjustment.y = screenHeight - corners[2].y;

        if (corners[0].y < 0)
            adjustment.y = -corners[0].y;

        transform.position += adjustment;

        gameObject.SetActive(true);
    }
}