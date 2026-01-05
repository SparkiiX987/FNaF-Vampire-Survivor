using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private List<AnimationTransform> animations = new List<AnimationTransform>();

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private List<PositionAndScale> positionAndScales = new List<PositionAndScale>();

    private Transform meshtransform;

    private int currentAnim = 0;

    private void Start()
    {
        meshtransform = animator.transform;
    }

    public void PlayNext()
    {
        if(currentAnim >= animations.Count) 
        {
            currentAnim = 0;
        }

        PlayAnimation(currentAnim++);
    }

    private void PlayAnimation(int _index)
    {
        meshtransform.position = positionAndScales[(int)animations[_index].GetTransformScaleType].animationPosition;
        meshtransform.localScale = positionAndScales[(int)animations[_index].GetTransformScaleType].animationScale;

        animator.Play(animations[_index].GetAnimationName);
        print(animations[_index].GetAnimationName);
    }
}
