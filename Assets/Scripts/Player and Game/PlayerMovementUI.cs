using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUI : MonoBehaviour
{
    public GameObject arrow;
    public GameObject playerCanvas;

    public Dictionary<KeyCode, Space> keysSpaces = new Dictionary<KeyCode, Space>();
    Dictionary<Vector3, KeyCode> keyDirections = new Dictionary<Vector3, KeyCode>()
        {
            {Vector3.forward, KeyCode.RightArrow},
            {Vector3.back, KeyCode.LeftArrow},
            {Vector3.left, KeyCode.UpArrow},
            {Vector3.right, KeyCode.DownArrow}
        };

    Player player;
    PlayerMovement movement;

    bool isMoving;

    void Awake()
    {
        player = GetComponent<Player>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        player.OnPlayerTurnStateChanged += CheckTurnState;
        Space.OnPlayerPass += SetupMovementOptions;
        Space.OnPlayerReverse += SetupMovementOptions;
    }

    void OnDisable()
    {
        player.OnPlayerTurnStateChanged -= CheckTurnState;
        Space.OnPlayerPass -= SetupMovementOptions;
        Space.OnPlayerReverse -= SetupMovementOptions;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            foreach (KeyCode key in keysSpaces.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    isMoving = false;
                    movement.MoveToSelectedSpace(keysSpaces[key]);
                    break;
                }
            }
        }
    }
   
    //display an arrow pointing at each space
    //calcuate the angle between the player's space and that space and match it with what key is needed to move there

    /// <summary>
    /// Activate this UI when the player starts to move
    /// </summary>
    /// <param name="state"></param>
    void CheckTurnState(TurnState state) 
    {
        isMoving = (state == TurnState.Moving);

        if (isMoving)
        {
            SetupMovementOptions(player, movement.currentSpace);
        }
    }

    void SetupMovementOptions(Player player, Space currentSpace)
    {
        if (this.player != player)
        {
            return;
        }
        //grab all the spaces we can move
        List<Space> spacesToGo = movement.GetPossibleMovementSpots();
        //List<Space> spacesToGo = currentSpace.GetPossibleSpaces(player);
        keysSpaces.Clear();

        foreach (Space space in spacesToGo)
        {
            //GameObject newArrow = Instantiate(arrow, playerCanvas.transform);
            //RectTransform rectTransform = newArrow.GetComponent<RectTransform>();

            //right now, forward is to the right
            Vector3 directionToSpace = space.transform.position - currentSpace.transform.position;

            if (keyDirections.ContainsKey(directionToSpace))
            {
                keysSpaces.Add(keyDirections[directionToSpace], space);
            }
            else 
            {
                //if it isn't a 90 degree angle from the other postion
                float angle = Vector3.Angle(currentSpace.transform.position, space.transform.position);
                //use the zOffset to figure out if we're going forwards or backwards
                Vector3 offset = space.transform.position - currentSpace.transform.position;
                float zOffset = offset.z;
                float xOffset = offset.x;

                Debug.Log($"Angle {angle}, a zOffset of {zOffset}, and a xOffset of {xOffset}");
                Debug.Log(space.transform.position - currentSpace.transform.position);

                //these x and z offsets give you the coordinates the next space is at
                // z is up and down
                // x is left and right
                //if mutliple spaces want to try and do the same angle, for now,
                //remove that key as a valid option to move so that the keys that are different remain
                if (zOffset < 0)
                {
                    CheckThenAddSpace(Vector3.back, space);
                }
                else if (zOffset > 0)
                {
                    CheckThenAddSpace(Vector3.forward, space);
                }

                if (xOffset < 0)
                {
                    CheckThenAddSpace(Vector3.left, space);
                }
                else if (xOffset > 0)
                {
                    CheckThenAddSpace(Vector3.right, space);
                }

                //rectTransform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }

        isMoving = true;
    }

    void CheckThenAddSpace(Vector3 vectorCheck, Space space)
    {
        KeyCode attemptedButton = keyDirections[vectorCheck];
        if (keysSpaces.ContainsKey(attemptedButton))
        {
            keysSpaces.Remove(attemptedButton);
        }
        else
        {
            keysSpaces.Add(attemptedButton, space);
        }
    }
}
