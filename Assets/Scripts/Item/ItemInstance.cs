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
}