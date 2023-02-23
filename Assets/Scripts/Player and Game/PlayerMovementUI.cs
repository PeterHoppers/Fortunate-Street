using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUI : MonoBehaviour
{
    public GameObject arrow;
    Transform playerCanvas;
    Canvas arrowCanvas;
    List<GameObject> arrowList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerCanvas = GetComponentInChildren<Canvas>().transform;
        arrowCanvas =  GetComponentInChildren<Canvas>();
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
        float xMidpoint;
        float zMidpoint;
        //GameObject newArrow = Instantiate(arrow, playerCanvas.transform);
        //RectTransform rectTransform = newArrow.GetComponent<RectTransform>();
        // === Calculates the Midpoint coordinates for where the arrow should be placed === \\

        Debug.Log("Current Space-" + currentSpace.name + ": " + currentSpace.transform.position + " --- TargetSpace -" + targetSpace.name + ": " + targetSpace.transform.position);
        if(currentSpace.transform.position.x != targetSpace.transform.position.x)
        {
            xMidpoint = (currentSpace.transform.position.x + targetSpace.transform.position.x) / 2;
            //Debug.Log("X Mid: " + xMidpoint);
        }
        else
        {
            xMidpoint = currentSpace.transform.position.x;
            //Debug.Log("Same X Point: " + xMidpoint);
        }

        if(currentSpace.transform.position.z != targetSpace.transform.position.z)
        {
            zMidpoint = (currentSpace.transform.position.z + targetSpace.transform.position.z) / 2;
            //Debug.Log("Z Mid: " + zMidpoint);
        }
        else
        {
            zMidpoint = currentSpace.transform.position.z;
            //Debug.Log("Same Z Point: " + zMidpoint);
        }

        // === Sets angle for arrow to be flat and point to an available space === \\
        Vector3 tarDir = targetSpace.transform.position - currentSpace.transform.position;
        Vector3 forward = currentSpace.transform.forward;
        float angle = Vector3.SignedAngle(tarDir, forward, Vector3.up);
        Debug.Log("Angle Calculation: " + angle);
        Quaternion rotation = Quaternion.Euler(0, angle+45, 90); // +45 required for model offset

        // === Creates the Arrow GameObject at the midpoint between spaces, and points it toward the targetspace === \\
        GameObject newArrow = Instantiate(arrow, new Vector3(xMidpoint, 0.5f, zMidpoint), rotation);

        arrowList.Add(newArrow);
    }

    // === Removes old arrows before creating the new ones === \\
    public void RemoveOldArrows()
    {
        if(arrowList.Count > 0 || arrowList != null)
        {
            foreach (var arrow in arrowList)
            {
                Destroy(arrow.gameObject);
            }
            arrowList.Clear();
        }
    }
}
