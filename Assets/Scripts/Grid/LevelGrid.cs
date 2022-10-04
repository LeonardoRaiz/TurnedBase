using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    public event EventHandler OnCreateGrid;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private Transform gridFloor;
    [SerializeField] private Transform gridVisual;
    private GridSystem gridSystem;

    private int width = 1;
    private int height = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe mais de um Sistema de ação das unidades" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem(width, height, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        gridSystem.CreateFloor(gridFloor);
    }

    public void CreateGrid(int width, int height)
    {
        SetWidth(width);
        SetHeight(height);
        gridSystem = new GridSystem(width, height, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        gridSystem.CreateFloor(gridFloor);
        Instantiate(gridVisual, transform.position, transform.rotation);
        OnCreateGrid?.Invoke(this, EventArgs.Empty);
    }


    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public int GetWidth()
    {
        return gridSystem.GetWidth();
    }

    public int GetHeight()
    {
        return gridSystem.GetHeight();
    }

    public Unit GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public void SetWidth(int width)
    {
        this.width = width;
    }

    public void SetHeight(int height)
    {
        this.height = height;
    }


}
