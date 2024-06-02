using UnityEngine;

public class RocketTurret : MonoBehaviour, ITurret
{
    [SerializeField] private Transform m_rocketSpawnPoint;
    [SerializeField] private float m_fireRate = 1f;
    [SerializeField] private float m_range = 10f;
    private float m_timer = 0f;
    private bool m_canFire = true;
    private Transform m_target;
    public bool IsPlaced { get; set; }
    private void Update()
    {
        if(!IsPlaced)
            return;
        CheckTarget();
        if (m_canFire)
        {
            Fire();
        }
        if (m_canFire)
        {
            float originalZRotation = transform.rotation.eulerAngles.z;
            transform.LookAt(new Vector3(m_target.position.x, transform.position.y, m_target.position.z));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = originalZRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    public void Fire()
    {
        if (m_timer >= m_fireRate)
        {
            GameObject rocket = ObjectPooler.SharedInstance.GetRocketPooledObject();
            rocket.SetActive(true);
            if (rocket.TryGetComponent(out IBullet rocketProjectile))
            {
                rocket.transform.position = m_rocketSpawnPoint.position;
                rocket.transform.rotation = m_rocketSpawnPoint.rotation;
                rocketProjectile.SetTarget(m_target);
                m_timer = 0f;
            }
        }
        else
        {
            m_timer += Time.deltaTime;
        }
    }

    public void CheckTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_range);

        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEnemy enemy))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }
        if (closestCollider != null)
        {
            closestCollider.TryGetComponent(out IEnemy enemy);
            m_target = enemy.MovementPrediction.transform;
            m_canFire = true;
        }
        else
        {
            m_canFire = false;
            m_target = null;
        }
    }
    public int ReturnTurret()
    {
        return 2;
    }

}
