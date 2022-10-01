using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUi : MonoBehaviour
{
    //[SerializeField] private Transform 

    private void Start() {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if(isBusy)
        {
            Show();
        } else {
            Hide();
        }
    }
}
