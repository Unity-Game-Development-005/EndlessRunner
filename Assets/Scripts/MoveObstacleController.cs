
using UnityEngine;


public class MoveObstacleController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // obstacle movement speed
    private float obstacleSpeed = 15f;

    // left boundary
    private float leftBoundary = -15f;




    private void Start()
    {
        // set the reference to the game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }



    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }


    private void MoveObject()
    {
        // if the game is running
        if (!gameController.gameOver)
        {
            // then move the obstacles
            transform.Translate(obstacleSpeed * Time.deltaTime * Vector3.left, Space.World);

            // if an object has reached the left boundary
            if (transform.position.x < leftBoundary)
            {
                Destroy(gameObject);
            }
        }
    }


} // end of class
