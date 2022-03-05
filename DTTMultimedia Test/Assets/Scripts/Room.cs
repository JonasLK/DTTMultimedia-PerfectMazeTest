using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject spawner;
    [HideInInspector] public GameObject spawnedFromSpawner;
    public List<GameObject> walls;

    public void AwakeRoom()
    {
        foreach(GameObject wall in walls) //add the walls to universal wall list to pick from
        {
            ScriptManager.mazeBase.walls.Add(wall);
        }

        MazeBase.spawnState = MazeBase.SpawnState.FindClosestSpawner;
        
    }

    public void FindClosestSpawner() //check for every wall and then every adjacent spawner to find the closest one and safe that in the wall script of that wall
    {
        foreach(GameObject wall in walls)
        {
            foreach(GameObject adjacentSpawner in spawner.GetComponent<SpawnerBase>().adjacentSpawners)
            {
                float distanceFromWallToSpawner = Vector3.Distance(wall.transform.position, adjacentSpawner.transform.position);
                if (distanceFromWallToSpawner < ScriptManager.mazeBase.roomDistance * 0.7f)
                {
                    wall.GetComponent<Wall>().closestSpawner = adjacentSpawner;
                    if(adjacentSpawner.GetComponent<SpawnerBase>().hasARoom == true)
                    {
                        ScriptManager.mazeBase.walls.Remove(wall);
                        if(adjacentSpawner == spawnedFromSpawner)
                        {
                            wall.SetActive(false);
                        }
                    }
                }
            }
            if(wall.GetComponent<Wall>().closestSpawner == null)
            {
                ScriptManager.mazeBase.walls.Remove(wall);
            }
        }
        MazeBase.spawnState = MazeBase.SpawnState.SpawnRoom;
    }
}