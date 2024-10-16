using System;
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
            case PopupButtonType.UseItem:
                buttonText.text = "사용";
                break;
            case PopupButtonType.UseToTarget:
                buttonText.text = "사용";
                break;        
            case PopupButtonType.Give:
                buttonText.text = "맡기기";
                break;
            case PopupButtonType.Throw:
                buttonText.text = "버리기";
                break;            
            case PopupButtonType.Target:
                buttonText.text = "사용";
                break;
            default:
                break;
        }
    }
    
    public void OnButtonClick()
    {
        switch(buttonType)
        {
            case PopupButtonType.Switch:
                SwitchDigimon();
                break;
            case PopupButtonType.UseItem:
                UseItem();
                break;
            case PopupButtonType.Target:
                ItemEffect();
                break;
            case PopupButtonType.Cancel:
                transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    private void ItemEffect()
    {
        int num = GetComponentInParent<PopupMenu>().num;

        switch(BattleSystem.Instance.itemWaitForUse.Attrs[0].Kind)
        {
            case ItemAttributeKind.Heal:
                if (GameManager.Instance.playerData.partyData.Digimons[num].currentHP == GameManager.Instance.playerData.partyData.Digimons[num].maxHP)
                {
                    StartCoroutine(BattleSystem.Instance.BattleText("사용해도 의미가 없다.", 2f));
                    return;
                }
                BattleSystem.Instance.ItemAction(GameManager.Instance.playerData, num, BattleSystem.Instance.itemWaitForUse);
                transform.parent.gameObject.SetActive(false);
            break;
        }
    }

    private void UseItem()
    {
        int num = GetComponentInParent<PopupMenu>().num;
        Item item = ItemTable.Instance[num];
        bool itemUsed = false;

        if (item == null) 
        {
            return;
        }
        Debug.Log(item.Attrs[0].Kind);
        switch (item.Attrs[0].Kind)
        {
            case ItemAttributeKind.Heal:
                BattleSystem.Instance.SwitchMenu(BattleMenu.DigimonMenu);
                BattleSystem.Instance.SetPartyUIState(PartyUIState.ItemTarget);
                BattleSystem.Instance.itemWaitForUse = item;
                transform.parent.gameObject.SetActive(false);
                break;
            case ItemAttributeKind.Digicatch:
                CatchDigimon(item);
                break;
        }
        if(itemUsed) GameManager.Instance.playerData.Inventory.RemoveItem(item, 1);
    }

    private void SwitchDigimon()
    {
        int num = GetComponentInParent<PopupMenu>().num;

        if (num == 0) //선택 디지몬이 첫번째 디지몬
        {
            StartCoroutine(BattleSystem.Instance.BattleText("이미 출전해있다.", 2f));
            return;
        }
        if (GameManager.Instance.playerData.partyData.Digimons[num].currentHP <= 0) //선택 디지몬의 체력이 0이면 return
        {
            StartCoroutine(BattleSystem.Instance.BattleText($"{GameManager.Instance.playerData.partyData.Digimons[num]}은 기절해있다!", 2f));
            return;
        }

        if (BattleSystem.Instance.IsDownSwitch) BattleSystem.Instance.SwitchPerform(GameManager.Instance.playerData, num, true);
        else BattleSystem.Instance.SwitchPerform(GameManager.Instance.playerData, num);
        transform.parent.gameObject.SetActive(false);
    }

    private void CatchDigimon(Item item)
    {
        if(GameManager.Instance.enemyData.IsTamer)
        {
            StartCoroutine(BattleSystem.Instance.BattleText("테이머의 디지몬을 잡을 수는 없다!", 2f));
            return;
        }

        BattleSystem.Instance.ItemAction(GameManager.Instance.playerData, BattleSystem.Instance.EnemyBattleEntity, item);
        transform.parent.gameObject.SetActive(false);
    }
}

public enum PopupButtonType
{
    Switch,
    Item,
    DigimonDetail,
    Cancel,
    UseItem,
    UseToTarget,
    Give,
    Throw,
    Target
}