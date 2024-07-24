using UnityEngine;

public class PartyUI : MonoBehaviour
{
    [SerializeField] private DigimonSlot[] DigimonSlot;
    [SerializeField] GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        for(int i = 0; i < gameManager.playerData.partyData.Digimons.Count; i++)
        {
            DigimonSlot[i].SetDigimon(gameManager.playerData.partyData.Digimons[i]);
        }
    }
}