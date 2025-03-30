using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AwakeBoss_TriggerCollider  : MonoBehaviour
{
    [Title("Collider")]
    [HideLabel]
    [SerializeField] private Collider _collider;
    [Title("Boss")] 
    public AIBossCharacterManager bossManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            bossManager.SetBossEventStatus(true);
            _collider.enabled = false;
        }
    }
}