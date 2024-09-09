using System.Collections;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    public SphereCollider _sphereCollider;

    [SerializeField] private Character _character;
    [SerializeField] private EnemyCharacter _enemyCharacter;

    private float _damageInterval = 1.5f;
    private Coroutine _damageCoroutine;
    private Collider _currentTarget;

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
            Debug.Log("enter");
            _damageCoroutine = StartCoroutine(ApplyPeriodicDamage(other));
            _currentTarget = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if (_currentTarget == other)
        {
            StopDamageCoroutine();
        }
    }

    private IEnumerator ApplyPeriodicDamage(Collider targetCollider)
    {
        while (true)
        {
            if (targetCollider == null || !targetCollider.gameObject.activeInHierarchy)
            {
                Debug.Log("Target out of range or inactive");
                StopDamageCoroutine();
                yield break;
            }

            // 레이어에 따른 데미지 적용
            int targetLayer = targetCollider.gameObject.layer;
            if (targetLayer == LayerMask.NameToLayer("Enemy"))
            {
                ApplyDamage(targetCollider.gameObject);
            }
            else if (targetLayer == LayerMask.NameToLayer("Clickable"))
            {
                EApplyDamage(targetCollider.gameObject);
            }
            else if (targetLayer == LayerMask.NameToLayer("Building"))
            {
                Debug.Log($"target : {targetCollider.gameObject}");
                BApplyDamage(targetCollider.gameObject);
            }

            yield return new WaitForSeconds(_damageInterval);
        }
    }

    private bool IsTargetInRange(Collider targetCollider)
    {
        // bounds.Contains에서 Distance 체크로 변경하여 더 정확한 범위 확인
        float distance = Vector3.Distance(targetCollider.transform.position, _sphereCollider.transform.position);
        return distance <= _sphereCollider.radius;
    }

    private void StopDamageCoroutine()
    {
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
            _currentTarget = null;
        }
    }

    private bool ShouldStartDamageCoroutine(Collider other)
    {
        bool isValidTarget = other.gameObject.layer == LayerMask.NameToLayer("Enemy") || 
                             other.gameObject.layer == LayerMask.NameToLayer("Clickable") ||
                             other.gameObject.layer == LayerMask.NameToLayer("Building");
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

    private void BApplyDamage(GameObject target)
    {
        UnitHealth buildingHealth = target.GetComponentInParent<UnitHealth>();

        if (buildingHealth != null)
        {
            float damage = _enemyCharacter != null ? _enemyCharacter.damage : 0f;
            buildingHealth.HPTakeDamage(damage);
        }
    }
}
