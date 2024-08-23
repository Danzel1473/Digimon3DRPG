using Unity.VisualScripting;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] public UnityEngine.GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private ItemDetail itemDetail;

    private ItemTab[] itemTabs;

    private GameManager gameManager;
    private ItemKind currentBagItemKind;
    public ItemKind CurrentBagItemKind => currentBagItemKind;
    public ItemDetail ItemDetail => itemDetail;

    public void Awake()
    {
        gameManager = UnityEngine.GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        currentBagItemKind = ItemKind.Heal;
        itemTabs = GetComponentsInChildren<ItemTab>();
    }

    public void OnEnable()
    {
        AddItemToInventoryForTest();
        UpdateBagItems();
    }

    private static void AddItemToInventoryForTest()
    {
        PlayerData playerData = UnityEngine.GameObject.FindWithTag("GameManager").GetComponentInChildren<PlayerData>();
        for(int i = 0; i < ItemTable.Instance.ItemTableLength; i++)
        {
            playerData.Inventory.AddItem(ItemTable.Instance[i], 1);
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
        Inventory inventory = gameManager.playerData.Inventory;
        if(inventory == null) return;

        //각 아이템에 대해 ItemSlot 인스턴스 생성
        foreach (ItemInstance itemInstance in inventory.Items)
        {
            if(itemInstance.item.Kind != currentBagItemKind)
                continue;

            UnityEngine.GameObject itemSlotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
            
            if(itemSlotObject.GetComponent<ItemSlot>() == null) return;
            itemSlotObject.GetComponent<ItemSlot>().SetItem(itemInstance);
        }
    }

}