using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialReader : MonoBehaviour
{
    public NumberDial[] numberDials;

    void Start()
    {
        SetValue(27);
    }

    /// <summary>
    /// Convert the value provided into being rendered on the attached number dials
    /// </summary>
    /// <param name="number"></param>
    public void SetValue(int number)
    {
        string numberString = number.ToString();
        int digits = numberString.Length;

        //if the number is longer than the number of dials we have, throw an error
        if (digits > numberDials.Length)
        {
            Debug.LogError($"{number} has more digits than the number of dials attached to {this.name}");
        }
        //if we don't have enough digits, add '0' to make the same amount of digits
        else if (digits < numberDials.Length)
        {
            numberString = numberString.PadLeft(numberDials.Length, '0');
        }

        for (int i = 0; i < numberDials.Length; i++)
        {
            string targetValue = numberString[i].ToString();
            int dialValue = int.Parse(targetValue);
            numberDials[i].SetValue(dialValue);
        }
    }

    public int GetValue()
    {
        string numberString = "";

        foreach (NumberDial dial in numberDials)
        {
            numberString += dial.GetValue().ToString();
        }

        return int.Parse(numberString);
    }

    public void PrintValue()
    {
        print($"The value is {GetValue()}");
    }
}
