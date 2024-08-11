[System.Serializable]
public class ItemInstance
{
    public Item item;
    public int quantity;

    public ItemInstance(int itemID, int quantity)
    {
        item = ItemTable.Instance[itemID];
        this.quantity = quantity;
    }

    public void SetItem(ItemInstance itemInstance)
    {
        item = itemInstance.item;
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