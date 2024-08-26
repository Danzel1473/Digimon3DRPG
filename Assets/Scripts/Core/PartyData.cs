using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    [SerializeField] private List<Digimon> digimons;

    public List<Digimon> Digimons => digimons;

    public void AddDigimon(Digimon digimon)
    {
        if(digimons.Count >= 6) return; //박스로 보내거나, 파티 디지몬을 박스로 보내고 새 디지몬을 파티에 넣는 로직

        digimons.Add(digimon);
    }
}