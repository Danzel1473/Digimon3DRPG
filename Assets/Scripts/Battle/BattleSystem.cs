using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private PlayerData enemyData;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TMP_Text uiText;

    [SerializeField] private BattleAnimationManager battleAnimationManager;

    [SerializeField] private DigimonBase pDigimonBaseSample; //테스트용 플레이어 디지몬
    [SerializeField] private DigimonBase eDigimonBaseSample; //테스트용 상대 디지몬


    private GameObject currentMenu;
    private List<GameObject> previousMenu = new List<GameObject>();

    private Move playerMove;

    private void Start()
    {
        currentMenu = rootMenu;

        //테스트용 코드
        playerData.partyData.AddDigimon(new Digimon(pDigimonBaseSample, 12));
        playerBattleEntity.SetDigimonData(playerData.partyData.Digimons[0]);

        enemyData.partyData.AddDigimon(new Digimon(eDigimonBaseSample, 5));
        enemyBattleEntity.SetDigimonData(enemyData.partyData.Digimons[0]);

        Debug.Log(ItemTable.Instance.GetItem(0).Name);

        StartCoroutine(SetupBattle());
        TurnStart();
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
        //서로의 디지몬 세팅
        playerBattleEntity.SetUp();
        enemyBattleEntity.SetUp();

        //서로의 디지몬의 HUD 세팅
        playerHUD.SetData(playerBattleEntity.Digimon);
        enemyHUD.SetData(enemyBattleEntity.Digimon);

        //Player 디지몬의 Move 목록을 버튼에 세팅
        playerHUD.SetMoveNames(playerBattleEntity.Digimon.Moves);
        yield return new WaitForSeconds(1f);
    }

    public void PlayerMove(Move move)
    {
        playerMove = move;
        AllHUDSetActivity(false);
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

        yield return StartCoroutine(BattleText($"{attacker.Digimon.digimonBase.name}은 {move.moveBase.name}을 사용했다."));

        yield return StartCoroutine(battleAnimationManager.PlayAttackAnimation(move, attacker));

        yield return StartCoroutine(battleAnimationManager.PlayDefenderDamageAnimation(defender));

        if (CalculateAccuracy(move, defender))
        {
            float multiplier = GetBattleMultiplier(move, defender);
            bool isFainted = TakeDamage(move, attacker, defender, multiplier);

            //HUD를 활성화하고 업데이트, 1초후에 비활성화한다.
            BattleHUD targetHUD = SetTargetHUD(defender);

            HUDSetActivity(targetHUD.gameObject, true);
            UpdateHUD(defender);
            yield return new WaitForSeconds(1f);
            AllHUDSetActivity(false);

            yield return new WaitForSeconds(0.5f);

            //기술 상성 체크 후 텍스트 출력
            if (multiplier >= 2)
            {
                yield return StartCoroutine(BattleText("효과는 굉장했다!"));
                yield return new WaitForSeconds(1f);
            }
            else if (multiplier == 0)
            {
                yield return StartCoroutine(BattleText("효과가 없었다..."));
                yield return new WaitForSeconds(1f);
            }
            
            if (isFainted)
            {
                defender.PlayFaintAnimation();
                yield return StartCoroutine(BattleText($"{defender.Digimon.digimonBase.DigimonName}은(는) 쓰러졌다."));
            }
        }

        else
        {
            yield return StartCoroutine(BattleText($"{defender.Digimon.digimonBase.DigimonName}은(는) 맞지 않았다!"));
            yield return new WaitForSeconds(1f);
        }
        
    }

    private BattleHUD SetTargetHUD(BattleEntity defender)
    {
        if (defender == playerBattleEntity) return playerHUD;
        else return enemyHUD;
    }

    private IEnumerator BattleText(string text)
    {
        uiText.text = text;

        textPanel.SetActive(true);
        yield return WaitForKeyPress(KeyCode.Z);
        textPanel.SetActive(false);
    }

    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        while(!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }

    public bool TakeDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        int damage = CalculateDamage(move, attacker, defender, multiplier);
        defender.Digimon.CurrentHP -= Mathf.Min(defender.Digimon.CurrentHP, damage);
        
        return defender.Digimon.CurrentHP <= 0;
    }

    private int CalculateDamage(Move move, BattleEntity attacker, BattleEntity defender, float multiplier)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;

        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) * multiplier + 2;

        return Mathf.FloorToInt(d * modifiers);
    }

    private bool CalculateAccuracy(Move move, BattleEntity defender)
    {
        if (Random.Range(0, 100) >= move.moveBase.Accuracy)
        {
            StartCoroutine(BattleText($"{defender.Digimon.digimonBase.name}은 맞지 않았다."));
            return false;
        }
        return true;
    }

    private static float GetBattleMultiplier(Move move, BattleEntity defender)
    {
        return ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType1)
                * ElementChart.GetMultiplier(move.moveBase.MoveType, defender.Digimon.digimonBase.ElementType2);
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
        BattleEntity moveTarget;

        if (playerSpeed > enemySpeed || (playerSpeed == enemySpeed && Random.Range(0, 2) == 0))
        {
            moveTarget = SetMoveTarget(player, playerMove);
            yield return (player, playerMove, moveTarget);
            if (enemy.Digimon.CurrentHP > 0) yield return (enemy, enemyMove, player);
        }
        else
        {
            moveTarget = SetMoveTarget(enemy, enemyMove);
            yield return (enemy, enemyMove, moveTarget);
            if (player.Digimon.CurrentHP > 0) yield return (player, playerMove, enemy);
        }
    }

    private BattleEntity SetMoveTarget(BattleEntity attacker, Move playerMove)
    {
        //MoveTarget의 정보에 따라 BattleEntity를 Return
        switch (playerMove.moveBase.MoveTarget)
        {
            case MoveTarget.Enemy:
                if(attacker == playerBattleEntity) return enemyBattleEntity;
                else return playerBattleEntity;
            case MoveTarget.Player:
                if(attacker == playerBattleEntity) return playerBattleEntity;
                else return enemyBattleEntity;
            case MoveTarget.Any:
                //TO DO:대상을 정하는 로직 구현
                break;
            default:
                return playerBattleEntity;
        }
        return null;
    }

    private void TurnStart()
    {
        currentMenu = rootMenu;
        AllHUDSetActivity(true);
        Debug.Log("Turn Start");
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

        //이전 메뉴
        GameObject switchMenu = previousMenu[previousMenu.Count -1];

        //현재 메뉴를 비활성화
        currentMenu.SetActive(false);
        //이전 메뉴를 활성화
        switchMenu.SetActive(true);

        currentMenu = switchMenu;
        previousMenu.RemoveAt(previousMenu.Count -1);
    }

    public void AllHUDSetActivity(bool activity)
    {
        playerHUD.gameObject.SetActive(activity);
        enemyHUD.gameObject.SetActive(activity);
        currentMenu.SetActive(activity);
    }

    public void HUDSetActivity(GameObject hud, bool activity)
    {
        hud.SetActive(activity);
    }
}