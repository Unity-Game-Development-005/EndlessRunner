
using UnityEngine;


public class MoveObjectController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    // obstacle movement speed
    public float obstacleSpeed = 15f;

    // left boundary
    public float leftBoundary = -15f;

    // direction of object
    private Vector3 objectDirection;

    // whether objects move left or right
    [SerializeField] private bool isMovingLeft;




    private void Start()
    {
        // set the reference to the game controller script
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        // object is to move left
        if (isMovingLeft)
        {
            // then set object direction to left
            objectDirection = Vector3.left;
        }

        // otherwise
        else
        {
            // set object direction to right
            objectDirection = Vector3.right;
        }
    }



    // Update is called once per frame
    void Update()
    {
        // if the game is running
        if (!gameController.gameOver)
        {
            // then move the obstacles
            transform.Translate(obstacleSpeed * Time.deltaTime * objectDirection);

            // if an obstacle has reached the left boundary
            if (transform.position.x < leftBoundary)
            {
                Destroy(gameObject);
            }
        }
    }


} // end of class
