using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuiteUI : MonoBehaviour
{
    public Suit representingSuite;
    public Color gatheredSuiteColor; //TODO: Have a single spot that both the suite space and the UI know what color they are

    Image suiteImage;
    Color32 defaultColor;

    void Start()
    {
        suiteImage = GetComponent<Image>();
        defaultColor = suiteImage.color;
    }

    public void GotSuite()
    {
        suiteImage.color = gatheredSuiteColor;
    }

    public void SpentSuite()
    {
        suiteImage.color = defaultColor;
    }
}
