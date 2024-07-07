using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleEntity playerBattleEntity;
    [SerializeField] private BattleEntity enemyBattleEntity;

    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject moveMenu;
    [SerializeField] private List<Button> moveButtons;

    public GameObject currentMenu;

    private Move playerMove;
    private Move enemyMove;
    private bool isPlayerTurn;

    private void Start()
    {
        currentMenu = rootMenu;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerBattleEntity.SetUp();
        enemyBattleEntity.SetUp();
        playerHUD.SetData(playerBattleEntity.Digimon);
        enemyHUD.SetData(enemyBattleEntity.Digimon);

        playerHUD.SetMoveNames(playerBattleEntity.Digimon.Moves);
        yield return new WaitForSeconds(1f);

        PlayerTurn();
    }

    public void PlayerMove(Move move)
    {
        playerMove = move;
        isPlayerTurn = true;
        currentMenu.SetActive(false);
        StartCoroutine(PerformBattle());
    }

    private IEnumerator PerformBattle()
    {
        enemyMove = enemyBattleEntity.Digimon.Moves[Random.Range(0, enemyBattleEntity.Digimon.Moves.Count)];
        
        var battleQueue = GetBattleOrder(playerBattleEntity, enemyBattleEntity, playerMove, enemyMove);
        foreach (var (attacker, move, defender) in battleQueue)
        {
            yield return StartCoroutine(PerformMove(attacker, move, defender));
            yield return new WaitForSeconds(1f);
            if (defender.Digimon.HP <= 0) break;
        }

        if (playerBattleEntity.Digimon.HP > 0 && enemyBattleEntity.Digimon.HP > 0)
        {
            PlayerTurn();
        }
    }

    private IEnumerator PerformMove(BattleEntity attacker, Move move, BattleEntity defender)
    {
        yield return StartCoroutine(AttackAnimation(move, attacker));
        yield return new WaitForSeconds(1f);

        bool isFainted = TakeDamage(move, attacker, defender);
        UpdateHUD(defender);

        if (isFainted)
        {
            Debug.Log($"{defender.Digimon.digimonBase.DigimonName} 은(는) 쓰러졌다.");
        }
    }

    private IEnumerator AttackAnimation(Move move, BattleEntity attacker)
    {
        Animator animator = attacker.GetComponentInChildren<Animator>();

        animator.SetBool("isSPAttack", move.moveBase.IsSpecial);
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender)
    {
        int damage = CalculateDamage(move, attacker, defender);
        if (damage <= 0)
        {
            Debug.Log("Attack missed!");
            return false;
        }

        defender.Digimon.HP -= Mathf.Min(defender.Digimon.HP, damage);
        StartCoroutine(DefenderDamageAnimation(defender));

        return defender.Digimon.HP <= 0;
    }

    private int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender)
    {
        if (UnityEngine.Random.Range(0, 100) >= move.moveBase.Accuracy)
        {
            return 0; // Attack missed
        }

        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.level + 10) / 250f;
        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) + 2;
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
            if (enemy.Digimon.HP > 0) yield return (enemy, enemyMove, player);
        }
        else
        {
            yield return (enemy, enemyMove, player);
            if (player.Digimon.HP > 0) yield return (player, playerMove, enemy);
        }
    }

    private IEnumerator DefenderDamageAnimation(BattleEntity defender)
    {
        defender.GetComponentInChildren<Animator>().SetTrigger("damaged");
        yield return null;
    }

    private void PlayerTurn()
    {
        currentMenu.SetActive(true);
        rootMenu.SetActive(true);
        isPlayerTurn = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentMenu == rootMenu) return;

            currentMenu.SetActive(false);
            rootMenu.SetActive(true);
        }
    }
}