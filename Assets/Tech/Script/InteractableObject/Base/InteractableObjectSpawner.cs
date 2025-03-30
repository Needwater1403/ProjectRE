using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableObjectSpawner : MonoBehaviour
{
    [Title("Objects")]
    [SerializeField] private GameObject interactableObjectPrefab;
    [SerializeField] private GameObject interactableObject;
    [Title("Basic Info")]
    [SerializeField] private int ID;
    private void Awake()
    {
        WorldObjectManager.Instance.AddObjectSpawner(this);
    }
    public void Spawn(List<InteractableObject> list)
    {
        if (interactableObjectPrefab != null)
        {
            gameObject.SetActive(false);
            interactableObject = Instantiate(interactableObjectPrefab, transform.position, transform.rotation, transform.parent);
            var io = interactableObject.GetComponent<InteractableObject>();
            if(io == null) io = interactableObject.GetComponentInChildren<InteractableObject>();
            io.ID = ID;
            io.OnSpawn();
            list.Add(io);
        }
    }
}
