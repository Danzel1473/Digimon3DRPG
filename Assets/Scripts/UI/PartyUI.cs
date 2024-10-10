using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    [SerializeField] private DigimonSlot slotPrefab;
    [SerializeField] private List<DigimonSlot> slots;
    [SerializeField] private GameObject partyPanel;
    public PartyUIState state = PartyUIState.InBattle;

    public void SetState(PartyUIState state)
    {
        this.state = state;
    }

    public void OnEnable()
    {
        List<Digimon> digimons = GameManager.Instance.playerData.partyData.Digimons;

        foreach (Transform child in partyPanel.transform)
        {
            Destroy(child.gameObject);
            slots.Clear();
        }

        for(int i = 0; i < digimons.Count; i++)
        {
            DigimonSlot digimonSlot = Instantiate(slotPrefab, partyPanel.transform);
            digimonSlot.UpdateDigimon(i);
            slots.Add(digimonSlot);
        }
    }

    public void OnDisable()
    {
        if(GameManager.Instance.popupMenu.gameObject.activeSelf)
        {
            GameManager.Instance.popupMenu.gameObject.SetActive(false);
        }
    }
}

public enum PartyUIState
{
    InBattle,
    OpenWorld,
    ItemTarget
}