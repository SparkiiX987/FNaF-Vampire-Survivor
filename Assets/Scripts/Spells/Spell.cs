using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Augmentation/Spell")]
public class Spell : PlayerComponent
{
    public float cooldown;
    public float currentCooldown;
    public string spellMethodName;
    public float damages;

    public LayerMask targetLayers;

    public void Use(Transform _transform)
    {
        currentCooldown = cooldown;

        Type type = typeof(SpellsMethods);
        MethodInfo methodInfo = type.GetMethod(spellMethodName);
        object[] _params = { _transform, targetLayers, cooldown };
        methodInfo.Invoke(spellMethodName, _params);
    }
}
