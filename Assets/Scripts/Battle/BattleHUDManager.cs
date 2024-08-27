using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDManager : MonoBehaviour
{
    //HUD Manager를 통해 HUD를 관리하도록 하기
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;
    public BattleHUD PlayerHUD => playerHUD;
    public BattleHUD EnemyHUD => enemyHUD;


    public void UpdateHUD(BattleEntity player, BattleEntity enemy)
    {
        playerHUD.SetData(player.Digimon);
        enemyHUD.SetData(enemy.Digimon);
    }

    public void SetMoveButtonData(BattleEntity player)
    {
        List<Move> moves = player.Digimon.Moves;
        List<Button> moveButtons = playerHUD.MoveButtons;
        if (moves == null || moveButtons == null) return;

        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i < moves.Count)
            {
                moveButtons[i].GetComponentInChildren<TMP_Text>().text = moves[i].moveBase.MoveName;
                moveButtons[i].GetComponent<MoveButton>().SetUp(moves[i].moveBase);
            }
            else
            {
                moveButtons[i].GetComponentInChildren<TMP_Text>().text = "";
            }
        }
    }

    public void HideHUD(bool isPlayer)
    {
        if (isPlayer)
        {
            playerHUD.gameObject.SetActive(false);
        }
        else
        {
            enemyHUD.gameObject.SetActive(false);
        }
    }

    public void ActiveHUD(bool isPlayer)
    {
        if (isPlayer)
        {
            playerHUD.gameObject.SetActive(true);
        }
        else
        {
            enemyHUD.gameObject.SetActive(true);
        }
    }

    public void ActiveHUD()
    {
        playerHUD.gameObject.SetActive(true);
        enemyHUD.gameObject.SetActive(true);
    }
}