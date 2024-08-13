using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text quantity;
    [SerializeField] Image itemSprite;

    public void UpdateUI(ItemInstance item)
    {
        itemSprite.sprite = item.item.Icon;
        itemName.text = item.item.Name;
        quantity.text = $"×{item.quantity}";
    }

    public void OnItemClick(Item item)
    {
    }

    public void OnCancelButtonClick()
    {
    }
}