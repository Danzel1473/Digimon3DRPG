using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    [SerializeField] private List<Digimon> digimons;

    public List<Digimon> Digimons => digimons;

    public void AddDigimon(Digimon digimon)
    {
        if(digimons.Count <= 6)
        {
            digimons.Add(digimon);
        }
    }
}