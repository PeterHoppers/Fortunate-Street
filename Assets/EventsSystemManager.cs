using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class EventsSystemManager : Singleton<EventsSystemManager>
{
    [Header("Component References")]
    public EventSystem eventSystem;
    public InputSystemUIInputModule inputSystemUIInputModule;

    public void SetCurrentSelectedGameObject(GameObject newSelectedGameObject)
    {
        eventSystem.SetSelectedGameObject(newSelectedGameObject);
        Button newSelectable = newSelectedGameObject.GetComponent<Button>();
        newSelectable.Select();
        newSelectable.OnSelect(null);
    }

    public void UpdateActionAssetToFocusedPlayer()
    {
        Player focusedPlayerController = GameManager.Instance.GetActivePlayer();
        inputSystemUIInputModule.actionsAsset = focusedPlayerController.controller.GetActionAsset();
    }

    public InputActionAsset GetInputActionAsset()
    {
        return inputSystemUIInputModule.actionsAsset;
    }
}
