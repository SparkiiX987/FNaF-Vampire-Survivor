using System.Collections.Generic;
using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    [SerializeField]
    private MapBounds mapBonds;

    [SerializeField]
    private LayerMask obstaclesLayer;

    private FlowField flowField;

    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
    };

    private Cell centerCell;

    private void Start()
    {
        flowField = new(mapBonds, directions);
        centerCell = flowField.cells[new Vector2Int(0, 0)];
        centerCell.direction = Vector2.zero;
        InitializeCosts();
    }

    public bool TryGetCellFromWorld(Vector3 worldPos, out Cell cell)
    {
        Vector2Int gridPos = WorldToCell(worldPos);
        return flowField.cells.TryGetValue(gridPos, out cell);
    }


    public Vector2Int WorldToCell(Vector3 _position)
    {
        int x = Mathf.FloorToInt(_position.x);
        int y = Mathf.FloorToInt(_position.z);
        return new Vector2Int(x, y);
    }

    public Cell GetCell(Vector2Int _cellPosition)
    {
        return flowField.cells[_cellPosition];
    }

    private void OnDrawGizmosSelected()
    {
        if (flowField == null)
            return;

        Vector3 cubeCenter = new();
        Vector3 cubeSize = Vector3.one;
        Vector3 dir = new();
        foreach (KeyValuePair<Vector2Int, Cell> cell in flowField.cells)
        {
            Gizmos.color = cell.Value.isWalkable ? Color.green : Color.red;

            cubeCenter.Set(cell.Key.x, 0, cell.Key.y);

            Gizmos.DrawWireCube(cubeCenter, cubeSize);

            dir.Set(cell.Value.direction.x, 0, cell.Value.direction.y);

            DrawArrow(dir, cubeCenter);
        }
    }

    private void DrawArrow(Vector3 _direction, Vector3 _center)
    {
        Gizmos.color = Color.blue;

        float arrowLength = 0.5f;

        Vector3 start = _center - _direction * arrowLength;
        Vector3 end = _center + _direction * arrowLength;

        float headLength = 0.15f;
        float headAngle = 25f;

        Gizmos.DrawLine(start, end);

        Vector3 right = Quaternion.Euler(0, headAngle, 0) * -_direction;
        Vector3 left = Quaternion.Euler(0, -headAngle, 0) * -_direction;

        Gizmos.DrawLine(end, end + right * headLength);
        Gizmos.DrawLine(end, end + left * headLength);
    }

    private void InitializeCosts()
    {
        foreach (Cell cell in flowField.cells.Values)
        {
            cell.cost = float.MaxValue;
            cell.direction = Vector2.zero;
        }

        centerCell.cost = 0f;
    }

    [ContextMenu("RecalculFlowField")]
    public void RecalculFlowField()
    {
        CheckWalkableCells();

        ComputeCostField();

        ComputeDirectionField();
    }

    private void ComputeCostField()
    {
        InitializeCosts();

        Queue<Cell> queue = new();
        queue.Enqueue(centerCell);

        while (queue.Count > 0)
        {
            Cell current = queue.Dequeue();

            foreach (Cell neighbour in current.neighbours)
            {
                float newCost = current.cost + neighbour.baseCost;

                if (newCost < neighbour.cost)
                {
                    neighbour.cost = newCost;
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

    private void ComputeDirectionField()
    {
        foreach (Cell cell in flowField.cells.Values)
        {
            float bestCost = cell.cost;
            Cell bestNeighbour = null;

            foreach (Cell neighbour in cell.neighbours)
            {
                if (neighbour.cost < bestCost)
                {
                    bestCost = neighbour.cost;
                    bestNeighbour = neighbour;
                }
            }

            if (bestNeighbour != null)
            {
                cell.direction =
                    ((Vector2)(bestNeighbour.cellPosition - cell.cellPosition)).normalized;
            }
        }
    }

    private void CheckWalkableCells()
    {
        Vector3 sphereCastLocation = Vector3.zero;

        foreach (Cell cell in flowField.cells.Values)
        {
            sphereCastLocation.Set(cell.cellPosition.x, 0, cell.cellPosition.y);

            bool isCellBlocked = Physics.CheckSphere(sphereCastLocation, 1, obstaclesLayer);

            cell.SetIsWalkable(!isCellBlocked);
        }
    }
}
