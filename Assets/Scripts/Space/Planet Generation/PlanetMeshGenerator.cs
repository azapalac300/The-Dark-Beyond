using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetMeshGenerator : MonoBehaviour {
    public const int resolution = 50;
    public static void GenerateMesh(PlanetSkeleton skeleton)
    {
        GameObject planet = skeleton.planetObject;
        MeshFilter f = planet.GetComponent<MeshFilter>();
        MeshRenderer M = planet.GetComponent<MeshRenderer>();
        M.material = (Material)Resources.Load("PlanetDiffuse");

        f.mesh = GeneratePlanetMesh(skeleton);
      

    }

    public static Mesh GeneratePlanetMesh(PlanetSkeleton skeleton)
    {
        List<Triangle> triangles = skeleton.faces;
        Mesh planetMesh = new Mesh();

        
        List<int> tris = new List<int>();
        List<Vector3> verts = new List<Vector3>();

        for (int i = 0; i < triangles.Count; i++)
        {
            tris.AddRange(triangles[i].IndexList);
        }

        for (int i = 0; i < skeleton.vertices.Count; i++)
        {
            verts.Add(skeleton.vertices[i].position);
        }


        planetMesh.vertices = verts.ToArray();
        planetMesh.triangles = tris.ToArray();
  
        planetMesh.normals = MakeNormals(verts, Vector3.zero);

        
        return planetMesh;
    }

    public static void  UpdateUVs(Mesh mesh,  Vector3 planetPos, ColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[mesh.vertices.Length];

     
        //For each vertex, calculate its biome

        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector3 normalizedPos = (mesh.vertices[i]).normalized;
            uv[i] = new Vector2(colorGenerator.CalculateBiome(normalizedPos), 0);

        }

        mesh.uv = uv;
    }


    private static Vector3[] MakeNormals(List<Vector3> points, Vector3 center)
    {
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < points.Count; i++)
        {
            //Unit vector from the vertex to the origin
            Vector3 d = (points[i] - center).normalized;
            //The shader flips the texture, then the normal map flips it again

            normals.Add(new Vector3(-d.x, d.y, d.z));

        }

        return normals.ToArray();
    }

    private static List<Triangle> MakeTriangles(PlanetSkeleton p)
    {
        List<Vertex> vertices = p.vertices;
        List<Triangle> triangles = new List<Triangle>();
        HashSet<Triangle> triangleHash = new HashSet<Triangle>();

        for (int i = 0; i < vertices.Count; i++)
        {

                for (int j = 0; j < vertices[i].neighbors.Count; j++)
                {
                    List<Vertex> sharedNeighbors = vertices[i].GetSharedNeighbors(vertices[i].neighbors[j]);

                    for (int k = 0; k < sharedNeighbors.Count; k++)
                    {
                        Triangle t = new Triangle(vertices[i], vertices[i].neighbors[j], sharedNeighbors[k]);

                        if (!triangleHash.Contains(t))
                        {
                            triangles.Add(t);
                            triangleHash.Add(t);
                        }
      
                    }


                }

        }
        return triangles;

    }


}

public class Triangle
{
    public Vertex a;
    public Vertex b;
    public Vertex c;

    public Vector3 vertexA { get { return a.position; } }
    public Vector3 vertexB { get { return b.position; } }
    public Vector3 vertexC { get { return c.position; } }

    public int indexA;
    public int indexB;
    public int indexC;

    public List<int> IndexList { get {
            return new List<int>
            {
                indexA,
                indexB,
                indexC
            };
        } }


    public Triangle(int aIndex, int bIndex, int cIndex, List<Vertex> vertices)
    {
        indexA = aIndex;
        indexB = bIndex;
        indexC = cIndex;

        a = vertices[indexA];
        b = vertices[indexB];
        c = vertices[indexC];
        SetupNeighbors();
    }

    public Triangle Copy(List<Vertex> vertices)
    {
        return new Triangle(indexA, indexB, indexC, vertices);
    }


    private void SetupNeighbors()
    {
        //Neighbor the vertices together for mesh manipulation
        //Neighbor connections go both ways 
        a.AddNeighbor(b);
        a.AddNeighbor(c);
        b.AddNeighbor(c);
    }

    public Triangle(Vertex a, Vertex b,  Vertex c)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        indexA = a.index;
        indexB = b.index;
        indexC = c.index;
        SetupNeighbors();
    }

}

