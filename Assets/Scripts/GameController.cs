using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event EventHandler OnWaveStarted;

    public static GameController Instance { get; private set; }
    public List<GameObject> Mobs;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() 
    {
        Mobs = new List<GameObject>();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        OnWaveStarted?.Invoke(this, EventArgs.Empty);
    }
}
