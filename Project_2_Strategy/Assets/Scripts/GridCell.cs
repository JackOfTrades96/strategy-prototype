using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
	public Vector2Int gridLoc;

	public bool walkable;
	public bool hasTree;
	public bool hasRock;

	public  GridCell(int x, int y, bool tree, bool rock, bool walk) {
		gridLoc = new Vector2Int(x, y);
		hasTree = tree;
		hasRock = rock;
		walkable = walk;
	}
}
