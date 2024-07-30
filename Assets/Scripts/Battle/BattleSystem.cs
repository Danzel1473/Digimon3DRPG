using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleEntity playerBattleEntity;
    [SerializeField] private BattleEntity enemyBattleEntity;
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject moveMenu;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerData EnemyData;

    [SerializeField] private BattleAnimationManager battleAnimationManager;

    [SerializeField] private DigimonBase pDigimonBaseSample; //테스트용
    [SerializeField] private DigimonBase eDigimonBaseSample; //테스트용


    private GameObject currentMenu;
    private List<GameObject> previousMenu;

    private Move playerMove;

    private void Start()
    {
        currentMenu = rootMenu;
        previousMenu = new List<GameObject>();

        //테스트용 코드
        playerData.partyData.AddDigimon(new Digimon(pDigimonBaseSample, 12));
        playerBattleEntity.SetDigimonData(playerData.partyData.Digimons[0]);

        EnemyData.partyData.AddDigimon(new Digimon(eDigimonBaseSample, 5));
        enemyBattleEntity.SetDigimonData(EnemyData.partyData.Digimons[0]);

        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            SwitchPreviousMenu();
        }
    }

    private IEnumerator SetupBattle()
    {
        playerBattleEntity.SetUp();
        enemyBattleEntity.SetUp();
        playerHUD.SetData(playerBattleEntity.Digimon);
        enemyHUD.SetData(enemyBattleEntity.Digimon);
        playerHUD.SetMoveNames(playerBattleEntity.Digimon.Moves);
        yield return new WaitForSeconds(1f);
        TurnStart();
    }

    public void PlayerMove(Move move)
    {
        playerMove = move;
        currentMenu.SetActive(false);
        StartCoroutine(PerformBattle());
    }

    private IEnumerator PerformBattle()
    {
        var enemyMove = GetEnemyMove();
        var battleQueue = GetBattleOrder(playerBattleEntity, enemyBattleEntity, playerMove, enemyMove);

        foreach (var (attacker, move, defender) in battleQueue)
        {
            yield return PerformMove(attacker, move, defender);
            if (defender.Digimon.CurrentHP <= 0) break;
        }

        yield return new WaitForSeconds(1f);
        TurnStart();
    }

    private Move GetEnemyMove()
    {
        return enemyBattleEntity.Digimon.Moves[Random.Range(0, enemyBattleEntity.Digimon.Moves.Count)];
    }

    private IEnumerator PerformMove(BattleEntity attacker, Move move, BattleEntity defender)
    {
        yield return StartCoroutine(battleAnimationManager.PlayAttackAnimation(move, attacker));

        yield return StartCoroutine(battleAnimationManager.PlayDefenderDamageAnimation(defender));

        bool isFainted = TakeDamage(move, attacker, defender);
        UpdateHUD(defender);

        if (isFainted)
        {
            defender.PlayFaintAnimation();
            Debug.Log($"{defender.Digimon.digimonBase.DigimonName} 은(는) 쓰러졌다.");
        }
    }

    public bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender)
    {
        int damage = CalculateDamage(move, attacker, defender);
        defender.Digimon.CurrentHP -= Mathf.Min(defender.Digimon.CurrentHP, damage);
        
        return defender.Digimon.CurrentHP <= 0;
    }

    private int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender)
    {
        if (Random.Range(0, 100) >= move.moveBase.Accuracy) return 0;

        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        float multiplier = ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType1)
        * ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType2);
        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) * multiplier + 2;

        return Mathf.FloorToInt(d * modifiers);
    }

    private void UpdateHUD(BattleEntity entity)
    {
        if (entity == playerBattleEntity)
        {
            playerHUD.SetData(playerBattleEntity.Digimon);
        }
        else
        {
            enemyHUD.SetData(enemyBattleEntity.Digimon);
        }
    }

    private IEnumerable<(BattleEntity, Move, BattleEntity)> GetBattleOrder(BattleEntity player, BattleEntity enemy, Move playerMove, Move enemyMove)
    {
        var playerSpeed = player.Digimon.Speed;
        var enemySpeed = enemy.Digimon.Speed;

        if (playerSpeed > enemySpeed || (playerSpeed == enemySpeed && Random.Range(0, 2) == 0))
        {
            yield return (player, playerMove, enemy);
            if (enemy.Digimon.CurrentHP > 0) yield return (enemy, enemyMove, player);
        }
        else
        {
            yield return (enemy, enemyMove, player);
            if (player.Digimon.CurrentHP > 0) yield return (player, playerMove, enemy);
        }
    }

    private void TurnStart()
    {
        rootMenu.SetActive(true);
        currentMenu = rootMenu;
    }

    public void SwitchMenu(GameObject menu)
    {
        currentMenu.SetActive(false);
        menu.SetActive(true);

        previousMenu.Add(currentMenu);
        currentMenu = menu;
    }

        public void SwitchPreviousMenu()
    {
        if(previousMenu.Count == 0) return;

        GameObject switchMenu = previousMenu[previousMenu.Count -1];

        currentMenu.SetActive(false);
        switchMenu.SetActive(true);

        currentMenu = switchMenu;
        previousMenu.RemoveAt(previousMenu.Count -1);
    }
}