using UnityEngine;

public class BattleEntity : MonoBehaviour
{
    [SerializeField] private DigimonBase digimonBase;
    [SerializeField] private int level;
    [SerializeField] private bool isPlayerUnit;
    private Animator animator;

    public Digimon Digimon { get; private set; }

    public void SetUp()
    {
        if (digimonBase == null) return;
        Digimon = new Digimon(digimonBase, level);
        Instantiate(digimonBase.DigimonModel, transform);
    }
}