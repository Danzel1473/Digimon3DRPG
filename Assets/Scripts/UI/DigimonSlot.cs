using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DigimonSlot : MonoBehaviour
{
    [SerializeField] private Image digimonIcon;
    [SerializeField] private TMP_Text digimonName;
    [SerializeField] private TMP_Text digimonLevel;
    [SerializeField] private TMP_Text digimonHP;
    [SerializeField] private Animator digimonAnimator;
    [SerializeField] private Slider digimonHPSlider;
    [SerializeField] private Digimon digimonData;


    public void UpdateDigimon(Digimon digimon)
    {
        digimonData = digimon;
        digimonIcon.sprite = digimonData.digimonBase.DigimonSprite;
        digimonAnimator.runtimeAnimatorController = digimonData.digimonBase.digimonSpriteAnimator;
        digimonName.text = digimonData.digimonBase.DigimonName;
        digimonLevel.text = $"Lv.{digimonData.Level}";
        digimonHP.text = $"{digimonData.CurrentHP} / {digimonData.MaxHP}";
        digimonHPSlider.value = (float)digimonData.CurrentHP / digimonData.MaxHP;
    }

    public void OnClick()
    {
        
    }
}