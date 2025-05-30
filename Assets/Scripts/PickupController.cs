
using System.Collections.Generic;
using UnityEngine;


public class PickupController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // set reference to pickup prefab
    public GameObject pickupPrefab;

    // list for spawned pickups
    private List<GameObject> spawnedPickupsList;


    // start position for coin spawner
    private Vector3 pickupSpawnPos = new Vector3(25f, 4f, 0f);


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
        float nextPickup = Random.Range(startDelay, startDelay * repeatRate);

        // spawn a random ball
        Invoke(nameof(SelectRandomSpawnTime), nextPickup);

        // select another randon drop rate
        Invoke(nameof(SpawnPickup), nextPickup);
    }


    private void SpawnPickup()
    {
        // instantiate the obstacle at random spawn location
        GameObject instantitatedObject = Instantiate(pickupPrefab, pickupSpawnPos, pickupPrefab.transform.rotation);

        // add the pickup to the spawned pickups list
        ///spawnedPickupsList.Add(instantitatedObject);
    }


} // end of class
