using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CustomResourceManagement;
using TreeEditor;

// TODO: Player class is getting too big and PlacementPresenter's been getting too many arguments.
public sealed class GridSystem
{
    // Hash Set is better
    private readonly HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    private readonly Grid grid;
    private readonly Mesh quadMesh;

    private readonly Material greenMaterial;
    private readonly Material redMaterial;

    private readonly List<Matrix4x4> greenMatrices  = new List<Matrix4x4>(64);
    private readonly List<Matrix4x4> redMatrices    = new List<Matrix4x4>(64);

    public GridSystem(Grid grid, Mesh quadMesh)
    {
        this.grid = grid;
        this.quadMesh = quadMesh;

        greenMaterial = ResourceLoader.GetResource<Material>(Materials.GreenMaterial);
        redMaterial = ResourceLoader.GetResource<Material>(Materials.RedMaterial);
        greenMaterial.enableInstancing = true;
        redMaterial.enableInstancing = true;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return grid.CellToWorld(cellPosition);
    }

    // origin should be center position of the building.
    public bool CanPlace(Vector2Int centerPosition, Vector2Int cellSize)
    {
        int halfSizeX = cellSize.x / 2;
        int halfSizeY = cellSize.y / 2;

        for (int offsetX = -halfSizeX; offsetX < cellSize.x - halfSizeX; offsetX++)
            for (int offsetY = -halfSizeY; offsetY < cellSize.y - halfSizeY; offsetY++)
            {
                var occupiedCell = new Vector2Int(centerPosition.x + offsetX, centerPosition.y + offsetY);
                if (!CanPlace(occupiedCell))
                    return false;
            }
        return true;
    }

    private bool CanPlace(Vector2Int cell)
    {
        return !occupiedCells.Contains(cell);
    }

    public void Occupy(Vector2Int centerPosition, Vector2Int cellSize)
    {
        int halfSizeX = cellSize.x / 2;
        int halfSizeY = cellSize.y / 2;

        for (int offsetX = -halfSizeX; offsetX < cellSize.x - halfSizeX; offsetX++)
            for (int offsetY = -halfSizeY; offsetY < cellSize.y - halfSizeY; offsetY++)
            {
                var occupiedCell = new Vector2Int(centerPosition.x + offsetX, centerPosition.y + offsetY);
                occupiedCells.Add(occupiedCell);
            }
    }

    public void Release(Vector2Int centerPosition, Vector2Int cellSize)
    {
        int halfSizeX = cellSize.x / 2;
        int halfSizeY = cellSize.y / 2;

        for (int offsetX = -halfSizeX; offsetX < cellSize.x - halfSizeX; offsetX++)
            for (int offsetY = -halfSizeY; offsetY < cellSize.y - halfSizeY; offsetY++)
            {
                var occupiedCell = new Vector2Int(centerPosition.x + offsetX, centerPosition.y + offsetY);
                occupiedCells.Remove(occupiedCell);
            }
    }

    public void DrawFootprintCells(Vector2Int centerPosition, Vector2Int cellSize)
    {

        if (quadMesh == null || greenMaterial == null || redMaterial == null) return;
        if (cellSize.x <= 0 || cellSize.y <= 0) return;

        greenMaterial.enableInstancing = true;
        redMaterial.enableInstancing = true;

        greenMatrices.Clear();
        redMatrices.Clear();

        int halfSizeX = cellSize.x / 2;
        int halfSizeY = cellSize.y / 2;

        Vector3 baseScale = grid != null
            ? new Vector3(grid.cellSize.x, 1f, grid.cellSize.z)
            : Vector3.one;

        Quaternion rot = Quaternion.Euler(90f, 0f, 0f);

        for (int offsetX = -halfSizeX; offsetX < cellSize.x - halfSizeX; offsetX++)
            for (int offsetY = -halfSizeY; offsetY < cellSize.y - halfSizeY; offsetY++)
            {
                Vector2Int cell = new(centerPosition.x + offsetX, centerPosition.y + offsetY);

                Vector3 world = (grid != null)
                    ? grid.GetCellCenterWorld(new Vector3Int(cell.x, 0, cell.y))
                    : new Vector3(cell.x + 0.5f, 0f, cell.y + 0.5f);

                if (Terrain.activeTerrain != null)
                {
                    float terrainY = Terrain.activeTerrain.SampleHeight(world)
                                   + Terrain.activeTerrain.transform.position.y;
                    world.y = terrainY + 1;
                }
                else
                {
                    world.y += 1;
                }

                var trs = Matrix4x4.TRS(world, rot, baseScale);

                if (CanPlace(cell)) greenMatrices.Add(trs);
                else redMatrices.Add(trs);
            }

        for (int i = 0; i < greenMatrices.Count; i += 1023)
        {
            int count = Mathf.Min(1023, greenMatrices.Count - i);
            Graphics.DrawMeshInstanced(quadMesh, 0, greenMaterial,
                greenMatrices.GetRange(i, count),
                null, UnityEngine.Rendering.ShadowCastingMode.Off, false);
        }
        for (int i = 0; i < redMatrices.Count; i += 1023)
        {
            int count = Mathf.Min(1023, redMatrices.Count - i);
            Graphics.DrawMeshInstanced(quadMesh, 0, redMaterial,
                redMatrices.GetRange(i, count),
                null, UnityEngine.Rendering.ShadowCastingMode.Off, false);
        }

        Debug.Log($"green:{greenMatrices.Count}, red:{redMatrices.Count}, centerPos:{centerPosition}, size:{cellSize}");
    }
}