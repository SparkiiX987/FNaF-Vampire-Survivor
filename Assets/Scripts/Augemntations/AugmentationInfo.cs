using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentationInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI augmentationName;

    [SerializeField]
    private TextMeshProUGUI augmentationRarity;

    [SerializeField]
    private TextMeshProUGUI augmentationDescription;

    [SerializeField]
    private List<Color> rarityColors;

    private PlayerComponent currentComponent;

    public void UpdateTexts(string _name, string _description, Rarity _rarity, PlayerComponent _component)
    {
        augmentationName.text = _name;
        augmentationDescription.text = _description;
        SetRarity(_rarity);
        currentComponent = _component;
    }

    public void ResetInfos()
    {
        currentComponent = null;
    }

    private void SetRarity(Rarity _rarity)
    {
        augmentationRarity.color = rarityColors[(int)_rarity];
        augmentationRarity.text = _rarity.ToString();
    }

    public void OnAugementationTake()
    {
        GameMode.playerRef.GetComponent<PlayerController>().AddUpgradeComponent(currentComponent);
        Time.timeScale = 1;
    }
}
