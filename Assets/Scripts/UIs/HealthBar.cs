using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] 
    private Slider bar;

    [SerializeField]
    private TextMeshProUGUI healthText;

    private void Start()
    {
       PlayerController.UpdateHealthBar += SetHealthOnHUD;
    }

    private void SetBarValue(float _value)
    {
        bar.value = _value;
    }

    private void SetTextValue(float _currentHealt, float _maxHealth)
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
