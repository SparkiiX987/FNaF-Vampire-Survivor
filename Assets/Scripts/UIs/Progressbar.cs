using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    [SerializeField]
    protected Slider bar;

    protected void SetBarValue(float _value)
    {
        bar.value = _value;
    }
}
