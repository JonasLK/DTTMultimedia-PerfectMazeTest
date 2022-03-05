using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;

    [HideInInspector] public float cameraOriginX;
    [HideInInspector] public float cameraOriginZ;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime);
    }

    public void SetCameraOrigin(Vector3 origin)
    {
        cameraOriginX = origin.x;
        cameraOriginZ = origin.z;
    }

    public void ResetCameraToOrigin()
    {
        transform.position = new Vector3(cameraOriginX, 200, cameraOriginZ);
    }
}
