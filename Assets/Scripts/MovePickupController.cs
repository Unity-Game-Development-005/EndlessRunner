
using UnityEngine;


public class MovePickupController : MonoBehaviour
{
    // get a reference to the game controller script
    private GameController gameController;


    private float pickupSpeed = 10f;

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
            transform.Translate(pickupSpeed * Time.deltaTime * Vector3.left, Space.World);

            // if an object has reached the left boundary
            if (transform.position.x < leftBoundary)
            {
                Destroy(gameObject);
            }
        }
    }


} // end of class
