using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class BattleSystem : MonoBehaviour
{
    [Header("Battle Entities")]
    [SerializeField] private BattleEntity playerBattleEntity;
    [SerializeField] private BattleEntity enemyBattleEntity;


    [Header("UI & Environments")]
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject moveMenu;
    [SerializeField] private GameObject textPanel;

    [SerializeField] private BattleHUDManager hudManager;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private BattleField battleField;

    [Header("ETC")]
    [SerializeField] private BattleAnimationManager battleAnimationManager;
    [SerializeField] public GameObject[] menus;
    public Item itemWaitForUse;
    private GameObject currentMenu;
    private List<GameObject> previousMenu = new List<GameObject>();

    private BattleAction playerAction;
    private BattleAction enemyAction;
    private PlayerData playerData;
    private PlayerData enemyData;

    private bool isDownSwitch;
    public bool IsDownSwitch => isDownSwitch;
    public bool gameover = false;
    public bool inputLock = false;

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
        if(!GameManager.Instance) return;
        playerData = GameManager.Instance.playerData;
        enemyData = GameManager.Instance.enemyData;
    }

    private void Start()
    {
        currentMenu = rootMenu;

        playerBattleEntity.SetDigimonData(playerData.partyData.Digimons[0]);
        enemyBattleEntity.SetDigimonData(enemyData.partyData.Digimons[0]);

        StartCoroutine(SetupBattle());
        TurnStart();
    }

    private void Update()
    {
        if(inputLock) return;
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

            if (playerSpeed > enemySpeed || (playerSpeed == enemySpeed && UnityEngine.Random.Range(0, 2) == 0))
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
            if(gameover) yield break;
            yield return ba.Action();
        }
        
        yield return CheckGameOver();

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

        TurnStart();
    }

    private Move GetEnemyMove()
    {
        return enemyBattleEntity.Digimon.Moves[UnityEngine.Random.Range(0, enemyBattleEntity.Digimon.Moves.Count)];
    }

    public BattleHUD SetTargetHUD(BattleEntity defender)
    {
        if (defender == playerBattleEntity) return hudManager.PlayerHUD;
        else return hudManager.EnemyHUD;
    }

    public IEnumerator BattleText(string text)
    {
        inputLock = true;
        uiText.text = text;

        textPanel.SetActive(true);
        yield return GameManager.Instance.WaitForKeyPress(KeyCode.Z);
        textPanel.SetActive(false);
        inputLock = false;
    }
    
    public IEnumerator BattleText(string text, float time)
    {
        inputLock = true;
        uiText.text = text;

        textPanel.SetActive(true);
        yield return new WaitForSeconds(time);
        textPanel.SetActive(false);
        inputLock = false;
    }
    public bool CalculateAccuracy(Move move, BattleEntity defender)
    {
        if (UnityEngine.Random.Range(0, 100) >= move.MoveBase.Accuracy)
        {
            StartCoroutine(BattleText($"{defender.Digimon.digimonName}은 맞지 않았다.", 2f));
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
        switch (playerMove.MoveBase.MoveTarget)
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
        hudManager.SetMoveButtonData(playerBattleEntity);
    }

    public void SwitchMenu(GameObject menu)
    {
        currentMenu.SetActive(false);
        menu.SetActive(true);

        previousMenu.Add(currentMenu);
        currentMenu = menu;
    }

    public void SwitchMenu(BattleMenu btMenu)
    {
        currentMenu.SetActive(false);
        menus[(int)btMenu].SetActive(true);

        previousMenu.Add(currentMenu);
        currentMenu = menus[(int)btMenu];
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

    public IEnumerator CheckGameOver()
    {
        bool playerDown = false;
        bool enemyDown = false;

        if(playerBattleEntity.Digimon.currentHP <= 0)
        {
            playerDown = true;
        }

        if(enemyBattleEntity.Digimon.currentHP <= 0)
        {
            enemyDown = true;
        }

        if(playerDown)
        {
            if(CheckPartyDown(playerData.partyData.Digimons))
            {
                yield return BattleText($"{playerData.playerName}은 패배했다.", 2f);
            }
            else
            {
                isDownSwitch = true;

                foreach(GameObject go in menus)
                {
                    if(go.GetComponent<PartyUI>())
                    {
                        AllHUDSetActivity(false);
                        go.SetActive(true);
                    }
                }
            }
        }
        else if(enemyDown)
        {
            if(CheckPartyDown(enemyData.partyData.Digimons))
            {
                yield return BattleWin();
            }
            else
            {
                int nextNum = 0;

                for(int i = 0; i < enemyData.partyData.Digimons.Count; i++)
                {
                    if(enemyData.partyData.Digimons[i].currentHP <= 0) continue;
                    else nextNum = i;
                    break;
                }

                SwitchPerform(enemyData, nextNum, true);
            }
        }
    }

    public void ActiveMenu(BattleMenu menu, bool active = true)
    {
        menus[(int)menu].SetActive(active);
        previousMenu.Add(currentMenu);
        currentMenu = menus[(int)menu];
    }

    public void SetPartyUIState(PartyUIState state)
    {
        menus[(int)BattleMenu.DigimonMenu].GetComponent<PartyUI>().SetState(state);
    }

    public IEnumerator BattleWin()
    {
        gameover = true;
        yield return BattleText($"{playerData.playerName}은 승리했다!", 2f);
        yield return new WaitForSeconds(1f);

        yield return GameManager.Instance.BattelExit();
    }

    public void BattleRun()
    {
        playerAction = new RunAction();
        StartCoroutine(PerformBattle());
    }

    public bool CheckPartyDown(List<Digimon> digimons)
    {
        bool isAllDown = true;
        foreach (Digimon eDigimon in digimons)
        {
            if(eDigimon.currentHP > 0)
            {
                isAllDown = false;
                break;
            }
        }
        return isAllDown;
    }

    public void SwitchPerform(PlayerData player, int partyNum, bool downSwitch = false)
    {
        isDownSwitch = downSwitch;
        playerAction = new SwitchAction(player, partyNum);

        if(isDownSwitch)
        {
            StartCoroutine(playerAction.Action());
            TurnStart();
            return;
        }

        StartCoroutine(PerformBattle());
    }


    public void ItemAction(PlayerData player, BattleEntity target, Item item)
    {
        playerAction = new ItemAction(player, target, item);

        StartCoroutine(PerformBattle());
    }

        public void ItemAction(PlayerData player, int partyNum, Item item)
    {
        playerAction = new ItemAction(player, partyNum, item);

        StartCoroutine(PerformBattle());
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

    public IEnumerator ShakeCoroutine(Transform cameraTransform, float duration, float magnitude)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float value = 0f;

        while (value < duration)
        {
            Vector3 randomPoint = originalPos + UnityEngine.Random.insideUnitSphere * magnitude;
            cameraTransform.localPosition = new Vector3(randomPoint.x, randomPoint.y, originalPos.z);

            value += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}

public enum BattleMenu
{
    BattleMenu,
    BagMenu,
    DigimonMenu
}