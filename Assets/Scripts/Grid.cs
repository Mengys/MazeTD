using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public TileData[,] Tiles;
    private int _width;
    private int _height;
    private TileData _baseTile;

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        GenerateGrid();
        TransformBuildingTilesIntoPath();
        GenerateMaze(GetRandomStartPositionForMazeGeneration());
        ResetTiles();
        FindBase();
        FindDeadEnds();
        GeneratePathToTile(_baseTile.Position.x, _baseTile.Position.y, 0);
    }

    private void GenerateGrid()
    {
        if (_width < 5 || _width % 2 == 0 || _height < 5 || _height % 2 == 0)
        {
            Debug.Log("Width and height must be not even and higher than 4");
            return;
        }  
        Tiles = new TileData[_width,_height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileData tileData = new TileData(TileType.Building, x, y);
                Tiles[x, y] = tileData;
            }
        }
    }

    private void TransformBuildingTilesIntoPath()
    {
        for (int x = 1; x < _width; x = x + 2)
        {
            for (int y = 1; y < _height; y = y + 2)
            {
                Tiles[x, y].Type = TileType.Path;
            }
        }
    }

    private void GenerateMaze(TileData start)
    {
        List<TileData> notVisitedNeighbours = GetNotVisitedNeighboursForMazeGeneration(start);
        start.IsVisited = true;

        if (notVisitedNeighbours.Count > 0)
        {
            TileData randomNeighbour = notVisitedNeighbours[Random.Range(0, notVisitedNeighbours.Count)];

            Tiles[(start.Position.x + randomNeighbour.Position.x) / 2, (start.Position.y + randomNeighbour.Position.y) / 2].Type = TileType.Path;

            GenerateMaze(randomNeighbour);
            GenerateMaze(start);
        }
    }

    private List<TileData> GetNotVisitedNeighboursForMazeGeneration(TileData mainTile)
    {
        List<TileData> notVisitedNeighbours = new List<TileData>();

        //left
        if (mainTile.Position.x != 1)
        {
            if (!Tiles[mainTile.Position.x - 2,mainTile.Position.y].IsVisited)
            {
                notVisitedNeighbours.Add(Tiles[mainTile.Position.x - 2, mainTile.Position.y]);
            }
        }

        //right
        if (mainTile.Position.x != _width - 2)
        {
            if (!Tiles[mainTile.Position.x + 2,mainTile.Position.y].IsVisited)
            {
                notVisitedNeighbours.Add(Tiles[mainTile.Position.x + 2, mainTile.Position.y]);
            }
        }

        //bottom
        if (mainTile.Position.y != 1)
        {
            if (!Tiles[mainTile.Position.x,mainTile.Position.y - 2].IsVisited)
            {
                notVisitedNeighbours.Add(Tiles[mainTile.Position.x, mainTile.Position.y - 2]);
            }
        }

        //top
        if (mainTile.Position.y != _height - 2)
        {
            if (!Tiles[mainTile.Position.x,mainTile.Position.y + 2].IsVisited)
            {
                notVisitedNeighbours.Add(Tiles[mainTile.Position.x, mainTile.Position.y + 2]);
            }
        }

        return notVisitedNeighbours;
    }

    private TileData GetRandomStartPositionForMazeGeneration()
    {
        return Tiles[Random.Range(0, (_width - 1) / 2) * 2 + 1,Random.Range(0, (_height - 1) / 2) * 2 + 1];
    }

    private void ResetTiles()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tiles[x, y].Reset();
            }
        }
    }

    public void FindBase()
    {
        GeneratePathToTile(1, 1, 0);
        TileData maxDistanceTile = GetMaxDistanceTile();
        ResetTiles();
        GeneratePathToTile(maxDistanceTile.Position.x, maxDistanceTile.Position.y, 0);
        maxDistanceTile = GetMaxDistanceTile();
        int maxDistance = maxDistanceTile.Distance;
        TileData tmp = maxDistanceTile;
        while (tmp.Distance != maxDistance/2)
        {
            tmp = Tiles[tmp.PreviousTile.x,tmp.PreviousTile.y];
        }
        tmp.Type = TileType.Base;
        _baseTile = tmp;
        ResetTiles();
    }

    private void GeneratePathToTile(int x, int y, int distance)
    {
        Tiles[x, y].Distance = distance;
        Tiles[x, y].IsVisited = true;
        foreach (TileData neighbour in GetNeighbours(Tiles[x,y]))
        {
            if ((neighbour.Type == TileType.Path || neighbour.Type == TileType.Base || neighbour.Type == TileType.Spawner ) && !neighbour.IsVisited)
            {
                neighbour.PreviousTile = Tiles[x, y].Position;
                GeneratePathToTile(neighbour.Position.x, neighbour.Position.y, distance + 1);
            }
        }
    }

    private TileData GetMaxDistanceTile()
    {
        TileData maxDistanceTile = new TileData(TileType.Path, -1, -1);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (maxDistanceTile.Distance < Tiles[x,y].Distance)
                {
                    maxDistanceTile = Tiles[x, y];
                }
            }
        }
        return maxDistanceTile;
    }

    private void FindDeadEnds()
    {
        for (int x = 1; x < _width; x += 2)
        {
            for (int y = 1; y < _height; y += 2)
            {
                int pathNeighbourCount = 0;
                foreach (TileData neighbour in GetNeighbours(Tiles[x,y]))
                {
                    if (neighbour.Type == TileType.Path || neighbour.Type == TileType.Base)
                        pathNeighbourCount++;
                }
                
                if (pathNeighbourCount == 1)
                {
                    Tiles[x, y].Type = TileType.Spawner;
                }
            }
        }
    }

    private List<TileData> GetNeighbours(TileData mainTile)
    {
        List<TileData> neighbours = new List<TileData>();
        neighbours.Add(Tiles[mainTile.Position.x - 1, mainTile.Position.y]);
        neighbours.Add(Tiles[mainTile.Position.x + 1, mainTile.Position.y]);
        neighbours.Add(Tiles[mainTile.Position.x, mainTile.Position.y - 1]);
        neighbours.Add(Tiles[mainTile.Position.x, mainTile.Position.y + 1]);
        return neighbours;
    }
}
