using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGridController : MonoBehaviour
{
    public static WorldGridController instance;
    private void Awake() {
        instance = this;
    }

    private Grid worldGrid;
    
    public List<GridCell> allTiles = new List<GridCell>();
    public List<GameObject> allTileObjects = new List<GameObject>();

    public GameObject rockCell;
    public GameObject treeCell;

    private void Start() {
        worldGrid = GetComponent<Grid>();
        
        GenerateRocks();
        GenerateTrees();
    }

    private void GenerateTrees() {
        NewTree(new Vector2Int(3,2));
        NewTree(new Vector2Int(-3,-2));
        NewTree(new Vector2Int(0,-3));
    }

    private void GenerateRocks() {
        NewRock(new Vector2Int(8, 4));
        NewRock(new Vector2Int(9, -4));
        NewRock(new Vector2Int(7, 1));
    }

    private void NewTree(Vector2Int pos) {
        var newTree = new GridCell(pos.x, pos.y, true, false, false);
        allTiles.Add(newTree);
        
        var gridPos = new Vector3Int(pos.x, pos.y, 0);
        var temp = worldGrid.CellToWorld(gridPos);
        var newTreeObj = Instantiate(treeCell, temp, Quaternion.identity, transform);
        allTileObjects.Add(newTreeObj);
    }
    
    private void NewRock(Vector2Int pos) {
        var newRock = new GridCell(pos.x, pos.y, false, true, false);
        allTiles.Add(newRock);
        
        var gridPos = new Vector3Int(pos.x, pos.y, 0);
        var temp = worldGrid.CellToWorld(gridPos);
        var newRockObj = Instantiate(rockCell, temp, Quaternion.identity, transform);
        allTileObjects.Add(newRockObj);
    }

    private void InstantiateTiles() {
        foreach (var tile in allTiles) {
            var gridPos = new Vector3Int(tile.gridLoc.x, tile.gridLoc.y, 0);
            var temp = worldGrid.CellToWorld(gridPos);
            if (tile.hasRock) {
                Instantiate(rockCell, temp, Quaternion.identity, transform);
            }else if (tile.hasTree) {
                Instantiate(treeCell, temp, Quaternion.identity, transform);
            }
            
        }
    }

    public GridCell GetTile(Vector3Int tileGridPos) {
        for (int i = 0; i < allTiles.Count; i++) {
            if (allTiles[i].gridLoc.x == tileGridPos.x && allTiles[i].gridLoc.y == tileGridPos.y) {
                return allTiles[i];
            }
        }
        return null;
    }

    public GameObject GetResource(Vector3Int gridPos) {
        for (int i = 0; i < allTileObjects.Count; i++) {
            if (allTileObjects[i].transform.position == new Vector3(gridPos.x, gridPos.y, 0)) {
                return allTileObjects[i];
            }
        }
        return null;
    }
    
    public bool IsTileWalkable(Vector3Int tileGridPos) {
        GridCell tile = GetTile(tileGridPos);
        if (GetTile(tileGridPos) == null) {
            return true;
        } else {
            return tile.walkable;
        }
    }
    
    public bool IsTileTree(Vector3Int tileGridPos) {
        GridCell tile = GetTile(tileGridPos);
        if (GetTile(tileGridPos) == null) {
            return false;
        } else {
            return tile.hasTree;
        }
    }
    
    public bool IsTileRock(Vector3Int tileGridPos) {
        GridCell tile = GetTile(tileGridPos);
        if (GetTile(tileGridPos) == null) {
            return false;
        } else {
            return tile.hasRock;
        }
    }
}
