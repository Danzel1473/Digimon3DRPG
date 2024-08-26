using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDManager : MonoBehaviour
{
    //HUD Manager를 통해 HUD를 관리하도록 하기
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;

    public void UpdateHUD(Digimon player, Digimon enemy)
    {
        playerHUD.SetData(player);
        playerHUD.SetData(enemy);
    }

    public void SetMoveButtonData(List<Move> moves)
    {
        List<Button> moveButtons = playerHUD.MoveButtons;
        if (moves == null || moveButtons == null) return;

        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i < moves.Count)
            {
                moveButtons[i].GetComponentInChildren<TMP_Text>().text = moves[i].moveBase.MoveName;
                moveButtons[i].gameObject.SetActive(true);
            }
            else
            {
                moveButtons[i].gameObject.SetActive(false);
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
}