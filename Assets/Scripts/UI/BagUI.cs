using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] public GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private ItemDetail itemDetail;

    private ItemTab[] itemTabs;

    private ItemKind currentBagItemKind;
    public ItemKind CurrentBagItemKind => currentBagItemKind;
    public ItemDetail ItemDetail => itemDetail;

    public void Awake()
    {
        currentBagItemKind = ItemKind.Heal;
        itemTabs = GetComponentsInChildren<ItemTab>();
    }

    public void OnEnable()
    {
        UpdateBagItems();
    }

    public void OnDisable()
    {
        if(GameManager.Instance.popupMenu.gameObject.activeSelf)
        {
            GameManager.Instance.popupMenu.gameObject.SetActive(false);
        }
    }

    public void SwitchItemTab(ItemKind itemKind)
    {
        currentBagItemKind = itemKind;
        UpdateBagItems();
    }

    private void UpdateBagItems()
    {
        //기존 슬롯 제거
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        //플레이어 인벤토리 가져오기
        Inventory inventory = GameManager.Instance.playerData.Inventory;
        if(inventory == null) return;

        //각 아이템에 대해 ItemSlot 인스턴스 생성
        foreach (ItemInstance itemInstance in inventory.Items)
        {
            if(itemInstance.item.Kind != currentBagItemKind)
                continue;

            GameObject itemSlotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
            
            if(itemSlotObject.GetComponent<ItemSlot>() == null) return;
            itemSlotObject.GetComponent<ItemSlot>().SetItem(itemInstance);
        }
    }
}