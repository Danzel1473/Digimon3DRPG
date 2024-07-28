using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create New Item")]
public class ItemBase : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemTarget useTarget;
    [SerializeField] private ItemCategory category;
    [SerializeField] private ItemEffect effect;


    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;

    public virtual void Use(BattleEntity target)
    {
        effect.Effect(target);
    }
}

enum ItemTarget
{
    PlayerDigimon,
    EnemyDigimon,
    AllOfPlayerDigimons,
    None
}

enum ItemCategory
{
    Healing,
    Battle,
    Etc
}