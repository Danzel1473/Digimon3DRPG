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
    [SerializeField] private PlayerData EnemyData;
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
        //서로의 디지몬 세팅
        playerBattleEntity.SetUp();
        enemyBattleEntity.SetUp();

        //서로의 디지몬의 HUD 세팅
        playerHUD.SetData(playerBattleEntity.Digimon);
        enemyHUD.SetData(enemyBattleEntity.Digimon);

        //Player 디지몬의 Move 목록을 버튼에 세팅
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
        AllHUDSetActivity(false);
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

        float multiplier = GetBattleMultiplier(move, defender);
        bool isFainted = TakeDamage(move, attacker, defender, multiplier);
        UpdateHUD(defender);

        yield return new WaitForSeconds(2f);

        if (multiplier >= 2)
        {
            string text = "효과는 굉장했다!";
            yield return StartCoroutine(BattleText(text));
        }

        if (isFainted)
        {
            defender.PlayFaintAnimation();
            string text = $"{defender.Digimon.digimonBase.DigimonName} 은(는) 쓰러졌다.";
            yield return StartCoroutine(BattleText(text));
        }
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
        if (Random.Range(0, 100) >= move.moveBase.Accuracy)
        {
            StartCoroutine(BattleText($"{defender.Digimon.digimonBase.name}은 맞지 않았다."));
            return 0;
        }

        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Digimon.Level + 10) / 250f;
        
        float d = a * move.moveBase.Power * ((float)attacker.Digimon.Attack / defender.Digimon.Defense) * multiplier + 2;

        return Mathf.FloorToInt(d * modifiers);
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
            HUDSetActivity(playerHUD.gameObject, true);
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

        //이전 메뉴
        GameObject switchMenu = previousMenu[previousMenu.Count -1];

        //현재 메뉴를 비활성화
        currentMenu.SetActive(false);
        //이전 메뉴를 활성화
        switchMenu.SetActive(true);

        currentMenu = switchMenu;
        previousMenu.RemoveAt(previousMenu.Count -1);
    }

    public void ToggleUI(GameObject ui)
    {
        ui.SetActive(!ui.activeSelf);
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