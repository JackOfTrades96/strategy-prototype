using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuController : MonoBehaviour
{
    public GameObject contextMenu;
    
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            EnableContextMenu(Input.mousePosition);
        }
    }

    public GameObject moveButton;
    public GameObject treeButton;
    public GameObject rockButton;

    private Vector3 cellWorldPos;
    private GameObject currentHighlightCell;

    private void EnableContextMenu(Vector3 pos) {
        Destroy(currentHighlightCell);
        
        contextMenu.SetActive(true);
        contextMenu.transform.position = pos;
        //Check position to spawn the contextMenu
        if (pos.x >= Screen.width / 2) {
            //Spawn on mouse left
            var temp = new Vector2(contextMenu.transform.localPosition.x - contextMenu.GetComponent<RectTransform>().sizeDelta.x, contextMenu.transform.localPosition.y);
            contextMenu.transform.localPosition = temp;
        }
        if (pos.y <= Screen.height / 2) {
            var temp = new Vector2(contextMenu.transform.localPosition.x, contextMenu.transform.localPosition.y + contextMenu.GetComponent<RectTransform>().sizeDelta.y);
            contextMenu.transform.localPosition = temp;
        }
        var camPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        cellWorldPos = new Vector3(camPos.x, camPos.y, 0);
        Color color = new Color(1f, 1f, 0f, .25f);
        currentHighlightCell = GetComponent<PlayerMovement_GridBased>().HighlightCell(cellWorldPos, color);
        
        //Check the tile to see what actions can be taken
        var player = GetComponent<PlayerMovement_GridBased>();
        if (WorldGridController.instance.IsTileWalkable(player.GetGridPosition(cellWorldPos))) {
            moveButton.SetActive(true);
        } else {
            moveButton.SetActive(false);
        }
        if (WorldGridController.instance.IsTileRock(player.GetGridPosition(cellWorldPos))) {
            rockButton.SetActive(true);
        } else {
            rockButton.SetActive(false);
        }
        if (WorldGridController.instance.IsTileTree(player.GetGridPosition(cellWorldPos))){
            treeButton.SetActive(true);
        }else{
            treeButton.SetActive(false);
        }
    }

    public void ContextMenuMoveToTile() {
        var player = GetComponent<PlayerMovement_GridBased>();
        player.GeneratePath(player.GetGridPosition(cellWorldPos));
        contextMenu.SetActive(false);
        Destroy(currentHighlightCell);
    }

    public void ContextMenuGatherResource() {
        var player = GetComponent<PlayerMovement_GridBased>();
        player.GeneratePathToResource(player.GetGridPosition(cellWorldPos));
        player.waitingToGather = true;
        player.resourceBeingGathered = WorldGridController.instance.GetResource(player.GetGridPosition(cellWorldPos));
        contextMenu.SetActive(false);
        Destroy(currentHighlightCell);
    }

}
