using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CustomResourceManagement;
using TreeEditor;
using Unity.Profiling.LowLevel;

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
    public bool CanPlace(Vector2Int cellPosition, Vector2Int cellSize)
    {
        Vector2Int origin = MouseToOrigin(cellPosition, cellSize);

        for (int x = 0; x < cellSize.x; x++)
            for (int y = 0; y < cellSize.y; y++)
            {
                var cell = new Vector2Int(origin.x + x, origin.y + y);
                if (!CanPlace(cell))
                    return false;
            }

        return true;
    }

    private bool CanPlace(Vector2Int cellPosition)
    {
        return !occupiedCells.Contains(cellPosition);
    }

    public void Occupy(Vector2Int cellPosition, Vector2Int cellSize)
    {
        Vector2Int origin = MouseToOrigin(cellPosition, cellSize);

        for (int x = 0; x < cellSize.x; x++)
            for (int y = 0; y < cellSize.y; y++)
            {
                var cell = new Vector2Int(origin.x + x, origin.y + y);
                occupiedCells.Add(cell);
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

    // Pivot: far left bottom
    public Vector3 GetFootprintPivotWorld(Vector2Int originCell, Vector2Int size)
    {
        Vector3 originWorld = CellToWorld(new Vector3Int(originCell.x, 0, originCell.y));
        Vector3 half = new Vector3(size.x * grid.cellSize.x, 0f, size.y * grid.cellSize.z) * 0.5f;

        return originWorld + half;
    }

    public void DrawFootprintCells(Vector2Int centerPosition, Vector2Int cellSize)
    {
        if (quadMesh == null || greenMaterial == null || redMaterial == null) return;
        if (cellSize.x <= 0 || cellSize.y <= 0) return;

        greenMaterial.enableInstancing = true;
        redMaterial.enableInstancing = true;

        greenMatrices.Clear();
        redMatrices.Clear();

   
        Vector2Int origin = MouseToOrigin(centerPosition, cellSize);

        Vector3 baseScale = grid != null
            ? new Vector3(grid.cellSize.x, 1f, grid.cellSize.z)
            : Vector3.one;

        Quaternion rot = Quaternion.Euler(90f, 0f, 0f);

 
        for (int x = 0; x < cellSize.x; x++)
            for (int y = 0; y < cellSize.y; y++)
            {
                Vector2Int cell = new(origin.x + x, origin.y + y);

                Vector3 world = (grid != null)
                    ? grid.GetCellCenterWorld(new Vector3Int(cell.x, 0, cell.y))
                    : new Vector3(cell.x + 0.5f, 0f, cell.y + 0.5f);

                if (Terrain.activeTerrain != null)
                {
                    float terrainY = Terrain.activeTerrain.SampleHeight(world)
                                   + Terrain.activeTerrain.transform.position.y;
                    world.y = terrainY;
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
    }

    public Vector2Int MouseToOrigin(Vector2Int mouseCell, Vector2Int size)
    {
        int ox = size.x / 2;
        int oy = size.y / 2;
        return new Vector2Int(mouseCell.x - ox, mouseCell.y - oy);
    }
}