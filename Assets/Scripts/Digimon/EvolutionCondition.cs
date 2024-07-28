public enum EvolutionConditionType
{
    Level,
    Friendship,
    TimeOfDay,
    Virus,
    Data,
    Vaccine,
    Area
}

[System.Serializable]
public class EvolutionCondition
{
    public EvolutionConditionType ConditionType;
    public int Value;
    public bool RequiresDayTime;
    public string RequiredArea;
}
