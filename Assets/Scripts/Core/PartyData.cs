using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    [SerializeField] private List<Digimon> digimons = new List<Digimon>();

    public List<Digimon> Digimons => digimons;

    public void AddDigimon(Digimon digimon)
    {
        if(digimons.Count >= 6) return;

        digimons.Add(digimon);
    }

    public void DeleteDigimon(Digimon digimon)
    {
        digimons.Remove(digimon);
    }

    public Digimon this[int num] => digimons[num];
}