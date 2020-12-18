using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
   
    Vector3 playerPosition;

    public float renderDistance;
    public float scaleFactor;
    public GameObject testVertex;
    public float tileSize;
    public TerrainSkeleton terrainSkeleton;

    [SerializeField]
    public AnimationCurve heightFunction;

    public float maxHeight;
    //make tiles 10x10 for now
    //Use vertices as guidance points for a grid mesh
    //Use binary marching squares (on or off quads) to simulate LOD

    public bool setupTerrain;
    // Start is called before the first frame update
    void Start()
    {
        //SetupTerrain();
        //Load position data once I have it
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(Vector3.zero, new Vector3(0, 80000, 0), Color.white);
        
        //UpdateTerrain();
    }


    public void SetupTerrain(TerrestrialPlanet planet)
    {
        //For now, spawn the player at a random location on the map

        terrainSkeleton = new TerrainSkeleton();

        List<Vertex> vertices = planet.skeleton.vertices;
        playerPosition = vertices[Random.Range(0, vertices.Count)].position;
        GameObject o = Instantiate(testVertex, playerPosition, Quaternion.identity);
        Vector3 planetPosition = planet.skeleton.position;

        Vector3 relativePos = (playerPosition - planetPosition).normalized;
        Quaternion rotation = Quaternion.FromToRotation(relativePos, Vector3.up);

       
        GameObject points = new GameObject();

      
        //Place vertices, using a scaling matrix to make them bigger

        //make sure we are always on top of the sphere

        for (int i = (int)(-renderDistance/tileSize); i < renderDistance/tileSize; i++)
        {
            for(int j = (int)(-renderDistance/tileSize); j < renderDistance/tileSize; j++)
            {
                

                Vector3 position = new Vector3(i*tileSize, 0, j * tileSize);

                if (Vector3.Distance(position, Vector3.zero) < renderDistance)
                {
                    //Use spatial hashing instead of a list of lists
                    GridPoint gridPoint = new GridPoint();
                    gridPoint.position = position;
                    
                    gridPoint.x = i;
                    gridPoint.y = j;
                    gridPoint.UpdateGridPoint();
                    terrainSkeleton.grid.Add(gridPoint);
                    terrainSkeleton.MakeTile(gridPoint);
                }
            }

        }

        ShapeTerrain(planet, vertices, points, terrainSkeleton.grid);
        //create mesh
        Mesh m = terrainSkeleton.GenerateMesh();
        GameObject surface = new GameObject();
        surface.name = "Ground";
        MeshRenderer renderer = surface.AddComponent<MeshRenderer>();
        surface.AddComponent<MeshCollider>();
        renderer.material = (Material)Resources.Load("Materials/DiffuseGreen");
        MeshFilter filter = surface.AddComponent<MeshFilter>();
        filter.mesh = m;
        //planet.gameObject.SetActive(false);
    }

    void ShapeTerrain(TerrestrialPlanet planet, List<Vertex> vertices, GameObject points, Grid grid)
    {
        Vector3 planetPosition = planet.skeleton.position;

        Vector3 relativePos = (playerPosition - planetPosition).normalized;
        Quaternion rotation = Quaternion.FromToRotation(relativePos, Vector3.up);

        Vector3 transformVector = -1 * planetPosition + (new Vector3(0, -planet.skeleton.radius * scaleFactor, 0));
        Matrix4x4 scalingMatrix = Matrix4x4.TRS(transformVector, rotation, Vector3.one * scaleFactor);
        playerPosition = scalingMatrix.MultiplyPoint3x4(playerPosition);

        float searchRadius = planet.skeleton.radius/10 * scaleFactor * 4 / tileSize;

        for (int i = 0; i < vertices.Count; i++)
        {

            Vector3 groundPos = scalingMatrix.MultiplyPoint3x4(vertices[i].position);
           
            if (Vector3.Distance(groundPos, playerPosition) < renderDistance)
            {

                AlterTerrainPoints(groundPos, searchRadius, vertices[i], grid);

            }
            

        }

    }

    void AlterTerrainPoints(Vector3 objectPosition, float searchRadius, Vertex vertex, Grid grid)
    {
        //Attempt to alter the terrain without raycasting
        Vector3 flatPos = new Vector3((int)objectPosition.x, 0, (int)objectPosition.z);
        Vector3 keyPos = flatPos / tileSize;

        Debug.DrawLine(flatPos, flatPos + new Vector3(0, 80000, 0), Color.white, 50);

        float maxDist = tileSize * searchRadius;

        for(int x = (int)(keyPos.x-searchRadius); x < (int)(keyPos.x + searchRadius); x++)
        {

            for (int y = (int)(keyPos.z - searchRadius); y < (int)(keyPos.z + searchRadius); y++)
            {


                if(grid.Has(x, y)){
                    float distFactor = (maxDist - Vector3.Distance(grid[x, y].position, flatPos))/maxDist;
                    //for now, do linear height mapping
                    float eval = heightFunction.Evaluate(distFactor);
                    grid[x, y].position.y =  Mathf.Max(grid[x, y].position.y, maxHeight * vertex.elevation * eval);

                   /* if (grid[x, y].position.y > 0)
                    {
                        Debug.Log("Vertex Elevation: " + vertex.elevation + "Grid position y: " + grid[x, y].position.y);
                    }*/
                    grid[x, y].height = Mathf.Max(grid[x,y].height, vertex.elevation * eval);
                    grid[x, y].inMap = true;

                }
               
             
            }

        }
        
    }


    Vertex TransformVertex(Vertex v)
    {
        return v;
    }
}



public class TerrainSkeleton
{
    public List<Vector3> anchorPoints;
    public Grid grid;
    HashSet<string> tileKeys;

    public List<Tile> tiles;

    public TerrainSkeleton()
    {
        anchorPoints = new List<Vector3>();
        grid = new Grid();
        tiles = new List<Tile>();
        tileKeys = new HashSet<string>();

    }
    public List<Tile> GenerateTiles()
    {
        //Using a dictionary guarantees no repeat tiles.
        HashSet<string> keys = new HashSet<string>();

        for(int i = -grid.xSize; i < grid.xSize; i++)
        {
            for(int j = -grid.xSize; j < grid.ySize; j++)
            {
                string key = Grid.Key(i, j);
                if (!keys.Contains(key) && grid.Has(key))
                {
                    GridPoint gp = grid[i, j];

                   

                }
                
                
            }
        }
        return tiles;
    }

    public void MakeTile(GridPoint gp)
    {
        if (!gp.edge && !tileKeys.Contains(gp.key) && grid.Has(gp.key))
        {
            Tile t = new Tile(gp);
            tiles.Add(t);

            tileKeys.Add(gp.key);
            tileKeys.Add(gp.b.key);
            tileKeys.Add(gp.c.key);
            tileKeys.Add(gp.d.key);
        }
    }

    public Mesh GenerateMesh()
    {
       
        Mesh mesh = new Mesh();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        //Get vertices
        for(int i = 0; i < grid.count; i++)
        {
            //draw height map
            Color c = Color.black;
            if (grid[i].inMap)
            {
                c = new Color(grid[i].height, 1 - grid[i].height, 0.5f * grid[i].height);
               
            }

            vertices.Add(grid[i].position);
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            triangles.AddRange(tiles[i].T1);
            triangles.AddRange(tiles[i].T2);
        }



        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.RecalculateNormals();
        return mesh;

    }
}

public class GridPoint
{
    public Vector3 position;
    public GridPoint b, c, d;
    public int index;
    public float height;
    public bool inMap;
    
    public bool edge { get { return (b == null || c == null || d == null); } }
    public int x;
    public int y;
    public string key { get { return Grid.Key(x, y);  } }

    public void UpdateGridPoint()
    {
        bool flagA = false;
        bool flagB = false;

       
    }
    
}





public class Tile
{
    public GridPoint a, b, c, d;

    //Four triangles to eliminate backface culling
    public List<int> T1, T2, T3, T4;
    public Tile(GridPoint gp)
    {
        a = gp;
        b = gp.b;
        c = gp.c;
        d = gp.d;

        T1 = new List<int>();
        T2 = new List<int>();
        T3 = new List<int>();
        T4 = new List<int>();
        MakeTris();
    }

    public void MakeTris()
    {


        T1.Add(b.index);
        T1.Add(a.index);
        T1.Add(c.index);

        T1.Add(b.index);
        T1.Add(c.index);
        T1.Add(a.index);

        T2.Add(b.index);
        T2.Add(d.index);
        T2.Add(c.index);

        T2.Add(b.index);
        T2.Add(c.index);
        T2.Add(d.index);

    }

    public void Draw()
    {
        Debug.DrawLine(a.position, b.position, Color.red);
        Debug.DrawLine(a.position, c.position, Color.red);
        Debug.DrawLine(a.position, d.position, Color.red);
        Debug.DrawLine(b.position, c.position, Color.red);
        Debug.DrawLine(b.position, d.position, Color.red);
        Debug.DrawLine(c.position, d.position, Color.red);
    }
}

public class Grid
{
    private Dictionary<string, GridPoint> gridPoints;
    private Dictionary<int, string> gpIndices;
    public int xSize { get; private set; }
    public int ySize { get; private set; }
    public int count { get; private set; }

    public Grid( )
    {

        xSize = 0;
        ySize = 0;
        gridPoints = new Dictionary<string, GridPoint>();
        gpIndices = new Dictionary<int, string>();
    }

    public static string Key(int x, int y)
    {
        string s = x.ToString() + " " + y.ToString();
        return s;
    }

    public void Add(GridPoint gp)
    {
        //Set neighbors


        if (!gridPoints.ContainsKey(Key(gp.x, gp.y)))
        {

            
            if (gridPoints.ContainsKey(Key(gp.x - 1, gp.y)))
            {
                gp.b = gridPoints[Key(gp.x - 1, gp.y)];
                if(Vector3.Distance(gp.position, gp.b.position) > 3000)
                {

                    Debug.Log("Key A: " + gp.key + " Key B: " + gp.b.key);
                }
            }

            if (gridPoints.ContainsKey(Key(gp.x, gp.y - 1)))
            {
                gp.c = gridPoints[Key(gp.x, gp.y - 1)];
                if (Vector3.Distance(gp.position, gp.c.position) > 3000)
                {
                    Debug.Log("Key A: " + gp.key + " Key C: " + gp.c.key); ;
                }
            }

            if (gridPoints.ContainsKey(Key(gp.x - 1, gp.y - 1)))
            {
                gp.d = gridPoints[Key(gp.x - 1, gp.y - 1)];
                if (Vector3.Distance(gp.position, gp.d.position) > 3000)
                {
                    Debug.Log("Key A: " + gp.key + " Key D: " + gp.d.key); ;
                }
            }
            gp.index = count;
            gridPoints.Add(gp.key, gp);
            gpIndices.Add(count++, gp.key);

            xSize = Mathf.Max(xSize, gp.x);
            ySize = Mathf.Max(ySize, gp.y);

            //drawLinks(gp);

        }

    }



    public static void drawLinks(GridPoint gridPoint, Color c)
    {
        if (!gridPoint.edge)
        {
            Debug.DrawLine(gridPoint.position, gridPoint.b.position, c, 50);
            Debug.DrawLine(gridPoint.position, gridPoint.c.position, c, 50);
            Debug.DrawLine(gridPoint.position, gridPoint.d.position, c, 50);
            Debug.DrawLine(gridPoint.b.position, gridPoint.c.position, c, 50);
            Debug.DrawLine(gridPoint.b.position, gridPoint.d.position, c, 50);
            Debug.DrawLine(gridPoint.c.position, gridPoint.d.position, c, 50);
        }
    }
    public bool Has (string key)
    {
        return gridPoints.ContainsKey(key);
    }
    public bool Has(int i, int j)
    {
        return gridPoints.ContainsKey(Key(i, j));
    }
    public bool Has(int i)
    {
        return gridPoints.ContainsKey(gpIndices[i]);
    }

    //Access operator
    public GridPoint this[int i, int j]{
        get  { return gridPoints[Key(i, j)]; } 
    }
    
    public GridPoint this[int i]
    {
        get { return gridPoints[gpIndices[i]]; } 
    }
}