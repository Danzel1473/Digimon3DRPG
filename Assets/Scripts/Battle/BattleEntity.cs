using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleEntity : MonoBehaviour
{
    [SerializeField] public bool IsPlayer { get; set; }
    public bool canAttack = true;
    public bool isDefeated = false;

    public bool CanAttack => canAttack;
    public bool IsDefeated => isDefeated;

    public Digimon Digimon { get; private set; }
    private BattleAnimation battleAnimation;

    public void SetUp()
    {
        if (Digimon == null) return;

        var digimonModel = Instantiate(Digimon.digimonBase.DigimonModel, transform);
        battleAnimation = digimonModel.GetComponentInChildren<BattleAnimation>();
    }

    public void SetDigimonData(Digimon digimon)
    {
        Digimon = digimon;
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