using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : Character
{
    public void ShootArrow()
    {
        if (activeSkillInstance.target == null || SkillInfos[0].SkillPrefab == null) return;

        Vector3 targetPosition = activeSkillInstance.target.transform.position;
        Vector3 spawnPosition = transform.position + Vector3.up;
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);

        GameObject arrow = Instantiate(SkillInfos[0].SkillPrefab, spawnPosition, spawnRotation);
        ArrowProjectile arrowProjectile = arrow.AddComponent<ArrowProjectile>();
        arrowProjectile.Initialize(targetPosition);
    }
}
