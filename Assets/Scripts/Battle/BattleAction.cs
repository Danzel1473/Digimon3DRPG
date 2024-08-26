using System.Collections;
using UnityEngine;

public abstract class BattleAction
{
    protected BattleEntity user;
    protected BattleEntity target;

    public BattleEntity User => user;
    public BattleEntity Target => target;

    public abstract IEnumerator Action();
}

public class MoveAction : BattleAction
{
    private Move actionMove;
    private BattleSystem battleSystem;

    public Move ActionMove => actionMove;

    public MoveAction(BattleEntity user, BattleEntity target, Move move)
    {
        this.user = user;
        this.target = target;
        actionMove = move;
        battleSystem = GameObject.FindWithTag("BattleManager").GetComponent<BattleSystem>();
    }

    public override IEnumerator Action()
    {
        if (actionMove == null || user == null || target == null || battleSystem == null) yield break;
        if(user.Digimon.CurrentHP == 0) yield break;

        switch (actionMove.moveBase.MoveEffect)
        {
            case MoveEffect.Deal:
                yield return ExecuteDealingMove(user, target, actionMove);
                break;
            case MoveEffect.Heal:
                yield return ExecuteHealMove(user, target, actionMove);
                break;
        }
    }

    private IEnumerator ExecuteDealingMove(BattleEntity attacker, BattleEntity defender, Move move)
    {
        yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.Name}은 {move.moveBase.name}을 사용했다.", 2f));
        
        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

        if (!battleSystem.CalculateAccuracy(move, defender))
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.Name}은(는) 맞지 않았다!", 2f));
            yield break;
        }

        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayDefenderDamageAnimation(defender));

        float multiplier = GetBattleMultiplier(move, defender);
        if (multiplier == 0)
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과가 없었다...", 2f));
            yield break;
        }

        bool isFainted = TakeDamage(move, attacker, defender, multiplier);

        BattleHUD targetHUD = battleSystem.SetTargetHUD(defender);
        battleSystem.HUDSetActivity(targetHUD.gameObject, true);
        battleSystem.UpdateHUD(defender);
        yield return new WaitForSeconds(1f);
        battleSystem.AllHUDSetActivity(false);

        if (multiplier >= 2)
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과는 굉장했다!", 2f));
        }

        if (isFainted)
        {
            defender.PlayFaintAnimation();
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.Name}은(는) 쓰러졌다.", 2f));
        }
    }

    private IEnumerator ExecuteHealMove(BattleEntity attacker, BattleEntity defender, Move move)
    {
        yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.Name}은 {move.moveBase.name}을 사용했다.", 2f));
        
        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

        if (!CalculateAccuracy(move, defender))
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.Name}은(는) 맞지 않았다!", 2f));
            yield break;
        }

        float multiplier = GetBattleMultiplier(move, defender);
        if (multiplier == 0)
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과가 없었다...", 2f));
            yield break;
        }

        TakeHeal(move, attacker, defender);

        BattleHUD targetHUD = battleSystem.SetTargetHUD(defender);
        battleSystem.HUDSetActivity(targetHUD.gameObject, true);
        battleSystem.UpdateHUD(defender);
        yield return new WaitForSeconds(1f);
        battleSystem.AllHUDSetActivity(false);
    }

    protected float GetBattleMultiplier(Move move, BattleEntity defender)
    {
        return ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType1)
                * ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType2);
    }

    protected int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) * multiplier + 2;

        return Mathf.FloorToInt(d * modifiers);
    }

    protected bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        int damage = CalculateDamage(move, attacker, defender, multiplier);
        defender.Digimon.CurrentHP -= Mathf.Min(defender.Digimon.CurrentHP, damage);
        return defender.Digimon.CurrentHP <= 0;
    }

    protected int CalculateHeal(Move move, BattleEntity attacker, BattleEntity defender)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) + 2;

        return Mathf.FloorToInt(d * modifiers);
    }

    protected bool TakeHeal(Move move, BattleEntity attacker, BattleEntity defender)
    {
        int heal = CalculateHeal(move, attacker, defender);
        defender.Digimon.CurrentHP += Mathf.Min(defender.Digimon.MaxHP - defender.Digimon.CurrentHP, heal);
        return defender.Digimon.CurrentHP <= 0;
    }

    protected bool CalculateAccuracy(Move move, BattleEntity defender)
    {
        if (Random.Range(0, 100) >= move.moveBase.Accuracy)
        {
            return false;
        }
        return true;
    }
}