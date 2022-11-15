using UnityEngine;

public class TileData
{
    public TileType Type { get; set; }
    public bool IsVisited { get; set; }
    public Vector2Int PreviousTile { get; set; }
    public int Distance { get; set; }
    public Vector2Int Position { get; set; }

    public TileData(TileType type, int x, int y)
    {
        Type = type;
        Position = new Vector2Int(x, y);
        IsVisited = false;
        PreviousTile = new Vector2Int(0, 0);
        Distance = 0;
    }

    public void Reset()
    {
        IsVisited = false;
        PreviousTile = new Vector2Int(0, 0);
        Distance = 0;
    }
}
