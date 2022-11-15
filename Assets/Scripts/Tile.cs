using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _color;
    
    public Vector2Int Position;

    protected void SetColor()
    {
        GetComponent<SpriteRenderer>().color = _color;
    }
}
