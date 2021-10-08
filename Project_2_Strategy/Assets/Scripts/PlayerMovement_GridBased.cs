using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using TMPro;

using Unity.Mathematics;

using UnityEditor;

using UnityEngine;
using UnityEngine.Experimental.AI;
public class PlayerMovement_GridBased : MonoBehaviour
{
    [SerializeField] private Vector2 startPos; //Used to set the player's spawn point
    [SerializeField] private Grid worldGrid; //Reference to the grid gameobject that the player exists on

    [SerializeField] private List<Vector3> positionQue = new List<Vector3>(); //This is used to track the positions the player moves throughy
    private List<GameObject> pathTiles = new List<GameObject>(); //This is used to track the positions the player moves throughy
    public bool moving = false; //Used to say when the player is following the que
    private int moveIndex = 0;
    private ContextMenuController contextMenuController;

    [SerializeField] private GameObject pathSquare;

    [SerializeField] private GameObject highlightSprite;
    
    [Header("Player Settings")]
    [Range(0,1)][SerializeField] private float stepSpeed; //How fast we move between each point (1 = 1 second);
    public enum MovementType
    {
        DrawPath,
        ContextMenu
    }
    [SerializeField] private MovementType movementType;


    private void Start() {
        transform.position = startPos; //Set our position to the start
        contextMenuController = GetComponent<ContextMenuController>();
    }

    private void Update() {
        if (movementType == MovementType.DrawPath) {
             if (Input.GetMouseButton(0) && !moving) { //While the player is holding the mouse button down
                var mousePos = GridMousePos(); //Get the position of the mouse on the grid
                if (positionQue.Count > 0) { //If the Que already has stuff in it
                    if (positionQue[positionQue.Count-1] != mousePos) { //Check the mouse position against the last added mouse position
                       positionQue.Add(mousePos); //Add it if they are not the same
                       var newTile = Instantiate(pathSquare, mousePos, quaternion.identity, null);
                       pathTiles.Add(newTile);
                    }
                } else {
                    positionQue.Add(mousePos); //If the que is empty, then just add it without checking anything to start the list off
                    var newTile = Instantiate(pathSquare, mousePos, quaternion.identity, null);
                    pathTiles.Add(newTile);
                }
             }
            
             if (Input.GetMouseButtonUp(0) && !moving) { //When the player lifts the mouse button 
                 
                 if (positionQue.Count == 1) { //If there is only one position in the que then just go there
                     Move(positionQue[0]);
                 } else { //Otherwise send the position que list and use the overloaded method 
                     Move(positionQue);
                 }
             }
             contextMenuController.enabled = false;
        }else if (movementType == MovementType.ContextMenu && !GetComponent<ContextMenuController>().isActiveAndEnabled) {
            contextMenuController.enabled = true;
        }

        // if (Input.GetMouseButtonDown(1) && !moving) {
        //     GeneratePath(GridMousePos());
        // }
    }

    public Vector3Int GetGridPosition(Vector3 pos) {
        return worldGrid.WorldToCell(pos);
    }

    public void GeneratePath(Vector3 pathDest) {
        if (!moving) {
            var currentGridPos = worldGrid.WorldToCell(transform.position);
        
            var horizontal = pathDest.x - currentGridPos.x;
            var vertical = pathDest.y - currentGridPos.y;

            for (int x = 0; x < Mathf.Abs(horizontal); x++) {
                if (horizontal < 0) {
                    //Left
                    currentGridPos = new Vector3Int(currentGridPos.x -= 1, currentGridPos.y, 0);
                    positionQue.Add(currentGridPos);
                } else {
                    //Right
                    currentGridPos = new Vector3Int(currentGridPos.x += 1, currentGridPos.y, 0);
                    positionQue.Add(currentGridPos);
                }
            }

            for (int y = 0; y < Mathf.Abs(vertical); y++) {
                if (vertical > 0) {
                    //Up
                    currentGridPos = new Vector3Int(currentGridPos.x, currentGridPos.y += 1, 0);
                    positionQue.Add(currentGridPos);
                } else {
                    //Down
                    currentGridPos = new Vector3Int(currentGridPos.x, currentGridPos.y -= 1, 0);
                    positionQue.Add(currentGridPos);
                }
            }

            foreach (var position in positionQue) {
                var newTile = Instantiate(pathSquare, position, quaternion.identity, null);
                pathTiles.Add(newTile);
            }
            
            Move(positionQue);
        }
    }

    //Get mouse position as a grid cell
    public Vector3 GridMousePos() {
        var temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); //Converts the mouse pos on screen to the world
        var zeroTemp = new Vector3(temp.x, temp.y, 0); //Sets the z of that to be 0 so its not the cameras z
        return worldGrid.WorldToCell(zeroTemp); //Return the mouse position to the world, but in the form of the grid values
    }

    //Call when you want the player to move to a position (using world co-ordinates)
    public void Move(Vector3 moveTo) {
        var newPos = new Vector3(moveTo.x, moveTo.y, 0); //Set the z value to 0
        // Debug.Log("Moving to: " + worldGrid.WorldToCell(newPos));
        transform.position = worldGrid.WorldToCell(newPos); //Move the player to the new position (translated from world space to grid space)
    }

    //Method used for moving through the que
    public void Move(List<Vector3> movePositions) {
        StartCoroutine(MoveThroughQue());
        moving = true;
    }

    
    IEnumerator MoveThroughQue() {
        //Freaky little self made loop
        if (moveIndex != positionQue.Count) {
            yield return new WaitForSeconds(stepSpeed);
            Move(positionQue[moveIndex]);
            moveIndex++;
            StartCoroutine(MoveThroughQue());
        }else if (moveIndex >= positionQue.Count) {
            positionQue.Clear(); //Clear the whole path (this is a better code than the one commented out below cause it clears at the end and not during)
            moving = false; //Also the player is no longer moving
            moveIndex = 0; //Reset the moveIndex
            foreach (var path in pathTiles) {
                Destroy(path.gameObject);
            }
            pathTiles.Clear();
        }
    }

    public GameObject HighlightCell(Vector3 worldPos, Color colorToUse) {
        var temp = GetGridPosition(worldPos);
        var highlightSpot = Instantiate(highlightSprite, temp, quaternion.identity, null);
        highlightSpot.name = "Highlight Sprite";
        highlightSpot.GetComponent<SpriteRenderer>().color = colorToUse;
        return highlightSpot;
    }

}
