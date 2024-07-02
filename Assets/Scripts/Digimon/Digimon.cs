using UnityEngine;

[CreateAssetMenu(fileName = "New Digimon", menuName = "Digimon")]
public class Digimon : ScriptableObject
{
    public string digimonName;
    public int level;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int defensePower;
    public int specialAttackPower;
    public int specialDefensePower;
    public int speed;
    public GameObject digimonPrefab;
    public RuntimeAnimatorController animatorController;
    public Skill[] skills;
}
