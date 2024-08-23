using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private TMP_Text enemyNameText;
    [SerializeField] private TMP_Text enemyLevelText;
    [SerializeField] private TMP_Text enemyHPText;
    [SerializeField] private Slider enemyHPBar;
    [SerializeField] private List<Button> moveButtons;
    [SerializeField] private UnityEngine.GameObject playerHUD;
    [SerializeField] private UnityEngine.GameObject enemyHUD;

    public void SetData(Digimon digimon, bool isPlayer)
    {
        if (isPlayer)
        {
            playerNameText.text = digimon.digimonBase.DigimonName;
            playerLevelText.text = digimon.Level.ToString();
            playerHPBar.value = (float)digimon.CurrentHP / digimon.digimonBase.HP;
            playerHPText.text = $"{digimon.CurrentHP}/{digimon.digimonBase.HP}";
        }
        else
        {
            enemyNameText.text = digimon.digimonBase.DigimonName;
            enemyLevelText.text = digimon.Level.ToString();
            enemyHPBar.value = (float)digimon.CurrentHP / digimon.digimonBase.HP;
            enemyHPText.text = $"{digimon.CurrentHP}/{digimon.digimonBase.HP}";
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
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

    public void UpdateHUD(BattleEntity entity)
    {
        if (entity == null) return;

        if (entity.IsPlayer)
        {
            playerHPBar.value = (float)entity.Digimon.CurrentHP / entity.Digimon.digimonBase.HP;
            playerHPText.text = $"{entity.Digimon.CurrentHP}/{entity.Digimon.digimonBase.HP}";
        }
        else
        {
            enemyHPBar.value = (float)entity.Digimon.CurrentHP / entity.Digimon.digimonBase.HP;
            enemyHPText.text = $"{entity.Digimon.CurrentHP}/{entity.Digimon.digimonBase.HP}";
        }
    }

    public void HideHUD(bool isPlayer)
    {
        if (isPlayer)
        {
            playerHUD.SetActive(false);
        }
        else
        {
            enemyHUD.SetActive(false);
        }
    }
}