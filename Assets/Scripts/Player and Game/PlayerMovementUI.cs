using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUI : MonoBehaviour
{
    public GameObject arrow;
    Transform playerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        playerCanvas = GetComponentInChildren<Canvas>().transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    //display an arrow pointing at each space
    //calcuate the angle between the player's space and that space and match it with what key is needed to move there

    /// <summary>
    /// Activate this UI when the player starts to move
    /// </summary>
    /// <param name="state"></param>
    public void SpawnArrowForSpace(Space currentSpace, Space targetSpace) 
    {
        //GameObject newArrow = Instantiate(arrow, playerCanvas.transform);
        //RectTransform rectTransform = newArrow.GetComponent<RectTransform>();

        //if it isn't a 90 degree angle from the other postion
        float angle = Vector3.Angle(currentSpace.transform.position, targetSpace.transform.position);

        Debug.Log($"Angle {angle}");
        Debug.Log(targetSpace.transform.position - currentSpace.transform.position);

        //rectTransform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
