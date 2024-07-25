using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] public GameObject itemSlotPrefab;
    [SerializeField] private ItemInstance itemSlot;
    [SerializeField] private Transform content;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void OnEnable()
    {
        Inventory inventory = gameManager.GetComponentInChildren<PlayerData>().Inventory;
        foreach(ItemInstance item in inventory.Items)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, content);
            itemSlot.
        }
    }

    public void Update()
    {
        

    }
}