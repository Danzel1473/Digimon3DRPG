using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] public GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    private GameManager gameManager;
    private PartyUI partyUI;

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        partyUI = GetComponentInChildren<PartyUI>();
    }

    public void OnEnable()
    {
        AddItemToInventoryForTest();
        PopulateBag();
        partyUI.UpdateUI();
    }

    private static void AddItemToInventoryForTest()
    {
        PlayerData playerData = GameObject.FindWithTag("GameManager").GetComponentInChildren<PlayerData>();
        for(int i = 0; i < ItemTable.Instance.ItemTableLength; i++)
        {
            playerData.Inventory.AddItem(ItemTable.Instance[i], 1);
        }
    }

    private void PopulateBag()
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
            GameObject itemSlotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
            
            if(itemSlotObject.GetComponent<ItemSlot>() == null) return;
            itemSlotObject.GetComponent<ItemSlot>().UpdateUI(itemInstance);
        }
    }
}