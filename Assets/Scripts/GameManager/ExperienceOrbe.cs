using UnityEngine;
using UnityEngine.Pool;

public class ExperienceOrbe : MonoBehaviour
{
    public ObjectPool<GameObject> pool;

    [SerializeField]
    private int experince;

    public void InitOrbe(int _experience)
    {
        experince = _experience;
    }

    public int GetExperiences()
    {
        return experince; 
    }

    public void PickUpOrbe()
    {
        pool.Release(gameObject);
    }
}
