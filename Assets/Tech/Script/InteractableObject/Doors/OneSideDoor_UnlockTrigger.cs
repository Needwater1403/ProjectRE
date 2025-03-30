using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OneSideDoor_UnlockTrigger : MonoBehaviour
{
    [Title("Doors")]
    [HideLabel]
    [SerializeField] private OneSideDoor_InteractableObject _oneSideDoor;
    [Title("Collider")]
    [HideLabel]
    [SerializeField] private Collider _collider;
    [Title("Params")]
    [SerializeField] private bool canBeOpenOnThisSide;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            _oneSideDoor.SetCanBeOpenStatus(canBeOpenOnThisSide);
        }
    }
}
