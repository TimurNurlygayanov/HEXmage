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

        generateMap();

        foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            character.GetComponent<Character>().InitiateCharacter(grid);
        }

        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.GetComponent<GameController>().Init(grid);
    }

    // Update is called once per frame
    void generateMap()
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

                if (Vector3.Distance(new_position, transform.position) > radius)
                {
                    continue;
                }


                    PathNode new_tile = null;
                bool blocked = false;

                if (-1 <= z + x && z + x <= 1)
                {
                    new_tile = Instantiate(this.hex_field_with_grass);
                }
                else
                {
                    // select the field type
                    int field_type = Random.Range(1, 14);

                    if (field_type < 9)
                    {
                        new_tile = Instantiate(this.hex_field_with_grass);
                    }
                    else
                    if (field_type < 10)
                    {
                        new_tile = Instantiate(this.hex_field_with_stone);
                        blocked = true;
                    }
                    else if (field_type < 11)
                    {
                        new_tile = Instantiate(this.hex_field_with_tree1);
                        blocked = true;
                    }
                    else if (field_type < 12)
                    {
                        new_tile = Instantiate(this.hex_field_with_tree2);
                        blocked = true;
                    } else
                    {
                        new_tile = Instantiate(this.hex_field_with_grass);
                    }
                }

                if (z % 2 == 0)
                {
                    new_tile.transform.position = new Vector3(x * xOffset, 0.0f, z * zOffset);
                }
                else
                {
                    new_tile.transform.position = new Vector3(x * xOffset + xOffset / 2, 0.0f, z * zOffset);
                }

                setTileInfo(new_tile, x + maxTileX, z + maxTileZ, blocked);
            }
        }

        // cleanMap();
    }

    void setTileInfo(PathNode tile, int x, int z, bool blocked)
    {
        tile.transform.parent = transform;
        // PathNode node = tile.AddComponent<PathNode>();
        tile.x = x;
        tile.z = z;

        // mark all trees as blocked fields
        if (blocked) tile.status = "blocked";

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
                if (Vector3.Distance(grid.gridArray[i, j].transform.position, transform.position) > radius)
                {
                    Destroy(grid.gridArray[i, j].gameObject);
                    grid.gridArray[i, j] = null;
                }
            }
        }
    }
}
