using UnityEngine;

public class GridController : MonoBehaviour
{
    public static Grid Grid { get; private set; }

    private void Awake()
    {
        Grid = GetComponent<Grid>();
    }

    public static Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return Grid.WorldToCell(worldPosition);
    }

    public static Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return Grid.CellToWorld(cellPosition);
    }
}
