using UnityEngine;


[System.Serializable]
public class PlayerData
{
    [SerializeField] public string playerName;
    [SerializeField] public PartyData partyData;
    [SerializeField] private Inventory inventory;

    public Inventory Inventory => inventory;
}