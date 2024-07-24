using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpponentData
{
    public List<Digimon> OpponentParty = new List<Digimon>();

    public void AddDigimonToParty(Digimon digimon)
    {
        if (OpponentParty.Count < 6) // Assuming max party size is 6
        {
            OpponentParty.Add(digimon);
        }
        else
        {
            //놓아줄지 박스로 보낼지 여부
        }
    }

    public void RemoveDigimonFromParty(Digimon digimon)
    {
        if (OpponentParty.Contains(digimon))
        {
            OpponentParty.Remove(digimon);
        }
    }
}