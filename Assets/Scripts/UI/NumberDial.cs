using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberDial : MonoBehaviour
{
    public Button upArrow;
    public Button downArrow;
    public TMP_Text numberDisplay;

    int number;

    void Start()
    {
        upArrow.onClick.AddListener(IncrementNumber);
        downArrow.onClick.AddListener(DecrementNumber);
    }

    public void IncrementNumber()
    {
        number++;

        if (number > 9)
        {
            number = 0;
        }

        UpdateDisplay();
    }

    public void DecrementNumber()
    {
        number--;

        if (number < 0)
        {
            number = 9;
        }

        UpdateDisplay();
    }

    public int GetValue()
    {
        return number;
    }

    public void SetValue(int newNumber)
    {
        number = newNumber;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        numberDisplay.text = number.ToString();
    }
}
