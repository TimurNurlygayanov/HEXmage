using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusObject : MonoBehaviour
{
    private IEnumerator SmoothLerp(Vector3 finalPos, float time)
    {
        Vector3 startingPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Move(Vector3 finalPos, float duration=1f)
    {
        StartCoroutine(SmoothLerp(finalPos, duration));
    }
}
