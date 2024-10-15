using UnityEngine;


[System.Serializable]
public class PlayerData
{
    [SerializeField] public string playerName;
    [SerializeField] public PartyData partyData = new PartyData();
    [SerializeField] private Inventory inventory;
    [SerializeField] private bool isTamer;
    public Inventory Inventory => inventory;
    public bool IsTamer => isTamer;

    public PlayerData(Digimon digimon)
    {
        playerName = digimon.digimonName;
        partyData.AddDigimon(digimon);
        isTamer = false;
    }
}