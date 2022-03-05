using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBase : MonoBehaviour
{
    [Header("Important variables (Fill in before starting)")]
    public float roomDistance;
    public bool randomStart;

    [Header("Important variables UnevenMaze only (Fill in before starting)")]
    public bool unevenMaze;
    public int roomAmount;

    [Header("Important variables RectangleMaze only(Fill in before starting)")]
    public int width;
    public int height;

    [Header("Things to spawn (Fill in before starting)")]
    public GameObject unevenSpawner;
    public GameObject roomSpawner;
    public GameObject startRoom;
    public GameObject stairRoom;
    public GameObject endRoom;
    public List<GameObject> toSpawnRooms;

    [Header("Spawned rooms and spawners (for viewing only)")]
    public List<GameObject> unevenSpawnerSpawners;
    public List<GameObject> roomSpawners;
    public List<GameObject> spawnedRooms;
    public List<GameObject> walls;

    [Header("Other")]
    public GameObject mazeButtons;
    [HideInInspector]public GameObject parentToSpawn;
    private GameObject parent;
    public static SpawnState spawnState;

    private bool lastRoomSpawned;

    public enum SpawnState
    {
        AwaitingInput,
        StartUnevenSpawnProcess,
        AwakenUnevenSpawner,
        SpawnUnevenSpawner,
        SpawnSquareSpawners,
        StartSpawnProcess,
        StartSpawnProcessRandom,
        AwakenRoom,
        FindClosestSpawner,
        SpawnRoom,
        Finishing,
        Finished
    }

    private Room justSpawnedRoomScript;
    private UnevenSpawner newestUnevenSpawnerScript;

    private Vector3 roomSpawnerSpawnPosition;

    [HideInInspector] public GameObject selectedStartSpawner;

    private int counter1;
    private int counter2;

    public void Awake()
    {
        walls = new List<GameObject>();
        ScriptManager.mazeBase = this; //make maze base a static so it's easily accessable
        spawnState = SpawnState.AwaitingInput;
    }

    public void StartStateMachine()
    {
        unevenSpawnerSpawners.Clear();
        spawnedRooms.Clear();
        lastRoomSpawned = false;
        //spawn everythin under parent to easily delete maze
        parent = Instantiate(parentToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        //spawn empty gameobject to make a grid
        roomSpawnerSpawnPosition = new Vector3(0, 0, 0);
        if (unevenMaze == true)
        {
            spawnState = SpawnState.StartUnevenSpawnProcess;
        }
        else
        {
            spawnState = SpawnState.SpawnSquareSpawners;
        }
    }

    public void finishedSpawning(GameObject input)
    {
        input.SetActive(!input.activeSelf);
    }

    public void DeleteMaze()
    {
        spawnState = SpawnState.AwaitingInput;
        Destroy(parent);
    }

    public void RoomAmountInputField(string input)
    {
        roomAmount = int.Parse(input);
    }

    public void HeigthInputField(string input)
    {
        height = int.Parse(input);
    }

    public void WidthInputField(string input)
    {
        width = int.Parse(input);
    }

    public void UnevenMazeButton(bool input)
    {
        unevenMaze = input;
    }

    public void RandomStartButton(bool input)
    {
        randomStart = input;
    }

    public void SpawnSquareSpawners()
    {
        if(counter1 < height)//make spawners spawn in a rectanglegrid
        {
            for (counter2 = 0; counter2 < width; counter2++)
            {
                roomSpawners.Add(Instantiate(roomSpawner, roomSpawnerSpawnPosition, Quaternion.identity, parent.transform));
                roomSpawnerSpawnPosition.z += roomDistance;
            }
            roomSpawnerSpawnPosition.x += roomDistance;
            roomSpawnerSpawnPosition.z = 0;
            counter2 = 0;
            counter1++;
        }
        else
        {
            counter1 = 0;

            if (roomSpawners.Count == height * width)
            {
                foreach (GameObject Spawner in roomSpawners)
                {
                    Spawner.GetComponent<SpawnerBase>().visualSquare.SetActive(false);
                }
            }

            //let every spawner know which spawners are around them
            if (randomStart == true) //if randomStart is true then pick a random spawner to start
            {
                spawnState = SpawnState.StartSpawnProcessRandom;
            }
            else
            {
                SetActiveStartSpawnerSelectingButtons();
            }
        }
    }

    public void SpawnUnevenSpawners(Vector3 spawnerSpawnPosition)
    {
        GameObject newestUnevenSpawner = Instantiate(unevenSpawner, spawnerSpawnPosition, Quaternion.identity, parent.transform);
        newestUnevenSpawnerScript = newestUnevenSpawner.GetComponent<UnevenSpawner>();
        roomSpawners.Add(newestUnevenSpawner);
        spawnState = SpawnState.AwakenUnevenSpawner;
    }

    public void Update()
    {
        switch (spawnState) //state machine so the spawn process doesnt happen out of order (but makes it a little slow with large mazes i found out later)
        {
            case SpawnState.AwaitingInput:
                break;

            case SpawnState.StartUnevenSpawnProcess:
                SpawnUnevenSpawners(new Vector3(0,0,0));
                break;

            case SpawnState.AwakenUnevenSpawner:
                newestUnevenSpawnerScript.AwakenSpawner();
                break;

            case SpawnState.SpawnUnevenSpawner:
                if (roomSpawners.Count == roomAmount)
                {
                    foreach(GameObject Spawner in roomSpawners)
                    {
                        Spawner.GetComponent<SpawnerBase>().visualSquare.SetActive(false);
                    }

                    if (randomStart == true) //if randomStart is true then pick a random spawner to start
                    {
                        spawnState = SpawnState.StartSpawnProcessRandom;
                    }
                    else
                    {
                        SetActiveStartSpawnerSelectingButtons();
                    }
                }
                else
                {
                    GameObject selectedUnevenSpawnerSpawner = unevenSpawnerSpawners[Random.Range(0, unevenSpawnerSpawners.Count)];
                    SpawnUnevenSpawners(selectedUnevenSpawnerSpawner.transform.position);
                    unevenSpawnerSpawners.Remove(selectedUnevenSpawnerSpawner);
                }
                break;

            case SpawnState.SpawnSquareSpawners:
                SpawnSquareSpawners();
                break;

            case SpawnState.StartSpawnProcess:
                ScriptManager.cameraMovement.SetCameraOrigin(selectedStartSpawner.transform.position);
                StartSpawnProcess(selectedStartSpawner);
                break;

            case SpawnState.StartSpawnProcessRandom:
                GameObject randomStartSpawner = roomSpawners[Random.Range(0, roomSpawners.Count)];
                ScriptManager.cameraMovement.SetCameraOrigin(randomStartSpawner.transform.position);
                StartSpawnProcess(randomStartSpawner);
                break;

            case SpawnState.AwakenRoom:
                justSpawnedRoomScript.AwakeRoom();
                break;

            case SpawnState.FindClosestSpawner:
                justSpawnedRoomScript.FindClosestSpawner();
                break;

            case SpawnState.SpawnRoom:
                SpawnRoom();
                break;

            case SpawnState.Finishing:
                mazeButtons.SetActive(!mazeButtons.activeSelf);
                spawnState = SpawnState.Finished;
                break;

            case SpawnState.Finished:
                break;

            default:
                Debug.LogError("stateChanger reached default state");
                break;
        }
    }

    public void StartSpawnProcess(GameObject startSpawner) //spawn the first room
    {
        GameObject justSpawnedRoom = Instantiate(startRoom, startSpawner.transform.position, Quaternion.identity, parent.transform);
        SpawnerBase startSpawnerBase = startSpawner.GetComponent<SpawnerBase>();
        startSpawnerBase.room = justSpawnedRoom;
        startSpawnerBase.hasARoom = true;
        justSpawnedRoomScript = justSpawnedRoom.GetComponent<Room>();
        justSpawnedRoomScript.spawner = startSpawner;
        roomSpawners.Remove(startSpawner);
        spawnedRooms.Add(justSpawnedRoom);

        spawnState = SpawnState.AwakenRoom;
    }

    //grab a random list from the walls list and spawn a room, turn the wall off and take it out of the wall list, repeat
    public void SpawnRoom()
    {
        if (lastRoomSpawned == true)
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
            walls.Clear();
            spawnState = SpawnState.Finishing;
            return;
        }
        else if (walls.Count > 0)
        {
            GameObject wallToSpawnFrom = walls[Random.Range(0, walls.Count)];
            GameObject closestSpawner = wallToSpawnFrom.GetComponent<Wall>().closestSpawner;
            if (wallToSpawnFrom.GetComponent<Wall>().closestSpawner.GetComponent<SpawnerBase>().hasARoom == false)
            {
                if (roomSpawners.Count == 1)
                {
                    InstantiateRoom(endRoom, closestSpawner, wallToSpawnFrom.GetComponentInParent<Room>().spawner);
                    lastRoomSpawned = true;
                }
                else
                {
                    InstantiateRoom(toSpawnRooms[Random.Range(0, toSpawnRooms.Count)], closestSpawner, wallToSpawnFrom.GetComponentInParent<Room>().spawner);
                }

                if (walls.Count > 0)
                {
                    spawnState = SpawnState.AwakenRoom;
                }
            }

            walls.Remove(wallToSpawnFrom);
            wallToSpawnFrom.SetActive(false);
        }
        else
        {
            spawnState = SpawnState.AwakenRoom;
        }
    }

    public void InstantiateRoom(GameObject chosenRoomToSpawn, GameObject closestSpawner, GameObject spawnerBelongingToWall)
    {
        GameObject justSpawnedRoom = Instantiate(chosenRoomToSpawn, closestSpawner.transform.position, Quaternion.identity, parent.transform);
        justSpawnedRoomScript = justSpawnedRoom.GetComponent<Room>();
        justSpawnedRoomScript.spawner = closestSpawner;
        justSpawnedRoomScript.spawnedFromSpawner = spawnerBelongingToWall;
        SpawnerBase closestSpawnerBase = closestSpawner.GetComponent<SpawnerBase>();
        closestSpawnerBase.room = justSpawnedRoom;
        closestSpawnerBase.hasARoom = true;
        roomSpawners.Remove(closestSpawner);
        spawnedRooms.Add(justSpawnedRoom);
    }

    public void SetActiveStartSpawnerSelectingButtons()
    {
        foreach (GameObject spawner in roomSpawners)
        {
            spawner.GetComponent<SpawnerBase>().SetActiveButton();
        }

        spawnState = SpawnState.AwaitingInput;
    }
}