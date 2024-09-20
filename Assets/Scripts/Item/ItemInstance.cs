[System.Serializable]
public class ItemInstance
{
    public int itemId;
    public Item item => ItemTable.Instance[itemId];
    public int quantity;

    public ItemInstance(int itemID, int quantity)
    {
        itemId = itemID;
        this.quantity = quantity;
    }

    public void SetItem(ItemInstance itemInstance)
    {
        itemId = itemInstance.itemId;
        quantity = itemInstance.quantity;
    }
    
    public void IncreaseQuantity()
    {
        quantity++;
    }

    public void DecreaseQuantity()
    {
        quantity--;
    }
}