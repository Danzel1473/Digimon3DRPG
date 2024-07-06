using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleEntity playerBattleEntity;
    [SerializeField] BattleEntity enemyBattleEntity;

    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] GameObject rootMenu;
    [SerializeField] GameObject moveMenu;
    [SerializeField] List<Button> moveButtons;

    public GameObject currentMenu;

    int currentAction;
    Move currentMove;

    Button currentButton;


    private void Start()
    {
        currentMenu = rootMenu;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerBattleEntity.SetUp();
        enemyBattleEntity.SetUp();
        playerHUD.SetData(playerBattleEntity.digimon);
        enemyHUD.SetData(enemyBattleEntity.digimon);

        playerHUD.SetMoveNames(playerBattleEntity.digimon.Moves);
        yield return new WaitForSeconds(1f);
    }

    public void PlayerMove(Move move)
    {
        currentMove = move;
        StartCoroutine(PerformPlayerMove());
    }

    IEnumerator PerformPlayerMove()
    {
        yield return AttackAnimation(currentMove, playerBattleEntity);
        //AfterAttackTiming();
    }

    public void AfterAttackTiming()
    {
        bool isFainted = TakeDamage(currentMove, playerBattleEntity, enemyBattleEntity);
        playerHUD.SetData(playerBattleEntity.digimon);
        enemyHUD.SetData(enemyBattleEntity.digimon);


        if (isFainted)
        {
            print(enemyBattleEntity.digimon.digimonBase.DigimonName + "은(는) 쓰러졌다.");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator AttackAnimation(Move move, BattleEntity Attacker)
    {
        if (move.moveBase.IsSpecial)
        {
            Attacker.GetComponentInChildren<Animator>().SetBool("isSPAttack", true);
        }
        else
        {
            Attacker.GetComponentInChildren<Animator>().SetBool("isSPAttack", false);
        }
        Attacker.GetComponentInChildren<Animator>().SetTrigger("attack");

        yield return null;
    }

    public bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender)
    {
        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.digimon.level + 10) / 250f;
        float d = a * move.moveBase.Power * ((float)attacker.digimon.Attack / defender.digimon.Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        int randomValue = UnityEngine.Random.Range(0, 100);
        if (move.moveBase.Accuracy < randomValue)
        {
            print("명중률: " + move.moveBase.Accuracy + " 랜덤밸류: " + randomValue + "감나빗");
            return false;
        }

        defender.digimon.HP -= Mathf.Min(defender.digimon.HP, damage);
        StartCoroutine(EnemyDamagedAnimation(defender));

        if (defender.digimon.HP <= 0)
        {
            print("Dead");
            return true;
        }
        else
        {
            print("Alive");
            return false;
        }
    }

    IEnumerator EnemyDamagedAnimation(BattleEntity defender)
    {
        defender.GetComponentInChildren<Animator>().SetTrigger("damaged");
        yield return null;
    }

    IEnumerator EnemyMove()
    {
        yield return 0;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(currentMenu == rootMenu) return;
            
            currentMenu.SetActive(false);
            rootMenu.SetActive(true);
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentAction < 1) ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentAction > 0) --currentAction;
        }
    }
}