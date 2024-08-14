using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ItemInstance iInst;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text quantity;
    [SerializeField] Image itemSprite;
    ItemDetail itemDetail;

    public void SetItem(ItemInstance item)
    {
        iInst = item;
        UpdateUI();
    }

    public void UpdateUI()
    {
        itemSprite.sprite = iInst.item.Icon;
        itemName.text = iInst.item.Name;
        quantity.text = $"×{iInst.quantity}";
    }
    
    public void OnItemClick(Item item)
    {
    }

    public void OnCancelButtonClick()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!itemDetail) itemDetail = GetComponentInParent<BagUI>().ItemDetail;

        if (itemDetail != null && iInst != null)
        {
            itemDetail.SetData(iInst.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!itemDetail) itemDetail = GetComponentInParent<BagUI>().ItemDetail;

        if (itemDetail != null)
        {
            itemDetail.ClearData();
        }
    }
}