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

    public void SetDigimon(Digimon digimon)
    {
        digimonIcon.sprite = digimon.digimonBase.DigimonSprite;
        digimonAnimator.runtimeAnimatorController = digimon.digimonBase.digimonSpriteAnimator;
        digimonName.text = digimon.digimonBase.DigimonName;
        digimonLevel.text = $"Lv.{digimon.Level}";
        digimonHP.text = $"{digimon.CurrentHP} / {digimon.MaxHP}";
        digimonHPSlider.value = (float)digimon.CurrentHP / digimon.MaxHP;
    }
}