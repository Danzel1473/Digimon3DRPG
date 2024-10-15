using UnityEngine;
using UnityEngine.AI;

public class DigimonAIMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float moveRadius = 5f;
    public float randomMoveInterval = 5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        InvokeRepeating("RandomOrChaseMove", 0f, randomMoveInterval);
    }

    private void Update()
    {
        // 속도에 따른 애니메이션 전환
        float speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }

    private void RandomOrChaseMove()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, moveRadius, 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
    }
}