using System;
using UnityEngine;

public class SpawnerTile : PathTile
{
    [SerializeField] private GameObject _mobPrefab;
    public bool isEnabled;

    private void Start() 
    {
        SetColor();
        isEnabled = false;
        GameController.Instance.OnWaveStarted += StartWave;
    }

    private void SpawnMob()
    {
        GameObject mob = Instantiate(_mobPrefab, transform.position, Quaternion.identity);
        mob.GetComponent<Mob>().SetNextTileToBase(NextToBaseTile);
        GameController.Instance.Mobs.Add(mob);
    }

    private void StartWave(object sender, EventArgs e)
    {
        SpawnMob();
    }
}
