using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isPlayer1;
    [SerializeField] private bool isPlayer2;


    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;
    private HealthSystem healthSystem;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
    }
    void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsPlayer1() && !TurnSystem.Instance.IsPlayer1Turn()) ||
        (!IsPlayer1() && TurnSystem.Instance.IsPlayer1Turn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);

        }
    }

    public bool IsPlayer1()
    {
        return isPlayer1;
    }

    public bool IsPlayer2()
    {
        return isPlayer2;
    }

    public void Damange(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        Destroy(gameObject);
    }

}
