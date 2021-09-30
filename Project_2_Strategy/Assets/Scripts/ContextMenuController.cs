using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.PlayerLoop;

public class ContextMenuController : MonoBehaviour
{
    public GameObject contextMenu;
    
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            EnableContextMenu(Input.mousePosition);
        }
    }

    private Vector3 cellWorldPos;
    private GameObject currentHighlightCell;

    private void EnableContextMenu(Vector3 pos) {
        Destroy(currentHighlightCell);
        contextMenu.SetActive(true);
        contextMenu.transform.position = pos;
        Debug.Log(Input.mousePosition.x);
        Debug.Log(pos.x);
        Debug.Log(contextMenu.GetComponent<RectTransform>().sizeDelta.x);
        if (pos.x >= Screen.width / 2) {
            //Spawn on mouse left
            var temp = new Vector2(contextMenu.transform.localPosition.x - contextMenu.GetComponent<RectTransform>().sizeDelta.x, contextMenu.transform.localPosition.y);
            contextMenu.transform.localPosition = temp;
        }

        if (pos.y <= Screen.height / 2) {
            Debug.Log("ME");
            var temp = new Vector2(contextMenu.transform.localPosition.x, contextMenu.transform.localPosition.y + contextMenu.GetComponent<RectTransform>().sizeDelta.y);
            contextMenu.transform.localPosition = temp;
        }
        var camPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        cellWorldPos = new Vector3(camPos.x, camPos.y, 0);
        currentHighlightCell = GetComponent<PlayerMovement_GridBased>().HighlightCell(cellWorldPos, Color.magenta);
    }

    public void ContextMenuMoveToTile() {
        var player = GetComponent<PlayerMovement_GridBased>();
        player.GeneratePath(player.GetGridPosition(cellWorldPos));
        contextMenu.SetActive(false);
        Destroy(currentHighlightCell);
    }
}
