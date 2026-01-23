using TMPro;
using UnityEngine;

public class ExperienceBar : Progressbar
{
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        PlayerController.UpdateExperienceBar += SetBarValue;
        PlayerExperienceManager.UpdateLevel += SetLevel;
    }

    private void OnDisable()
    {
        PlayerController.UpdateExperienceBar -= SetBarValue;
        PlayerExperienceManager.UpdateLevel -= SetLevel;
    }

    private void SetLevel(int _level)
    {
        levelText.text = _level.ToString();
    }
}
