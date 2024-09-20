using UnityEngine;

namespace D3R.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        
        Rigidbody rb;

        Vector3 moveVectorInput;
        Vector3 moveDirection;
        Vector3 rotationDirection;

        [SerializeField] float speed = 10f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] Camera targetCamera;
        PlayerAnimator pa;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            pa = GetComponentInChildren<PlayerAnimator>();
        }

        public void AddMoveVectorInput(Vector3 moveVector)
        {
            moveVectorInput = moveVector;
        }

        private void Update()
        {
            if(GameManager.Instance.state != GameManager.SituState.OpenWorld) return;
            HandleMovement();
            HandleRotation();
            HandleAnimation();
        }

        private void HandleAnimation()
        {
            pa.SetSpeed(moveVectorInput.magnitude);
        }

        private void HandleMovement()
        {
            moveDirection = targetCamera.transform.forward * moveVectorInput.z;
            moveDirection += targetCamera.transform.right * moveVectorInput.x;
            moveDirection.y = 0f;

            Vector3 moveVelocity = moveDirection * speed;
            moveVelocity += Physics.gravity;

            rb.velocity = moveVelocity;
        }

        private void HandleRotation()
        {
            if(moveDirection.magnitude > 0f)
            {
                rotationDirection = moveDirection;
            }
            if(rotationDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                transform.rotation = rotation;
            }
        }
    }
}