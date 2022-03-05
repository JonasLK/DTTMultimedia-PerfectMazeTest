using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    public bool hasARoom;
    public GameObject visualSquare;
    public GameObject Button;
    public GameObject room;
    public List<GameObject> adjacentSpawners;

    public void Awake()
    {
        GetComponent<SphereCollider>().radius = ScriptManager.mazeBase.roomDistance * 1.1f;
    }

    public void addAdjacentSpawners()
    {
        visualSquare.SetActive(false);
        foreach (GameObject spawner in ScriptManager.mazeBase.roomSpawners)
        {
            if (Vector3.Distance(gameObject.transform.position, spawner.transform.position) < ScriptManager.mazeBase.roomDistance * 1.1f && spawner != gameObject)
            {
                adjacentSpawners.Add(spawner);
            }
        }
    }

    public void OnTriggerEnter(Collider o)
    {
        if(o.isTrigger == false && o.gameObject.layer == 3)
        {
            adjacentSpawners.Add(o.gameObject);
        }
    }

    public void SetActiveButton()
    {
        Button.SetActive(!Button.activeSelf);
    }
    
    public void ChoseStartingSpawner(GameObject attachedSpawner)
    {
        ScriptManager.mazeBase.selectedStartSpawner = attachedSpawner;
        ScriptManager.mazeBase.SetActiveStartSpawnerSelectingButtons();
        MazeBase.spawnState = MazeBase.SpawnState.StartSpawnProcess;
    }
}