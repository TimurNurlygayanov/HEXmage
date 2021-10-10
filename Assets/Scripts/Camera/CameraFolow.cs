
using System.Collections;
using UnityEngine;

// Tutorials
// https://www.youtube.com/watch?v=urNrY7FgMao
// https://www.youtube.com/watch?v=xcn7hz7J7sI
// https://www.youtube.com/watch?v=MFQhpwc6cKE
//

public class CameraFolow : MonoBehaviour
{
    public Transform target;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.125f;

    public Vector3 offset = new Vector3(3, 4, 0);

    public float rotationSpeed = 1f;
    public float ySpeed = 0.5f;

    public float zoomSpeed = 4;

    public float minDistance = 2f;
    public float maxDistance = 7f;

    private float mouse_x = 0;
    private float mouse_y = 0;

    Vector3 relativePos;
    Quaternion rotation;

    public void Start()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    public void LateUpdate()
    {
        RotateAndZoom();

        // define new position and smoothly move the camera with interpolation
        Vector3 newPosition = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (target.gameObject.GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0))
        {
            relativePos = target.position - transform.position;
            rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100 * smoothFactor);
            // transform.LookAt(target);
        }
    }

    private void RotateAndZoom()
    {
        if (Input.GetMouseButton(1))
        {

            if (Input.GetAxis("Mouse X") > 0)
            {
                mouse_x = 0.1f;
            }
            else if (Input.GetAxis("Mouse X") < 0)
            {
                mouse_x = -0.1f;
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                mouse_y = 0.1f;
            }
            else if (Input.GetAxis("Mouse Y") < 0)
            {
                mouse_y = -0.1f;
            }

            // rotate the camera smoothly
            Quaternion camTurnAngle = Quaternion.AngleAxis(mouse_x * rotationSpeed, Vector3.up);
            offset = camTurnAngle * offset;

            // allow to find the good height of a camera
            // offset += transform.up * mouse_y * ySpeed;
        }

        // zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            transform.LookAt(target);
            offset += transform.forward * scroll * zoomSpeed;
        }

        // control distance of a camera
        if (offset.magnitude < minDistance)
        {
            offset = offset.normalized * minDistance;
        }
        if (offset.magnitude > maxDistance)
        {
            offset = offset.normalized * maxDistance;
        }
    }


}
