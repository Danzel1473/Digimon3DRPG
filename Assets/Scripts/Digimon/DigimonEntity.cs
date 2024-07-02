using UnityEngine;

public class DigimonEntity : MonoBehaviour
{
    public Digimon digimonData;
    private GameObject digimonModel;
    private Animator animator;

    public int currentHealth;

    private Skill[] currentSkills;

    public void Initialize(Digimon data, GameObject model)
    {
        digimonData = data;
        digimonModel = model;
        currentHealth = digimonData.maxHealth;

        //애니메이터 셋업
        animator = digimonModel.GetComponent<Animator>();
        if (animator != null && digimonData.animatorController != null)
        {
            animator.runtimeAnimatorController = digimonData.animatorController;
        }

        currentSkills = digimonData.skills;
    }

    public void TakeDamage(Skill skill)
    {
        //테스트용 데미지
        int damage = 5;

        currentHealth -= Mathf.Max(currentHealth, damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, digimonData.maxHealth);
    }

    public void PerformSkill(int skillIndex)
    {
        if (animator != null)
        {
            animator.SetTrigger("AttackTrigger");
        }
    }

    public Skill ChooseSkill()
    {
        //무작위 스킬 선택
        int index = Random.Range(0, currentSkills.Length);
        return currentSkills[index];
    }
}