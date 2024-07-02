using UnityEngine;

public enum SkillType
{
    Physical,
    Special
}

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public int power;
    public int accuracy;
    public int powerPoint; //기술의 사용 가능 횟수
    public SkillType skillType;
    public Sprite skillSprite;
}