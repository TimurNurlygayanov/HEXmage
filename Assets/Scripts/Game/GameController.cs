using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HexGrid grid;

    private PathNode start_node;
    private PathNode end_node;

    public PathFinder pathFinder;

    public GameStatuses status = GameStatuses.idle;

    public Character active_character;

    public List<Player> players = new List<Player>();
    public int active_player_number = 0;

    public CameraFocusObject cameraFocus;

    public void Init(HexGrid grid)
    {
        this.grid = grid;
        pathFinder = new PathFinder(grid);
    }


    public void Update()
    {
        if (status == GameStatuses.use_skill)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.transform != null)
                    {
                        GameObject target = hit.transform.gameObject;

                        active_character.skills[active_character.active_skill].Activate(target);

                        status = GameStatuses.idle;
                    }
                }
            }
        }

        if (status == GameStatuses.search_path)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.gameObject.tag == "PathNode")
                        {
                            end_node = hit.transform.gameObject.GetComponent<PathNode>();

                            pathFinder.findPath(start_node, end_node);
                            pathFinder.drawPath();

                            active_character.MoveByPath(pathFinder.full_path);

                            cameraFocus.Move(pathFinder.full_path[pathFinder.full_path.Count-1].transform.position, 3f);

                            status = GameStatuses.idle;
                        }
                    }
                }
            } else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.gameObject.tag == "PathNode")
                        {
                            end_node = hit.transform.gameObject.GetComponent<PathNode>();

                            pathFinder.clearPath();
                            pathFinder.findPath(start_node, end_node);
                            pathFinder.drawPath();
                        }
                    }
                }
            }
        }
    }


    public void SwitchTurn()
    {
        active_player_number += 1;
        if (active_player_number >= players.Count)
        {
            active_player_number = 0;
        }

        active_character = players[active_player_number].GetCharacter();
        cameraFocus.Move(active_character.transform.position, 1f);
    }


    public void startSearchPath()
    {
        active_character = players[active_player_number].GetCharacter();

        start_node = grid.gridArray[active_character.x, active_character.z];

        status = GameStatuses.search_path;
    }

    public Character GetActiveCharacter()
    {
        return this.active_character;
    }
}
