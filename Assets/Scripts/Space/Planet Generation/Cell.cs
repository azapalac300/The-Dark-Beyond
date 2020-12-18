using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Cell exists to deal with terrain generation metadata just like Vertex exists to deal with mesh generation
public class Cell {
    public Vertex parent;
    public List<Cell> neighbors;


    public float heightVal;

    public Cell(Vertex v)
    {
        parent = v;
        neighbors = new List<Cell>();

        for(int i = 0; i < parent.neighbors.Count; i++)
        {
            neighbors.Add(parent.neighbors[i].cell);
        }
    }
    
}
