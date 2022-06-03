using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    [Tooltip("For now, the first index of connected Space needs to be included in the first section of connecting sections")]
    public BoardSection[] connectingSections;
    public Space[] connectedSpaces;

    //public Dictionary<BoardSection, Space> connectingSections;
}
