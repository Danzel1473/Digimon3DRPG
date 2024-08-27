using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupButton : Button
{
    public ButtonType buttonType;

    public void SwitchDigimon(int partyNum)
    {
        BattleSystem.Instance.SwitchDigimonEntity(partyNum);
    }
}

public enum ButtonType
{
    Switch,
    Item,
    DigimonDetail,
    Cancel
}