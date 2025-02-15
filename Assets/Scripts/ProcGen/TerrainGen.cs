using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
    public static float zOffset = 0.0f; //Next z position to spawn blocks
    public static float yOffset = 0.0f; //Next y position to spawn blocks
    public float distanceToSpawn; //Distance to player to spawn new blocks
    public int blocksToSpawn; //Initial amount of blocks to spawn

    public Block[] blocks; //Array of the different types of blocks to spawn
    public Block checkpointBlock;
    public Block emptyBlock; //Empty block for the start of each level

    public float distToCheckpoint; //Total distance needed to spawn the next checkpoint
    private int checkpointNumber; //The total number of checkpoints spawned
    private int checkpointCount;
    public int checkpointsToNextLvl; //The number of checkpoints to be reached until a level increase

    public static int level = 0; //The current level (determines difficulty)
    public int maxLevel; //Maximum level
    public int lengthIncrement; //The increase in a level length
    public float levelSpeedIncrement; //Increase in fall away speed after increasing level

    public bool checkpointSpawned;

    public static TerrainGen instance;
    public GameObject parent;
    public BuildingGen buildingGen;

    private GameObject player;
    private GameObject loadingAlert;

    private float loadTime = 0.2f;
    private float loadTimer;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        zOffset = 0.0f;
        yOffset = 0.0f;
        level = 0;

        player =  FindObjectOfType<PlayerMovement2>().gameObject;
        loadingAlert = FindObjectOfType<UIManager>().GetComponent<UIManager>().loadingAlert;

        loadTimer = loadTime;

        for (int i = 0; i < blocksToSpawn; i++) //Create initial starting area
        {
            Instantiate(emptyBlock.gameObject, new Vector3(0.0f, yOffset, zOffset), emptyBlock.gameObject.transform.rotation, parent.transform);
            zOffset += emptyBlock.length;
            yOffset += emptyBlock.heightOffset;
        }

        checkpointSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {

        // while(zOffset < distToCheckpoint*(checkpointNumber+1))
        // {
        //     if (loadingAlert != null)
        //     {
        //         loadingAlert.SetActive(true);
        //         loadTimer = loadTime;
        //     }

        //     SpawnTerrain();
        //     buildingGen.SpawnBuilding();
        // }
        StartCoroutine(LoadBlocks());

        if (loadingAlert != null) 
        {
            if (loadingAlert.activeSelf && loadTimer > 0)
            {
                loadTimer -= Time.deltaTime;
            }

            if (loadingAlert.activeSelf && loadTimer <= 0)
            {
                loadingAlert.SetActive(false);
            }
        }

        //Spawn a checkpoint
        if (zOffset >= distToCheckpoint*(checkpointNumber+1) && !checkpointSpawned)
        {
            checkpointNumber++;
            checkpointCount++;
            //Increase level after reaching set number of checkpoints
            if (checkpointCount >= checkpointsToNextLvl && level < maxLevel)
            {
                level += 1;
                distToCheckpoint += lengthIncrement;
                checkpointCount = 0;
                ConductorV2.getConductor().levelSpeed += levelSpeedIncrement;
            }
            Instantiate(checkpointBlock.gameObject, new Vector3(0.0f, yOffset, zOffset), checkpointBlock.gameObject.transform.rotation, parent.transform);
            zOffset += checkpointBlock.length;
            yOffset += checkpointBlock.heightOffset;
            checkpointSpawned = true;
        }

    }

    IEnumerator SpawnTerrain()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, blocks.Length);
        Instantiate(blocks[randomNumber].gameObject, new Vector3(0.0f, yOffset, zOffset), blocks[randomNumber].gameObject.transform.rotation, parent.transform);

        zOffset += blocks[randomNumber].length;
        yOffset += blocks[randomNumber].heightOffset;

        yield return new WaitForEndOfFrame();
    }

    public void ProgressCheckpoint()
    {
        if (checkpointSpawned)
        {
            //checkpointNumber++;
            //checkpointCount++;
            checkpointSpawned = false;

            if (loadingAlert != null)
            {
                loadingAlert.SetActive(true);
                loadTimer = loadTime;
            }
        }
    }

    IEnumerator LoadBlocks()
    {
        float startTime;
        float endTime;
        float elapsedTime = 0;
        float desiredTime = 0.015f;
        while(zOffset < distToCheckpoint*(checkpointNumber+1))
        {
            startTime = Time.realtimeSinceStartup;
            if (loadingAlert != null)
            {
                loadingAlert.SetActive(true);
                loadTimer = loadTime;
            }

            StartCoroutine(SpawnTerrain());
            buildingGen.SpawnBuilding();
            endTime = Time.realtimeSinceStartup;
            elapsedTime += endTime = startTime;

            if(elapsedTime >= desiredTime)
            {
                elapsedTime = 0;
                yield return null;
            }
        }

        yield return null;
    }
}
