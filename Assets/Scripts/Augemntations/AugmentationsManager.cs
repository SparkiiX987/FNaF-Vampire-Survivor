using System;
using System.Collections.Generic;
using UnityEngine;

public class AugmentationsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private List<AugmentationInfo> augmentationInfos = new List<AugmentationInfo>();

    [SerializeField]
    private List<AugmentationRarityChance> possiblesAugmentations = new List<AugmentationRarityChance>();

    private List<Tuple<int, int>> augmentationAlreadyShowed = new();

    private void Start()
    {
        PlayerExperienceManager.OpenLevelUpPanel += LevelUp;
    }

    private void OnDisable()
    {
        PlayerExperienceManager.OpenLevelUpPanel -= LevelUp;
    }

    private void LevelUp()
    {
        augmentationAlreadyShowed.Clear();

        panel.SetActive(true);
        Time.timeScale = 0;

        foreach (AugmentationInfo info in augmentationInfos)
        {
            PlayerComponent component = TakeRandomAugemntation();

            if(component is Passif statsAugment)
            {
                info.UpdateTexts(
                    statsAugment.componentName,
                    statsAugment.description,
                    statsAugment.rarity,
                    statsAugment
                    ); 
            }
        }
    }

    private PlayerComponent TakeRandomAugemntation()
    {
        int randomRarity = GetRandomRarityIndex();

        int index = UnityEngine.Random.Range(0, possiblesAugmentations.Count - 1);

        Tuple<int, int> rarityAugmentPair = new(randomRarity, index);

        if(augmentationAlreadyShowed.Contains(rarityAugmentPair))
        {
            return TakeRandomAugemntation();
        }

        augmentationAlreadyShowed.Add(rarityAugmentPair);
        return possiblesAugmentations[randomRarity].augmentations[index];
    }

    private int GetRandomRarityIndex()
    {
        int randomRarity = UnityEngine.Random.Range(0, 100);

        for(int i = 0;  i < possiblesAugmentations.Count; i++)
        {
            if (randomRarity >= (i - 1 >= 0 ? possiblesAugmentations[i - 1].rarityChance : 0) && randomRarity <= possiblesAugmentations[i].rarityChance)
            {
                return i;
            }
        }

        return 0;
    }
}

[System.Serializable]
public struct AugmentationRarityChance
{
    public Rarity rarity;
    public float rarityChance;
    public List<PlayerComponent> augmentations;
}
