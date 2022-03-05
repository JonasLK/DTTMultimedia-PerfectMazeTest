using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnevenSpawner : MonoBehaviour
{
    public List<GameObject> spawnerSpawners;

    private int counter;

    public void AwakenSpawner()
    {
        SetSpawnerPosition(ScriptManager.mazeBase.roomDistance);
        foreach (GameObject spawnerSpawner in spawnerSpawners)
        {
            foreach(GameObject spawner in ScriptManager.mazeBase.roomSpawners)
            {
                if(Vector3.Distance(spawnerSpawner.transform.position, spawner.transform.position) < ScriptManager.mazeBase.roomDistance * 0.5f)
                {
                    counter++;
                }
            }

            foreach(GameObject spawnerSpawner2 in ScriptManager.mazeBase.unevenSpawnerSpawners)
            {
                if (Vector3.Distance(spawnerSpawner.transform.position, spawnerSpawner2.transform.position) < ScriptManager.mazeBase.roomDistance * 0.5f || spawnerSpawner == spawnerSpawner2)
                {
                    counter++;
                }
            }

            if(counter <= 0 && ScriptManager.mazeBase.unevenSpawnerSpawners.Count < ScriptManager.mazeBase.roomAmount)
            {
                ScriptManager.mazeBase.unevenSpawnerSpawners.Add(spawnerSpawner);
            }
            else
            {
                counter = 0;
            }
        }

        MazeBase.spawnState = MazeBase.SpawnState.SpawnUnevenSpawner;
    }

    public void SetSpawnerPosition(float spawnerPosition)
    {
        for (int i = 0; i < spawnerSpawners.Count; i++)
        {
            if(i == 0)
            {
                spawnerSpawners[i].transform.position = new Vector3(spawnerPosition + transform.position.x, transform.position.y, transform.position.z);
            }
            else if(i == 1)
            {
                spawnerSpawners[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnerPosition);
            }
            else if(i == 2)
            {
                spawnerSpawners[i].transform.position = new Vector3(-spawnerPosition + transform.position.x, transform.position.y, transform.position.z);
            }
            else if(i == 3)
            {
                spawnerSpawners[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - spawnerPosition);
            }
        }
    }
}
