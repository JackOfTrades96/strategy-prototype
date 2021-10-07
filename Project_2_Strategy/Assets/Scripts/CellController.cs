using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class CellController : MonoBehaviour
{
    public GridCell cell;

    public void GatherResource() {
        WorldGridController.instance.allTiles.Remove(cell);
        WorldGridController.instance.allTileObjects.Remove(gameObject);
        Destroy(gameObject);
    }
}
