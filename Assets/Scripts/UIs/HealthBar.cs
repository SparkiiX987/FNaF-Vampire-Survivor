using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Progressbar
{

    [SerializeField]
    protected TextMeshProUGUI healthText;

    private void Start()
    {
       PlayerController.UpdateHealthBar += SetHealthOnHUD;
    }

    protected void SetTextValue(float _currentHealt, float _maxHealth)
    {
        healthText.text = $"{(int)_currentHealt} / {(int)_maxHealth}";
    }

    public void SetHealthOnHUD(float _currentHealt, float _maxHealth)
    {
        SetTextValue(_currentHealt, _maxHealth);

        float barValue = 1f - ((float)_currentHealt / (float)_maxHealth);
        SetBarValue(barValue);
    }

    private void OnDestroy()
    {
        PlayerController.UpdateHealthBar -= SetHealthOnHUD;
    }
}
