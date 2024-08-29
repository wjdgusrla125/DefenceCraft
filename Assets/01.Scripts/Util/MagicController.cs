using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public float explosionRadius = 5.0f;
    public float explosionForce = 700.0f;
    public SkillInfo skillInfo;
    
    void Start()
    {
        StartCoroutine(particleDestroy());
    }

    IEnumerator particleDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Explosion();
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    public void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            EnemyCharacter _enemy = hit.GetComponent<EnemyCharacter>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,explosionRadius);
            }

            if (_enemy != null)
            {
                _enemy.TakeDamage(skillInfo.Damage);
            }
        }
    }
}
