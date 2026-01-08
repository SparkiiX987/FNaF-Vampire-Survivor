using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    float damages;

    Vector3 avancementFactor;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    public void Initialize(Vector3 _dir, float _damages)
    {
        damages = _damages;
        avancementFactor = _dir.normalized * speed;
    }

    private void Update()
    {
        transform.position += avancementFactor * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"touche {other.gameObject.name}");
        if (!other.transform.parent.TryGetComponent(out AIBehaviour aiBehaviour))
        {
            return;
        }

        aiBehaviour.TakeDamages(damages);

        Destroy(gameObject);
    }
}
