using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public BattleManager battleManager;
    public Text playerHealthText;
    public Text enemyHealthText;
    public Button[] actionButtons;

    public bool playerActionChosen;
    public int chosenActionIndex;

    public void ShowPlayerOptions()
    {
        playerActionChosen = false;
        DigimonEntity playerDigimon = battleManager.playerDigimon;
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (i < playerDigimon.digimonData.skills.Length)
            {
                Skill skill = playerDigimon.digimonData.skills[i];
                actionButtons[i].GetComponentInChildren<Text>().text = $"{skill.skillName} 위력: {skill.power})";
                actionButtons[i].onClick.RemoveAllListeners();
                int index = i;
                actionButtons[i].onClick.AddListener(() => battleManager.OnPlayerChooseSkill(index));
                actionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                actionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealthBars()
    {
        DigimonEntity playerDigimon = battleManager.playerDigimon;
        DigimonEntity enemyDigimon = battleManager.enemyDigimon;
        playerHealthText.text = "HP: " + playerDigimon.currentHealth + "/" + playerDigimon.digimonData.maxHealth;
        enemyHealthText.text = "HP: " + enemyDigimon.currentHealth + "/" + enemyDigimon.digimonData.maxHealth;
    }
}