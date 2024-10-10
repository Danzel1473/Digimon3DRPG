using System;
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
        yield return ActionLogic(user, target, actionMove);
    }

    private IEnumerator ActionLogic(BattleEntity attacker, BattleEntity defender, Move move)
    {
        yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.digimonName}은 {move.MoveBase.MoveName}을 사용했다.", 2f));
        
        yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

        if (!battleSystem.CalculateAccuracy(move, defender))
        {
            yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 맞지 않았다!", 2f));
            yield break;
        }
        MoveEffect moveEffect = move.MoveBase.MoveEffect;
        switch(moveEffect)
        {
            case MoveEffect.Deal:
                yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayDefenderDamageAnimation(defender));

                float multiplier = GetBattleMultiplier(move, defender);
                if (multiplier == 0)
                {
                    yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과가 없었다...", 2f));
                    yield break;
                }

                bool isFainted = TakeDamage(move, attacker, defender, multiplier);

                //HUD 업데이트
                yield return UpdateHUD(defender);

                if (multiplier >= 2)
                {
                    yield return battleSystem.StartCoroutine(battleSystem.BattleText("효과는 굉장했다!", 2f));
                }

                if (isFainted)
                {
                    defender.PlayFaintAnimation();
                    yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 쓰러졌다.", 2f));
                }
            break;

            case MoveEffect.Heal:
                yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{attacker.Digimon.digimonName}은 {move.MoveBase.MoveName}을 사용했다.", 2f));
            
                yield return battleSystem.StartCoroutine(BattleAnimationManager.Instance.PlayAttackAnimation(move, attacker));

                if (!CalculateAccuracy(move, defender))
                {
                    yield return battleSystem.StartCoroutine(battleSystem.BattleText($"{defender.Digimon.digimonName}은(는) 맞지 않았다!", 2f));
                    yield break;
                }

                TakeHeal(move, attacker, defender);

                yield return UpdateHUD(defender);
            break;
        }

    }

    private IEnumerator UpdateHUD(BattleEntity defender)
    {
        BattleHUD targetHUD = battleSystem.SetTargetHUD(defender);
        battleSystem.HUDSetActivity(targetHUD.gameObject, true);
        battleSystem.UpdateHUD();
        yield return new WaitForSeconds(1f);
        battleSystem.AllHUDSetActivity(false);
    }

    protected float GetBattleMultiplier(Move move, BattleEntity defender)
    {
        float multiplier = 1f;
        foreach(ElementType type in defender.Digimon.Types)
        {
            multiplier *= ElementChart.GetMultiplier(move.MoveBase.MoveType, type);
        }
        return multiplier;
    }

    protected int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float b = move.MoveBase.MoveCategory == MoveCategory.Physical
            ? ((float)attacker.Digimon.Attack / defender.Digimon.Defense) 
            : ((float)attacker.Digimon.SpAttack / defender.Digimon.SpDefense);
            
        float d = a * move.MoveBase.Power * b * multiplier + 2;
        Debug.Log(Mathf.FloorToInt(d * modifiers));
        return Mathf.FloorToInt(d * modifiers);
    }

    protected bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        int damage = CalculateDamage(move, attacker, defender, multiplier);
        defender.Digimon.TakeDamage(damage);

        Debug.Log(defender.Digimon.CurrentHP);
        return defender.Digimon.CurrentHP <= 0;
    }

    protected int CalculateHeal(Move move, BattleEntity attacker, BattleEntity defender)
    {
        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float d = a * move.MoveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) + 2;

        return Mathf.FloorToInt(d * modifiers);
    }

    protected bool TakeHeal(Move move, BattleEntity attacker, BattleEntity defender)
    {
        int heal = CalculateHeal(move, attacker, defender);
        defender.Digimon.HealDigimon(heal);
        return defender.Digimon.CurrentHP <= 0;
    }

    protected bool CalculateAccuracy(Move move, BattleEntity defender)
    {
        if (UnityEngine.Random.Range(0, 100) >= move.MoveBase.Accuracy)
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

public class ItemAction : BattleAction
{
    protected PlayerData player;
    protected Item item;
    protected int partyNum;

    public ItemAction(PlayerData player, BattleEntity target, Item item)
    {
        this.player = player;
        this.target = target;
        this.item = item;
    }

    public ItemAction(PlayerData player, int partyNum, Item item)
    {
        this.player = player;
        this.partyNum = partyNum;
        this.item = item;
    }

    public override IEnumerator Action()
    {
        if(target == null)
        {
            yield return BattleSystem.Instance.BattleText($"{player.playerName}은 {GameManager.Instance.playerData.partyData.Digimons[partyNum].digimonName}에게 {item.Name}을 사용했다.", 2f);
            
        }
        else
        {
            yield return BattleSystem.Instance.BattleText($"{player.playerName}은 {target.Digimon.digimonName}에게 {item.Name}을 사용했다.", 2f);
        }

        GameManager.Instance.playerData.Inventory.RemoveItem(item, 1);

        switch(item.Attrs[0].Kind)
        {
            case ItemAttributeKind.Heal:
                yield return HealItemAction((int)item.Attrs[0].Value);
                break;
            case ItemAttributeKind.Digicatch:
                yield return DigicatchItemAction(item.Attrs[0].Value);
                break;
        }
        
        yield return 0;
    }

    public IEnumerator HealItemAction(int amount)
    {
        GameManager.Instance.playerData.partyData.Digimons[partyNum].HealDigimon(amount);
        if(partyNum == 0)
        {
            target = BattleSystem.Instance.PlayerBattleEntity;
            BattleHUD targetHUD = BattleSystem.Instance.SetTargetHUD(target);
            BattleSystem.Instance.HUDSetActivity(targetHUD.gameObject, true);
            BattleSystem.Instance.UpdateHUD();
        }

        yield return new WaitForSeconds(1f);

        BattleSystem.Instance.AllHUDSetActivity(false);
    }
    
    public IEnumerator DigicatchItemAction(float modifier)
    {
        Debug.Log("DigicatchItemAction on");
        BattleHUD targetHUD = BattleSystem.Instance.SetTargetHUD(target);

        BattleSystem.Instance.HUDSetActivity(targetHUD.gameObject, true);
        BattleSystem.Instance.UpdateHUD();

        float catchChance = CalculateCatchChance(target.Digimon, modifier);
        float randomValue = UnityEngine.Random.Range(0f, 10f);

        yield return new WaitForSeconds(1f);

        if (randomValue <= catchChance)
        {
            BattleSystem.Instance.DestroyModel(target);
            yield return BattleSystem.Instance.BattleText($"{target.Digimon.digimonName}을 성공적으로 잡았다!", 2f);

            if(GameManager.Instance.playerData.partyData.Digimons.Count < 6)
                GameManager.Instance.playerData.partyData.AddDigimon(target.Digimon);

            yield return BattleSystem.Instance.BattleWin();
        }
        else
        {
            yield return BattleSystem.Instance.BattleText($"{target.Digimon.digimonName}을 잡는 데 실패했다.", 2f);
        }

        BattleSystem.Instance.AllHUDSetActivity(false);
    }

    private float CalculateCatchChance(Digimon targetDigimon, float modifier)
    {
        float maxHP = targetDigimon.MaxHP;
        float currentHP = targetDigimon.CurrentHP;
        float ballModifier = modifier;

        // 포획률 계산 (포켓몬의 공식을 참고)
        float catchRate = (3 * maxHP - 2 * currentHP) * ballModifier / (3 * maxHP);

        return Mathf.Clamp(catchRate * 100f, 1f, 100f);
    }
}

public class RunAction : BattleAction
{
    public override IEnumerator Action()
    {
        if(GameManager.Instance.enemyData.IsTamer)
        {
            yield return BattleSystem.Instance.BattleText($"{GameManager.Instance.playerData.playerName}은 도망칠 수 없다.", 2f);
            yield break;
        }

        yield return BattleSystem.Instance.BattleText($"{GameManager.Instance.playerData.playerName}은 도망쳤다.", 2f);
        BattleSystem.Instance.gameover = true;
        yield return GameManager.Instance.BattelExit();
    }
}