using TMPro;
using UnityEngine;

public class PopupButton : MonoBehaviour
{
    public PopupButtonType buttonType;
    [SerializeField] private TMP_Text buttonText;

    public void SetType(PopupButtonType type)
    {
        buttonType = type;

        switch(buttonType)
        {
            case PopupButtonType.Switch:
                buttonText.text = "교체";
                break;
            case PopupButtonType.Item:
                buttonText.text = "아이템";
                break;
            case PopupButtonType.DigimonDetail:
                buttonText.text = "디지몬";
                break;
            case PopupButtonType.Cancel:
                buttonText.text = "취소";
                break;
            default:
                break;
        }
    }
    public void OnButtonClick()
    {
        int partyNum = GetComponentInParent<PopupMenu>().partyNum;
        switch(buttonType)
        {
            case PopupButtonType.Switch:
                if(partyNum == 0)
                {
                    StartCoroutine(BattleSystem.Instance.BattleText("이미 출전해있다.", 2f));
                    break;
                }
                BattleSystem.Instance.SwitchDigimonEntity(GameManager.Instance.playerData, partyNum);
                break;
        }
    }
}

public enum PopupButtonType
{
    Switch,
    Item,
    DigimonDetail,
    Cancel
}