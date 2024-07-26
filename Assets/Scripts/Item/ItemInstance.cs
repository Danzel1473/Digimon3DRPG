[System.Serializable]
public class ItemInstance
{
    public ItemBase itemBase;
    public int quantity;

    public ItemInstance(ItemBase itemBase, int quantity)
    {
        this.itemBase = itemBase;
        this.quantity = quantity;
    }

    public void SetItem(ItemInstance itemInstance)
    {
        itemBase = itemInstance.itemBase;
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