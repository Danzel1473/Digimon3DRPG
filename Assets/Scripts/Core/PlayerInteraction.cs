using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float maxDistance = 5f; // 최대 상호작용 거리

    private InteractiveObject currentInteractiveObject;

    void Update()
    {
        if(GameManager.Instance.State == GameManager.SituState.OpenWorld)
        {
            CheckInteraction();
            if(Input.GetKeyDown(KeyCode.E) && currentInteractiveObject != null)
            {
                currentInteractiveObject.Interaction();
            }
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Vector3 origin = gameObject.transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = Camera.main.transform.forward;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            InteractiveObject interactive = hit.collider.GetComponent<InteractiveObject>();
            if (interactive != null)
            {
                if (currentInteractiveObject != interactive)
                {
                    currentInteractiveObject = interactive;
                }
                InterectionUI.Instance.UpdateTextPosition(interactive.transform);
                return;
            }
        }
        else
        {
            InterectionUI.Instance.DisableText();
            return;
        }
    }
}