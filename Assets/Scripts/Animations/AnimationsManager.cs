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
        print(animations[0].GetAnimationName);
        meshtransform = animator.transform;
    }

    public void PlayNext()
    {
        PlayAnimation(++currentAnim);
    }

    private void PlayAnimation(int _index)
    {
        meshtransform.position = positionAndScales[(int)animations[_index].GetTransformScaleType].animationPosition;
        meshtransform.localScale = positionAndScales[(int)animations[_index].GetTransformScaleType].animationScale;

        animator.Play(_index);
    }
}
