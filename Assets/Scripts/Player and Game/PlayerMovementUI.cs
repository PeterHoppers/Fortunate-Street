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
    public void SpawnArrowForSpace(Space currentSpace, Space targetSpace, Vector3 directionToNextSpace) 
    {
        //GameObject newArrow = Instantiate(arrow, playerCanvas.transform);
        //RectTransform rectTransform = newArrow.GetComponent<RectTransform>();
        Debug.Log("Dir to Next Space: " + directionToNextSpace);
        // === Calculates the Midpoint coordinates for where the arrow should be placed === \\
        float xMidpoint = currentSpace.transform.position.x + (currentSpace.transform.position.x - targetSpace.transform.position.x) / 2;
        float yMidpoint = currentSpace.transform.position.y + (currentSpace.transform.position.y - targetSpace.transform.position.y) / 2; ;
        float zMidpoint = currentSpace.transform.position.z + (currentSpace.transform.position.z - targetSpace.transform.position.z) / 2; ;

        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        // === Creates the Arrow GameObject at the midpoint between spaces, and points it toward the targetspace === \\
        GameObject newArrow = Instantiate(arrow, new Vector3(xMidpoint, 1, zMidpoint), rotation, this.transform);
        newArrow.transform.LookAt(targetSpace.transform);

        arrowList.Add(newArrow);
        //if it isn't a 90 degree angle from the other postion
        float angle = Vector3.Angle(currentSpace.transform.position, targetSpace.transform.position);

        //Debug.Log($"Angle {angle}");
        //Debug.Log(targetSpace.transform.position - currentSpace.transform.position);

        //rectTransform.rotation = Quaternion.Euler(0, angle, 0);
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
