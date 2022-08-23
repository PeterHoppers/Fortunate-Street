using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// A prompt that appears in the middle of the screen with some text and a button to close it.
/// Used to inform the player that something is about to happen (like shops are closing)
/// </summary>
public class GenericPromptUI : MonoBehaviour
{
    public TMP_Text text;
    public Button closeUIBtn;

    /// <summary>
    /// If closing the display prompt needs to do something else as well,
    /// this allows an action (a function with no parameters and returns void)
    /// to be passed in and be called as well
    /// </summary>
    /// <param name="messageToDisplay"></param>
    /// <param name="calledFunction"></param>
    public void DisplayPrompt(string messageToDisplay, UnityAction calledFunction)
    {
        gameObject.SetActive(true);
        text.text = messageToDisplay;
        closeUIBtn.onClick.AddListener(ClosePrompt);
        closeUIBtn.onClick.AddListener(calledFunction);
        closeUIBtn.gameObject.SetActive(true);
    }

    public void DisplayPrompt(string messageToDisplay)
    {
        gameObject.SetActive(true);
        text.text = messageToDisplay;
        closeUIBtn.gameObject.SetActive(false);
    }    

    public void ClosePrompt()
    {
        gameObject.SetActive(false);
    }
}

