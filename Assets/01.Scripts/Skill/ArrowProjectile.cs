using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float Damage;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timeElapsed;
    private float totalTime = 1f; // 화살이 목표에 도달하는 총 시간

    public void Initialize(Vector3 target)
    {
        startPosition = transform.position;
        targetPosition = target;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float normalizedTime = timeElapsed / totalTime;

        // 포물선 궤적 계산
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
        float height = Mathf.Sin(normalizedTime * Mathf.PI) * 2f;
        currentPosition.y += height;

        transform.position = currentPosition;

        // 화살 방향 설정
        if (normalizedTime < 1f)
        {
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, (timeElapsed + Time.deltaTime) / totalTime);
            nextPosition.y += Mathf.Sin((normalizedTime + Time.deltaTime / totalTime) * Mathf.PI) * 2f;
            transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);
        }

        // 목표에 도달하거나 시간 초과 시 화살 제거
        if (normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //EnemyCharacter enemyCharacter = other.gameObject.GetComponentInParent<EnemyCharacter>();
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //enemyCharacter.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}