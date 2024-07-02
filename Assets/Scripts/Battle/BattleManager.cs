using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public DigimonEntity playerDigimon;
    public DigimonEntity enemyDigimon;
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public BattleUI battleUI;

    private bool playerActionChosen;
    private int playerChosenSkillIndex;

    private void Start()
    {
        battleUI.battleManager = this;
        SpawnDigimons();
        StartCoroutine(BattleRoutine());
    }

    private void SpawnDigimons()
    {
        GameObject playerDigimonGO = Instantiate(playerDigimon.digimonData.digimonPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        playerDigimon.Initialize(playerDigimon.digimonData, playerDigimonGO);

        
        GameObject enemyDigimonGO = Instantiate(enemyDigimon.digimonData.digimonPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
        enemyDigimon.Initialize(enemyDigimon.digimonData, enemyDigimonGO);
    }

    private IEnumerator BattleRoutine()
    {
        while (playerDigimon.currentHealth > 0 && enemyDigimon.currentHealth > 0)
        {
            playerActionChosen = false;
            battleUI.ShowPlayerOptions();
            yield return new WaitUntil(() => playerActionChosen);

            Skill playerSkill = playerDigimon.digimonData.skills[playerChosenSkillIndex];
            Skill enemySkill = enemyDigimon.ChooseSkill();

            DigimonEntity first, second;
            Skill firstSkill, secondSkill;

            if (playerDigimon.digimonData.speed >= enemyDigimon.digimonData.speed)
            {
                first = playerDigimon;
                second = enemyDigimon;
                firstSkill = playerSkill;
                secondSkill = enemySkill;
            }
            else
            {
                first = enemyDigimon;
                second = playerDigimon;
                firstSkill = enemySkill;
                secondSkill = playerSkill;
            }

            yield return ExecuteSkill(first, second, firstSkill);
            if (second.currentHealth > 0)
            {
                yield return ExecuteSkill(second, first, secondSkill);
            }

            battleUI.UpdateHealthBars();
        }

        EndBattle();
    }

    private IEnumerator ExecuteSkill(DigimonEntity attacker, DigimonEntity defender, Skill skill)
    {
        attacker.PerformSkill(System.Array.IndexOf(attacker.digimonData.skills, skill));
        defender.TakeDamage(skill);
        yield return new WaitForSeconds(1f);
    }

    private void EndBattle()
    {
        if (playerDigimon.currentHealth <= 0)
        {
            //패
        }
        else if (enemyDigimon.currentHealth <= 0)
        {
            //승
        }
    }

    public void OnPlayerChooseSkill(int skillIndex)
    {
        playerChosenSkillIndex = skillIndex;
        playerActionChosen = true;
    }
}