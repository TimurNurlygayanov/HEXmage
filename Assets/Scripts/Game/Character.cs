using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator myAnimator;
    [SerializeField] public int x;
    [SerializeField] public int z;

    public float speed = 1f;

    public int Health = 3;
    public int Manna = 3;
    public int Atletics = 1;

    private HexGrid grid;

    private List<PathNode> path = new List<PathNode>();

    private PathNode next_step = null;

    private PathNode tile;
    Vector3 new_position;
    Quaternion view_direction;

    private Material ground_material;

    private bool need_update = false;

    public Fireball fireball_prefab;


    public void Awake()
    {
        this.gameObject.SetActive(true);

        // to hide path
        ground_material = Resources.Load("Materials/Ground_0", typeof(Material)) as Material;

        myAnimator = this.GetComponent<Animator>();
    }

    public void InitiateCharacter(HexGrid grid)
    {
        this.grid = grid;

        tile = grid.gridArray[x, z];
        transform.position = tile.transform.position + new Vector3(0, 5, 0);

        // mark this field as blocked to ask path finder to ignore this field
        grid.gridArray[x, z].status = "blocked";
    }


    public void MoveByPath(List<PathNode> path_to_go) 
    {
        this.path = path_to_go;

        ClearTile(path[0]);

        this.path.Remove(this.path[0]);  // 0 node is the current position

        this.gameObject.SetActive(true);
        myAnimator.SetBool("run", true);
        this.GetComponent<Outline>().enabled = true;

        need_update = true;
    }

    private void ClearTile(PathNode tile)
    {
        tile.status = "free";
        tile.gameObject.GetComponent<Renderer>().material = ground_material;
    }

    public void SkillActivate()
    {
        Fireball fireball = Instantiate(fireball_prefab, transform.position + new Vector3(0, 1f, 0), transform.rotation * Quaternion.Euler(90f, 0f, -180f));
        fireball.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (path.Count > 0)
        {
            if (!next_step)
            {
                next_step = path[0];
                tile = grid.gridArray[next_step.x, next_step.z];

                x = next_step.x;
                z = next_step.z;

                new_position = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
                view_direction = Quaternion.LookRotation(new_position - transform.position);
            }

            if (transform.rotation != view_direction)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, view_direction, 200 * Time.fixedDeltaTime);
                view_direction = Quaternion.LookRotation(new_position - transform.position);

                // prevent character from folling
                view_direction.x = 0;
                view_direction.z = 0;

            } else if (new_position != transform.position)
            {
                // Move to the next tail:
                transform.position = Vector3.MoveTowards(transform.position, new_position, speed * Time.deltaTime);
            } else
            {
                ClearTile(next_step);
                path.Remove(next_step);
                next_step = null;
            }
        } else if (need_update)
        {
            need_update = false;

            // end of path:
            myAnimator.SetBool("run", false);

            this.GetComponent<Outline>().enabled = false;
        }
        
    }
}
