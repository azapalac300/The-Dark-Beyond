using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    
    public int index;
    private HashSet<int> neighborIndices;
    public List<Vertex> neighbors;
    public List<Triangle> triangles;
    public float radius;
    public Vector3 position;

    public float elevation; //Distance factor from center. So far can be 0, 0.5f, or 1

    public Cell cell;




    public Vertex(int index, Vector3 position)
    {
        this.index = index;
        this.position = position;
        neighborIndices = new HashSet<int>();
        neighbors = new List<Vertex>();
        radius = 0.5f;
        cell = new Cell(this);
    }

    //Hash based on index value
    public override bool Equals(object other)
    {
        return index == ((Vertex)other).index;

    }

    public override int GetHashCode()
    {
        return index.GetHashCode();
    }

    public void Update()
    {
        DrawNeighborLines(Color.cyan);
    }

    private void DrawNeighborLines(Color c)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {
                try
                {
                    Debug.DrawLine(position, neighbors[i].position, c);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Something went wrong");
                }
            }
        }
    }


    public bool HasNeighbor(Vertex v)
    {
        return neighborIndices.Contains(v.index);
    }

    public void AddNeighbor(Vertex v)
    {

        neighbors.Add(v);
        neighborIndices.Add(v.index);
        v.neighbors.Add(this);
        v.neighborIndices.Add(this.index);
    }

    public void RemoveNeighbor(Vertex v)
    {
        if (HasNeighbor(v))
        {
            v.neighbors.Remove(this);
            v.neighborIndices.Remove(this.index); 
            neighbors.Remove(v);
            neighborIndices.Remove(v.index);
        }
    }

    public List<Vertex> GetSharedNeighbors(Vertex v)
    {
        List<Vertex> sharedNeighbors = new List<Vertex>();

        for (int i = 0; i < neighbors.Count; i++)
        {
            if (v.HasNeighbor(neighbors[i]))
            {
                sharedNeighbors.Add(neighbors[i]);
            }
        }

        return sharedNeighbors;
    }


    public List<Triangle> Triangulate()
    {
        HashSet<Triangle> triangleHash = new HashSet<Triangle>();
        List<Triangle> triangles = new List<Triangle>();

        for (int i = 0; i < neighbors.Count; i++)
        {

            List<Vertex> sharedNeighbors = GetSharedNeighbors(neighbors[i]);

            for (int j = 0; j < sharedNeighbors.Count; ++j)
            {
                Triangle t = new Triangle(this, neighbors[i], sharedNeighbors[j]);
                if (!triangleHash.Contains(t))
                {
                    triangleHash.Add(t);
                    triangles.Add(t);
                }
            }
        }

        this.triangles = triangles;
        return triangles;
    }
}

