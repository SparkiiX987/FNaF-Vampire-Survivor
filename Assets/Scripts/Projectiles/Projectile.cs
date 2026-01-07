using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    float damages;

    Vector3 avancementFactor;

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
        other.GetComponent<AIBehaviour>().TakeDamages(damages);

        Destroy(gameObject);
    }
}
