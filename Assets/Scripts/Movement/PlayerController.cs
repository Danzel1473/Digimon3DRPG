using UnityEngine;

namespace D3R.Movement
{
    public class PlayerController : MonoBehaviour
    {
        Vector3 moveVector;
        [SerializeField] CharacterMovement characterMovement;

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        public void Update()
        {
            if(GameManager.Instance.state != GameManager.SituState.OpenWorld) return;

            moveVector.x = Input.GetAxisRaw("Horizontal");
            moveVector.z = Input.GetAxisRaw("Vertical");

            characterMovement.AddMoveVectorInput(moveVector);
        }
    }
}

