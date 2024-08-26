using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    [SerializeField] private DigimonSlot slotPrefab;
    [SerializeField] private GameObject PartyPanel;
    public void OnEnable()
    {
        List<Digimon> digimons = GameManager.Instance.playerData.partyData.Digimons;

        foreach (Transform child in PartyPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Digimon digimon in digimons)
        {
            DigimonSlot digimonSlot = Instantiate(slotPrefab, PartyPanel.transform);
            digimonSlot.UpdateDigimon(digimon);
        }
    }
}