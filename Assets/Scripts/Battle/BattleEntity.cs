using UnityEngine;

public class BattleEntity : MonoBehaviour
{
    [SerializeField] DigimonBase digimonBase;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    Animator animator;

    public Digimon digimon;

    public void SetUp()
    {
        if(digimonBase == null) return;
        digimon = new Digimon(digimonBase, level);
        Instantiate(digimonBase.DigimonModel, transform);
    }
}