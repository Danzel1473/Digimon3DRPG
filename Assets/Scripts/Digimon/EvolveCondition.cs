public enum EvConType
{
    Level,
    Friendship,
    Virus,
    Data,
    Vaccine,
    XAnityBody
}

[System.Serializable]
public class EvolveCondition
{
    public EvConType conditionType;
    public int Value;

    public bool CheckEvCon(Digimon digimon)
    {
        switch(conditionType)
        {
            case EvConType.Level:
                if(digimon.Level >= Value) return true;
                return false;
            case EvConType.Friendship:
                return false;
            case EvConType.XAnityBody:
                if(digimon.XAnityBody == true) return true;
                return false;
            default:
                return false;
        }
    }
}
