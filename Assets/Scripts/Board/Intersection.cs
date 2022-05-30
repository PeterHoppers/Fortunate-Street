using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public BoardSection[] connectingSections;
    Space space;

    private void Start()
    {
        space = GetComponent<Space>();
    }
}
