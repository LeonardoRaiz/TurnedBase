using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    public event EventHandler OnGenereteUnity;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private Unit player2Unit;
    [SerializeField] private LayerMask unitPlayerMask;

    private BaseAction selectedAction;
    private bool isBusy;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe mais de um Sistema de ac�o das unidades" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;


    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }


    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();

    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mousePosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mousePosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mousePosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);

            // switch (selectedAction)
            // {
            //     case MoveAction moveAction:
            //         if (moveAction.IsValidActionGridPosition(mousePosition))
            //         {
            //             SetBusy();
            //             moveAction.Ta(mousePosition, ClearBusy);
            //         }
            //         break;
            //     case SpinAction spinAction:
            //         SetBusy();
            //         spinAction.Spin(ClearBusy);
            //         break;
            // }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitPlayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }

                    if (selectedUnit.IsPlayer1())
                    {
                        if (unit.IsPlayer2())
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (selectedUnit.IsPlayer2())
                        {
                            if (unit.IsPlayer1())
                            {
                                return false;
                            }
                        }
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        //if(OnSelectedUnitChanged != null)
        //{
        //OnSelectedUnitChanged(this, EventArgs.Empty);
        //}
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void ChangeUnit()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition allGridPosition = new GridPosition(x, z);
                List<Unit> unitList = LevelGrid.Instance.GetUnitAtGridPosition(allGridPosition);

                if (selectedUnit.IsPlayer1())
                {

                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (unitList[i].IsPlayer2())
                        {
                            SetSelectedUnit(unitList[i]);
                            return;
                        }
                    }
                }
                else if (selectedUnit.IsPlayer2())
                {

                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (unitList[i].IsPlayer1())
                        {
                            SetSelectedUnit(unitList[i]);
                            return;
                        }
                    }
                }
            }
        }

    }

    public void GenerateUnit(int width, int height, int countUnity)
    {
        List<Vector3> list = new List<Vector3>(new Vector3[countUnity]);
        for (int i = 0; i < countUnity - 1; i++)
        {
            var position = new Vector3(evenNumber(width), 0, evenNumber(height));
            while (list.Contains(position))
            {
                position =  new Vector3(evenNumber(width), 0, evenNumber(height));
            }

            Instantiate(selectedUnit, position, Quaternion.identity);

            list[i] = position;
        }

        for (int i = 0; i < countUnity; i++)
        {
            var position = new Vector3(evenNumber(width), 0, evenNumber(height));
            while (list.Contains(position))
            {
                position =  new Vector3(evenNumber(width), 0, evenNumber(height));
            }

            Instantiate(player2Unit, position, Quaternion.identity);

            list[i] = position;
        }

        OnGenereteUnity?.Invoke(this, EventArgs.Empty);
    }

    private int evenNumber(int number)
    {
        var x = Random.Range(0, number);
        if (x % 2 == 0)
        {
            return x;
        } else
        {
            return x + 1;
        }
    }
}

