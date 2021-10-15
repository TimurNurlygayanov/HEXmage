using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public PathNode hex_field;
    public PathNode hex_field_with_grass;
    public PathNode hex_field_with_tree1;
    public PathNode hex_field_with_tree2;
    public PathNode hex_field_with_stone;

    [SerializeField] public int mapWidth = 100;
    [SerializeField] public int mapHeight = 100;
    [SerializeField] public float radius = 10f;
    [SerializeField] public float radius_total = 10.2f;

    private float zOffset = 1.52f;
    private float xOffset = 1.76f;

    int minTileX;
    int maxTileX;
    int minTileZ;
    int maxTileZ;

    public HexGrid grid;

    private PathNode start_node;
    private PathNode end_node;

    private Vector3 new_position;


    // Start is called before the first frame update
    public void Start()
    {
        grid = new HexGrid(mapWidth, mapHeight);

        List<Character> reserved = new List<Character>();

        foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            reserved.Add(character.GetComponent<Character>());
        }

        generateMap(reserved);

        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            bool result = character.GetComponent<Character>().InitiateCharacter(grid);

        }

        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.GetComponent<GameController>().Init(grid);
    }

    // Update is called once per frame
    void generateMap(List<Character> characters)
    {
        minTileX = (int) -mapWidth / 2;
        maxTileX = (int) mapWidth / 2;
        minTileZ = (int) -mapHeight / 2;
        maxTileZ = (int) mapHeight / 2;

        for (int x = minTileX; x < maxTileX; x++)
        {
            for (int z = minTileZ; z < maxTileZ; z++)
            {
                if (z % 2 == 0)
                {
                    new_position = new Vector3(x * xOffset, 0.0f, z * zOffset);
                }
                else
                {
                    new_position = new Vector3(x * xOffset + xOffset / 2, 0.0f, z * zOffset);
                }

                if (Vector3.Distance(new_position, transform.position) > radius_total)
                {
                    continue;
                }


                PathNode new_tile = null;
                bool blocked = false;

                if (Vector3.Distance(new_position, transform.position) < radius)
                {
                    // select the field type
                    int field_type = Random.Range(1, 30);
                    bool char_field = false;

                    foreach (Character character in characters)
                    {
                        if (Mathf.Abs(x + maxTileX - character.x) + Mathf.Abs(z + maxTileZ - character.z) < 2) char_field = true;
                    }

                    if (field_type < 26 || char_field == true)
                    {
                        new_tile = Instantiate(this.hex_field_with_grass);
                    }
                    else if (field_type < 28)
                    {
                        new_tile = Instantiate(this.hex_field_with_stone);
                        blocked = true;
                    }
                    else if (field_type < 29)
                    {
                        new_tile = Instantiate(this.hex_field_with_tree1);
                        blocked = true;
                    }
                    else if (field_type <= 30)
                    {
                        new_tile = Instantiate(this.hex_field_with_tree2);
                        blocked = true;
                    }
                }


                new_tile.transform.position = new_position;
                setTileInfo(new_tile, x + maxTileX, z + maxTileZ, blocked);
            }
        }
    }

    void setTileInfo(PathNode tile, int x, int z, bool blocked)
    {
        tile.transform.parent = transform;
        tile.x = x;
        tile.z = z;

        // mark all trees as blocked fields
        if (blocked) tile.status = "blocked";

        tile.original_material = tile.GetComponent<Renderer>().material;
        tile.name = x.ToString() + ", " + z.ToString();
        tile.tag = "PathNode";

        grid.gridArray[x, z] = tile;
    }

    void cleanMap()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (grid.gridArray[i, j]) Destroy(grid.gridArray[i, j].gameObject);
                grid.gridArray[i, j] = null;
            }
        }
    }
}
