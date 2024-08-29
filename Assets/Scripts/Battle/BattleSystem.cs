using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [Header ("Battle Entities")]
    [SerializeField] private BattleEntity playerBattleEntity;
    [SerializeField] private BattleEntity enemyBattleEntity;


    [Header ("UI & Environments")]
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject moveMenu;
    [SerializeField] private GameObject textPanel;

    [SerializeField] private BattleHUDManager hudManager;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private BattleField battleField;

    [Header ("ETC")]
    [SerializeField] private BattleAnimationManager battleAnimationManager;
    [SerializeField] public GameObject[] menus;
    private GameObject currentMenu;
    private List<GameObject> previousMenu = new List<GameObject>();

    private BattleAction playerAction;
    private BattleAction enemyAction;
    private PlayerData playerData;
    private PlayerData enemyData;

    public BattleEntity PlayerBattleEntity => playerBattleEntity;
    public BattleEntity EnemyBattleEntity => enemyBattleEntity;


    private static BattleSystem instance;
    public static BattleSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleSystem>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        playerData = GameManager.Instance.playerData;
        enemyData = GameManager.Instance.enemyData;
    }

    private void Start()
    {
        currentMenu = rootMenu;

        //테스트용 코드
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[1], 12));
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[3], 15));
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[1], 11));
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[1], 11));
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[1], 10));
        playerData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[1], 10));
        playerBattleEntity.SetDigimonData(playerData.partyData.Digimons[0]);

        enemyData.partyData.AddDigimon(new Digimon(DigimonTable.Instance[2], 8));
        enemyBattleEntity.SetDigimonData(enemyData.partyData.Digimons[0]);

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
        SetupHUD();
        yield return new WaitForSeconds(1f);
    }

    public void SetupHUD()
    {
        //서로의 디지몬의 HUD 세팅
        hudManager.UpdateHUD(playerBattleEntity, enemyBattleEntity);

        //Player 디지몬의 Move 목록을 버튼에 세팅
        hudManager.SetMoveButtonData(playerBattleEntity);
    }

    public void PlayerMove(Move move)
    {
        BattleEntity target = SetMoveTarget(playerBattleEntity, move);
        playerAction = new MoveAction(playerBattleEntity, target, move);

        StartCoroutine(PerformBattle());
    }


    private IEnumerator PerformBattle()
    {
        AllHUDSetActivity(false);

        List<BattleAction> actions = new List<BattleAction>();
        Move enemyMove = GetEnemyMove();
        BattleEntity target = SetMoveTarget(enemyBattleEntity, enemyMove);
        
        enemyAction = new MoveAction(enemyBattleEntity, target, enemyMove);
        if(playerAction is MoveAction && enemyAction is MoveAction)
        {
            int playerSpeed = playerBattleEntity.Digimon.Speed;
            int enemySpeed = enemyBattleEntity.Digimon.Speed;

            if (playerSpeed > enemySpeed || (playerSpeed == enemySpeed && Random.Range(0, 2) == 0))
            {
                actions.Add(playerAction);
                actions.Add(enemyAction);
            }
            else
            {
                actions.Add(enemyAction);
                actions.Add(playerAction);
            }
        }
        else
        {
            actions.Add(playerAction);
            actions.Add(enemyAction);
        }

        foreach(BattleAction ba in actions)
        {
            yield return ba.Action();
        }
        
        CheckGameOver();

        switch(battleField) //날씨 데미지는 스피드가 빠른 쪽부터 받음
        {
            case BattleField.SnowStorm:
                //얼음 타입 제외하고 데미지 주는 로직
                break;
            case BattleField.SandStorm:
                //바위 타입 제외하고 데미지 주는 로직
                break;
        }

        //턴 종료시에 상태에 따른 데미지 주는 로직 추가 예정
        
        yield return new WaitForSeconds(1f);

        if(playerBattleEntity.Digimon.CurrentHP == 0) {}

        TurnStart();
    }

    private Move GetEnemyMove()
    {
        return enemyBattleEntity.Digimon.Moves[Random.Range(0, enemyBattleEntity.Digimon.Moves.Count)];
    }

    public BattleHUD SetTargetHUD(BattleEntity defender)
    {
        if (defender == playerBattleEntity) return hudManager.PlayerHUD;
        else return hudManager.EnemyHUD;
    }

    public IEnumerator BattleText(string text)
    {
        uiText.text = text;

        textPanel.SetActive(true);
        yield return WaitForKeyPress(KeyCode.Z);
        textPanel.SetActive(false);
    }
    
    public IEnumerator BattleText(string text, float time)
    {
        uiText.text = text;

        textPanel.SetActive(true);
        yield return new WaitForSeconds(time);
        textPanel.SetActive(false);
    }

    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        while(!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }
    public bool CalculateAccuracy(Move move, BattleEntity defender)
    {
        if (Random.Range(0, 100) >= move.moveBase.Accuracy)
        {
            StartCoroutine(BattleText($"{defender.Digimon.Name}은 맞지 않았다.", 2f));
            return false;
        }
        return true;
    }
    public void UpdateHUD()
    {
        hudManager.UpdateHUD(playerBattleEntity, enemyBattleEntity);
    }

    private BattleEntity SetMoveTarget(BattleEntity attacker, Move playerMove)
    {
        //MoveTarget의 정보에 따라 BattleEntity를 Return
        switch (playerMove.moveBase.MoveTarget)
        {
            case MoveTarget.Enemy:
                if(attacker == playerBattleEntity) return enemyBattleEntity;
                else return playerBattleEntity;
            case MoveTarget.User:
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

    public void TurnStart()
    {
        currentMenu = rootMenu;
        playerData.partyData.Digimons[0] = playerBattleEntity.Digimon;
        AllHUDSetActivity(true);
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
        hudManager.SetActivityHUD(activity);
        currentMenu.SetActive(activity);
    }

    public void HUDSetActivity(GameObject hud, bool activity)
    {
        hud.SetActive(activity);
    }

    public void CheckGameOver()
    {
        bool playerDown = false;
        bool enemyDown = false;

        if(playerBattleEntity.Digimon.CurrentHP <= 0)
        {
            playerDown = true;
        }

        if(enemyBattleEntity.Digimon.CurrentHP <= 0)
        {
            enemyDown = true;
        }

        if(playerDown)
        {
            if(CheckPartyDown(playerData.partyData.Digimons))
            {
                StartCoroutine(BattleText($"{playerData.playerName}은 패배했다.", 2f));
            }
        }
        else if(enemyDown)
        {
            if(CheckPartyDown(enemyData.partyData.Digimons))
            {
                StartCoroutine(BattleText($"{playerData.playerName}은 승리했다!", 2f));
            }
        }
    }

    public bool CheckPartyDown(List<Digimon> digimons)
    {
        bool isAllDown = true;
        foreach (Digimon eDigimon in digimons)
        {
            if(eDigimon.CurrentHP > 0)
            {
                isAllDown = false;
                break;
            }
        }
        return isAllDown;
    }

    public void SwitchPerform(PlayerData player, int partyNum)
    {
        playerAction = new SwitchAction(player, partyNum);
        StartCoroutine(PerformBattle());
        //StartCoroutine(SwitchLogic(player, partyNum));
    }

    public void DestroyModel(BattleEntity player)
    {
        foreach (Transform child in player.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void MenuReset()
    {
        previousMenu.Clear();
        currentMenu = rootMenu;
    }
}