using System.Collections.Generic;

public static class ElementChart
{
    private static readonly Dictionary<(ElementType, ElementType), float> chart = new Dictionary<(ElementType, ElementType), float>
    {
        {(ElementType.Fire, ElementType.Grass), 2f}
    };

    public static float GetMultiplier(ElementType attackType, ElementType defenseType)
    {
        return chart.TryGetValue((attackType, defenseType), out float multiplier) ? multiplier : 1f;
    }
}