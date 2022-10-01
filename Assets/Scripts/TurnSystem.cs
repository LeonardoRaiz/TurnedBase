using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private bool isPlayer1Turn = true;

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

    private int turnNumber = 1;

    public void NextTurn()
    {
        turnNumber++;
        isPlayer1Turn = !isPlayer1Turn;
        ChangePlayer();
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayer1Turn()
    {
        return isPlayer1Turn;
    }

    private void ChangePlayer()
    {
        UnitActionSystem.Instance.ChangeUnit();
    }

}
