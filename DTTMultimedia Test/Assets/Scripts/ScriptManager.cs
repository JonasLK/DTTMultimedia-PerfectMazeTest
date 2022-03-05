using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static MazeBase mazeBase;
    public static CameraMovement cameraMovement;

    public void Awake()
    {
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
    }
}