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
    protected Move actionMove;
    protected BattleSystem battleSystem;

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

        BattleSystem.Instance.AllHUDSetActivity(false);

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
        yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.digimonName}은 {move.moveBase.MoveName}을 사용했다.", 2f));
        
        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

        if (!battleSystem.CalculateAccuracy(move, defender))
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 맞지 않았다!", 2f));
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
        battleSystem.UpdateHUD();
        yield return new WaitForSeconds(1f);
        battleSystem.AllHUDSetActivity(false);

        if (multiplier >= 2)
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과는 굉장했다!", 2f));
        }

        if (isFainted)
        {
            defender.PlayFaintAnimation();
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 쓰러졌다.", 2f));
        }
    }

    private IEnumerator ExecuteHealMove(BattleEntity attacker, BattleEntity defender, Move move)
    {
        yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.digimonName}은 {move.moveBase.MoveName}을 사용했다.", 2f));
        
        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

        if (!CalculateAccuracy(move, defender))
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 맞지 않았다!", 2f));
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
        battleSystem.UpdateHUD();
        yield return new WaitForSeconds(1f);
        battleSystem.AllHUDSetActivity(false);
    }

    protected float GetBattleMultiplier(Move move, BattleEntity defender)
    {
        return ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.DigimonBase.ElementType1)
                * ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.DigimonBase.ElementType2);
    }

    protected int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float b = move.moveBase.MoveCategory == MoveCategory.Physical
            ? ((float)attacker.Digimon.Attack / defender.Digimon.Defense) 
            : ((float)attacker.Digimon.SpAttack / defender.Digimon.SpDefense);
            
        float d = a * move.moveBase.Power * b * multiplier + 2;

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

public class SwitchAction : BattleAction
{
    PlayerData player;
    int partyNum;

    public SwitchAction(PlayerData player, int partyNum)
    {
        this.player = player;
        this.partyNum = partyNum;
    }

    public override IEnumerator Action()
    {
        BattleSystem.Instance.AllHUDSetActivity(false);

        yield return SwitchLogic(player, partyNum);
    }

    public IEnumerator SwitchDigimonEntity(PlayerData player, int partyNum)
    {
        yield return SwitchLogic(player, partyNum);
    }

    private IEnumerator SwitchLogic(PlayerData player, int partyNum)
    {
        foreach (GameObject menu in BattleSystem.Instance.menus) menu.SetActive(false);
        BattleSystem.Instance.MenuReset();

        if (player.partyData.Digimons[partyNum].CurrentHP <= 0)
        {
            yield return BattleSystem.Instance.BattleText($"{player.partyData.Digimons[partyNum].digimonName}은 기절해있다!", 2f);
            yield break;
        }

        //바꾸는 연출

        yield return BattleSystem.Instance.BattleText($"{player.playerName}은 {player.partyData.Digimons[0].digimonName}을 후퇴시켰다.", 2f);

        //파티의 디지몬 정보를 먼저 교체
        Digimon stack = player.partyData.Digimons[partyNum];
        player.partyData.Digimons[partyNum] = player.partyData.Digimons[0];
        player.partyData.Digimons[0] = stack;

        BattleEntity target;

        //어떤 BattleEntity에서 처리할지 정함
        if (player == GameManager.Instance.playerData)
        {
            target = BattleSystem.Instance.PlayerBattleEntity;
        }
        else
        {
            target = BattleSystem.Instance.EnemyBattleEntity;
        }

        if (target == null) yield break;

        BattleSystem.Instance.DestroyModel(target);

        yield return new WaitForSeconds(2f);

        yield return BattleSystem.Instance.BattleText($"{player.playerName}은 {player.partyData.Digimons[0].digimonName}을 내보냈다!", 2f);

        Debug.Log($"{player.partyData.Digimons[0].digimonName}, {player.partyData.Digimons[0].Level}");
        target.SetDigimonData(player.partyData.Digimons[0]);
        target.SetUp();

        yield return new WaitForSeconds(2f);

        BattleSystem.Instance.SetupHUD();
        BattleSystem.Instance.MenuReset();

        BattleSystem.Instance.menus[0].gameObject.SetActive(true);
        
        BattleSystem.Instance.AllHUDSetActivity(true);
    }
}