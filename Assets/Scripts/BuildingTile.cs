using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTile : Tile
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _towerPrefab;
    
    private bool _isBuilt;

    private void Start() 
    {
        SetColor();
    }

    private void OnMouseDown() {
        if (!_isBuilt)
        {
            BuildTower();
        }
    }

    private void OnMouseEnter() {
        _highlight.SetActive(true);
    }

    private void OnMouseExit() {
        _highlight.SetActive(false);
    }

    private void BuildTower()
    {
        var tower = Instantiate(_towerPrefab, transform.position, Quaternion.identity);
        _isBuilt = true;
    }
}
