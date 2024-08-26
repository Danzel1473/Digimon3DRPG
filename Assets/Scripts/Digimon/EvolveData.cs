using System.Collections.Generic;

[System.Serializable]
public class EvolveData
{
    public DigimonBase evolveTo;
    public EvolveCondition[] conditions;

    public bool canEvole(Digimon digimon)
    {
        foreach(EvolveCondition evCon in conditions)
        {
            if(!evCon.CheckEvCon(digimon))
            {
                return false;
            }
        }
        return true;
    }
}