using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseUI : MonoBehaviour
{
    [SerializeField] private Button createMapBtn;
    [SerializeField] private TMP_InputField inputWidth;
    [SerializeField] private TMP_InputField inputHeight;
    [SerializeField] private TMP_InputField inputCount;

    int width;
    int height;
    int count;
    private void Start()
    {
        createMapBtn.onClick.AddListener(() =>
        {
            if (int.TryParse(inputWidth.text, out width) && int.TryParse(inputHeight.text, out height))
            {
                LevelGrid.Instance.CreateGrid(width, height);
            } 

            if (int.TryParse(inputCount.text, out count))
            {
                UnitActionSystem.Instance.GenerateUnit(width, height, count);
            } 

            gameObject.SetActive(false);

        });
    }

    

    
}
