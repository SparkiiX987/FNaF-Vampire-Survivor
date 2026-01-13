using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    
    public FlowField(MapBounds _mapBonds, Vector2Int[] _directions)
    {
        for (int x = _mapBonds.mapStartPoint.x; x < _mapBonds.mapSize.x; x++)
        {
            for (int y = _mapBonds.mapStartPoint.y; y < _mapBonds.mapSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                cells[position] = new Cell
                {
                    cellPosition = position,
                    baseCost = 1,
                };
            }
        }

        foreach (KeyValuePair<Vector2Int, Cell> cell in cells)
        {
            List<Cell> neighbours = new List<Cell>();

            foreach (Vector2Int dir in _directions)
            {
                Vector2Int potentialNeighbourPosition = cell.Key + dir;
                if (cells.ContainsKey(potentialNeighbourPosition))
                {
                    neighbours.Add(cells[potentialNeighbourPosition]);
                }
            }

            cell.Value.neighbours = neighbours.ToArray();
        }
    }
}

public class Cell
{
    public Vector2Int cellPosition;
    public Vector2 direction = Vector2.one;
    public float cost = float.MaxValue;
    public float baseCost;
    public List<AIBehaviour> agent = new();
    public Cell[] neighbours;
    public bool isWalkable = true;
    public float force = 1;

    public void SetIsWalkable(bool _isWalkable)
    {
        isWalkable = _isWalkable;
        baseCost = _isWalkable ? 1 : 1000000;
    }
}

[System.Serializable]
public struct MapBounds
{
    public Vector2Int mapStartPoint; 
    public Vector2Int mapSize; 
}