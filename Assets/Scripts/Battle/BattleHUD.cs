using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private List<Button> moveButtons;

    public void SetData(Digimon digimon)
    {
        nameText.text = digimon.digimonBase.DigimonName;
        levelText.text = digimon.Level.ToString();
        hpBar.value = (float)digimon.CurrentHP / digimon.digimonBase.MaxHP;
        HPText.text = $"{digimon.CurrentHP}/{digimon.digimonBase.MaxHP}";
    }

    public void SetMoveNames(List<Move> moves)
    {
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

    public void ShowHUD(bool show)
    {
        gameObject.SetActive(show);
    }
}