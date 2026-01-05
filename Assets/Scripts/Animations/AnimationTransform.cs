using UnityEngine;

[System.Serializable]
public class AnimationTransform
{
    [SerializeField]
    private AnimationClip clip;

    [SerializeField]
    private transformScaleByAnim transformScaleType;

    public string GetAnimationName => clip.name;

    public transformScaleByAnim GetTransformScaleType => transformScaleType;
}

[System.Serializable]
public class PositionAndScale
{
    public Vector3 animationPosition;

    public Vector3 animationScale;
}

public enum transformScaleByAnim
{
    noOffset = 0,
    littleOffset = 1,
    OffsetAndScale = 2,
}
