using UnityEngine;

public class PartyUI : MonoBehaviour
{
    [SerializeField] private DigimonSlot[] DigimonSlot;
    [SerializeField] GameManager gameManager;

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // public void OnEnable()
    // {
    //     UpdateUI();
    // }

    public void UpdateUI()
    {
        for(int i = 0; i < gameManager.playerData.partyData.Digimons.Count; i++)
        {
            DigimonSlot[i].UpdateDigimon(gameManager.playerData.partyData.Digimons[i]);
        }
    }
}