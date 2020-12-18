using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSkeleton
{
    public List<Vertex> vertices;
    public List<Triangle> faces;

    public Vector3 position;
    public GameObject planetObject;
    public float radius;


    public PlanetSkeleton( Vector3 position,  float radius)
    {
        vertices = new List<Vertex>();
        faces = new List<Triangle>();
        this.position = position;
        planetObject = ((PlanetFactory)Resources.Load("PlanetFactory")).Terrestrial();
        planetObject.transform.position = position;
        planetObject.name = "Planet";
        this.radius = radius;
    }

    public PlanetSkeleton Copy()
    {
        PlanetSkeleton copy = new PlanetSkeleton(position, radius);


        //Copy over vertices
       for(int i = 0; i < vertices.Count; i++)
        {
            copy.vertices.Add(new Vertex(vertices[i].index, vertices[i].position));
        }


        //Copy over triangles
        for(int i = 0; i < faces.Count; i++)
        {
            copy.faces.Add(faces[i].Copy(copy.vertices));
        }

        copy.faces.AddRange(faces);
        return copy;
    }




}

