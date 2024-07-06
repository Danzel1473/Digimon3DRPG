using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text HPText;

    [SerializeField] Slider hpBar;
    [SerializeField] List<Button> moveButtons;

    public void SetData(Digimon digimon)
    {
        nameText.text = digimon.digimonBase.DigimonName;
        levelText.text = digimon.level.ToString();
        hpBar.value = (float)digimon.HP / digimon.digimonBase.MaxHP;
        HPText.text = digimon.HP + "/" + digimon.digimonBase.MaxHP;

        //디버그용
        print(digimon.HP);
    }

    public void SetMoveNames(List<Move> moves)
    {
        if(moves == null) return;
        if(moveButtons == null) return;
        if(moves.Count == 0) return;

        for(int i = 0; i < moveButtons.Count; i++)
        {
            if(i < moves.Count)
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
}