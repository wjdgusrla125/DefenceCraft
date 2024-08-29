using System.Collections;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    public SphereCollider _sphereCollider;

    [SerializeField] private Character _character;
    [SerializeField] private EnemyCharacter _enemyCharacter;

    private float _damageInterval = 1.5f;

    private Coroutine _damageCoroutine;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _enemyCharacter = GetComponentInParent<EnemyCharacter>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (_character != null)
        {
            _damageInterval = _character.SkillInfos[0].hitInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_character != null)
        {
            _character.ManageSkills();
        }
        
        if (_damageCoroutine == null && ShouldStartDamageCoroutine(other))
        {
            _damageCoroutine = StartCoroutine(ApplyPeriodicDamage(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }

    private IEnumerator ApplyPeriodicDamage(Collider targetCollider)
    {
        while (true)
        {
            if (targetCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ApplyDamage(targetCollider.gameObject);
            }
            else if (targetCollider.gameObject.layer == LayerMask.NameToLayer("Clickable"))
            {
                Debug.Log($"target : {targetCollider.gameObject}");
                EApplyDamage(targetCollider.gameObject);
            }

            yield return new WaitForSeconds(_damageInterval);
        }
    }

    private bool ShouldStartDamageCoroutine(Collider other)
    {
        bool isValidTarget = other.gameObject.layer == LayerMask.NameToLayer("Enemy") || 
                             other.gameObject.layer == LayerMask.NameToLayer("Clickable");
        return isValidTarget;
    }

    private void ApplyDamage(GameObject target)
    {
        EnemyCharacter enemyCharacter = target.GetComponentInParent<EnemyCharacter>();
        
        if (enemyCharacter != null)
        {
            float damage = _character != null ? _character.SkillInfos[0].Damage : 0f;
            
            enemyCharacter.TakeDamage(damage);
        }
        else
        {
            Debug.Log("No enemyCharacter");
        }
        
    }

    private void EApplyDamage(GameObject target)
    {
        Character character = target.GetComponentInParent<Character>();

        if (character != null)
        {
            float damage = _enemyCharacter != null ? _enemyCharacter.damage : 0f;
            
            character.TakeDamage(damage);
        }
        else
        {
            Debug.Log("No Character");
        }
    }
}
