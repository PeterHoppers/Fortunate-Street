using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementUI : MonoBehaviour
{
    public GameObject arrowUI;
    List<GameObject> arrowList = new List<GameObject>();

    /// <summary>
    /// Display an arrow pointing at each space. Calcuates the angle between the player's space and that space and match it with what key is needed to move there
    /// </summary>
    /// <param name="state"></param>
    public void SpawnArrowsForSpace(Space currentSpace, List<Space> targetSpaces) 
    {
        foreach (Space space in targetSpaces)
        {
            SpawnArrow(currentSpace, space);
        }
    }

    void SpawnArrow(Space currentSpace, Space targetSpace)
    {
        float xMidpoint;
        float zMidpoint;
        Quaternion rotation;

        // === Calculates the Midpoint coordinates for where the arrow should be placed === \\
        if (currentSpace.transform.position.x != targetSpace.transform.position.x)
        {
            xMidpoint = (currentSpace.transform.position.x + targetSpace.transform.position.x) / 2;
        }
        else
        {
            xMidpoint = currentSpace.transform.position.x;
        }

        if (currentSpace.transform.position.z != targetSpace.transform.position.z)
        {
            zMidpoint = (currentSpace.transform.position.z + targetSpace.transform.position.z) / 2;
        }
        else
        {
            zMidpoint = currentSpace.transform.position.z;
        }

        // === Sets angle for arrow to be flat and point to an available space === \\
        Vector3 tarDir = targetSpace.transform.position - currentSpace.transform.position;
        Vector3 forward = currentSpace.transform.forward;
        float angle = Vector3.SignedAngle(tarDir, forward, Vector3.up);

        if (angle == 0 || angle == 180)
        {
            rotation = Quaternion.Euler(0, angle + 45, 90); // +45 required for model offset
        }
        else
        {
            rotation = Quaternion.Euler(0, (angle * -1) + 45, 90); // +45 required for model offset
        }

        // === Creates the Arrow GameObject at the midpoint between spaces, and points it toward the targetspace === \\
        // Let's use pooling for the arrows so that we don't keep creating and destorying them
        GameObject arrow = arrowList.Where(x => x.gameObject.activeSelf != true).FirstOrDefault();
        if (arrow != null)
        {
            arrow.transform.position = new Vector3(xMidpoint, 0.5f, zMidpoint);
            arrow.transform.rotation = rotation;
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrow = Instantiate(arrowUI, new Vector3(xMidpoint, 0.5f, zMidpoint), rotation);
        }

        arrowList.Add(arrow);
    }

    // === Removes old arrows before creating the new ones === \\
    public void RemoveOldArrows()
    {
        if(arrowList.Count > 0 || arrowList != null)
        {
            foreach (var arrow in arrowList)
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }
}
