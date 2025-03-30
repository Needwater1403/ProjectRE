using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame_TriggerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WorldAIManager.Instance.DestroyAICharacters();
        PlayerUIManager.Instance.SetPauseMenu(true);
        PlayerUIManager.Instance.pauseMenuManager._winLabel.gameObject.SetActive(true);
        PlayerUIManager.Instance.pauseMenuManager.DisableExitButton();
    }
}
