using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    private float speed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetSpeed(float inputSpeed)
    {
        speed = inputSpeed;
    }
}