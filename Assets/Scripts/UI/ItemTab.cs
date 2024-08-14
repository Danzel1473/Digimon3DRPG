using UnityEngine;

public enum ItemKind
{
    Heal,
    Digicatch,
    Important,
    Etc
}

public class ItemTab : MonoBehaviour
{
    [SerializeField] ItemKind itemKind;
    BagUI bagUI;

    public void Awake()
    {
        bagUI = GetComponentInParent<BagUI>();
    }

    public void onClick()
    {
        if(bagUI == null) return;
        if(bagUI.CurrentBagItemKind == itemKind) return;
        
        bagUI.SwitchItemTab(itemKind);
    }
}