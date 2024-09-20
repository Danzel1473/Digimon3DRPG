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
            if(data.npcData == null) return;
            GameManager.Instance.enemyData = data.npcData;
            GameManager.Instance.state = GameManager.SituState.Battle;
            InterectionUI.Instance.DisableText();
            //Scene currentScene = SceneManager.GetActiveScene();

            SceneManager.LoadScene("BattleScene");
        }
    }
}