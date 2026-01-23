using System;
using UnityEngine;

[System.Serializable]
public class PlayerExperienceManager
{
    [SerializeField]
    private AnimationCurve experienceCurve;

    private int experienceNeeded;
    private int currentExperience;

    public static event Action<int> UpdateLevel;
    public static event Action OpenLevelUpPanel;

    public int level { get; private set; }

    public void InitLevels()
    {
        level = 1;
        experienceNeeded = Mathf.RoundToInt(experienceCurve.Evaluate(level));
        UpdateLevel.Invoke(level);
    }

    public float GetExperienceAvancement()
    {
        return (float)currentExperience / (float)experienceNeeded;
    }

    public void AddExperience(int _amount)
    {
        currentExperience += _amount;

        if(currentExperience >= experienceNeeded)
        {
            currentExperience -= experienceNeeded;

            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experienceNeeded = Mathf.RoundToInt(experienceCurve.Evaluate(level));
        OnLevelUp();
    }

    private void OnLevelUp()
    {
        UpdateLevel.Invoke(level);
        OpenLevelUpPanel.Invoke();
    }
}
