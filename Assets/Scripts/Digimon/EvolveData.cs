using System;

[Serializable]
public class EvolveData
{
    public int evolveDigimonNum;
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