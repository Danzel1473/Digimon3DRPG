using System.Collections.Generic;

public static class ElementChart
{
    private static readonly Dictionary<(ElementType, ElementType), float> chart = new Dictionary<(ElementType, ElementType), float>
    {
        {(ElementType.Fire, ElementType.Water), 0.5f},
        {(ElementType.Fire, ElementType.Grass), 2f},
        {(ElementType.Fire, ElementType.Ice), 2f},
        {(ElementType.Fire, ElementType.Steel), 2f},

        {(ElementType.Water, ElementType.Fire), 2f},
        {(ElementType.Water, ElementType.Grass), 0.5f},
        {(ElementType.Water, ElementType.Rock), 2f},
        {(ElementType.Water, ElementType.Steel), 0.5f},

        {(ElementType.Grass, ElementType.Water), 2f},
        {(ElementType.Grass, ElementType.Rock), 2f},
        {(ElementType.Grass, ElementType.Steel), 0.5f},

        {(ElementType.Electric, ElementType.Water), 2f},
        {(ElementType.Electric, ElementType.Wind), 2f},


    };

    public static float GetMultiplier(ElementType attackType, ElementType defenseType)
    {
        return chart.TryGetValue((attackType, defenseType), out float multiplier) ? multiplier : 1f;
    }
}