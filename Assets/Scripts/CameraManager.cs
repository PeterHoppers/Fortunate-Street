using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool isFollowingPlayer;
    public Vector3 distanceAway;

    Player followingPlayer;

    void OnEnable()
    {
        GameManager.OnPlayerActiveChange += SetFollowingPlayer;
    }

    void OnDisable()
    {
        GameManager.OnPlayerActiveChange -= SetFollowingPlayer;
    }    

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            //this should probably be redone to lerp to the player's position to create a smoother camera
            transform.position = followingPlayer.transform.position + distanceAway;
        }
    }

    public void SetFollowingPlayer(Player player)
    {
        followingPlayer = player;
    }
}
