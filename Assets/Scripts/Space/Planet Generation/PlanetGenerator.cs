using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetGenerator : MonoBehaviour {
    //Triangulation on points calculated with Poisson distribution on the surface of a sphere

    public float planetRadius;
    public int nVertices;
    public int precision;
    public float vertexRadius;
    public bool generatePlanet;


    private static PlanetSkeleton _baseSkeleton;

    // Use this for initialization
    void Start() {
        generatePlanet = false;
    }


    public static GameObject GenerateTerrestrialPlanet()
    {
        PlanetSpecs customSpecs = GetPlanetSpecs(PlanetPreset.Default);

        

        if(_baseSkeleton == null)
        {
            _baseSkeleton = GeneratePlanetSkeleton(customSpecs, Vector3.zero);
        }

        PlanetSkeleton p = _baseSkeleton.Copy();
        TerrestrialPlanet planet = p.planetObject.GetComponent<TerrestrialPlanet>();
        planet.Type = PlanetType.Terrestrial;

        planet.skeleton = p;

        return p.planetObject;
    }


    // Update is called once per frame
    void Update() {
        if (generatePlanet)
        {
            GenerateTerrestrialPlanet();
            generatePlanet = false;
        }
    }

    #region private generator functions
    private static PlanetSkeleton GeneratePlanetSkeleton(PlanetSpecs planetSpecs, Vector3 location)
    {

        PlanetSkeleton p = new PlanetSkeleton(location, planetSpecs.radius);
        p = GenerateIcosphere(p, planetSpecs.lod, planetSpecs.radius);

        return p;
    }


    //Use the same package method to generate asteroids when the time comes
    //Create mesh for the planet BEFORE making asteroids


    //Icosphere method for generating planets
    public  static PlanetSkeleton GenerateIcosphere(PlanetSkeleton p, int lod, float radius)
    {

        float t = (1 + Mathf.Sqrt(5)) / 2;

        List<Vector3> icoPositions = new List<Vector3>
        {
        new Vector3(-1, t, 0),
        new Vector3(1, t, 0),
        new Vector3(-1, -t, 0),
        new Vector3( 1, -t,  0),

        new Vector3(0, -1, t),
        new Vector3(0, 1, t),
        new Vector3(0, -1, -t),
        new Vector3(0, 1, -t),

        new Vector3(t, 0, -1),
        new Vector3(t, 0, 1),
        new Vector3(-t, 0, -1),
        new Vector3(-t, 0, 1)
    };


        List<Vertex> icoVerts = new List<Vertex>();
        List<GameObject> icoVertObjects = new List<GameObject>();
        for(int i = 0; i < icoPositions.Count; i++)
        {
            icoVerts.Add(MakeVertex(icoPositions[i].normalized*p.radius, p));
        }

        List<Triangle> faces = new List<Triangle>()
        {

            new Triangle(0, 11, 5, icoVerts),
            new Triangle(0, 5, 1, icoVerts),
            new Triangle(0, 1, 7, icoVerts),
            new Triangle(0, 7, 10, icoVerts),
            new Triangle(0, 10, 11, icoVerts),

            new Triangle(1, 5, 9, icoVerts),
            new Triangle(5, 11, 4, icoVerts),
            new Triangle(11, 10, 2, icoVerts),
            new Triangle(10, 7, 6, icoVerts),
            new Triangle(7, 1, 8, icoVerts),

            new Triangle(3, 9, 4, icoVerts),
            new Triangle(3, 4, 2, icoVerts),
            new Triangle(3, 2, 6, icoVerts),
            new Triangle(3, 6, 8, icoVerts),
            new Triangle(3, 8, 9, icoVerts),

            new Triangle(4, 9, 5, icoVerts),
            new Triangle(2, 4, 11, icoVerts),
            new Triangle(6, 2, 10, icoVerts),
            new Triangle(8, 6, 7, icoVerts),
            new Triangle(9, 8, 1, icoVerts)

        };


        //Subdivide the icosphere

        Dictionary<string, int> indexHash = new Dictionary<string, int>();


        for (int i = 0; i < lod; i++)
        {
            List<Triangle> subFaces = new List<Triangle>();
            foreach (Triangle tri in faces)
            { 
                //Subdivide the edges of the triangle
                Vertex midA = MakeMidVertex(tri.a, tri.b, ref indexHash, p);
                Vertex midB = MakeMidVertex(tri.a, tri.c, ref indexHash, p);
                Vertex midC = MakeMidVertex(tri.b, tri.c, ref indexHash, p);

                //Create 4 triangles based on the new points 
                subFaces.Add(new Triangle(midB, midA, tri.a));
                subFaces.Add(new Triangle(midC, midA, tri.b));
                subFaces.Add(new Triangle(midC, midB, tri.c));
                subFaces.Add(new Triangle(midA, midB, midC));

                //Create 4 more triangles based on the new points to eliminate backface culling
                //Make sure to swap their normal vectors
                subFaces.Add(new Triangle(tri.a, midA, midB));
                subFaces.Add(new Triangle(tri.b, midA, midC));
                subFaces.Add(new Triangle(tri.c, midB, midC));
                subFaces.Add(new Triangle(midC, midB, midA));

            }
            faces.Clear();
            faces.AddRange(subFaces);
        }

        p.faces.AddRange(faces);
        return p;
    }

    static Vertex MakeMidVertex(Vertex a, Vertex b,  
       ref Dictionary<string, int> indexHash, PlanetSkeleton p)
    {
        Vector3 mid = GetMiddlePoint(a.position, b.position);
        mid = mid.normalized * p.radius;

        string midIndexHash = MakeMidIndexHash(a.index, b.index);

        if (!indexHash.ContainsKey(midIndexHash))
        {
            Vertex midVertex = MakeVertex(mid, p);
            indexHash.Add(midIndexHash, midVertex.index);
            return midVertex;
        }
        else
        {
            return p.vertices[indexHash[midIndexHash]];
        }

    }

    static string MakeMidIndexHash(int indexA, int indexB)
    {
        bool firstIsSmaller = indexA < indexB;
        int smallerIndex = firstIsSmaller ? indexA : indexB;
        int greaterIndex = firstIsSmaller ? indexB : indexA;

        return smallerIndex + " " + greaterIndex;
    }

   static Vector3 GetMiddlePoint(Vector3 v1, Vector3 v2)
   {

        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z)/2.0f;
    }


    private static Vertex MakeVertex(Vector3 position, PlanetSkeleton planetSkeleton)
    {
        Vertex vertex = new Vertex(planetSkeleton.vertices.Count, position);
        planetSkeleton.vertices.Add(vertex);
        return vertex;
    }


    private static Vector3 GeneratePointOnTangentCircle(Vector3 tangentCenter, float tangentRadius, Vector3 objectCenter, float objectRadius)
    {
        //Generate  random point on the edge of a circle, then move it to the edge of the sphere
        Vector2 point2D = (Random.insideUnitCircle.normalized * tangentRadius);
        Vector3 point = new Vector3(point2D.x, 0, point2D.y);
        Vector3 delta = tangentCenter - objectCenter;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.down, delta);
        Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        point = transformMatrix.MultiplyPoint3x4(point);
        point += tangentCenter;

        //Make sure the point is adjacent with the sphere instead of on a tangent plane
        point = (point - objectCenter).normalized * objectRadius;
        point += objectCenter;
        return point;
    }

    private static PlanetSpecs GetPlanetSpecs(PlanetPreset preset)
    {

        switch (preset)
        {
            //For now, all planets will be large planets
            case PlanetPreset.Large:
                //Planet radius, number of vertices, min placement radius
                return new PlanetSpecs(100, 1, 10);

            case PlanetPreset.Medium:
                return new PlanetSpecs(50, 1, 10);

            case PlanetPreset.Default:
                return new PlanetSpecs(100, 3, 10);

            default:
                return new PlanetSpecs();

        }

    }
  
    #endregion
}


[System.Serializable]
public class PlanetSpecs
{
    public float radius;
    public int nVertices;
    public int lod;
    public float vertexRadius;

    public PlanetSpecs()
    {
        Debug.LogError("Error! This planet spec does not exist yet");
    }

    public PlanetSpecs(float radius, int lod, float minRadius)
    {
        this.radius = radius;
        this.vertexRadius = minRadius;
        this.lod = lod;
    }

    //Make a third constructor if I need to vary precision
}



public enum PlanetType
{
    Gas,
    Terrestrial,
    Moon,
    Station

}

//Generating planets will be based on planet presets
public enum PlanetPreset
{

    Tiny,
    Medium,
    Large,
    Default
   

}

//Generating asteroids will be based on asteroid presets
enum AsteroidPreset
{
    Tiny,
    Small,
    Medium,
    Large

}
