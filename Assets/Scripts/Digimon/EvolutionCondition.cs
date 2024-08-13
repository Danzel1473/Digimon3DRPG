public enum EvolutionConditionType
{
    Level,
    Friendship,
    Virus,
    Data,
    Vaccine,
    Area
}

[System.Serializable]
public class EvolutionCondition
{
    public EvolutionConditionType ConditionType;
    public string RequiredArea;
    public int Value;
}
