using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HexGrid grid;

    private PathNode start_node;
    private PathNode end_node;

    public PathFinder pathFinder;

    public string status = "idle";
    public Character character;

    public void Init(HexGrid grid)
    {
        this.grid = grid;
        pathFinder = new PathFinder(grid);
    }


    public void Update()
    {
        if (status == "search_path")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.gameObject.tag == "PathNode")
                        {
                            end_node = hit.transform.gameObject.GetComponent<PathNode>();

                            end_node.GetComponent<Renderer>().material.color = Color.yellow;

                            pathFinder.findPath(start_node, end_node);
                            pathFinder.drawPath();

                            character.MoveByPath(pathFinder.full_path);

                            status = "idle";
                        }
                    }
                }


            }
        }
    }


    public void startSearchPath()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();

        start_node = grid.gridArray[character.x, character.z];
        start_node.GetComponent<Renderer>().material.color = Color.red;

        status = "search_path";
    }
}
