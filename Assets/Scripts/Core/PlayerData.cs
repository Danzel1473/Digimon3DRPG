using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    //[SerializeField] private PartyData partyData;
    [SerializeField] public PartyData partyData;
    [SerializeField] private Inventory inventory;

    //public PartyData PartyData => partyData;
    public Inventory Inventory => inventory;

}