using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableObject : MonoBehaviour
{
    [TabGroup("Interactive","Base")]
    [Title("Basic Info")] 
    public int ID;
    [TabGroup("Interactive","Base")]
    [TextArea] public string popUpText;
    [TabGroup("Interactive","Base")]
    [SerializeField] protected Collider interactRangeCollider;
    [HideInInspector] public bool isCompletelySpawned;
    protected virtual void Awake()
    {
        if (interactRangeCollider == null) interactRangeCollider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        // DO SOMETHING DEPENDS ON WHAT OBJECT
        interactRangeCollider.enabled = false;
        PlayerManager.Instance._controlInteractive.RemoveInteractiveObject(this);
        PlayerUIManager.Instance.popUpsManager.HideInteractivePopUp();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(Constants.PlayerTag)) return;
        // PASS TH INTERACTION TO THE PLAYER
        PlayerManager.Instance._controlInteractive.AddInteractiveObject(this);
    }
    
    public virtual void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(Constants.PlayerTag)) return;
        // REMOVE TH INTERACTION FROM THE PLAYER
        PlayerManager.Instance._controlInteractive.RemoveInteractiveObject(this);
    }

    public virtual Transform GetInteractColliderTransform()
    {
        return interactRangeCollider.transform;
    }
    public virtual void OnSpawn()
    {
        isCompletelySpawned = true;
    }
    
    public virtual void OnDespawn()
    {
        
    }
        
    public virtual void SetColliderStatus(bool isEnable)
    {
        interactRangeCollider.enabled = isEnable;
    }
}
