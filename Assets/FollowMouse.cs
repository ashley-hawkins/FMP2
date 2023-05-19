using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = (Vector3Int.FloorToInt(cam.ScreenToWorldPoint(Input.mousePosition) / 16) * 16);
        newPosition.x += 8;
        newPosition.y += 8;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
