using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DisplayWorldGrid : MonoBehaviour
{
    public int width, height;
    public GameObject gridSprite;
    public Vector2 offset;

    private void Start() {
        GenerateGrid();
    }

    private void GenerateGrid() {
        //Used so the grid doesnt start in the world 0,0,0
        var startX = -width / 2;
        var startY = -height / 2;
        //Nested for loop to get the width and height
        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                var pos = new Vector2(startX + w + offset.x, startY + h + offset.y);
                Instantiate(gridSprite, pos, quaternion.identity, transform); //Create the grid cells with the prefab provided.
            }
        }
    }
}
