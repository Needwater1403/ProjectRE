using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{
    public static PlayTimeManager Instance;
    public float PlayTime { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PlayTime = WorldSaveGameManager.Instance.currentCharacterSaveData.playTime;
    }

    private void FixedUpdate()
    {
        PlayTime += Time.deltaTime;
    }
}
