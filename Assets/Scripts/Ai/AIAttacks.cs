using System.Collections.Generic;
using UnityEngine;

public class AIAttacks : MonoBehaviour
{
    [SerializeField]
    private List<AnimationAttackEventTime> attackAnim;

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameMode.playerRef.transform;

        foreach (AnimationAttackEventTime anim in attackAnim)
        {
            anim.animationClip.AddEvent(new AnimationEvent
            {
                time = anim.Time,
                functionName = "OnAttackHit"
            });
        }
    }

    public void OnAttackHit()
    {
        print("damages");
        playerTransform.GetComponent<PlayerController>().TakeDamages(GetComponentInParent<AIBehaviour>().stats.GetDamages);
    }
}
