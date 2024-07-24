using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleEntity : MonoBehaviour
{
    [SerializeField] private DigimonBase digimonBase;
    [SerializeField] private int level;
    [SerializeField] public bool IsPlayer { get; set; }

    public Digimon Digimon { get; private set; }
    private BattleAnimation battleAnimation;

    public void SetUp()
    {
        if (digimonBase == null) return;
        Digimon = new Digimon(digimonBase, level);
        var digimonModel = Instantiate(digimonBase.DigimonModel, transform);
        battleAnimation = digimonModel.GetComponentInChildren<BattleAnimation>();
    }

    public void SetDigimonData(Digimon digimon)
    {
        digimonBase = digimon.digimonBase;
        level = digimon.Level;
    }

    public void PlayAttackAnimation(bool isSPAttack)
    {
        if (battleAnimation != null)
        {
            battleAnimation.PlayAttackAnimation(isSPAttack);
        }
    }

    public void PlayDamageAnimation()
    {
        if (battleAnimation != null)
        {
            battleAnimation.PlayDamageAnimation();
        }
    }

    public void PlayFaintAnimation()
    {
        if (battleAnimation != null)
        {
            battleAnimation.PlayFaintAnimation();
        }
    }

    public void PlayMoveParticle(MoveBase moveBase, Transform target)
    {
        if (battleAnimation != null && moveBase.ParticlePrefab != null)
        {
            battleAnimation.PlayMoveParticle(moveBase.ParticlePrefab, target);
        }
    }
}