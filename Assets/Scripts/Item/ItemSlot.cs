[System.Serializable]
public class ItemSlot
{
    public ItemBase Item;
    public int Quantity;

    public ItemSlot(ItemBase item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    public void IncreaseQuantity()
    {
        Quantity++;
    }

    public void DecreaseQuantity()
    {
        Quantity--;
    }
}