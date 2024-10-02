using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteraction : InteractiveObject
{
    public override void Interaction()
    {
        NPCData data = gameObject.GetComponent<NPCData>();
        NPCType npcType = data.npcType;
        if(npcType == NPCType.Enemy)
        {
            if (data.npcData == null) return;

            StartCoroutine(GameManager.Instance.BattelEnter(data));
        }
    }
}