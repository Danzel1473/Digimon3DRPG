using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DigimonSlot : Button
{
    [SerializeField] private Image digimonIcon;
    [SerializeField] private TMP_Text digimonName;
    [SerializeField] private TMP_Text digimonLevel;
    [SerializeField] private TMP_Text digimonHP;
    [SerializeField] private Animator digimonAnimator;
    [SerializeField] private Slider digimonHPSlider;
    [SerializeField] private Digimon digimonData;
    int i;


    public void UpdateDigimon(int i)
    {
        this.i = i;
        digimonData = GameManager.Instance.playerData.partyData.Digimons[i];
        digimonIcon.sprite = digimonData.digimonBase.DigimonSprite;
        digimonAnimator.runtimeAnimatorController = digimonData.digimonBase.digimonSpriteAnimator;
        digimonName.text = digimonData.digimonBase.DigimonName;
        digimonLevel.text = $"Lv.{digimonData.Level}";
        digimonHP.text = $"{digimonData.CurrentHP} / {digimonData.MaxHP}";
        digimonHPSlider.value = (float)digimonData.CurrentHP / digimonData.MaxHP;
    }

    public void OnClick()
    {
        PopupMenu pm = GameManager.Instance.popupMenu;

        pm.gameObject.SetActive(false);
        GameManager.Instance.popupMenu.gameObject.transform.position = transform.position;
        List<PopupButtonType> pbt = new List<PopupButtonType>
        {
            PopupButtonType.Switch,
            PopupButtonType.Item,
            PopupButtonType.DigimonDetail,
            PopupButtonType.Cancel
        };

        pm.SetMenu(i, pbt);
        pm.ShowMenuAtPosition(transform.position);
    }


}