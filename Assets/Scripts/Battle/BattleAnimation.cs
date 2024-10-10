using System.Collections;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation(bool isSPAttack)
    {
        animator.SetBool("isSPAttack", isSPAttack);
        animator.SetTrigger("attack");
    }

    public void PlayDamageAnimation()
    {
        animator.SetTrigger("damaged");
        StartCoroutine(BattleSystem.Instance.ShakeCoroutine(Camera.main.transform, 0.1f, 0.2f));
        
    }

    public void PlayFaintAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public void PlayMoveParticle(GameObject particlePrefab, Transform target)
    {
        if (particlePrefab != null)
        {
            StartCoroutine(PlayMoveParticleCoroutine(particlePrefab, target));
        }
    }

    private IEnumerator PlayMoveParticleCoroutine(GameObject particlePrefab, Transform target)
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        while (particle != null && Vector3.Distance(particle.transform.position, target.position) > 0.1f)
        {
            particle.transform.position = Vector3.MoveTowards(particle.transform.position, target.position, Time.deltaTime * 10);
            yield return null;
        }

        if (particleSystem != null)
        {
            particleSystem.Stop();
            Destroy(particle, particleSystem.main.duration);
        }
        else
        {
            Destroy(particle);
        }
    }
}