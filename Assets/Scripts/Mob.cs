using UnityEngine;

public class Mob : MonoBehaviour, IMob
{
    private float _speed = 2f;
    private float _health = 10f;
    private Vector2Int _nextTile;
    public float DistanceToBase;

    private void Start()
    {
        CalculateDistance();
    }

    void Update()
    {
        Move();
        if (DistanceToBase == 0)
        {
            Kill();
        }
    }

    public void Move()
    {   
        Vector3 direction = GridController.PathTiles[_nextTile].transform.position - transform.position;
        direction.Normalize();
        Vector3 translateVector = direction * _speed * Time.deltaTime;
        float distanceToNextTile = Vector3.Distance(GridController.PathTiles[_nextTile].transform.position, transform.position);

        if (translateVector.magnitude < distanceToNextTile)
        {
            transform.position += translateVector;
            DistanceToBase -= translateVector.magnitude;
        }
        else 
        {
            transform.position = GridController.PathTiles[_nextTile].transform.position;
            DistanceToBase = GridController.PathTiles[_nextTile].GetComponent<PathTile>().Distance;
            _nextTile = GridController.PathTiles[_nextTile].GetComponent<PathTile>().NextToBaseTile;
        }
    }

    public void SetNextTileToBase(Vector2Int tile)
    {
        _nextTile = tile;
    }

    public void Damage(float damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
        {
            Kill();
        }
    }

    private void CalculateDistance()
    {
        DistanceToBase = GridController.PathTiles[_nextTile].GetComponent<PathTile>().Distance + Vector3.Distance(GridController.PathTiles[_nextTile].transform.position, transform.position);
    }

    private void Kill()
    {
        GameController.Instance.Mobs.Remove(gameObject);
        Destroy(gameObject);
    }
}
