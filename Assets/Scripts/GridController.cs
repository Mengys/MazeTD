using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private GameObject _buildingTilePrefab;
    [SerializeField] private GameObject _pathTilePrefab;
    [SerializeField] private GameObject _baseTilePrefab;
    [SerializeField] private GameObject _deadEndTilePrefab;

    public static Dictionary<Vector2Int, GameObject> PathTiles;
    public static Dictionary<Vector2Int, GameObject> SpawnerTiles;

    private void Start()
    {
        PathTiles = new Dictionary<Vector2Int, GameObject>();
        SpawnerTiles = new Dictionary<Vector2Int, GameObject>();

        Grid grid = new Grid(_width, _height);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 tilePosition = new Vector3((x - (float)_width / 2 + 0.5f), (y - (float)_height / 2 + 0.5f));
                if (grid.Tiles[x, y].Type == TileType.Building)
                {
                    var createdTile = Instantiate(_buildingTilePrefab, tilePosition, Quaternion.identity, transform);
                }
                else if (grid.Tiles[x, y].Type == TileType.Path)
                {
                    var createdTile = Instantiate(_pathTilePrefab, tilePosition, Quaternion.identity, transform);
                    PathTiles.Add(new Vector2Int(x, y), createdTile);
                    createdTile.GetComponent<PathTile>().NextToBaseTile = grid.Tiles[x, y].PreviousTile;
                    createdTile.GetComponent<PathTile>().Distance = grid.Tiles[x, y].Distance;
                }
                else if (grid.Tiles[x, y].Type == TileType.Spawner)
                {
                    var createdTile = Instantiate(_deadEndTilePrefab, tilePosition, Quaternion.identity, transform);
                    PathTiles.Add(new Vector2Int(x, y), createdTile);
                    SpawnerTiles.Add(new Vector2Int(x, y), createdTile);
                    createdTile.GetComponent<SpawnerTile>().NextToBaseTile = grid.Tiles[x, y].PreviousTile;
                    // Debug.Log(grid.Tiles[x, y].PreviousTile);
                    createdTile.GetComponent<SpawnerTile>().Distance = grid.Tiles[x, y].Distance;
                }
                else if (grid.Tiles[x, y].Type == TileType.Base)
                {
                    var createdTile = Instantiate(_baseTilePrefab, tilePosition, Quaternion.identity, transform);
                    PathTiles.Add(new Vector2Int(x, y), createdTile);
                    createdTile.GetComponent<BaseTile>().NextToBaseTile = grid.Tiles[x, y].Position;
                    createdTile.GetComponent<BaseTile>().Distance = grid.Tiles[x, y].Distance;
                }
            }
        }


    }
}
