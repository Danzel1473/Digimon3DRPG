using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text quantity;
    [SerializeField] Image itemSprite;

    public void Awake()
    {

    }

    public void UpdateUI(ItemInstance item)
    {
        itemSprite.sprite = item.itemBase.Icon;
        itemName.text = item.itemBase.ItemName;
        quantity.text = $"×{item.quantity}";
    }
}