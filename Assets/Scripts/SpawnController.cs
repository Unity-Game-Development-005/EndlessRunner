
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // array for obtacle prefabs
    public GameObject[] obstaclePrefabs;

    // list for spawned obstacles
    private List<GameObject> spawnedObstaclesList;

    // start position for obstacle spawner
    private Vector3 obstacleSpawnPos = new Vector3(25f, 0f, 0f);

    // start delay in seconds for the obstacle spawner
    private float startDelay = 2;

    // the time between spawns in seconds
    public float repeatRate = 2;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set reference to game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        // start the obstacle spawner
        SelectRandomSpawnTime();
    }


    private void SelectRandomSpawnTime()
    {
        // if the game is not running
        if (gameController.gameOver)
        {
            // stop spawning obstacles
            CancelInvoke();
        }

        // select a random drop time base on the start delay time
        float nextObstacle = Random.Range(startDelay, startDelay * repeatRate);

        // spawn a random ball
        Invoke(nameof(SelectRandomSpawnTime), nextObstacle);

        // select another randon drop rate
        Invoke(nameof(SpawnRandomObstacles), nextObstacle);
    }


    private void SpawnRandomObstacles()
    {
        // select a random obstacle
        int randomObstacle = Random.Range(0, obstaclePrefabs.Length);

        // instantiate the obstacle at random spawn location
        Instantiate(obstaclePrefabs[randomObstacle], obstacleSpawnPos, obstaclePrefabs[randomObstacle].transform.rotation);

        // add the obstacle to the spawned obstacles list
        ///spawnedObstaclesList.Add(instantitatedObject);
    }


} // end of class
