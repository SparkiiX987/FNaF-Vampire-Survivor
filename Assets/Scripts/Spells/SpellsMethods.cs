using UnityEngine;

public static class SpellsMethods
{
    public static void Laser(Transform _playerTransform, LayerMask _enemiesMask, float _cooldown)
    {
        RaycastHit[] hits = Physics.SphereCastAll(_playerTransform.position, 10, Vector3.up, 10, _enemiesMask);

        if(hits.Length <= 0 ) { return; }

        Vector3 nearest = hits[0].collider.transform.position;
        int nearestIndex = 0;

        for(int i = 1; i < hits.Length; i++)
        {
            if (Vector3.Distance(_playerTransform.position, hits[i].collider.transform.position) < Vector3.Distance(_playerTransform.position, nearest))
            {
                nearest = hits[i].collider.transform.position;
                nearestIndex = i;
            }
        }

        _playerTransform.GetComponent<PlayerController>().FireLaser(_cooldown, nearest);
    }
}
