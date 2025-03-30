using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject_TriggerCollider : MonoBehaviour
{
    public GameObject targetObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            targetObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
