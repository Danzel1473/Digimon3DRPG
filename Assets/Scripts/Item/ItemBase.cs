using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create New Item")]
public class ItemBase : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private itemTarget useTarget;

    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;

    public virtual void Use(BattleEntity target)
    {
        //사용 효과 정의
    }
}

enum itemTarget
{
    PlayerDigimon,
    EnemyDigimon,
    AllOfPlayerDigimons,
    None
}