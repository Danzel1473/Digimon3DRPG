using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemDescription;
    [SerializeField] Image itemIcon;
    

    public void SetData(Item item)
    {
        itemName.text = item.Name;
        itemDescription.text = item.Description;
        itemIcon.sprite = item.Icon;
        itemIcon.gameObject.SetActive(true);
    }

    public void ClearData()
    {
        itemIcon.gameObject.SetActive(false);
        itemName.text = "";
        itemDescription.text = "";
        itemIcon.sprite = null;
    }
}